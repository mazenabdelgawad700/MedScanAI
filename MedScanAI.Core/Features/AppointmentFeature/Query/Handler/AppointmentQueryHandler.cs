using AutoMapper;
using MediatR;
using MedScanAI.Core.Features.AppointmentFeature.Query.Model;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;

namespace MedScanAI.Core.Features.AppointmentFeature.Query.Handler
{
    public class AppointmentQueryHandler :
        IRequestHandler<GetDoctorsForAppointmentsQuery, ReturnBase<List<GetDoctorsForAppointmentsResponse>>>
    {

        private readonly IAppointmentService _appointmentsService;
        private readonly IMapper _mapper;

        public AppointmentQueryHandler(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentsService = appointmentService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<List<GetDoctorsForAppointmentsResponse>>> Handle(GetDoctorsForAppointmentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var doctorsResult = await _appointmentsService.GetDoctorsForAppointmentsAsync();

                if (!doctorsResult.Succeeded)
                    return ReturnBaseHandler.Failed<List<GetDoctorsForAppointmentsResponse>>(doctorsResult.Message);

                var mappedResult = _mapper.Map<List<GetDoctorsForAppointmentsResponse>>(doctorsResult.Data);

                return ReturnBaseHandler.Success(mappedResult, doctorsResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<List<GetDoctorsForAppointmentsResponse>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
