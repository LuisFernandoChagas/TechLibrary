using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Filters;

public class ExceptionFilters : IExceptionFilter {
    public void OnException(ExceptionContext context) {
        // Check if the exception is a TechLibraryException
        if(context.Exception is TechLibraryException techLibraryException) {
            // Set the status code from the exception
            context.HttpContext.Response.StatusCode = (int) techLibraryException.GetStatusCode();

            context.Result = new ObjectResult(new ResponseErrorMessagesJson {
                ErrorMessages = techLibraryException.GetErrorMessages()
            });
        } else {
            // Set the status code 500 (Internal Server Error)
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new ResponseErrorMessagesJson {
                //ErrorMessages = ["Erro desconhecido"]
                ErrorMessages = [context.Exception.Message]
            });
        }
        ;
    }
}
