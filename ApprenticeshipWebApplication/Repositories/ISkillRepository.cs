using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface ISkillRepository
    {
        public List<Skill> GetAchievedSkillsForTraining(int trainingId);
        public List<Skill> GetAllSkillsForTrainingObjectives(string studentId);


    }
}
