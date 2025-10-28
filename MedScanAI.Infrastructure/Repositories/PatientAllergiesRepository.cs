using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class PatientAllergiesRepository : BaseRepository<PatientAllergy>, IPatientAllergiesRepository
    {
        public PatientAllergiesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
