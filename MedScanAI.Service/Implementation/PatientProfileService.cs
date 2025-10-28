using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;

namespace MedScanAI.Service.Implementation
{
    internal class PatientProfileService : IPatientProfileService
    {

        private readonly IChronicDiseasesRepository _chronicDiseasesRepository;
        private readonly ICurrentMedicationRepository _currentMedicationRepository;
        private readonly IPatientAllergiesRepository _patientAllergiesRepository;

        public PatientProfileService(IChronicDiseasesRepository chronicDiseasesRepository, ICurrentMedicationRepository currentMedicationRepository, IPatientAllergiesRepository patientAllergiesRepository)
        {
            _chronicDiseasesRepository = chronicDiseasesRepository;
            _currentMedicationRepository = currentMedicationRepository;
            _patientAllergiesRepository = patientAllergiesRepository;
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
    }
}
