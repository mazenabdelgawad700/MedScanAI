using AutoMapper;
using MediatR;
using MedScanAI.Core.Features.AppointmentFeature.Command.Model;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.AppointmentFeature.Command.Handler
{
    public class AppointmentCommandHandler :
        IRequestHandler<MakeAppointmentCommand, ReturnBase<bool>>
    {

        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentCommandHandler(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }
        public async Task<ReturnBase<bool>> Handle(MakeAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentEntity = _mapper.Map<Domain.Entities.Appointment>(request);

                var makeAppointmentResult = await _appointmentService.MakeAppointmentAsync(appointmentEntity);

                if (!makeAppointmentResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(makeAppointmentResult.Message);

                return ReturnBaseHandler.Success(true, makeAppointmentResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
