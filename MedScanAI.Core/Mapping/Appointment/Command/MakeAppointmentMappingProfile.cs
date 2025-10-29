using AutoMapper;
using MedScanAI.Core.Features.AppointmentFeature.Command.Model;

namespace MedScanAI.Core.Mapping.Appointment.Command
{
    public class MakeAppointmentMappingProfile : Profile
    {
        public MakeAppointmentMappingProfile()
        {
            CreateMap<MakeAppointmentCommand, Domain.Entities.Appointment>();
        }
    }
}
