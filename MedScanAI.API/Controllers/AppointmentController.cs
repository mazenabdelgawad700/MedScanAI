﻿using MedScanAI.API.Base;
using MedScanAI.Core.Features.AppointmentFeature.Command.Model;
using MedScanAI.Core.Features.AppointmentFeature.Query.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedScanAI.API.Controllers
{
    [Route("api/appointment/[action]")]
    public class AppointmentController : AppControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> MakeAppointment([FromBody] MakeAppointmentCommand command)
        {
            var result = await Mediator.Send(command);
            return ReturnResult(result);
        }

        [HttpGet]
        [Authorize(Roles = "Patient, Admin")]
        public async Task<IActionResult> GetDoctors([FromQuery] GetDoctorsForAppointmentsQuery query)
        {
            var result = await Mediator.Send(query);
            return ReturnResult(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetForToday([FromQuery] GetTodayAppointmentsQuery query)
        {
            var result = await Mediator.Send(query);
            return ReturnResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmAppointmentCommand command)
        {
            var result = await Mediator.Send(command);
            return ReturnResult(result);
        }
    }
}
