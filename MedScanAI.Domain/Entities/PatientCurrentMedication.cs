using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedScanAI.Domain.Entities
{
    [Table("PatientCurrentMedications")]
    public class PatientCurrentMedication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public string PatientId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Dosage { get; set; }

        [MaxLength(100)]
        public string Frequency { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Patient Patient { get; set; }
    }
}
