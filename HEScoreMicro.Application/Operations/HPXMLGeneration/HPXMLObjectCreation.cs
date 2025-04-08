
using GenericController.Application.Mapper.Reply;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using HEScoreMicro.Application.HPXMLClasses;
using HEScoreMicro.Application.HPXMLClasses.ZoneRoofs;
using HEScoreMicro.Domain.Entity.ZoneRoofAttics;
using HEScoreMicro.Application.HPXMLClasses.ZoneFloors;
using HEScoreMicro.Application.HPXMLClasses.ZoneWalls;
using System.ComponentModel;
using System.Numerics;
using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;

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
            List<Attic> attics = new List<Attic>();
            List<Floor> floors = new List<Floor>();
            List<Wall> walls = new List<Wall>();
            List<Roof> roofs = new List<Roof>();
            List<Skylight> skylights = new List<Skylight>();

            this.GenerateAtticsObject(building.Data.ZoneRoof, attics, floors, walls, roofs, skylights);

            if (building.Failed)
            {
                return new ResponseDTO<HPXML>
                {
                    Failed = true,
                    Message = "Building not found."
                };
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
                            }
                        },
                        Enclosure = new Enclosure()
                        {
                            AirInfiltration = this.GenerateAirInfiltrationObject(building.Data.About),
                            Attics = new Attics
                            {
                                Attic = attics
                            },
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
        public void GenerateAtticsObject(ZoneRoofDTO zoneRoofDTOs, List<Attic> attics, List<Floor> floors, List<Wall> walls, List<Roof> roofs, List<Skylight> skylights)
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
                    attic.AttachedToWall = new AttachedToWall()
                    {
                        IdRef = id + "-wall-1",
                    };
                    this.GenerateAtticWallObject(zoneRoofDTO, walls, attic.AttachedToWall.IdRef,0);

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
                i++;
            }
        }
        public void GenerateAtticRoofObject(RoofAtticDTO roofAtticDTO, List<Roof> roofs, List<Skylight> skylights, string idref,int type)
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
            if (roofAtticDTO.RoofColor == "White") { 
                roof.RoofColor = "reflective";
            }
            else if (roofAtticDTO.RoofColor == "Light") {
                roof.RoofColor = "light";
            }
            else if (roofAtticDTO.RoofColor == "Medium") {
                roof.RoofColor = "medium";
            }
            else if (roofAtticDTO.RoofColor == "Medium Dark") {
                roof.RoofColor = "medium dark";
            }
            else if (roofAtticDTO.RoofColor == "Dark") {
                roof.RoofColor = "dark";
            }
            else if (roofAtticDTO.RoofColor == "Cool Color") {
                roof.SolarAbsorptance = roofAtticDTO.Absorptance;
            }
            else{
                roof.RoofColor = null;
            }

            // Roof Area and Roof Insulation for type = 0
            if(type == 0 || type == 2)
            {
                roof.Area = roofAtticDTO.RoofArea;
                roof.Insulation.AssemblyEffectiveRValue = roofAtticDTO.RoofInsulation;
            }else if (type == 1)
            {
                roof.Area = roofAtticDTO.CathedralCeilingArea;
                roof.Insulation.AssemblyEffectiveRValue = roofAtticDTO.CathedralCeilingInsulation;
            }

            // Roof Skylights
            if (roofAtticDTO.SkylightsPresent == true)
            {
                GenerateSkylightObject(roofAtticDTO, skylights, idref);
            }

            roofs.Add(roof);
        }
        public void GenerateAtticFloorObject(RoofAtticDTO roofAtticDTO, List<Floor> floors, string idref,int type)
        {

        }
        public void GenerateAtticWallObject(RoofAtticDTO roofAtticDTO, List<Wall> walls, string idref, int type)
        {

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
            if(roofAtticDTO.KnowSkylightSpecification == true)
            {
                skylight.SHGC = roofAtticDTO.SHGC;
                skylight.UFactor = roofAtticDTO.UFactor;
            }
            else
            {
                if(roofAtticDTO.FrameMaterial == "Aluminum")
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
                        else if(roofAtticDTO.GlazingType == "Insulating low-E, argon gas fill")
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
                    else if(roofAtticDTO.Panes == "Triple-Pane")
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


        // Get Premitive Type response ======================================================================
        public string GetEventType(Domain.Entity.AddressDTO addressDTO)
        {
            if (addressDTO.AssessmentType == "Test") return "construction-period testing/daily test out";
            else if (addressDTO.AssessmentType == "preconstruction") return "preconstruction";
            else return "initial";
        }
        public string GetResidentialFacilityType(string? dwellingUnitType)
        {
            if (dwellingUnitType == "Single-Family Detached") return "single-family detached";
            else if (dwellingUnitType == "Townhouse/Rowhouse/Duplex") return "multi-family - town homes";
            else if (dwellingUnitType == "Multifamily Building Unit") return "apartment unit";
            else return "manufactured home";
        }

    }
}
