using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        ApplicationDbContext context;
        public SchoolRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<School> GetAllSchools()
        {
            return context.schools.ToList();
        }
    }
}
