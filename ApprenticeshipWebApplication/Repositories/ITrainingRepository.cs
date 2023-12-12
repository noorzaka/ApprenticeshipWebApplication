using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface ITrainingRepository
    {
        public List<Training> GetAllTrainings();
        public Task AddTraining(Training training, List<int> objectiveIds);
        public Training GetTraining(int Id);
        public  Task UpdateTraining(Training updatedTraining, List<int> objectiveIds);

        Task DeleteTraining(int id);
        public List<Training> GetAllTrainingForTeamLeader(string teamLeaderId);
        public List<Training> GetTrainingsForStudent(string studentId);
        public List<Training> GetAllTrainingsForSchoolSupervisor(string schoolSupervisorId);
        public int GetTrainingIdForStudent(string studentId);


    }
}
