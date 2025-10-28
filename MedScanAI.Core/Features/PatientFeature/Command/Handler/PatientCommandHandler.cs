using MediatR;
using MedScanAI.Core.Features.PatientFeature.Command.Model;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.PatientFeature.Command.Handler
{
    public class PatientCommandHandler :
        IRequestHandler<CreatePatientProfileCommand, ReturnBase<bool>>
    {
        private readonly IPatientProfileService _patientProfileService;

        public PatientCommandHandler(IPatientProfileService patientProfileService)
        {
            _patientProfileService = patientProfileService;
        }


        public async Task<ReturnBase<bool>> Handle(CreatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _patientProfileService.CreatePatientProfileAsync(
                    request.ChronicDiseases,
                    request.CurrentMedication,
                    request.Allergies,
                    request.PatientId
                );

                if (!result.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(result.Message);


                return ReturnBaseHandler.Success(true, result.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
