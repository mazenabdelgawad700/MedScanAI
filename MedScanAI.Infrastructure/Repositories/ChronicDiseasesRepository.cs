using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class ChronicDiseasesRepository : BaseRepository<PatientChronicDisease>, IChronicDiseasesRepository
    {
        public ChronicDiseasesRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
