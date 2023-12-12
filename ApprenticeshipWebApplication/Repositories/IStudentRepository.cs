using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface IStudentRepository
    {
        public List<Student> GetAllStudents();
        public Task AddStudent(Student student, string password);
        public Student GetStudent(string Id);
        public Task Update(Student student);
        Task Delete(string id);
        public string GetStudentEmailByTrainingId(int trainingId);

    }
}