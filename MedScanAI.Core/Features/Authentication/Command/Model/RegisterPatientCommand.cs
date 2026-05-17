using MediatR;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.Authentication.Command.Model
{
    public class RegisterPatientCommand : IRequest<ReturnBase<string>>
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
