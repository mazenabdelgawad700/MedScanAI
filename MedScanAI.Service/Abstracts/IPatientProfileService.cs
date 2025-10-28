using MedScanAI.Shared.Base;

namespace MedScanAI.Service.Abstracts
{
    public interface IPatientProfileService
    {
        Task<ReturnBase<bool>> CreatePatientProfileAsync(List<string> ChronicDiseases, List<string> CurrentMedication, List<string> Allergies, string patientId);
    }
}
