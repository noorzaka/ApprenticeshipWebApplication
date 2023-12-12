using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApprenticeshipWebApplication.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        ApplicationDbContext context;
        private readonly UserManager<Person> _userManager;

        public StudentRepository(ApplicationDbContext context, UserManager<Person> _userManager)
        {
            this.context = context;
            this._userManager = _userManager;
        }
        public List<Student> GetAllStudents()
        {
            return context.students.Include(x => x.school).ToList();
        }
        public async Task AddStudent(Student student, string password)
        {
            await _userManager.CreateAsync(student, password);
        }
        public Student GetStudent(string Id)
        {
            var student = context.students.Where(x => x.Id == Id).SingleOrDefault();
            return student;
        }
        public async Task Update(Student student)
        {
            var oldStudent = context.students.Where(x => x.Id == student.Id).SingleOrDefault();

            oldStudent.firstName = student.firstName;
            oldStudent.secondName = student.secondName;
            oldStudent.thirdName = student.thirdName;
            oldStudent.lastName = student.lastName;
            oldStudent.PhoneNumber = student.PhoneNumber;
            oldStudent.schoolId = student.schoolId;

            await context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            var student = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(student);

        }
        public string GetStudentEmailByTrainingId(int trainingId)
        {
            var training = context.trainings.Include(x=> x.student).Where(t => t.trainingId == trainingId).SingleOrDefault();
            return training.student.Email;
        }

    }
}
