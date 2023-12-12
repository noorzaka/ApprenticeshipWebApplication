using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        ApplicationDbContext context;
        public CompanyRepository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public List<Company>GetAllCompanies()
        {
            return context.companies.ToList();
        }
    }
}
