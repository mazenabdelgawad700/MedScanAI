using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedScanAI.Domain.Entities
{
    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [Required]
        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }

        public int YearsOfExperience { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ApplicationUser ApplicationUser { get; set; }
        public Specialization Specialization { get; set; }

        // One-to-Many relationships
        public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
