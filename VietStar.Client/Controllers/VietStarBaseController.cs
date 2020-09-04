using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    public class VietStarBaseController : Controller
    {
        public readonly CurrentProcess _process;
        public readonly IMapper _mapper;
        public VietStarBaseController(CurrentProcess process, IMapper mapper = null)
        {
            _process = process;
            _mapper = mapper;
            
        }
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new DefaultContractResolver()
        };

        public IActionResult ToResponse()
        {
            var model = new ResponseJsonModel();
            bool hasError = _checkHasError(model);
            return Json(model, _jsonSerializerSettings);
        }
        public IActionResult ToResponse<T>(T data)
        {
            var model = new ResponseJsonModel<T>();
            if (!_checkHasError(model))
                model.data = data;

            return Json(model, _jsonSerializerSettings);
        }
        public IActionResult ToResponse(bool isSuccess)
        {
            var model = new ResponseJsonModel();

            if (!_checkHasError(model))
                model.success = isSuccess;

            return Json(model, _jsonSerializerSettings);
        }
        private bool _checkHasError(ResponseJsonModel model)
        {
            var hasError = _process.HasError;
            if (hasError)
            {
                var errorMessage = _process.ToError();

                model.error = new ErrorJsonModel()
                {
                    result = false,
                    code = errorMessage.Message,
                    trace_keys = errorMessage.TraceKeys
                };
            }
            model.success = !hasError;
            return hasError;
        }
    }
    public class ResponseJsonModel
    {
        public ErrorJsonModel error { get; set; }
        public bool success { get; set; }
    }

    public class ResponseJsonModel<T> : ResponseJsonModel
    {
        public T data { get; set; }
    }
    public class ErrorJsonModel
    {
        public string code { get; set; }
        public List<object> trace_keys { get; set; }
        public bool result { get; set; }
    }
}
