using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface ITeamLeaderRepository
    {
        public List<TeamLeader> GetAllTeamLeaders();
        public Task AddTeamLeader(TeamLeader teamLeader, string password);
        public TeamLeader GetTeamLeader(string Id);
        public Task Update(TeamLeader teamLeader);
        Task Delete(string id);
    }
}
