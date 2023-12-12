using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipWebApplication.Repositories
{
    public class ObjectiveRepository : IObjectiveRepository
    {
        ApplicationDbContext context;
        public ObjectiveRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Objective> GetAllObjectives()
        {
            return context.objectives.ToList();
        }
        public List<Objective> GetObjectivesForTraining(int id)
        {
            return context.trainingObjectives
         .Where(x => x.trainingId == id)
         .Select(x => x.objective)
         .ToList();
        }
        public List<Objective> GetAchievedObjectivesForTraining(int trainingId)
        {
            var achievedObjectives = context.trainings
                .Where(t => t.trainingId == trainingId)
                .SelectMany(t => t.assignments
                    .Where(a => a.reports.Any(r => r.reportStatus.reportStatusId == 1))
                    .SelectMany(a => a.assignmentObjectives
                        .Select(ao => ao.objective)
                    )
                )
                .Distinct()
                .ToList();

            return achievedObjectives;
        }



    }
}

