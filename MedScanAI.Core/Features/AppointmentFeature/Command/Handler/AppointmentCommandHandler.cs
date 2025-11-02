using AutoMapper;
using MediatR;
using MedScanAI.Core.Features.AppointmentFeature.Command.Model;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.AppointmentFeature.Command.Handler
{
    public class AppointmentCommandHandler :
        IRequestHandler<MakeAppointmentCommand, ReturnBase<bool>>,
        IRequestHandler<ConfirmAppointmentCommand, ReturnBase<bool>>,
        IRequestHandler<CompleteAppointmentCommand, ReturnBase<bool>>
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

        public async Task<ReturnBase<bool>> Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var confirmResult = await _appointmentService.ConfirmAppointmentAsync(request.AppointmentId);

                if (!confirmResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(confirmResult.Message);

                return ReturnBaseHandler.Success(confirmResult.Data, confirmResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var completeResult = await _appointmentService.CompleteAppointmentAsync(request.AppointmentId);

                if (!completeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(completeResult.Message);

                return ReturnBaseHandler.Success(completeResult.Data, completeResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
