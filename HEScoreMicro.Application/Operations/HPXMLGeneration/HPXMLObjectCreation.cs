
using EnergyScore.Application.Templates.HPXMLs;
using GenericController.Application.Mapper.Reply;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Data.SqlTypes;

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
                                ResidentialFacilityType = this.GetResidentialFacilityType(building.Data.Address.DwellingUnitType)
                            }
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
            return new ResponseDTO<string> { Data = xmlString, Failed=false, Message = "String Gernerated Successfully" };
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

        // Get Premitive Type response ======================================================================
        public string GetEventType(Domain.Entity.AddressDTO addressDTO)
        {
            if(addressDTO.AssessmentType == "Test") return "construction-period testing/daily test out";
            else if(addressDTO.AssessmentType == "preconstruction") return "preconstruction";
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
