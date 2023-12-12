using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.ViewModels
{
    public class TrainingViewModel
    {
        public Training Training { get; set; }
        public List<Skill> AchievedSkills { get; set; }
        public List<Objective> AchievedObjectives { get; set; }



    }
}
