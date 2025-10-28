using MedScanAI.API.Base;
using MedScanAI.Core.Features.PatientFeature.Command.Model;
using MedScanAI.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedScanAI.API.Controllers
{
    [Route("api/patient/[action]")]
    public class PatientController : AppControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateProfile([FromBody] CreatePatientProfileCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }
    }
}
