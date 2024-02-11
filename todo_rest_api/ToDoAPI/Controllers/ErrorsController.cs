using Microsoft.AspNetCore.Mvc;
using System.Net;
using TodoAPI.Errors;

namespace Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/errors")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        // Handles errors and exceptions
        [Route("{code}")]
        public ActionResult Error(int code)
        {
            HttpStatusCode parsedCode = (HttpStatusCode)code;
            var error = new MainError(code, parsedCode.ToString());

            return new ObjectResult(error);
        }
    }
}