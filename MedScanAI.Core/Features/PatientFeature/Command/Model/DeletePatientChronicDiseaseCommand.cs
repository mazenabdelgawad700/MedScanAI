using MediatR;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.PatientFeature.Command.Model
{
    public class DeletePatientChronicDiseaseCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
    }
}
