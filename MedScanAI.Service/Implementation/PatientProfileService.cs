﻿using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SharedResponse;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Service.Implementation
{
    internal class PatientProfileService : IPatientProfileService
    {

        private readonly IChronicDiseasesRepository _chronicDiseasesRepository;
        private readonly ICurrentMedicationRepository _currentMedicationRepository;
        private readonly IPatientAllergiesRepository _patientAllergiesRepository;
        private readonly IPatientRepository _patientRepository;

        public PatientProfileService(IChronicDiseasesRepository chronicDiseasesRepository, ICurrentMedicationRepository currentMedicationRepository, IPatientAllergiesRepository patientAllergiesRepository, IPatientRepository patientRepository)
        {
            _chronicDiseasesRepository = chronicDiseasesRepository;
            _currentMedicationRepository = currentMedicationRepository;
            _patientAllergiesRepository = patientAllergiesRepository;
            _patientRepository = patientRepository;
        }

        public async Task<ReturnBase<bool>> CreatePatientProfileAsync(List<string> ChronicDiseases, List<string> CurrentMedication, List<string> Allergies, string patientId)
        {
            var transaction = await _chronicDiseasesRepository.BeginTransactionAsync();
            try
            {
                var chronicDiseases = ChronicDiseases
                    .Select(cd => new PatientChronicDisease { Name = cd, PatientId = patientId, Notes = "" }).ToList();

                var currentMedications = CurrentMedication
                    .Select(cm => new PatientCurrentMedication { Name = cm, PatientId = patientId }).ToList();

                var allergies = Allergies
                    .Select(a => new PatientAllergy { Name = a, PatientId = patientId }).ToList();

                var saveChronicDiseasesResult = await _chronicDiseasesRepository.AddRangeAsync(chronicDiseases);

                if (!saveChronicDiseasesResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(saveChronicDiseasesResult.Message);
                }

                var saveCurrentMedicationResult = await _currentMedicationRepository.AddRangeAsync(currentMedications);

                if (!saveCurrentMedicationResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(saveCurrentMedicationResult.Message);
                }

                var saveAllergiesResult = await _patientAllergiesRepository.AddRangeAsync(allergies);

                if (!saveAllergiesResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(saveAllergiesResult.Message);
                }

                await transaction.CommitAsync();
                return ReturnBaseHandler.Success(true, "Profile created successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ReturnBase<GetPatientProfileResponse>> GetPatientProfileAsync(string patientId)
        {
            try
            {
                var patient = await _patientRepository.GetTableNoTracking().Data
                    .FirstOrDefaultAsync(x => x.Id == patientId);

                var chronicDiseasesResult = await _chronicDiseasesRepository.GetTableNoTracking().Data.Where(x => x.PatientId == patientId).ToListAsync();

                var currentMedicationsResult = await _currentMedicationRepository.GetTableNoTracking().Data.Where(x => x.PatientId == patientId).ToListAsync();

                var allergiesResult = await _patientAllergiesRepository.GetTableNoTracking().Data.Where(x => x.PatientId == patientId).ToListAsync();

                var response = new GetPatientProfileResponse
                {
                    Id = patientId,
                    FullName = patient?.FullName ?? "",
                    Email = patient?.Email ?? "",
                    PhoneNumber = patient?.PhoneNumber ?? "",
                    ChronicDiseases = chronicDiseasesResult.Select(cd => cd.Name).ToList(),
                    CurrentMedication = currentMedicationsResult.Select(cm => cm.Name).ToList(),
                    Allergies = allergiesResult.Select(a => a.Name).ToList()
                };

                return ReturnBaseHandler.Success(response);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetPatientProfileResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
