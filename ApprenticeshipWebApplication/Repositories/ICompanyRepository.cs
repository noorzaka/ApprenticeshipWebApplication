using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface ICompanyRepository
    {
        public List<Company> GetAllCompanies();
    }
}
