namespace MedScanAI.Shared.SahredResponse
{
    public class GetPatientProfileResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> ChronicDiseases { get; set; }
        public List<string> Allergies { get; set; }
        public List<string> CurrentMedication { get; set; }
    }
}
