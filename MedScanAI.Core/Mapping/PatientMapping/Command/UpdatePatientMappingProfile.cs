using AutoMapper;
using MedScanAI.Core.Features.PatientFeature.Command.Model;
using MedScanAI.Domain.Entities;

namespace MedScanAI.Core.Mapping.PatientMapping.Command
{
    public class UpdatePatientMappingProfile : Profile
    {
        public UpdatePatientMappingProfile()
        {
            CreateMap<UpdatePatientProfileCommand, Patient>();
        }
    }
}
