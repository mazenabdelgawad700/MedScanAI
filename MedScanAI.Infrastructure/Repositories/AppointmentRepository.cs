﻿using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;
using MedScanAI.Shared.SharedResponse;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Doctor> _doctors;
        private readonly DbSet<Appointment> _appointments;
        public AppointmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _doctors = _dbContext.Set<Doctor>();
            _appointments = _dbContext.Set<Appointment>();
        }

        public async Task<ReturnBase<bool>> ConfirmAppointmentAsync(int appointmentId)
        {
            try
            {
                var appointment = await _appointments.Where(x => x.Id == appointmentId).FirstOrDefaultAsync();

                if (appointment is null)
                    return ReturnBaseHandler.Failed<bool>("Failed to retrieve appointment");

                appointment.Status = "Confirmed";

                _appointments.Update(appointment);
                await _dbContext.SaveChangesAsync();

                return ReturnBaseHandler.Success(true, "Appointment confirmed successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> GetDoctorsForAppointmentsAsync()
        {
            try
            {
                var today = DateTime.Now.DayOfWeek.ToString().ToLower();
                var todayDate = DateTime.Today;
                var currentTime = DateTime.Now.TimeOfDay;

                // Get all doctors who are active and available right now
                var doctors = await _dbContext.Doctors
                    .Include(d => d.Specialization)
                    .Include(d => d.Schedules)
                    .Where(d => d.IsActive)
                    .Where(d => d.Schedules.Any(s =>
                        s.DayOfWeek == today &&
                        s.IsAvailable &&
                        s.StartTime <= currentTime &&
                        s.EndTime >= currentTime))
                    .ToListAsync();

                var doctorResponses = new List<GetDoctorsForAppointmentsResponse>();

                foreach (var doctor in doctors)
                {
                    // Get the doctor's schedule for today
                    var schedule = doctor.Schedules.FirstOrDefault(s => s.DayOfWeek == today && s.IsAvailable);
                    if (schedule == null) continue;

                    // Get all appointments for today for this doctor
                    var todaysAppointments = await _dbContext.Appointments
                        .Where(a => a.DoctorId == doctor.Id && a.Date.Date == todayDate)
                        .OrderBy(a => a.Date)
                        .ToListAsync();

                    // Find last appointment end time (if any)
                    TimeSpan lastAppointmentEnd = schedule.StartTime;

                    if (todaysAppointments.Any())
                    {
                        var lastAppointment = todaysAppointments.Last();
                        lastAppointmentEnd = lastAppointment.Date.TimeOfDay;
                    }

                    // Generate available start times between lastAppointmentEnd and schedule.EndTime
                    // Example: 30-minute intervals
                    var availableTimes = new List<string>();
                    var slotLength = TimeSpan.FromMinutes(30); // you can make this configurable
                    var nextAvailable = lastAppointmentEnd;

                    if (nextAvailable < currentTime)
                        nextAvailable = currentTime;

                    while (nextAvailable.Add(slotLength) <= schedule.EndTime)
                    {
                        bool isBooked = todaysAppointments.Any(a =>
                            a.Date.TimeOfDay == nextAvailable ||
                            (a.Date.TimeOfDay < nextAvailable.Add(slotLength) && a.Date.TimeOfDay > nextAvailable)
                        );

                        if (!isBooked)
                        {
                            // Convert to 12-hour format with AM/PM for Egypt
                            availableTimes.Add(DateTime.Today
                                .Add(nextAvailable)
                                .ToString("hh:mm tt", new System.Globalization.CultureInfo("en-EG")));
                        }

                        nextAvailable = nextAvailable.Add(slotLength);
                    }


                    doctorResponses.Add(new GetDoctorsForAppointmentsResponse
                    {
                        Id = doctor.Id,
                        FullName = doctor.FullName,
                        Specialization = doctor.Specialization?.Name,
                        YearsOfExperience = doctor.YearsOfExperience,
                        AvailableStartTimes = availableTimes
                    });
                }

                return ReturnBaseHandler.Success(doctorResponses);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<List<GetDoctorsForAppointmentsResponse>>(
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<List<GetTodayAppointmentsResponse>>> GetTodayAppointmentsAsync()
        {
            try
            {
                var today = DateTime.Today;

                var todayAppointments = await _dbContext.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .Where(a => a.Date.Date == today)
                    .Select(a => new GetTodayAppointmentsResponse
                    {
                        Id = a.Id,
                        Time = a.Date.ToString("hh:mm tt", new System.Globalization.CultureInfo("en-EG")),
                        DoctorName = a.Doctor != null ? a.Doctor.FullName : "غير محدد",
                        PatientName = a.Patient != null
                                    ? a.Patient.FullName
                                    : (string.IsNullOrEmpty(a.PatientName) ? "زائر" : a.PatientName),
                        Status = a.Status
                    })
                    .ToListAsync();

                return ReturnBaseHandler.Success(todayAppointments);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<List<GetTodayAppointmentsResponse>>(
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

    }
}