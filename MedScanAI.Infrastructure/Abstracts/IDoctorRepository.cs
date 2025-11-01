﻿using MedScanAI.Domain.Entities;
using MedScanAI.Domain.IBaseRepository;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SharedResponse;

namespace MedScanAI.Infrastructure.Abstracts
{
    public interface IDoctorRepository : IBaseRepository<Doctor>
    {
        Task<ReturnBase<GetDoctorAppointmentsAndDoctorInfoResponse>> GetDoctorAppointmentsAndDoctorInfoAsync(string doctorId);
    }
}
