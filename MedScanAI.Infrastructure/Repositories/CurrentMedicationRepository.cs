using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class CurrentMedicationRepository : BaseRepository<PatientCurrentMedication>, ICurrentMedicationRepository
    {
        public CurrentMedicationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
