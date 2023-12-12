using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApprenticeshipWebApplication.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        ApplicationDbContext context;

        public TrainingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Training> GetAllTrainings()
        {
            return context.trainings
               .Include(t => t.schoolSupervisor)
               .Include(t => t.student)
               .Include(t => t.teamLeader)
                .Include(t => t.trainingObjectives)
                  .ThenInclude(x => x.objective)
                     .ToList();
        }
        public Training GetTraining(int Id)
        {
            var training = context.trainings
                .Where(x => x.trainingId == Id)
                .Include(x => x.trainingObjectives)
                    .ThenInclude(to => to.objective)
                .SingleOrDefault();

            return training;
        }

        public async Task AddTraining(Training training, List<int> objectiveIds)
        {
            context.trainings.Add(training);
            await context.SaveChangesAsync();
            foreach (var ojId in objectiveIds)
            {
                context.trainingObjectives.Add(
                new TrainingObjectives()
                {
                    objectiveId = ojId,
                    trainingId = training.trainingId

                });
            }
            await context.SaveChangesAsync();

        }
        /* public async Task UpdateTraining(Training updatedTraining, List<int> objectiveIds)
         {
             var existingTraining = context.trainings
             .Include(t => t.trainingObjectives) // Ensure trainingObjectives are loaded
            .SingleOrDefault(t => t.trainingId == updatedTraining.trainingId);
                 existingTraining.studentId = updatedTraining.studentId;
                 existingTraining.schoolSupervisorId = updatedTraining.schoolSupervisorId;
                 existingTraining.teamLeaderId = updatedTraining.teamLeaderId;
                 existingTraining.startDate = updatedTraining.startDate;
                 existingTraining.endDate = updatedTraining.endDate;
                 existingTraining.trainingObjectives.Clear();

             foreach (var objectiveId in objectiveIds)
             {
                 existingTraining.trainingObjectives.Add(new TrainingObjectives
                 {
                     objectiveId = objectiveId,
                     trainingId = existingTraining.trainingId
                 });
             }
             context.Update(existingTraining);
             await context.SaveChangesAsync();

         }*/

        public async Task UpdateTraining(Training updatedTraining, List<int> objectiveIds)
        {
            var existingTraining = GetTraining(updatedTraining.trainingId);

            if (existingTraining != null)
            {
                // Update basic training properties
                existingTraining.trainingName= updatedTraining.trainingName;
                existingTraining.studentId = updatedTraining.studentId;
                existingTraining.schoolSupervisorId = updatedTraining.schoolSupervisorId;
                existingTraining.teamLeaderId = updatedTraining.teamLeaderId;
                existingTraining.startDate = updatedTraining.startDate;
                existingTraining.endDate = updatedTraining.endDate;
                await context.SaveChangesAsync();

                // Clear existing trainingObjectives
                existingTraining.trainingObjectives.Clear();

                // Add new trainingObjectives based on the provided objectiveIds
                foreach (var objectiveId in objectiveIds)
                {
                    existingTraining.trainingObjectives.Add(new TrainingObjectives
                    {
                        objectiveId = objectiveId,
                        trainingId = existingTraining.trainingId
                    });
                }

                // Update the existing training in the context
                context.Update(existingTraining);

                // Save changes to the database
                await context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the training is not found
                throw new InvalidOperationException($"Training with ID {updatedTraining.trainingId} not found.");
            }
        }


        public async Task DeleteTraining(int id)
        {
            var training = context.trainings.Find(id);

            context.trainings.Remove(training);
            await context.SaveChangesAsync();
        }
        public List<Training> GetAllTrainingForTeamLeader(string teamLeaderId)
        {
            return context.trainings
                .Where(t => t.teamLeaderId == teamLeaderId)
                .Include(t => t.schoolSupervisor)
                .Include(t => t.student)
                .Include(t => t.teamLeader)
                .Include(t => t.trainingObjectives)
                .ThenInclude(x => x.objective)
                .ToList();
        }
        public List<Training> GetAllTrainingsForSchoolSupervisor(string schoolSupervisorId)
        {
            return context.trainings
                .Where(t => t.schoolSupervisorId == schoolSupervisorId)
                .Include(t => t.schoolSupervisor)
                .Include(t => t.student)
                .Include(t => t.teamLeader)
                .Include(t => t.trainingObjectives)
                .ThenInclude(x => x.objective)
                .ToList();
        }
        public List <Training> GetTrainingsForStudent(string studentId)
        {
            return context.trainings
                .Where(t => t.studentId == studentId)
                .Include(t => t.schoolSupervisor)
                .Include(t => t.student)
                .Include(t => t.teamLeader)
                .Include(t => t.trainingObjectives)
                .ThenInclude(x => x.objective)
                .ToList();

        }
        public int GetTrainingIdForStudent(string studentId)
        {
            var training = context.trainings.FirstOrDefault(x => x.studentId == studentId);
            return training.trainingId;
        }





    }
}


