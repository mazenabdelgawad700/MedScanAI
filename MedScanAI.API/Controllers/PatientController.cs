﻿using MedScanAI.API.Base;
using MedScanAI.Core.Features.PatientFeature.Command.Model;
using MedScanAI.Core.Features.PatientFeature.Query.Model;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;
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

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetProfile([FromBody] GetPatientProfileQuery query)
        {
            ReturnBase<GetPatientProfileResponse> response = await Mediator.Send(query);
            return ReturnResult(response);
        }
    }
}
