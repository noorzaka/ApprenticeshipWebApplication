using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipWebApplication.Repositories
{
    public class AssignmentRepository:IAssignmentRepository
    {
        ApplicationDbContext context;
        public AssignmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Assignment> GetAssignmentsForTraining(int trainingId)
        {
           return context.assignments.Where(x => x.trainingId== trainingId)
                      .Include(t => t.training)
                      .Include(t => t.assignmentObjectives)
                      .ThenInclude(x => x.objective)
                      
                      .ToList()
                      ;
        }
        public async Task AddAssignment(Assignment assignment, List<int> objectiveIds)
        {
            context.assignments.Add(assignment);
            await context.SaveChangesAsync();
            foreach (var ojId in objectiveIds)
            {
                context.assignmentObjectives.Add(
                new AssignmentObjectives()
                {
                    objectiveId = ojId,
                    assignmentId = assignment.assignmentId

                });
            }
            await context.SaveChangesAsync();

        }
        public Assignment GetAssignment(int Id)
        {
            var assignment = context.assignments
                .Where(x => x.assignmentId == Id)
                .Include(x => x.assignmentObjectives)
                    .ThenInclude(to => to.objective)
                .SingleOrDefault();

            return assignment;
        }
        public async Task Update(Assignment updatedAssignment, List<int> objectiveIds)
        {
            var existingAssignment = GetAssignment(updatedAssignment.assignmentId);

                existingAssignment.assignmentTitle = updatedAssignment.assignmentTitle;
                existingAssignment.assignmentDescription = updatedAssignment.assignmentDescription;
                existingAssignment.assignmentNotes = updatedAssignment.assignmentNotes;
                existingAssignment.startDate = updatedAssignment.startDate;
                existingAssignment.endDate = updatedAssignment.endDate;
               
                await context.SaveChangesAsync();

                existingAssignment.assignmentObjectives.Clear();
                foreach (var objectiveId in objectiveIds)
                {
                    existingAssignment.assignmentObjectives.Add(new AssignmentObjectives
                    {
                        objectiveId = objectiveId,
                        assignmentId = existingAssignment.assignmentId
                    });
                }
                context.Update(existingAssignment);
                await context.SaveChangesAsync();
            
            
        }
        public List<Assignment> GetAssignmentsForStudent(string studentId)
        {
            return context.assignments
                .Include(x => x.training)
                .Where(x => x.training.studentId == studentId)
                .Include(x => x.assignmentObjectives)
                    .ThenInclude(to => to.objective)
                .ToList();
        }
        public async Task DeleteAssignment(int assignmentId)
        {
            var assignment = await context.assignments.FindAsync(assignmentId);
            context.assignments.Remove(assignment);
            await context.SaveChangesAsync();

        }
        public int GetTaskCountForTraining(int trainingId)
        {
            int taskCount = context.assignments
                .Count(assignmet => assignmet.trainingId == trainingId);

            return taskCount;
        }


    }
}
