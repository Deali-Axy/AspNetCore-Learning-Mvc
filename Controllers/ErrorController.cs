using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyManagement.ViewModels;

namespace StudyManagement.Controllers
{
    public class ErrorController : Controller
    {
        private ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this._logger = logger;
        }

        [Route("status-code/{statusCode}")]
        public IActionResult StatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var viewModel = new StatusCodeViewModel
            {
                Code = statusCode,
                Path = statusCodeResult.OriginalPath,
                QueryString = statusCodeResult.OriginalQueryString,
            };

            _logger.LogWarning(viewModel.ToString());

            switch (statusCode)
            {
                case 404:
                    viewModel.Message = "页面未找到";
                    break;
            }

            return View("StatusCode", viewModel);
        }

        [AllowAnonymous]
        [Route("exception")]
        public IActionResult ExceptionHandler()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var viewModel = new ExceptionViewModel
            {
                Path = exception.Path,
                Message = exception.Error.Message,
                StackTrace = exception.Error.StackTrace,
            };

            _logger.LogError(viewModel.ToString());

            return View("Exception", viewModel);
        }

        public string Test()
        {
            throw new Exception("test exception");
        }
    }
}