using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Restaurante.Web.Core;

namespace Restaurante.Web.Controllers.Api
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected static ObjectResult ControllerBasicValidation<T>(Response<T> response,
                                                                   ModelStateDictionary? modelState = null,
                                                                   int? statusCode = null)
        {
            if (modelState is not null && !modelState.IsValid)
            {
                return new ObjectResult(Response<T>.Failure("Debe ajustar los errores de validación"))
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }

            if (statusCode is >= 100 and <= 599)
            {
                return new ObjectResult(response) { StatusCode = statusCode };
            }

            return new ObjectResult(response)
            {
                StatusCode = response.IsSuccess ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest
            };
        }

        protected static ObjectResult ControllerBasicValidation(Response response,
                                                                ModelStateDictionary? modelState = null,
                                                                int? statusCode = null)
        {
            if (modelState is not null && !modelState.IsValid)
            {
                return new ObjectResult(Core.Response.Failure("Debe ajustar los errores de validación"))
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }

            if (statusCode is >= 100 and <= 599)
            {
                return new ObjectResult(response) { StatusCode = statusCode };
            }

            return new ObjectResult(response)
            {
                StatusCode = response.IsSuccess ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest
            };
        }
    }
}