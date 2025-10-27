using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
