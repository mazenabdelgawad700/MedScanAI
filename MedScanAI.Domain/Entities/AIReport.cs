using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedScanAI.Domain.Entities
{
    [Table("AIReports")]
    public class AIReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public int? AppointmentId { get; set; }

        [MaxLength(500)]
        public string ReportFileUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }
    }
}
