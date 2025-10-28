using MediatR;
using MedScanAI.Core.Features.PatientFeature.Query.Model;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SahredResponse;

namespace MedScanAI.Core.Features.PatientFeature.Query.Handler
{
    public class PatientQueryHandler :
        IRequestHandler<GetPatientProfileQuery, ReturnBase<GetPatientProfileResponse>>
    {
        private readonly IPatientProfileService _patientProfileService;


        public PatientQueryHandler(IPatientProfileService patientProfileService)
        {
            _patientProfileService = patientProfileService;
        }

        public async Task<ReturnBase<GetPatientProfileResponse>> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _patientProfileService.GetPatientProfileAsync(request.PatientId);

                if (!result.Succeeded)
                    return ReturnBaseHandler.Failed<GetPatientProfileResponse>(result.Message);


                return ReturnBaseHandler.Success(result.Data!, "Patient profile retrieved successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetPatientProfileResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
