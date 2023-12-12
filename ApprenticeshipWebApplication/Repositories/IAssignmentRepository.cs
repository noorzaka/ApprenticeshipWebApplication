using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface IAssignmentRepository
    {
        public List<Assignment> GetAssignmentsForTraining(int trainingId);
        public  Task AddAssignment(Assignment assignment, List<int> objectiveIds);
        public Assignment GetAssignment(int Id);
        public List<Assignment> GetAssignmentsForStudent(string studentId);

        public Task Update(Assignment updatedAssignment, List<int> objectiveIds);
        public Task DeleteAssignment(int assignmentId);
        public int GetTaskCountForTraining(int trainingId);




    }
}
