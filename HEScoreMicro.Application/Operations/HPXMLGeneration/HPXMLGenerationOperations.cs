
using GenericController.Application.Mapper.Reply;
using HEScoreMicro.Application.Reply;
using HEScoreMicro.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceReference1;

namespace HEScoreMicro.Application.Operations.HPXMLGeneration
{
    public interface IHPXMLGenerationOperations
    {
        Task<ResponseDTO<string>> GetHPXMLString(Guid Id);
        Task<ResponseDTO<string>> GetBase64HPXMLString(Guid Id);
        Task<ResponseDTO<int>> SubmitInputs(Guid Id);
        Task<ValidationDTO> ValidateInputs(Guid Id);
        Task<ResponseDTO<List<Object>>> GeneratePDF(Guid Id);
        Task<ResponseDTO<BuildingDTO>> ClearOldPdfNumber(Guid Id);
    }
    public class HPXMLGenerationOperations : IHPXMLGenerationOperations
    {
        private readonly IConfiguration _configuration;
        private readonly IHPXMLObjectCreation _hPXMLObjectCreation;
        private readonly st_api_handlerPortClient _st_Api_HandlerPort;
        private readonly IBuildingOperations _buildingOperations;
        public HPXMLGenerationOperations(IHPXMLObjectCreation hPXMLObjectCreation, st_api_handlerPortClient st_Api_HandlerPort,
            IConfiguration configuration, IBuildingOperations buildingOperations)
        {
            _st_Api_HandlerPort = st_Api_HandlerPort;
            _hPXMLObjectCreation = hPXMLObjectCreation;
            _configuration = configuration;
            _buildingOperations = buildingOperations;
        }
        public async Task<ResponseDTO<string>> GetHPXMLString(Guid Id)
        {
            var obj = await _hPXMLObjectCreation.CreateHPXMLObject(Id);
            if (obj.Failed)
            {
                return new ResponseDTO<string>
                {
                    Failed = true,
                    Message = "HPXML object creation failed."
                };
            }
            var hpxmlString = await _hPXMLObjectCreation.GenerateHPXMLString(obj.Data);
            if (hpxmlString.Failed)
            {
                return hpxmlString;
            }
            return new ResponseDTO<string>
            {
                Failed = false,
                Message = "HPXML string generated successfully.",
                Data = hpxmlString.Data
            };
        }
        public async Task<ResponseDTO<string>> GetBase64HPXMLString(Guid Id)
        {
            var obj = await GetHPXMLString(Id);
            if (obj.Failed)
            {
                return obj;
            }
            var base64Stringres = await _hPXMLObjectCreation.GenerateHPXMLBase64String(obj.Data);
            if (base64Stringres.Failed)
            {
                return base64Stringres;
            }
            return new ResponseDTO<string>
            {
                Failed = false,
                Message = "HPXML string generated successfully.",
                Data = base64Stringres.Data
            };
        }
        public async Task<ResponseDTO<string>> GetSessionToken()
        {
            // Call the API to get the session token
            get_session_token get_Session_Token = new get_session_token()
            {
                user_key = _configuration.GetConnectionString("ApiKey"),
                password = _configuration.GetConnectionString("Password"),
                user_name = _configuration.GetConnectionString("UserName"),
            };
            var val = await _st_Api_HandlerPort.get_session_tokenAsync(get_Session_Token);
            return new ResponseDTO<string>
            {
                Failed = false,
                Message = "Session token generated successfully.",
                Data = val.get_session_token_result.session_token
            };
        }
        public async Task<Response> DestorySessionToken(string sessionToken)
        {
            destroy_session_token destroy_Session_Token = new destroy_session_token()
            {
                session_token = sessionToken,
                user_key = _configuration.GetConnectionString("ApiKey")
            };
            var res = await _st_Api_HandlerPort.destroy_session_tokenAsync(destroy_Session_Token);
            if (res.destroy_session_token_result.result != "OK")
                return new Response
                {
                    Failed = true,
                    Message = "Session token not destroyed.",
                };
            return new Response
            {
                Failed = false,
                Message = "Session token destroyed successfully.",
            };
        }
        public async Task<ResponseDTO<int>> SubmitInputs(string sessionToken, string base64HPXML)
        {
            // Call the API to submit the inputs
            submit_hpxml_inputs submit_Hpxml_InputsRequest = new submit_hpxml_inputs()
            {
                session_token = sessionToken,
                hpxml = base64HPXML,
                user_key = _configuration.GetConnectionString("ApiKey")
            };
            var res = await _st_Api_HandlerPort.submit_hpxml_inputsAsync(submit_Hpxml_InputsRequest);
            return new ResponseDTO<int>
            {
                Data = res.submit_hpxml_inputs_result.building_id,
                Failed = !(res.submit_hpxml_inputs_result.result == create_building_responseResult.OK),
                Message = res.submit_hpxml_inputs_result.message,
            };
        }
        public async Task<ResponseDTO<int>> SubmitInputs(Guid Id)
        {
            var res = await ValidateInputs(Id);
            if (res.Failed)
            {
                return new ResponseDTO<int>
                {
                    Failed = true,
                    Message = "Validation Failed."
                };
            }
            var sessionToken = await GetSessionToken();
            if (sessionToken.Failed)
            {
                return new ResponseDTO<int>
                {
                    Failed = true,
                    Message = "Session token generation failed."
                };
            }
            var base64HPXML = await GetBase64HPXMLString(Id);
            if (base64HPXML.Failed)
            {
                return new ResponseDTO<int>
                {
                    Failed = true,
                    Message = $"{base64HPXML.Message}"
                };
            }
            var submitInputRes = await SubmitInputs(sessionToken.Data, base64HPXML.Data);
            if (!submitInputRes.Failed)
            {
                await _buildingOperations.UpdateBuildingNumber(Id, submitInputRes.Data);
            }
            return submitInputRes;
        }
        public async Task<ValidationDTO> ValidateInputs(string hpxmlBase64String)
        {
            var req = new files
            {
                user_key = _configuration.GetConnectionString("ApiKey"),
                file1 = hpxmlBase64String,
            };
            var res = await _st_Api_HandlerPort.validate_hpxmlAsync(req);
            var errorList = new List<Object>();
            if (res.validate_hpxml_result.home_json == null)
            {
                foreach (var message in res.validate_hpxml_result.validation_message)
                {
                    errorList.Add(new
                    {
                        Message = message.message,
                        Type = message.type,
                        Field = message.field,
                    });
                }
            }

            return new ValidationDTO
            {
                Failed = (errorList.Count() != 0),
                Errors = errorList,
                HomeJson = res.validate_hpxml_result.home_json
            };
        }
        public async Task<ValidationDTO> ValidateInputs(Guid Id)
        {
            var obj = await GetBase64HPXMLString(Id);
            if (obj.Failed || obj.Data == "" || obj.Data == null)
            {
                return new ValidationDTO() { Failed = true, Message = obj.Message };
            }
            return await ValidateInputs(obj.Data);
        }
        public async Task<ResponseDTO<List<Object>>> GeneratePDF(int buildingId, string sessionToken)
        {
            building_label building_Label = new building_label()
            {
                building_id = buildingId,
                session_token = sessionToken,
                user_key = _configuration.GetConnectionString("ApiKey"),
            };
            var res = await _st_Api_HandlerPort.generate_labelAsync(building_Label);
            if (res.building_label_result.result != building_label_resultResult.OK)
            {
                return new ResponseDTO<List<Object>> { Failed = false, Message = res.building_label_result.message };
            }
            var fileObjects = new List<Object>();
            foreach (var fileObj in res.building_label_result.file)
            {
                fileObjects.Add(new
                {
                    Link = fileObj.url,
                    Type = fileObj.type,
                });
            }
            return new ResponseDTO<List<Object>>
            {
                Failed = false,
                Message = "PDF Generated Successfully",
                Data = fileObjects
            };
        }
        public async Task<ResponseDTO<List<Object>>> GeneratePDF(Guid Id)
        {
            var sessionToken = await GetSessionToken();
            if (sessionToken.Failed)
            {
                return new ResponseDTO<List<Object>>
                {
                    Failed = true,
                    Message = "Session token generation failed."
                };
            }
            var building = await _buildingOperations.GetById(Id);
            if (building.Failed)
            {
                return new ResponseDTO<List<Object>>
                {
                    Failed = true,
                    Message = "Building not found."
                };
            }
            var buildingNumber = building.Data.Number;
            if (buildingNumber == null)
            {
                var submitRes = await SubmitInputs(Id);
                if (submitRes.Failed)
                {
                    return new ResponseDTO<List<Object>>
                    {
                        Failed = true,
                        Message = "Building Number generation failed."
                    };
                }
                buildingNumber = submitRes.Data;
            }

            var res = await GeneratePDF((int)buildingNumber, sessionToken.Data);
            await DestorySessionToken(sessionToken.Data);
            return res;
        }
        public async Task<ResponseDTO<BuildingDTO>> ClearOldPdfNumber(Guid Id)
        {
            Building building = new Building()
            {
                Id = Id,
                Number = null,
            };
            var res = await _buildingOperations.Update(building);
            return res;
        }
    }
}
