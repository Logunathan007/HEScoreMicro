
using GenericController.Application.Mapper.Reply;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using HEScoreMicro.Application.HPXMLClasses;
using HEScoreMicro.Application.HPXMLClasses.ZoneRoofs;
using HEScoreMicro.Application.HPXMLClasses.ZoneFloors;
using HEScoreMicro.Application.HPXMLClasses.ZoneWalls;
using HEScoreMicro.Application.HPXMLClasses.Systems;
using HEScoreMicro.Domain.Entity.HeatingCoolingSystems;
using System.Text.RegularExpressions;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Domain.Entity.ZoneFloors;
using HEScoreMicro.Domain.Entity.OtherSystems;
using HEScoreMicro.Domain.Entity.Address;
using HEScoreMicro.Domain.Entity.ZoneWindows;
using HEScoreMicro.Domain.Entity.ZoneWalls;
using HEScoreMicro.Application.HPXMLClasses.EnergyStars;

namespace HEScoreMicro.Application.Operations.HPXMLGeneration
{
    public interface IHPXMLObjectCreation
    {
        Task<ResponseDTO<HPXML>> CreateHPXMLObject(Guid Id);
        Task<ResponseDTO<string>> GenerateHPXMLString(HPXML hpxml);
        Task<ResponseDTO<string>> GenerateHPXMLBase64String(string hpxmlString);
    }
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    public class HPXMLObjectCreation(IBuildingOperations _buildingOperations) : IHPXMLObjectCreation
    {
        // Interface Implemented Methods =========================================================================
        public async Task<ResponseDTO<HPXML>> CreateHPXMLObject(Guid Id)
        {
            var building = await _buildingOperations.GetById(Id);

            // Inside Enclosure
            //
            List<Floor> floors = new List<Floor>();
            List<WallHPXML> walls = new List<WallHPXML>();
            List<Attic> attics = new List<Attic>();
            List<Roof> roofs = new List<Roof>();
            List<Skylight> skylights = new List<Skylight>();
            List<FoundationHPXML> foundations = new List<FoundationHPXML>();
            List<Slab> slabs = new List<Slab>();
            List<FoundationWall> foundationWalls = new List<FoundationWall>();
            List<WindowHPXML> windows = new List<WindowHPXML>();

            if (building.Failed)
            {
                return new ResponseDTO<HPXML>
                {
                    Failed = true,
                    Message = "Building not found."
                };
            }
            if (building.Data.About == null)
            {
                return new ResponseDTO<HPXML>
                {
                    Failed = true,
                    Message = "About data not found."
                };
            }

            if (building.Data.ZoneRoof != null)
                this.GenerateAtticsObject(building.Data.ZoneRoof, attics, floors, walls, roofs, skylights);

            if (building.Data.ZoneFloor != null)
                this.GenerateFoundationFloorsObject(building.Data.ZoneFloor, floors, foundations, slabs, foundationWalls);

            if (building.Data.ZoneWall != null)
                this.GenerateZoneWallsObject(building.Data.ZoneWall, walls);

            if (building.Data.ZoneWindow != null)
                this.GenerateZoneWindowObject(building.Data.ZoneWindow, windows, walls, building.Data.Address.DwellingUnitType);

            HVAC hvac = null;
            if (building.Data.HeatingCoolingSystem != null)
            {
                hvac = this.GenerateSystems(building.Data.HeatingCoolingSystem);
            }

            var hpxml = new HPXML()
            {
                XMLTransactionHeaderInformation = new XMLTransactionHeaderInformation()
                {
                    XMLType = "HPXML",
                    XMLGeneratedBy = "Inpection Depot",
                    CreatedDateAndTime = DateTime.Now,
                    Transaction = "create",
                },
                SoftwareInfo = new SoftwareInfo()
                {
                    SoftwareProgramUsed = "Inspection Depot",
                    SoftwareProgramVersion = "2.0"
                },
                Building = new BuildingHPXML()
                {
                    BuildingID = new BuildingID()
                    {
                        Id = "bldg-1"
                    },
                    Site = new Site()
                    {
                        SiteID = new SiteID()
                        {
                            Id = "Site-1"
                        },
                        Address = this.GenerateAddressObject(building.Data.Address)
                    },
                    ProjectStatus = new ProjectStatus()
                    {
                        EventType = this.GetEventType(building.Data.Address),
                        Date = building.Data.About.AssessmentDate.ToString("yyyy-MM-dd"),
                    },
                    BuildingDetails = new BuildingDetails()
                    {
                        BuildingSummary = new BuildingSummary()
                        {
                            BuildingConstruction = new BuildingConstruction()
                            {
                                AverageCeilingHeight = building.Data.About.InteriorFloorToCeilingHeight,
                                NumberofBedrooms = building.Data.About.NumberOfBedrooms,
                                ConditionedFloorArea = building.Data.About.TotalConditionedFloorArea,
                                ManufacturedHomeSections = building.Data.About.ManufacturedHomeType,
                                YearBuilt = building.Data.About.YearBuilt,
                                NumberofConditionedFloorsAboveGrade = building.Data.About.StoriesAboveGroundLevel,
                                ResidentialFacilityType = this.GetResidentialFacilityType(building.Data.Address.DwellingUnitType),
                                NumberofUnitsInBuilding = building.Data.About.NumberofUnitsInBuilding,
                            },
                            Site = new BSSite
                            {
                                OrientationOfFrontOfHome = building.Data.About.DirectionFacedByFrontOfHome,
                            }
                        },
                        Enclosure = new Enclosure()
                        {
                            AirInfiltration = this.GenerateAirInfiltrationObject(building.Data.About),
                            Attics = attics.Any() ? new Attics
                            {
                                Attic = attics
                            } : null,
                            Floors = floors.Any() ? new Floors
                            {
                                Floor = floors
                            } : null,
                            Walls = walls.Any() ? new Walls
                            {
                                Wall = walls
                            } : null,
                            Roofs = roofs.Any() ? new Roofs
                            {
                                Roof = roofs
                            } : null,
                            Skylights = skylights.Any() ? new Skylights
                            {
                                Skylight = skylights
                            } : null,
                            Foundations = foundations.Any() ? new Foundations
                            {
                                Foundation = foundations
                            } : null,
                            FoundationWalls = foundationWalls.Any() ? new FoundationWalls
                            {
                                FoundationWall = foundationWalls
                            } : null,
                            Slabs = slabs.Any() ? new Slabs
                            {
                                Slab = slabs
                            } : null,
                            Windows = windows.Any() ? new Windows
                            {
                                Window = windows
                            } : null,
                        },
                        Systems = new HpxmlSystems()
                        {
                            WaterHeating = this.GenerateWaterHeater(building.Data.WaterHeater),
                            Photovoltaics = this.GeneratePhotovoltaics(building.Data.PVSystem),
                            HVAC = hvac
                        },
                        GreenBuildingVerifications = (building.Data.EnergyStar?.EnergyStarPresent == true) ? new GreenBuildingVerifications() : null
                    },
                    extension = (building.Data.About.Comments == null) ? null : new BuildingExtension
                    {
                        Comments = building.Data.About.Comments,
                    }
                },
                Contractor = (building.Data.EnergyStar?.ContractorBusinessName != null) ? new Contractor()
                {
                    ContractorDetails = new ContractorDetails
                    {
                        SystemIdentifier = new SystemIdentifier
                        {
                            Id = "contractor-1"
                        },
                        BusinessInfo = new BusinessInfo
                        {
                            SystemIdentifier = new SystemIdentifier
                            {
                                Id = "contractor-1-businessinfo-1"
                            },
                            BusinessName = building.Data.EnergyStar.ContractorBusinessName,
                            extension = (building.Data.EnergyStar?.ContractorZipCode != null) ? new BusinessInfoextension
                            {
                                ZipCode = building.Data.EnergyStar?.ContractorZipCode?.ToString("D5")
                            } : null
                        }
                    }
                } : null,
                Project = new Project()
                {
                    ProjectDetails = new ProjectDetails
                    {
                        StartDate = building.Data?.EnergyStar?.StartDate?.ToString("yyyy-MM-dd"),
                        CompleteDateActual = building.Data?.EnergyStar?.CompletionDate?.ToString("yyyy-MM-dd"),
                    },
                },
            };
            return new ResponseDTO<HPXML>
            {
                Data = hpxml,
                Failed = false,
                Message = "HPXML object created successfully."
            };
        }
        public async Task<ResponseDTO<string>> GenerateHPXMLString(HPXML hpxml)
        {
            var serializer = new XmlSerializer(typeof(HPXML));
            string xmlString;
            using (var stringWriter = new Utf8StringWriter())
            {
                var settings = new XmlWriterSettings
                {
                    Indent = false,
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.UTF8
                };
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, hpxml);
                }
                xmlString = stringWriter.ToString();
                xmlString = Regex.Replace(xmlString, @"<\w+\s+xsi:nil=""true""\s*/>", "");
            }
            return new ResponseDTO<string> { Data = xmlString, Failed = false, Message = "String Gernerated Successfully" };
        }

        public async Task<ResponseDTO<string>> GenerateHPXMLBase64String(string hpxmlString)
        {
            string base64HPXML = Convert.ToBase64String(Encoding.UTF8.GetBytes(hpxmlString));
            return new ResponseDTO<string> { Data = base64HPXML, Failed = false, Message = "String Gernerated Successfully" };
        }

        // Get Object Type response =========================================================================
        public AddressHPXML GenerateAddressObject(AddressDTO addressDTO)
        {
            return new AddressHPXML()
            {
                Address1 = addressDTO.StreetAddress,
                Address2 = addressDTO.AddressLine,
                CityMunicipality = addressDTO.City,
                AddressType = "street",
                StateCode = addressDTO.State,
                ZipCode = addressDTO.ZipCode.ToString("D5")
            };
        }

        public AirInfiltration GenerateAirInfiltrationObject(AboutDTO aboutDTO)
        {
            return new AirInfiltration()
            {
                AirInfiltrationMeasurement = new AirInfiltrationMeasurement()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = "AirInfiltrationMeasurement-1"
                    },
                    HousePressure = aboutDTO.BlowerDoorTestConducted ? 50 : null,
                    BuildingAirLeakage = aboutDTO.BlowerDoorTestConducted ? new BuildingAirLeakage()
                    {
                        UnitofMeasure = "CFM",
                        AirLeakage = aboutDTO.AirLeakageRate,
                    } : null,
                    LeakinessDescription = (aboutDTO.AirSealed == true) ? "tight" : "average"
                },
                AirSealing = (aboutDTO.AirSealed == true) ? new AirSealing()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = "AirSealing-1"
                    },
                } : null,
            };
        }
        public void GenerateAtticsObject(ZoneRoofDTO zoneRoofDTOs, List<Attic> attics, List<Floor> floors, List<WallHPXML> walls, List<Roof> roofs, List<Skylight> skylights)
        {
            int i = 1;
            foreach (var zoneRoofDTO in zoneRoofDTOs.RoofAttics)
            {
                var id = "attic-" + i;
                var type = zoneRoofDTO.AtticOrCeilingType;
                var attic = new Attic()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                };
                // Veted attic
                switch (type)
                {
                    case "Unconditioned Attic":
                        attic.AtticType = new AtticType { Attic = new AtticTypes() };
                        if (zoneRoofDTO.KneeWallPresent == true)
                        {
                            attic.AttachedToWall = new AttachedToWall()
                            {
                                IdRef = id + "-wall-1",
                            };
                            this.GenerateAtticWallObject(zoneRoofDTO, walls, attic.AttachedToWall.IdRef);
                        }
                        attic.AttachedToRoof = new AttachedToRoof()
                        {
                            IdRef = id + "-roof-1",
                        };
                        this.GenerateAtticRoofObject(zoneRoofDTO, roofs, skylights, attic.AttachedToRoof.IdRef);
                        attic.AttachedToFloor = new AttachedToFloor()
                        {
                            IdRef = id + "-floor-1",
                        };
                        this.GenerateAtticFloorObject(zoneRoofDTO, floors, attic.AttachedToFloor.IdRef);
                        break;
                    case "Cathedral Ceiling":
                        attic.AtticType = new AtticType { CathedralCeiling = new CathedralCeiling() };
                        attic.AttachedToRoof = new AttachedToRoof()
                        {
                            IdRef = id + "-roof-1",
                        };
                        this.GenerateAtticRoofObject(zoneRoofDTO, roofs, skylights, attic.AttachedToRoof.IdRef);
                        break;
                    case "Flat Roof":
                        attic.AtticType = new AtticType { FlatRoof = new FlatRoof() };
                        attic.AttachedToRoof = new AttachedToRoof()
                        {
                            IdRef = id + "-roof-1",
                        };
                        this.GenerateAtticRoofObject(zoneRoofDTO, roofs, skylights, attic.AttachedToRoof.IdRef);
                        break;
                    // TODO For bellow other unit hpxml generating not clealy mentiond in documentaiont need to clarigy
                    case "Below Other Unit":
                        attic.AtticType = new AtticType { BelowApartment = new BelowApartment() };
                        attic.AttachedToFloor = new AttachedToFloor()
                        {
                            IdRef = id + "-floor-1",
                        };
                        this.GenerateAtticFloorObject(zoneRoofDTO, floors, attic.AttachedToFloor.IdRef);
                        break;
                }

                attics.Add(attic);
                i++;
            }
        }
        public void GenerateAtticRoofObject(RoofAtticDTO roofAtticDTO, List<Roof> roofs, List<Skylight> skylights, string idref)
        {
            Roof roof = new Roof
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref
                }
            };
            roof.Insulation = new Insulation()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref + "-insulation-1"
                }
            };
            if (roofAtticDTO.Construction == "Roof with Radiant Barrier")
            {
                // TODO Need to clarify in document for roof deck
                roof.RadiantBarrier = true;
            }
            else if (roofAtticDTO.Construction == "Roof with Rigid Foam Sheathing")
            {
                Layer layer = new Layer()
                {
                    InstallationType = "continuous",
                    InsulationMaterial = new InsulationMaterial()
                    {
                        Rigid = "eps"
                    },
                    NominalRValue = 10
                };
                roof.Insulation.Layer = new List<Layer> { layer };
            }

            // RoofType
            if (roofAtticDTO.ExteriorFinish == "Composition Shingles or Metal")
            {
                roof.RoofType = "shingles";
            }
            else if (roofAtticDTO.ExteriorFinish == "Wood Shakes")
            {
                roof.RoofType = "wood shingles or shakes";
            }
            else if (roofAtticDTO.ExteriorFinish == "Clay Tile")
            {
                roof.RoofType = "slate or tile shingles";
            }
            else if (roofAtticDTO.ExteriorFinish == "Concrete Tile")
            {
                roof.RoofType = "concrete";
            }
            else if (roofAtticDTO.ExteriorFinish == "Tar or Gravel")
            {
                roof.RoofType = "plastic/rubber/synthetic sheeting";
            }
            else
            {
                roof.RoofType = null;
            }

            // RoofColor and SolarAbsorptance
            if (roofAtticDTO.RoofColor == "White")
            {
                roof.RoofColor = "reflective";
            }
            else if (roofAtticDTO.RoofColor == "Light")
            {
                roof.RoofColor = "light";
            }
            else if (roofAtticDTO.RoofColor == "Medium")
            {
                roof.RoofColor = "medium";
            }
            else if (roofAtticDTO.RoofColor == "Medium Dark")
            {
                roof.RoofColor = "medium dark";
            }
            else if (roofAtticDTO.RoofColor == "Dark")
            {
                roof.RoofColor = "dark";
            }
            else if (roofAtticDTO.RoofColor == "Cool Color")
            {
                roof.SolarAbsorptance = roofAtticDTO.Absorptance;
            }
            else
            {
                roof.RoofColor = null;
            }

            // Roof Area and Roof Insulation for type = 0
            roof.Area = roofAtticDTO.RoofArea;
            roof.Insulation.AssemblyEffectiveRValue = roofAtticDTO.RoofInsulation;

            // Roof Skylights
            if (roofAtticDTO.SkylightsPresent == true)
            {
                GenerateSkylightObject(roofAtticDTO, skylights, idref);
            }
            roofs.Add(roof);
        }
        public void GenerateAtticFloorObject(RoofAtticDTO roofAtticDTO, List<Floor> floors, string idref)
        {
            Floor floor = new Floor()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref,
                },
                Area = roofAtticDTO.AtticFloorArea ?? roofAtticDTO.RoofArea,
                Insulation = (roofAtticDTO.AtticFloorInsulation == null) ? null : new Insulation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-insulation-1"
                    },
                    AssemblyEffectiveRValue = roofAtticDTO.AtticFloorInsulation
                }
            };
            floors.Add(floor);
        }
        public void GenerateAtticWallObject(RoofAtticDTO roofAtticDTO, List<WallHPXML> walls, string idref)
        {
            if (roofAtticDTO.KneeWallPresent == true)
            {
                WallHPXML wall = new WallHPXML()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref
                    },
                    ExteriorAdjacentTo = "attic",
                    InteriorAdjacentTo = "living space",
                    Area = roofAtticDTO.KneeWallArea,
                    AtticWallType = "knee wall",
                    WallType = new WallType
                    {
                        WoodStud = new WoodStud()
                    },
                    Insulation = new Insulation()
                    {
                        SystemIdentifier = new SystemIdentifier
                        {
                            Id = idref + "-insulation-1"
                        },
                        AssemblyEffectiveRValue = roofAtticDTO.KneeWallInsulation,
                    }
                };
                walls.Add(wall);
            }
        }
        public void GenerateSkylightObject(RoofAtticDTO roofAtticDTO, List<Skylight> skylights, string idref)
        {
            var id = idref + "-skylight-1";
            Skylight skylight = new Skylight
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = id
                },
                Area = roofAtticDTO.SkylightArea,
            };
            if (roofAtticDTO.KnowSkylightSpecification == true)
            {
                skylight.SHGC = roofAtticDTO.SHGC;
                skylight.UFactor = roofAtticDTO.UFactor;
            }
            else
            {
                if (roofAtticDTO.FrameMaterial == "Aluminum")
                {
                    skylight.FrameType = new FrameType
                    {
                        Aluminum = new Aluminum()
                    };
                    if (roofAtticDTO.Panes == "Single-Pane")
                    {
                        skylight.GlassLayers = "single-pane";
                        if (roofAtticDTO.GlazingType == "Clear")
                        {
                            skylight.GlassType = "other";
                        }
                        else if (roofAtticDTO.GlazingType == "Tinted")
                        {
                            skylight.GlassType = "tinted";
                        }
                    }
                    else if (roofAtticDTO.Panes == "Double-Pane")
                    {
                        skylight.GlassLayers = "double-pane";
                        if (roofAtticDTO.GlazingType == "Clear")
                        {
                            skylight.GlassType = "other";
                        }
                        else if (roofAtticDTO.GlazingType == "Tinted")
                        {
                            skylight.GlassType = "tinted";
                        }
                        else if (roofAtticDTO.GlazingType == "Solar-Control low-E")
                        {
                            skylight.GlassType = "low-e";
                        }
                    }
                }
                else if (roofAtticDTO.FrameMaterial == "Aluminum with Thermal Break")
                {
                    skylight.FrameType = new FrameType
                    {
                        Aluminum = new Aluminum { ThermalBreak = true }
                    };
                    if (roofAtticDTO.Panes == "Double-Pane")
                    {
                        skylight.GlassLayers = "double-pane";
                        if (roofAtticDTO.GlazingType == "Clear")
                        {
                            skylight.GlassType = "other";
                        }
                        else if (roofAtticDTO.GlazingType == "Tinted")
                        {
                            skylight.GlassType = "tinted";
                        }
                        else if (roofAtticDTO.GlazingType == "Insulating low-E, argon gas fill")
                        {
                            skylight.GlassType = "low-e";
                            skylight.GasFill = "argon";
                        }
                        else if (roofAtticDTO.GlazingType == "Solar-Control low-E")
                        {
                            skylight.GlassType = "reflective";
                        }
                    }
                }
                else if (roofAtticDTO.FrameMaterial == "Wood or Vinyl")
                {
                    skylight.FrameType = new FrameType
                    {
                        Wood = new Wood()
                    };
                    if (roofAtticDTO.Panes == "Single-Pane")
                    {
                        skylight.GlassLayers = "single-pane";
                        if (roofAtticDTO.GlazingType == "Clear")
                        {
                            skylight.GlassType = "other";
                        }
                        else if (roofAtticDTO.GlazingType == "Tinted")
                        {
                            skylight.GlassType = "tinted";
                        }
                    }
                    else if (roofAtticDTO.Panes == "Double-Pane")
                    {
                        skylight.GlassLayers = "double-pane";
                        if (roofAtticDTO.GlazingType == "Clear")
                        {
                            skylight.GlassType = "other";
                        }
                        else if (roofAtticDTO.GlazingType == "Tinted")
                        {
                            skylight.GlassType = "tinted";
                        }
                        else if (roofAtticDTO.GlazingType == "Solar-Control low-E")
                        {
                            skylight.GlassType = "reflective";
                        }
                        else if (roofAtticDTO.GlazingType == "Solar-Control low-E, argon gas fill")
                        {
                            skylight.GlassType = "low-e";
                            skylight.GasFill = "argon";
                        }
                        // TODO Not mention in documentation need to verify
                        else if (roofAtticDTO.GlazingType == "Insulating low-E")
                        {
                            skylight.GlassType = "reflective";
                        }
                        else if (roofAtticDTO.GlazingType == "Insulating low-E, argon gas fill")
                        {
                            skylight.GlassType = "low-e";
                            skylight.GasFill = "argon";
                        }
                    }
                    else if (roofAtticDTO.Panes == "Triple-Pane")
                    {
                        skylight.GlassLayers = "triple-pane";
                    }
                }
            }
            // Solar Screen is present or not
            // < ExteriorShading >< Type > solar screens </ Type ></ ExteriorShading >
            if (roofAtticDTO.SolarScreen == true)
            {
                skylight.ExteriorShading = new ExteriorShading()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id + "-exterior-shading-1"
                    },
                    Type = "solar screens"
                };
            }

            // AttachedToRoof
            skylight.AttachedToRoof = new AttachedToRoof()
            {
                IdRef = idref
            };
            skylights.Add(skylight);
        }
        public void GenerateFoundationFloorsObject(ZoneFloorDTO zoneFloorDTO, List<Floor> floors, List<FoundationHPXML> foundations, List<Slab> slabs, List<FoundationWall> foundationWalls)
        {
            int i = 1;
            foreach (var zoneFloor in zoneFloorDTO.Foundations)
            {
                var id = "foundation-" + i;
                FoundationHPXML foundation = new FoundationHPXML()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                    FoundationType = GetFoundationType(zoneFloor.FoundationType),
                };
                switch (zoneFloor.FoundationType)
                {
                    case "Slab-on-grade foundation":
                        foundation.AttachedToSlab = new AttachedToSlab()
                        {
                            IdRef = id + "-slab-1"
                        };
                        this.GenerateSlabObject(zoneFloor, slabs, foundation.AttachedToSlab.IdRef);
                        break;
                    case "Belly and Wing":
                        foundation.AttachedToFloor = new AttachedToFloor()
                        {
                            IdRef = id + "-floor-1"
                        };
                        this.GenerateFoundationFloorObject(zoneFloor, floors, foundation.AttachedToFloor.IdRef);
                        break;
                    case "Above Other Unit":            // TODO For Above other unit hpxml generating not clealy mentiond in documentaiont need to clarigy
                        foundation.AttachedToFloor = new AttachedToFloor()
                        {
                            IdRef = id + "-floor-1"
                        };
                        this.GenerateFoundationFloorObject(zoneFloor, floors, foundation.AttachedToFloor.IdRef);
                        break;
                    case "Conditioned Basement": // TODO Where i need to place foundation area
                        foundation.AttachedToFoundationWall = new AttachedToFoundationWall()
                        {
                            IdRef = id + "-foundation-wall-1"
                        };
                        this.GenerateFoundationWallObject(zoneFloor, foundationWalls, foundation.AttachedToFoundationWall.IdRef);
                        break;
                    default:
                        foundation.AttachedToFoundationWall = new AttachedToFoundationWall()
                        {
                            IdRef = id + "-foundation-wall-1"
                        };
                        this.GenerateFoundationWallObject(zoneFloor, foundationWalls, foundation.AttachedToFoundationWall.IdRef);
                        foundation.AttachedToFloor = new AttachedToFloor()
                        {
                            IdRef = id + "-floor-1"
                        };
                        this.GenerateFoundationFloorObject(zoneFloor, floors, foundation.AttachedToFloor.IdRef);
                        break;
                }
                foundations.Add(foundation);
                i++;
            }
        }
        public void GenerateSlabObject(FoundationDTO zoneFloor, List<Slab> slabs, string idref)
        {
            Slab slab = new Slab()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref
                },
                Area = zoneFloor.FoundationArea,
                PerimeterInsulation = new PerimeterInsulation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-perimeter-insulation-1"
                    },
                    //AssemblyEffectiveRValue = zoneFloor.SlabInsulationLevel,
                    Layer = new List<Layer>{  new Layer(){
                        NominalRValue = zoneFloor.SlabInsulationLevel
                    }}
                }
            };
            slabs.Add(slab);
        }
        public void GenerateFoundationWallObject(FoundationDTO zoneFloor, List<FoundationWall> foundationWalls, string idref)
        {
            FoundationWall foundationWall = new FoundationWall()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref
                },
                Area = zoneFloor.FoundationArea,
                Insulation = new Insulation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-insulation-1"
                    },
                    AssemblyEffectiveRValue = zoneFloor.FoundationwallsInsulationLevel,
                }
            };
            foundationWalls.Add(foundationWall);
        }
        public void GenerateFoundationFloorObject(FoundationDTO zoneFloor, List<Floor> floors, string idref)
        {
            Floor floor = new Floor()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref
                },
                Area = zoneFloor.FoundationArea,
                Insulation = (zoneFloor.FloorInsulationLevel == null) ? null : new Insulation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-insulation-1"
                    },
                    AssemblyEffectiveRValue = zoneFloor.FloorInsulationLevel,
                }
            };
            floors.Add(floor);
        }
        public void GenerateZoneWallsObject(ZoneWallDTO zoneWallDTO, List<WallHPXML> walls)
        {
            int i = 1;
            foreach (var zoneWall in zoneWallDTO.Walls)
            {
                var id = "wall-" + i;

                WallHPXML wall = new WallHPXML()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                    Insulation = (zoneWall.AdjacentTo == "Other Unit") ? null : new Insulation()
                    {
                        SystemIdentifier = new SystemIdentifier
                        {
                            Id = id + "-insulation-1"
                        },
                        AssemblyEffectiveRValue = zoneWall.WallInsulationLevel,
                    },
                    InteriorAdjacentTo = "living space",
                    ExteriorAdjacentTo = this.GetExteriorAdjacentTo(zoneWall.AdjacentTo),

                    // TODO Need to clarify
                    Area = zoneWallDTO.Walls.Count > 1 ? 20 : null
                };
                if (zoneWall.AdjacentTo != "Outside")
                {
                    walls.Add(wall);
                    i++;
                    continue;
                }
                // For Wall Type
                if (zoneWall.Construction == "Wood Frame")
                {
                    wall.WallType = new WallType
                    {
                        WoodStud = new WoodStud()
                    };
                }
                else if (zoneWall.Construction == "Wood Frame with rigid foam sheathing")
                {
                    wall.WallType = new WallType
                    {
                        WoodStud = new WoodStud
                        {
                            ExpandedPolystyreneSheathing = true
                        }
                    };
                    wall.Insulation.Layer = new List<Layer>
                    {
                        new Layer
                        {
                            InstallationType = "continuous",
                            InsulationMaterial = new InsulationMaterial
                            {
                                Rigid = "eps"
                            },
                        }
                    };
                }
                else if (zoneWall.Construction == "Wood Frame with Optimum Value Engineering (OVE)")
                {
                    wall.WallType = new WallType
                    {
                        WoodStud = new WoodStud
                        {
                            OptimumValueEngineering = true
                        }
                    };
                }
                else if (zoneWall.Construction == "Structural Brick")
                {
                    wall.WallType = new WallType
                    {
                        StructuralBrick = new StructuralBrick()
                    };
                    wall.Insulation.Layer = new List<Layer>
                    {
                        new Layer
                        {
                            NominalRValue = 5
                        },
                        new Layer
                        {
                            NominalRValue = 5
                        }
                    };
                }
                else if (zoneWall.Construction == "Concrete Block or Stone")
                {
                    wall.WallType = new WallType
                    {
                        ConcreteMasonryUnit = new ConcreteMasonryUnit()
                    };
                }
                else if (zoneWall.Construction == "Straw Bale")
                {
                    wall.WallType = new WallType
                    {
                        StrawBale = new StrawBale()
                    };
                }
                else if (zoneWall.Construction == "Steel Frame")
                {
                    // TODO must need to clarify

                }

                //Wall Siding
                switch (zoneWall.ExteriorFinish)
                {
                    case "Wood Siding, Fiber Cement, Composite Shingle, or Masonite Siding":
                        wall.Siding = "wood siding";
                        break;
                    case "Stucco":
                        wall.Siding = "stucco";
                        break;
                    case "Vinyl Siding":
                        wall.Siding = "vinyl siding";
                        break;
                    case "Aluminum Siding":
                        wall.Siding = "aluminum siding";
                        break;
                    case "Brick Veneer":
                        wall.Siding = "brick veneer";
                        break;
                    default:
                        wall.Siding = "none";
                        break;
                }
                walls.Add(wall);
                i++;
            }
        }
        public void GenerateZoneWindowObject(ZoneWindowDTO zoneWindowDTO, List<WindowHPXML> windows, List<WallHPXML> walls, string type)
        {
            int i = 1;
            var listWindowArea = new List<double?>()
            {
                zoneWindowDTO.WindowAreaFront,zoneWindowDTO.WindowAreaBack,
                zoneWindowDTO.WindowAreaRight,zoneWindowDTO.WindowAreaLeft
            };
            listWindowArea.Sort();
            listWindowArea.Reverse();
            List<string> exteriorWallIds = walls.Where(obj => obj.ExteriorAdjacentTo == "outside").Select(obj => obj.SystemIdentifier.Id).ToList(); ;
            int exteriorWallCount = exteriorWallIds.Count;
            int windowCount = zoneWindowDTO.Windows.Count;
            foreach (var zoneWindow in zoneWindowDTO.Windows)
            {
                var id = "window-" + i;
                WindowHPXML window = new WindowHPXML()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                    Area = listWindowArea[i - 1],
                };
                if (type == "Single-Family Detached")
                {
                    window.AttachedToWall = new AttachedToWall()
                    {
                        IdRef = exteriorWallCount == 1 ? "wall-1" : "wall-" + i
                    };
                }
                else if (type == "Townhouse/Rowhouse/Duplex")
                {
                    window.AttachedToWall = new AttachedToWall()
                    {
                        IdRef = exteriorWallIds[i - 1]
                    };
                }

                if (zoneWindow.KnowWindowSpecification == true)
                {
                    window.SHGC = zoneWindow.SHGC;
                    window.UFactor = zoneWindow.UFactor;
                }
                else
                {
                    if (zoneWindow.FrameMaterial == "Aluminum")
                    {
                        window.FrameType = new FrameType
                        {
                            Aluminum = new Aluminum()
                        };
                        if (zoneWindow.Panes == "Single-Pane")
                        {
                            window.GlassLayers = "single-pane";
                            if (zoneWindow.GlazingType == "Clear")
                            {
                                window.GlassType = "other";
                            }
                            else if (zoneWindow.GlazingType == "Tinted")
                            {
                                window.GlassType = "tinted";
                            }
                        }
                        else if (zoneWindow.Panes == "Double-Pane")
                        {
                            window.GlassLayers = "double-pane";
                            if (zoneWindow.GlazingType == "Clear")
                            {
                                window.GlassType = "other";
                            }
                            else if (zoneWindow.GlazingType == "Tinted")
                            {
                                window.GlassType = "tinted";
                            }
                            else if (zoneWindow.GlazingType == "Solar-Control low-E")
                            {
                                window.GlassType = "low-e";
                            }
                        }
                    }
                    else if (zoneWindow.FrameMaterial == "Aluminum with Thermal Break")
                    {
                        window.FrameType = new FrameType
                        {
                            Aluminum = new Aluminum { ThermalBreak = true }
                        };
                        if (zoneWindow.Panes == "Double-Pane")
                        {
                            window.GlassLayers = "double-pane";
                            if (zoneWindow.GlazingType == "Clear")
                            {
                                window.GlassType = "other";
                            }
                            else if (zoneWindow.GlazingType == "Tinted")
                            {
                                window.GlassType = "tinted";
                            }
                            else if (zoneWindow.GlazingType == "Insulating low-E, argon gas fill")
                            {
                                window.GlassType = "low-e";
                                window.GasFill = "argon";
                            }
                            else if (zoneWindow.GlazingType == "Solar-Control low-E")
                            {
                                window.GlassType = "reflective";
                            }
                        }
                    }
                    else if (zoneWindow.FrameMaterial == "Wood or Vinyl")
                    {
                        window.FrameType = new FrameType
                        {
                            Wood = new Wood()
                        };
                        if (zoneWindow.Panes == "Single-Pane")
                        {
                            window.GlassLayers = "single-pane";
                            if (zoneWindow.GlazingType == "Clear")
                            {
                                window.GlassType = "other";
                            }
                            else if (zoneWindow.GlazingType == "Tinted")
                            {
                                window.GlassType = "tinted";
                            }
                        }
                        else if (zoneWindow.Panes == "Double-Pane")
                        {
                            window.GlassLayers = "double-pane";
                            if (zoneWindow.GlazingType == "Clear")
                            {
                                window.GlassType = "other";
                            }
                            else if (zoneWindow.GlazingType == "Tinted")
                            {
                                window.GlassType = "tinted";
                            }
                            else if (zoneWindow.GlazingType == "Solar-Control low-E")
                            {
                                window.GlassType = "reflective";
                            }
                            else if (zoneWindow.GlazingType == "Solar-Control low-E, argon gas fill")
                            {
                                window.GlassType = "low-e";
                                window.GasFill = "argon";
                            }
                            // TODO Not mention in documentation need to verify
                            else if (zoneWindow.GlazingType == "Insulating low-E")
                            {
                                window.GlassType = "reflective";
                            }
                            else if (zoneWindow.GlazingType == "Insulating low-E, argon gas fill")
                            {
                                window.GlassType = "low-e";
                                window.GasFill = "argon";
                            }
                        }
                        else if (zoneWindow.Panes == "Triple-Pane")
                        {
                            window.GlassLayers = "triple-pane";
                        }
                    }
                }
                // Solar Screen is present or not
                // < ExteriorShading >< Type > solar screens </ Type ></ ExteriorShading >
                if (zoneWindow.SolarScreen == true)
                {
                    window.ExteriorShading = new ExteriorShading()
                    {
                        SystemIdentifier = new SystemIdentifier
                        {
                            Id = id + "-exterior-shading-1"
                        },
                        Type = "solar screens"
                    };
                }
                windows.Add(window);
                i++;
            }
        }
        public HVAC GenerateSystems(HeatingCoolingSystemDTO? hcs)
        {
            HVAC hVAC = new HVAC();
            List<HeatingSystem> heatingSystems = new List<HeatingSystem>();
            List<CoolingSystem> coolingSystems = new List<CoolingSystem>();
            List<HeatPump> heatPumps = new List<HeatPump>();
            List<HVACDistribution> distributionSystems = new List<HVACDistribution>();
            int i = 1;
            foreach (var system in hcs.Systems)
            {
                var id = "system-" + i;
                if (system.HeatingSystemType.EndsWith("heat pump") || system.CoolingSystemType.EndsWith("heat pump"))
                {
                    this.GenerateHeatPumpObject(system, heatPumps, distributionSystems, id);
                }
                this.GenerateHeatingSystemObject(system, heatingSystems, distributionSystems, id);
                this.GenerateCoolingSystemObject(system, coolingSystems, distributionSystems, id);
                i++;
            }
            hVAC.HVACPlant = new HVACPlant()
            {
                HeatingSystem = heatingSystems.Count == 0 ? null : heatingSystems,
                CoolingSystem = coolingSystems.Count == 0 ? null : coolingSystems,
                HeatPump = heatPumps.Count == 0 ? null : heatPumps,
            };
            hVAC.HVACDistribution = distributionSystems.Count == 0 ? null : distributionSystems;
            return hVAC;
        }

        public void GenerateHeatingSystemObject(SystemsDTO system, List<HeatingSystem> heatingSystems, List<HVACDistribution> distributionSystems, string id)
        {
            if (system == null || system.HeatingSystemType == "None" || system.HeatingSystemType.EndsWith("heat pump"))
            {
                return;
            }
            id += "-heater-1";
            HeatingSystem hs = new HeatingSystem
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = id
                }
            };
            hs.HeatingSystemType = new HeatingSystemType();

            hs.YearInstalled = system.HeatingSystemYearInstalled;

            bool dultFlag = false;
            bool efficiencyUnitFlag = true;
            switch (system.HeatingSystemType)
            {
                case "Central gas furnace": //central_furnace
                    hs.HeatingSystemType.Furnace = new Furnace();
                    hs.HeatingSystemFuel = "natural gas";
                    dultFlag = true;
                    break;
                case "Room (through-the-wall) gas furnace": //wall_furnace
                    hs.HeatingSystemType.WallFurnace = new WallFurnace();
                    hs.HeatingSystemFuel = "natural gas";
                    break;
                case "Gas boiler": //boiler
                    hs.HeatingSystemType.Boiler = new Boiler();
                    hs.HeatingSystemFuel = "natural gas";
                    break;
                case "Propane (LPG) central furnace": //central_furnace
                    hs.HeatingSystemType.Furnace = new Furnace();
                    hs.HeatingSystemFuel = "propane";
                    dultFlag = true;
                    break;
                case "Propane (LPG) wall furnace": //wall_furnace
                    hs.HeatingSystemType.WallFurnace = new WallFurnace();
                    hs.HeatingSystemFuel = "propane";
                    break;
                case "Propane (LPG) boiler": //boiler
                    hs.HeatingSystemType.Boiler = new Boiler();
                    hs.HeatingSystemFuel = "propane";
                    break;
                case "Oil furnace": //central_furnace
                    hs.HeatingSystemType.Furnace = new Furnace();
                    hs.HeatingSystemFuel = "fuel oil";
                    dultFlag = true;
                    break;
                case "Oil boiler": //boiler
                    hs.HeatingSystemType.Boiler = new Boiler();
                    hs.HeatingSystemFuel = "fuel oil";
                    break;
                case "Electric furnace": //central_furnace
                    hs.HeatingSystemType.Furnace = new Furnace();
                    hs.HeatingSystemFuel = "electricity";
                    dultFlag = true;
                    break;
                case "Electric baseboard heater": //baseboard
                    hs.HeatingSystemType.ElectricResistance = new ElectricResistance();
                    hs.HeatingSystemFuel = "electricity";
                    efficiencyUnitFlag = false;
                    break;
                case "Electric boiler": //boiler
                    hs.HeatingSystemType.Boiler = new Boiler();
                    hs.HeatingSystemFuel = "electricity";
                    break;
                case "Wood stove": //stove
                    hs.HeatingSystemType.Stove = new Stove();
                    hs.HeatingSystemFuel = "wood";
                    efficiencyUnitFlag = false;
                    break;
                case "Pellet stove": //stove
                    hs.HeatingSystemType.Stove = new Stove();
                    hs.HeatingSystemFuel = "wood pellets";
                    efficiencyUnitFlag = false;
                    break;
            }
            if (system.PercentAreaServed != null)
            {
                if (system.CoolingSystemType == "None")
                {
                    hs.FractionHeatLoadServed = system.PercentAreaServed / 100;
                }
                else
                {
                    hs.FractionHeatLoadServed = (system.PercentAreaServed / 100) / 2d;
                }
            }
            if (system.KnowHeatingEfficiency == true && efficiencyUnitFlag)
            {
                hs.AnnualHeatingEfficiency = new AnnualHeatingEfficiency()
                {
                    Value = system.HeatingSystemEfficiencyValue,
                    Units = "AFUE",
                };
            }
            else
            {
                hs.YearInstalled = system.HeatingSystemYearInstalled;
            }

            if (dultFlag)
            {
                // if common distribution system then sharing the distribution system
                if (id.StartsWith("system-1"))
                {
                    var ds = distributionSystems.FirstOrDefault(obj => obj.SystemIdentifier.Id.StartsWith("system-1"));
                    if (ds != null)
                    {
                        hs.DistributionSystem = new DistributionSystem
                        {
                            IdRef = ds.SystemIdentifier.Id
                        };
                    }
                }
                if (id.StartsWith("system-2"))
                {
                    var ds = distributionSystems.FirstOrDefault(obj => obj.SystemIdentifier.Id.StartsWith("system-2"));
                    if (ds != null)
                    {
                        hs.DistributionSystem = new DistributionSystem
                        {
                            IdRef = ds.SystemIdentifier.Id
                        };
                    }
                }
                // else create new distribution system
                hs.DistributionSystem = new DistributionSystem
                {
                    IdRef = id + "-distribution-1"
                };
                this.GenerateDistributionSystemObject(system, distributionSystems, id + "-distribution-1");
            }
            heatingSystems.Add(hs);
        }
        public void GenerateCoolingSystemObject(SystemsDTO system, List<CoolingSystem> coolingSystems, List<HVACDistribution> distributionSystems, string id)
        {
            id += "-cooler-1";
            if (system == null || system.CoolingSystemType == "None" || system.CoolingSystemType.EndsWith("heat pump"))
            {
                return;
            }
            CoolingSystem cs = new CoolingSystem
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = id
                }
            };
            switch (system.CoolingSystemType)
            {
                case "Central air conditioner": //split_dx
                    cs.CoolingSystemType = "central air conditioner";
                    break;
                case "Room air conditioner": //packaged_dx
                    cs.CoolingSystemType = "room air conditioner";
                    break;
                case "Direct evaporative cooling": //dec
                    cs.CoolingSystemType = "evaporative cooler";
                    break;
            }
            // TODO PercentAreaServed where i need to mention for heat cool heatpump
            if (system.PercentAreaServed != null)
            {
                if (system.HeatingSystemType == "None")
                {
                    cs.FractionCoolLoadServed = (system.PercentAreaServed / 100);
                }
                else
                {
                    cs.FractionCoolLoadServed = (system.PercentAreaServed / 100) / 2d;
                }
            }
            if (system.KnowCoolingEfficiency == true)
            {
                cs.AnnualCoolingEfficiency = new AnnualCoolingEfficiency()
                {
                    Value = system.CoolingSystemEfficiencyValue,
                    Units = system.CoolingSystemEfficiencyUnit,
                };
            }
            else
            {
                cs.YearInstalled = system.CoolingSystemYearInstalled;
            }
            if (cs.CoolingSystemType == "central air conditioner")
            {
                // if common distribution system then sharing the distribution system
                if (id.StartsWith("system-1"))
                {
                    var ds = distributionSystems.FirstOrDefault(obj => obj.SystemIdentifier.Id.StartsWith("system-1"));
                    if (ds != null)
                    {
                        cs.DistributionSystem = new DistributionSystem
                        {
                            IdRef = ds.SystemIdentifier.Id
                        };
                    }
                }
                if (id.StartsWith("system-2"))
                {
                    var ds = distributionSystems.FirstOrDefault(obj => obj.SystemIdentifier.Id.StartsWith("system-2"));
                    if (ds != null)
                    {
                        cs.DistributionSystem = new DistributionSystem
                        {
                            IdRef = ds.SystemIdentifier.Id
                        };
                    }
                }
                // else create new distribution system
                cs.DistributionSystem = new DistributionSystem
                {
                    IdRef = id + "-distribution-1"
                };
                this.GenerateDistributionSystemObject(system, distributionSystems, id + "-distribution-1");
            }
            coolingSystems.Add(cs);
        }
        public void GenerateHeatPumpObject(SystemsDTO system, List<HeatPump> heatPumps, List<HVACDistribution> distributionSystems, string id)
        {
            if (system == null)
            {
                return;
            }
            id += "-heatpump-1";
            HeatPump hp = new HeatPump
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = id
                },
            };

            if (!system.CoolingSystemType.EndsWith("heat pump") && system.HeatingSystemType.EndsWith("heat pump"))
            {
                hp.FractionCoolLoadServed = 0;
                if (system.PercentAreaServed != null)
                    hp.FractionHeatLoadServed = (system.PercentAreaServed / 100);
            }
            hp.AnnualHeatingEfficiency = system.HeatingSystemEfficiencyValue != null && system.HeatingSystemType.EndsWith("heat pump") ? new AnnualHeatingEfficiency()
            {
                Value = system.HeatingSystemEfficiencyValue,
            } : null;

            if (!system.HeatingSystemType.EndsWith("heat pump") && system.CoolingSystemType.EndsWith("heat pump"))
            {
                hp.FractionHeatLoadServed = 0;
                if (system.PercentAreaServed != null)
                    hp.FractionCoolLoadServed = (system.PercentAreaServed / 100);
            }
            hp.AnnualCoolingEfficiency = system.CoolingSystemEfficiencyValue != null && system.CoolingSystemType.EndsWith("heat pump") ? new AnnualCoolingEfficiency()
            {
                Value = system.CoolingSystemEfficiencyValue,
            } : null;

            if (system.HeatingSystemType.EndsWith("heat pump") && system.CoolingSystemType.EndsWith("heat pump") && system.PercentAreaServed != null)
            {
                hp.FractionCoolLoadServed = (system.PercentAreaServed / 100) / 2d;
                hp.FractionHeatLoadServed = (system.PercentAreaServed / 100) / 2d;
            }

            hp.YearInstalled = system.CoolingSystemYearInstalled ?? system.HeatingSystemYearInstalled;

            if (system.HeatingSystemType == "Electric heat pump" || system.CoolingSystemType == "Electric heat pump") //heat_pump
            {
                hp.HeatPumpType = "air-to-air";
                if (hp.AnnualHeatingEfficiency != null)
                {
                    hp.AnnualHeatingEfficiency.Units = system.HeatingSystemEfficiencyUnit;
                }
                if (hp.AnnualCoolingEfficiency != null)
                {
                    hp.AnnualCoolingEfficiency.Units = system.CoolingSystemEfficiencyUnit;
                }
            }
            else if (system.HeatingSystemType == "Minisplit (ductless) heat pump" || system.CoolingSystemType == "Minisplit (ductless) heat pump") //mini-split
            {
                hp.HeatPumpType = "mini-split";
                if (hp.AnnualHeatingEfficiency != null)
                {
                    hp.AnnualHeatingEfficiency.Units = system.HeatingSystemEfficiencyUnit;
                }
                if (hp.AnnualCoolingEfficiency != null)
                {
                    hp.AnnualCoolingEfficiency.Units = system.CoolingSystemEfficiencyUnit;
                }
            }
            else if (system.HeatingSystemType == "Ground coupled heat pump" || system.CoolingSystemType == "Ground coupled heat pump") // gchp
            {
                hp.HeatPumpType = "water-to-air";
                if (hp.AnnualHeatingEfficiency != null)
                {
                    hp.AnnualHeatingEfficiency.Units = "COP";
                }
                if (hp.AnnualCoolingEfficiency != null)
                {
                    hp.AnnualCoolingEfficiency.Units = "EER";
                }
            }

            if (hp.HeatPumpType != "mini-split")
            {
                // if common distribution system then sharing the distribution system
                if (id.StartsWith("system-1"))
                {
                    var ds = distributionSystems.FirstOrDefault(obj => obj.SystemIdentifier.Id.StartsWith("system-1"));
                    if (ds != null)
                    {
                        hp.DistributionSystem = new DistributionSystem
                        {
                            IdRef = ds.SystemIdentifier.Id
                        };
                    }
                }
                if (id.StartsWith("system-2"))
                {
                    var ds = distributionSystems.FirstOrDefault(obj => obj.SystemIdentifier.Id.StartsWith("system-2"));
                    if (ds != null)
                    {
                        hp.DistributionSystem = new DistributionSystem
                        {
                            IdRef = ds.SystemIdentifier.Id
                        };
                    }
                }
                // else create new distribution system
                hp.DistributionSystem = new DistributionSystem
                {
                    IdRef = id + "-distribution-1"
                };
                this.GenerateDistributionSystemObject(system, distributionSystems, id + "-distribution-1");
            }

            if (hp != null)
                heatPumps.Add(hp);
        }
        public void GenerateDistributionSystemObject(SystemsDTO system, List<HVACDistribution> distributionSystems, string id)
        {
            HVACDistribution hVACDistribution = new HVACDistribution
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = id
                },
                DistributionSystemType = new DistributionSystemType
                {
                    AirDistribution = new AirDistribution()
                    {
                        DuctLeakageMeasurement = (system.DuctLeakageTestPerformed == true) ? new DuctLeakageMeasurement()
                        {
                            DuctLeakage = new DuctLeakage
                            {
                                TotalOrToOutside = "to outside",
                                Units = "CFM25",
                                Value = system.DuctLeakageTestValue
                            },
                            LeakinessObservedVisualInspection = (system.DuctAreProfessionallySealed == true) ? "connections sealed w mastic" : null
                        } : null,
                        Ducts = this.GetDuctsObjects(system.DuctLocations, id)
                    },
                },
                HVACDistributionImprovement = (system.DuctLeakageTestPerformed == true) ? new HVACDistributionImprovement
                {
                    DuctSystemSealed = system.DuctAreProfessionallySealed == true ? true : false,
                } : null,
            };
            distributionSystems.Add(hVACDistribution);
        }
        public List<Ducts> GetDuctsObjects(ICollection<DuctLocationDTO> ductLocationDTOs, string id)
        {
            List<Ducts> ducts = new List<Ducts>();
            int i = 1;
            var count = ductLocationDTOs.Count;
            foreach (var ductLocationDTO in ductLocationDTOs)
            {
                var area = (count > 1) ? (ductLocationDTO.PercentageOfDucts == null ? 0 : ductLocationDTO.PercentageOfDucts / 100) : null;
                Ducts ductsObj = new Ducts
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id + "-duct-" + i,
                    },
                    DuctLocation = this.GetDuctLocation(ductLocationDTO.Location),
                    DuctInsulationThickness = (ductLocationDTO.DuctsIsInsulated == true) ? 1 : null,
                    FractionDuctArea = area ?? .5 // TODO Need to clarify
                };
                ducts.Add(ductsObj);
                i++;
            }
            return ducts;
        }
        public WaterHeating GenerateWaterHeater(WaterHeaterDTO waterHeater)
        {
            if (waterHeater == null)
            {
                return null;
            }
            WaterHeating wh = new WaterHeating
            {
                WaterHeatingSystem = new WaterHeatingSystem
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = "water-heater-1"
                    },
                },
            };
            switch (waterHeater.WaterHeaterType)
            {
                case "Electric Storage":
                    wh.WaterHeatingSystem.WaterHeaterType = "storage water heater";
                    wh.WaterHeatingSystem.FuelType = "electricity";
                    break;
                case "Natural Gas Storage":
                    wh.WaterHeatingSystem.WaterHeaterType = "dedicated boiler with storage tank";
                    wh.WaterHeatingSystem.FuelType = "natural gas";
                    break;
                case "Propane(LPG) Storage":
                    wh.WaterHeatingSystem.WaterHeaterType = "dedicated boiler with storage tank";
                    wh.WaterHeatingSystem.FuelType = "propane";
                    break;
                case "Oil Storage":
                    wh.WaterHeatingSystem.WaterHeaterType = "dedicated boiler with storage tank";
                    wh.WaterHeatingSystem.FuelType = "fuel oil";
                    break;
                case "Electric Instantaneous":
                    wh.WaterHeatingSystem.WaterHeaterType = "instantaneous water heater";
                    wh.WaterHeatingSystem.FuelType = "electricity";
                    break;
                case "Gas Instantaneous":
                    wh.WaterHeatingSystem.WaterHeaterType = "instantaneous water heater";
                    wh.WaterHeatingSystem.FuelType = "natural gas";
                    break;
                case "Propane Instantaneous":
                    wh.WaterHeatingSystem.WaterHeaterType = "instantaneous water heater";
                    wh.WaterHeatingSystem.FuelType = "propane";
                    break;
                case "Oil Instantaneous":
                    wh.WaterHeatingSystem.WaterHeaterType = "instantaneous water heater";
                    wh.WaterHeatingSystem.FuelType = "fuel oil";
                    break;
                case "Electric Heat Pump":
                    wh.WaterHeatingSystem.WaterHeaterType = "heat pump water heater";
                    wh.WaterHeatingSystem.FuelType = "electricity";
                    break;
                case "Boiler with indirect tank":
                    wh.WaterHeatingSystem.WaterHeaterType = "space-heating boiler with storage tank";
                    break;
                case "Boiler with tankless coil":
                    wh.WaterHeatingSystem.WaterHeaterType = "space-heating boiler with tankless coil";
                    break;
            }
            wh.WaterHeatingSystem.ModelYear = waterHeater.YearOfManufacture;
            if (waterHeater.KnowWaterHeaterEnergyFactor == true || wh.WaterHeatingSystem.ModelYear == null)
            {
                if (waterHeater.Unit == "EF")
                {
                    wh.WaterHeatingSystem.EnergyFactor = waterHeater.EnergyValue;
                }
                else if (waterHeater.Unit == "UEF")
                {
                    wh.WaterHeatingSystem.UniformEnergyFactor = waterHeater.EnergyValue;
                }
            }
            return wh;
        }
        public Photovoltaics GeneratePhotovoltaics(PVSystemDTO pvSystem)
        {
            if (pvSystem == null || pvSystem.HasPhotovoltaic == false)
            {
                return null;
            }

            var pv = new Photovoltaics()
            {
                PVSystem = new PVSystemHPXML()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = "pv-system-1"
                    },
                    ArrayOrientation = pvSystem.DirectionPanelsFace,
                    ArrayTilt = this.GetArrayTilt(pvSystem.AnglePanelsAreTilted),
                    YearModulesManufactured = pvSystem.YearInstalled,
                }
            };
            if (pvSystem.KnowSystemCapacity == true)
            {
                pv.PVSystem.MaxPowerOutput = pvSystem.DCCapacity;
            }
            else
            {
                pv.PVSystem.NumberOfPanels = pvSystem.NumberOfPanels;
            }
            return pv;
        }

        // Condtioned based Selection ======================================================================
        public string GetEventType(AddressDTO addressDTO)
        {
            switch (addressDTO.AssessmentType)
            {
                case "Test":
                    return "construction-period testing/daily test out";
                case "preconstruction":
                    return "preconstruction";
                default:
                    return "audit";
            }
        }
        public string GetResidentialFacilityType(string? dwellingUnitType)
        {
            switch (dwellingUnitType)
            {
                case "Single-Family Detached":
                    return "single-family detached";
                case "Townhouse/Rowhouse/Duplex":
                    return "multi-family - town homes";
                case "Multifamily Building Unit":
                    return "apartment unit";
                default:
                    return "manufactured home";
            }
        }
        public FoundationType GetFoundationType(string foundationType)
        {
            var type = new FoundationType();
            switch (foundationType)
            {
                case "Slab-on-grade foundation":
                    type.SlabOnGrade = new SlabOnGrade();
                    break;
                case "Unconditioned Basement":
                    type.Basement = new Basement()
                    {
                        Conditioned = false
                    };
                    break;
                case "Conditioned Basement":
                    type.Basement = new Basement()
                    {
                        Conditioned = true
                    };
                    break;
                case "Unvented Crawlspace / Unconditioned Garage":
                    type.Crawlspace = new Crawlspace()
                    {
                        Vented = false
                    };
                    break;
                case "Vented Crawlspace":
                    type.Crawlspace = new Crawlspace()
                    {
                        Vented = true
                    };
                    break;
                case "Belly and Wing":
                    type.BellyAndWing = new BellyAndWing();
                    break;
                case "Above Other Unit":
                    type.AboveApartment = new AboveApartment();
                    break;
            }
            return type;
        }
        public string GetExteriorAdjacentTo(string wallType)
        {
            switch (wallType)
            {
                case "Outside":
                    return "outside";
                case "Other Unit":
                    return "other housing unit";
                case "Other Heated Space":
                    return "other heated space";
                case "Other Non-Freezing Space":
                    return "other multifamily buffer space";
                case "Other Multi - Family Buffer Space":
                    return "other non-freezing space";
                default:
                    return "";
            }
        }
        public string GetDuctLocation(string ductLocation)
        {
            // TODO need to be add
            switch (ductLocation)
            {
                case "Conditioned space":
                    return "living space";
                case "Under slab":
                    return "under slab";
                case "Exterior wall":
                    return "exterior wall";
                case "Outside":
                    return "outside";
                case "Unconditioned Basement":
                    return "basement - unconditioned";
                case "Unconditioned Attic":
                    return "attic - unconditioned";
                case "Unvented Crawlspace / Unconditioned Garage":
                    return "garage";
                case "Vented Crawlspace":
                    return "crawlspace - vented";
                default:
                    return null;
            }
        }
        public int GetArrayTilt(string angle)
        {
            switch (angle)
            {
                case "Flat":
                    return 7;
                case "Low slope":
                    return 22;
                case "Medium slope":
                    return 37;
                case "Steep slope":
                    return 90;
                default:
                    return 0;
            }
        }
    }
}
