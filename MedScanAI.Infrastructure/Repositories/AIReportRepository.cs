using MedScanAI.Domain.Entities;
using MedScanAI.Infrastructure.Abstracts;
using MedScanAI.Infrastructure.Context;
using MedScanAI.Infrastructure.RepositoryBase;
using Microsoft.EntityFrameworkCore;

namespace MedScanAI.Infrastructure.Repositories
{
    internal class AIReportRepository : BaseRepository<AIReport>, IAIReportRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<AIReport> _dbSet;

        public AIReportRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<AIReport>();
        }
    }
}
