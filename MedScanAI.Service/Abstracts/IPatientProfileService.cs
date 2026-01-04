using MedScanAI.Domain.Entities;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SharedResponse;

namespace MedScanAI.Service.Abstracts
{
    public interface IPatientProfileService
    {
        Task<ReturnBase<bool>> CreatePatientProfileAsync(List<string> ChronicDiseases, List<string> CurrentMedication, List<string> Allergies, string patientId);

        Task<ReturnBase<GetPatientProfileResponse>> GetPatientProfileAsync(string patientId);
        Task<ReturnBase<bool>> UpdatePatientProfileAsync(Patient patient);
        Task<ReturnBase<bool>> UpdatePatientAllergyAsync(PatientAllergy patientAllergy);
        Task<ReturnBase<bool>> UpdatePatientChronicDiseaseAsync(PatientChronicDisease patientChronicDisease);
        Task<ReturnBase<bool>> UpdatePatientCurrentMedicationAsync(PatientCurrentMedication patientCurrentMedication);
        Task<ReturnBase<bool>> DeletePatientAllergyAsync(int Id);
        Task<ReturnBase<bool>> DeletePatientChronicDiseaseAsync(int Id);
        Task<ReturnBase<bool>> DeletePatientCurrentMedicationAsync(int Id);
    }
}
