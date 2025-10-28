﻿using MedScanAI.API.Base;
using MedScanAI.Core.Features.Doctor.Command.Model;
using MedScanAI.Core.Features.Doctor.Query.Model;
using MedScanAI.Shared.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedScanAI.API.Controllers
{
    [Route("api/doctor/[action]")]
    public class DoctorController : AppControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> DeleteDoctor([FromBody] DeleteDoctorCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> RestoreDoctor([FromBody] RestoreDoctorCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return ReturnResult(response);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDoctorsQuery query)
        {
            var response = await Mediator.Send(query);
            return ReturnResult(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActive([FromQuery] GetAllActiveDoctorsQuery query)
        {
            var response = await Mediator.Send(query);
            return ReturnResult(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActiveCount([FromQuery] GetActiveDoctorsCountQuery query)
        {
            var response = await Mediator.Send(query);
            return ReturnResult(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCount([FromQuery] GetAllDoctorsCountQuery query)
        {
            var response = await Mediator.Send(query);
            return ReturnResult(response);
        }
    }
}
