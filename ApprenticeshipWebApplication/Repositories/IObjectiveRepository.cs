using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface IObjectiveRepository
    {
        public List<Objective> GetAllObjectives();
        public List<Objective> GetObjectivesForTraining(int id);
        public List<Objective> GetAchievedObjectivesForTraining(int trainingId);

    }
}
