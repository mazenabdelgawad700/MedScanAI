﻿using MedScanAI.Domain.Entities;
using MedScanAI.Domain.IBaseRepository;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;
using MedScanAI.Shared.SharedResponse;

namespace MedScanAI.Infrastructure.Abstracts
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> GetDoctorsForAppointmentsAsync();
        Task<ReturnBase<List<GetTodayAppointmentsResponse>>> GetTodayAppointmentsAsync();
        Task<ReturnBase<bool>> ConfirmAppointmentAsync(int appointmentId);
    }
}
