using MedScanAI.API.Base;
using MedScanAI.Core.Features.Authentication.Command.Model;
using MedScanAI.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedScanAI.API.Controllers
{
    [Route("api/authentication/[action]")]
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordEmail([FromBody] SendResetPasswordEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
    }
}
