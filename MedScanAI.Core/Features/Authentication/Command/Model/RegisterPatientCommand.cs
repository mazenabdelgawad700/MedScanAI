using MediatR;
using MedScanAI.Shared.Base;

namespace MedScanAI.Core.Features.Authentication.Command.Model
{
    public class RegisterPatientCommand : IRequest<ReturnBase<bool>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
