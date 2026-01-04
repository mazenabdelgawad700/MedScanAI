using AutoMapper;
using MedScanAI.Domain.Entities;
using MedScanAI.Shared.SharedRequest;

namespace MedScanAI.Core.Mapping.PatientMapping.Command
{
    public class UpdatePatientHistoryMappingProfile : Profile
    {
        public UpdatePatientHistoryMappingProfile()
        {
            CreateMap<PatientUpdateProfileSharedRequest, PatientAllergy>();
            CreateMap<PatientUpdateProfileSharedRequest, PatientChronicDisease>();
            CreateMap<PatientUpdateProfileSharedRequest, PatientCurrentMedication>();
        }
    }
}
