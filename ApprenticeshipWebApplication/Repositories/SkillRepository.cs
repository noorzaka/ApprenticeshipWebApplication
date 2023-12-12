using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipWebApplication.Repositories
{
    public class SkillRepository :ISkillRepository
    {
        ApplicationDbContext context;
        public SkillRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
      
        public List<Skill> GetAchievedSkillsForTraining(int trainingId)
        {
            var achievedSkills = context.trainings
                .Where(t => t.trainingId == trainingId)
                .SelectMany(t => t.assignments
                    .Where(a => a.reports.Any(r => r.reportStatus.reportStatusId == 1))
                    .SelectMany(a => a.assignmentObjectives
                        .SelectMany(ao => ao.objective.objectiveSkills.Select(os => os.skill))
                    )
                )
                .Distinct()
                .ToList();

            return achievedSkills;
        }
        public List<Skill> GetAllSkillsForTrainingObjectives(string studentId)
        {
            var allSkills = context.trainings
                .Where(t => t.studentId == studentId)
                .SelectMany(t => t.trainingObjectives
                    .SelectMany(to => to.objective.objectiveSkills.Select(os => os.skill))
                )
                .Distinct()
                .ToList();

            return allSkills;
        }



    }
}
