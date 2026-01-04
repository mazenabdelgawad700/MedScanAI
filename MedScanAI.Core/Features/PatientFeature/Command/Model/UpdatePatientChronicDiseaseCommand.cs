using MediatR;
using MedScanAI.Shared.Base;
using MedScanAI.Shared.SharedRequest;

namespace MedScanAI.Core.Features.PatientFeature.Command.Model
{
    public class UpdatePatientChronicDiseaseCommand : IRequest<ReturnBase<bool>>
    {
        public PatientUpdateProfileSharedRequest ChronicDisease { get; set; }
    }
}
