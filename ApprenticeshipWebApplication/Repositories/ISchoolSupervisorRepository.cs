using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface ISchoolSupervisorRepository
    {
        public List<SchoolSupervisor> GetAllSchoolSupervisors();
        public Task AddSchoolSupervisor(SchoolSupervisor schoolSupervisor, string password);
        public SchoolSupervisor GetSchoolSupervisor(string Id);
        public Task Update(SchoolSupervisor schoolSupervisor);
        Task Delete(string id);
    }
}