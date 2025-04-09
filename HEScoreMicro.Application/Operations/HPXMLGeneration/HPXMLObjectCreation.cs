
using GenericController.Application.Mapper.Reply;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using HEScoreMicro.Application.HPXMLClasses;
using HEScoreMicro.Application.HPXMLClasses.ZoneRoofs;
using HEScoreMicro.Application.HPXMLClasses.ZoneFloors;
using HEScoreMicro.Application.HPXMLClasses.ZoneWalls;
using HEScoreMicro.Application.HPXMLClasses.Systems;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;

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
            List<Wall> walls = new List<Wall>();
            List<Attic> attics = new List<Attic>();
            List<Roof> roofs = new List<Roof>();
            List<Skylight> skylights = new List<Skylight>();
            List<Foundation> foundations = new List<Foundation>();
            List<Slab> slabs = new List<Slab>();
            List<FoundationWall> foundationWalls = new List<FoundationWall>();
            List<Window> windows = new List<Window>();

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
                this.GenerateZoneWallsObject(building.Data.ZoneWall, walls, building.Data.Address.DwellingUnitType);

            if (building.Data.ZoneWindow != null)
                this.GenerateZoneWindowObject(building.Data.ZoneWindow, windows, building.Data.ZoneWall.ExteriorWallSame);

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
                Building = new Building()
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
                        }
                    }
                }
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
                //xmlString = Regex.Replace(xmlString, @"<\w+\s+xsi:nil=""true""\s*/>", "");
            }
            return new ResponseDTO<string> { Data = xmlString, Failed = false, Message = "String Gernerated Successfully" };
        }

        public async Task<ResponseDTO<string>> GenerateHPXMLBase64String(string hpxmlString)
        {
            string base64HPXML = Convert.ToBase64String(Encoding.UTF8.GetBytes(hpxmlString));
            return new ResponseDTO<string> { Data = base64HPXML, Failed = false, Message = "String Gernerated Successfully" };
        }

        // Get Object Type response =========================================================================
        public Address GenerateAddressObject(Domain.Entity.AddressDTO addressDTO)
        {
            return new Address()
            {
                Address1 = addressDTO.StreetAddress,
                Address2 = addressDTO.AddressLine,
                CityMunicipality = addressDTO.City,
                AddressType = "street",
                StateCode = addressDTO.State,
                ZipCode = addressDTO.ZipCode
            };
        }

        public AirInfiltration GenerateAirInfiltrationObject(Domain.Entity.AboutDTO aboutDTO)
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
                    LeakinessDescription = (!aboutDTO.BlowerDoorTestConducted) ? ((aboutDTO.AirSealed == true) ? "tight" : "average") : null,
                },
                AirSealing = (aboutDTO.BlowerDoorTestConducted) ? null : new AirSealing()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = "AirSealing-1"
                    },
                },
            };
        }
        public void GenerateAtticsObject(Domain.Entity.ZoneRoofAttics.ZoneRoofDTO zoneRoofDTOs, List<Attic> attics, List<Floor> floors, List<Wall> walls, List<Roof> roofs, List<Skylight> skylights)
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
                if (type == "Unconditioned Attic")
                {
                    attic.AtticType = new AtticType { Attic = new AtticTypes() };
                    attic.AttachedToWall = (zoneRoofDTO.KneeWallPresent == true) ? new AttachedToWall()
                    {
                        IdRef = id + "-wall-1",
                    } : null;
                    this.GenerateAtticWallObject(zoneRoofDTO, walls, attic.AttachedToWall.IdRef, 0);

                    attic.AttachedToRoof = new AttachedToRoof()
                    {
                        IdRef = id + "-roof-1",
                    };
                    this.GenerateAtticRoofObject(zoneRoofDTO, roofs, skylights, attic.AttachedToRoof.IdRef, 0);

                    attic.AttachedToFloor = new AttachedToFloor()
                    {
                        IdRef = id + "-floor-1",
                    };
                    this.GenerateAtticFloorObject(zoneRoofDTO, floors, attic.AttachedToFloor.IdRef, 0);
                }
                else if (type == "Cathedral Ceiling")
                {
                    attic.AtticType = new AtticType { CathedralCeiling = new CathedralCeiling() };

                    attic.AttachedToRoof = new AttachedToRoof()
                    {
                        IdRef = id + "-roof-1",
                    };
                    this.GenerateAtticRoofObject(zoneRoofDTO, roofs, skylights, attic.AttachedToRoof.IdRef, 1);
                }
                else if (type == "Flat Roof")
                {
                    attic.AtticType = new AtticType { FlatRoof = new FlatRoof() };

                    attic.AttachedToRoof = new AttachedToRoof()
                    {
                        IdRef = id + "-roof-1",
                    };
                    this.GenerateAtticRoofObject(zoneRoofDTO, roofs, skylights, attic.AttachedToRoof.IdRef, 2);
                }
                attics.Add(attic);
                i++;
            }
        }
        public void GenerateAtticRoofObject(Domain.Entity.ZoneRoofAttics.RoofAtticDTO roofAtticDTO, List<Roof> roofs, List<Skylight> skylights, string idref, int type)
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
        public void GenerateAtticFloorObject(Domain.Entity.ZoneRoofAttics.RoofAtticDTO roofAtticDTO, List<Floor> floors, string idref, int type)
        {
            Floor floor = new Floor()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref + "-floor-1",
                },
                Area = roofAtticDTO.AtticFloorArea,
                Insulation = new Insulation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-floor-1-insulation-1"
                    },
                    AssemblyEffectiveRValue = roofAtticDTO.AtticFloorInsulation
                }
            };
            floors.Add(floor);
        }
        public void GenerateAtticWallObject(Domain.Entity.ZoneRoofAttics.RoofAtticDTO roofAtticDTO, List<Wall> walls, string idref, int type)
        {
            if (roofAtticDTO.KneeWallPresent == true)
            {
                Wall wall = new Wall()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-wall-1"
                    },
                    ExteriorAdjacentTo = "attic",
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
                            Id = idref + "-wall-1-insulation-1"
                        },
                        AssemblyEffectiveRValue = roofAtticDTO.KneeWallInsulation,
                    }
                };
                walls.Add(wall);
            }
        }
        public void GenerateSkylightObject(Domain.Entity.ZoneRoofAttics.RoofAtticDTO roofAtticDTO, List<Skylight> skylights, string idref)
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
        public void GenerateFoundationFloorsObject(Domain.Entity.ZoneFloorDTO zoneFloorDTO, List<Floor> floors, List<Foundation> foundations, List<Slab> slabs, List<FoundationWall> foundationWalls)
        {
            int i = 1;
            foreach (var zoneFloor in zoneFloorDTO.Foundations)
            {

                var id = "foundation-" + i;
                Foundation foundation = new Foundation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                    FoundationType = GetFoundationType(zoneFloor.FoundationType),
                };
                if (zoneFloor.FoundationType == "Slab-on-grade foundation")
                {
                    foundation.AttachedToSlab = new AttachedToSlab()
                    {
                        IdRef = id + "-slab-1"
                    };
                    this.GenerateSlabObject(zoneFloor, slabs, foundation.AttachedToSlab.IdRef);
                }
                else if (zoneFloor.FoundationType == "Belly and Wing")
                {
                    foundation.AttachedToFloor = new AttachedToFloor()
                    {
                        IdRef = id + "-floor-1"
                    };
                    this.GenerateFoundationFloorObject(zoneFloor, floors, foundation.AttachedToFloor.IdRef);
                }
                else
                {
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
                }
                foundations.Add(foundation);
                i++;
            }
        }
        public void GenerateSlabObject(Domain.Entity.FoundationDTO zoneFloor, List<Slab> slabs, string idref)
        {
            Slab slab = new Slab()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref
                },
                //Area = zoneFloor.FoundationArea,
                PerimeterInsulation = new PerimeterInsulation()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = idref + "-perimeter-insulation-1"
                    },
                    AssemblyEffectiveRValue = zoneFloor.SlabInsulationLevel,
                }
            };
            slabs.Add(slab);
        }
        public void GenerateFoundationWallObject(Domain.Entity.FoundationDTO zoneFloor, List<FoundationWall> foundationWalls, string idref)
        {
            FoundationWall foundationWall = new FoundationWall()
            {
                SystemIdentifier = new SystemIdentifier
                {
                    Id = idref
                },
                //Area = zoneFloor.FoundationArea,
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
        public void GenerateFoundationFloorObject(Domain.Entity.FoundationDTO zoneFloor, List<Floor> floors, string idref)
        {
            Floor floor = new Floor()
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
                    AssemblyEffectiveRValue = zoneFloor.FloorInsulationLevel,
                }
            };
            floors.Add(floor);
        }
        public void GenerateZoneWallsObject(Domain.Entity.ZoneWalls.ZoneWallDTO zoneWallDTO, List<Wall> walls, string buildingType)
        {
            int i = 1;
            foreach (var zoneWall in zoneWallDTO.Walls)
            {
                var id = "wall-" + i;

                Wall wall = new Wall()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                    Insulation = new Insulation()
                    {
                        SystemIdentifier = new SystemIdentifier
                        {
                            Id = id + "-insulation-1"
                        },
                        AssemblyEffectiveRValue = zoneWall.WallInsulationLevel,
                    },
                    InteriorAdjacentTo = "living space",
                    ExteriorAdjacentTo = this.GetExteriorAdjacentTo(buildingType)
                };

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
                else if (zoneWall.Construction == "Wood Frame with Optimum Value Engineering(OVE)")
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
                    // TODO Need to clarify
                }

                //Wall Siding
                if (zoneWall.ExteriorFinish == "Wood Siding, Fiber Cement, Composite Shingle, or Masonite Siding")
                {
                    wall.Siding = "wood siding";
                }
                else if (zoneWall.ExteriorFinish == "Stucco")
                {
                    wall.Siding = "stucco";
                }
                else if (zoneWall.ExteriorFinish == "Vinyl Siding")
                {
                    wall.Siding = "vinyl siding";
                }
                else if (zoneWall.ExteriorFinish == "Aluminum Siding")
                {
                    wall.Siding = "aluminum siding";
                }
                else if (zoneWall.ExteriorFinish == "Brick Veneer")
                {
                    wall.Siding = "brick veneer";
                }
                else
                {
                    wall.Siding = "none";
                }
                walls.Add(wall);
                i++;
            }
        }
        public void GenerateZoneWindowObject(Domain.Entity.ZoneWindows.ZoneWindowDTO zoneWindowDTO,List<Window> windows, bool? singleWall)
        {
            int i = 1;
            var listWindowArea = new List<double?>()
            {
                zoneWindowDTO.WindowAreaFront,zoneWindowDTO.WindowAreaBack,
                zoneWindowDTO.WindowAreaRight,zoneWindowDTO.WindowAreaLeft
            };
            foreach(var zoneWindow in zoneWindowDTO.Windows)
            {
                var id = "window-" + i;
                Window window = new Window()
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = id
                    },
                    Area = listWindowArea[i - 1],
                    AttachedToWall = new AttachedToWall()
                    {
                        IdRef = singleWall == true ? "wall-1" : "wall-" + i
                    },
                };
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

        public WaterHeating GenerateWaterHeater(Domain.Entity.WaterHeaterDTO waterHeater)
        {
            /*Electric Storage*/
            /*Natural Gas Storage*/
            /*Propane(LPG) Storage*/
            /*Oil Storage*/
            /*Electric Instantaneous*/
            /*Gas Instantaneous*/
            /*Propane Instantaneous*/
            /*Oil Instantaneous*/
            /*Electric Heat Pump*/
            return new WaterHeating
            {
                WaterHeatingSystem = new WaterHeatingSystem
                {
                    SystemIdentifier = new SystemIdentifier
                    {
                        Id = "water-heater-1"
                    },
                }
            };
        }
        public Photovoltaics GeneratePhotovoltaics(Domain.Entity.PVSystemDTO pvSystem)
        {
            if (pvSystem == null || pvSystem.HasPhotovoltaic == false)
            {
                return null;
            }

            var pv = new Photovoltaics()
            {
                PVSystem = new PVSystem()
                {
                    ArrayOrientation = pvSystem.DirectionPanelsFace,
                    ArrayTilt = this.GetArrayTilt(pvSystem.AnglePanelsAreTilted),
                    YearInstalled = pvSystem.YearInstalled,
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
        public string GetEventType(Domain.Entity.AddressDTO addressDTO)
        {
            switch (addressDTO.AssessmentType)
            {
                case "Test":
                    return "construction-period testing/daily test out";
                case "preconstruction":
                    return "preconstruction";
                default:
                    return "initial";
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
            }
            return type;
        }
        public string GetExteriorAdjacentTo(string buildingType)
        {
            switch (buildingType)
            {
                case "apartment unit":
                    return "other housing unit";
                default:
                    return "outside";
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
