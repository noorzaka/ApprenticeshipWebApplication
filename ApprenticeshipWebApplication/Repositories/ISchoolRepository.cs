using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface ISchoolRepository
    {
        public List<School> GetAllSchools();
    }
}

