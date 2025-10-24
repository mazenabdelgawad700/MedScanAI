using AutoMapper;
using MedScanAI.Core.Features.Authentication.Command.Model;
using MedScanAI.Domain.Entities;

namespace MedScanAI.Core.Mapping.Authentication.Command
{
    public class RegsiterPatientCommandMappingProfile : Profile
    {
        public RegsiterPatientCommandMappingProfile()
        {
            CreateMap<RegisterPatientCommand, Patient>();
        }
    }
}
