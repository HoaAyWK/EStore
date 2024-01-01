using EStore.Api.Common.ApiRoutes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route(ApiRoutes.Error)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        return Problem(title: exception?.Message);
    }
}
