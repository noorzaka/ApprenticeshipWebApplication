using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApprenticeshipWebApplication.Repositories
{
    public class SchoolSupervisorRepository : ISchoolSupervisorRepository
    {
        ApplicationDbContext context;
        private readonly UserManager<Person> _userManager;

        public SchoolSupervisorRepository(ApplicationDbContext context, UserManager<Person> _userManager)
        {
            this.context = context;
            this._userManager = _userManager;
        }
        public List<SchoolSupervisor> GetAllSchoolSupervisors()
        {
            return context.schoolSupervisors.Include(x => x.school).ToList();
        }
        public async Task AddSchoolSupervisor(SchoolSupervisor schoolSupervisor, string password)
        {
            await _userManager.CreateAsync(schoolSupervisor, password);
        }
        public SchoolSupervisor GetSchoolSupervisor(string Id)
        {
            var schoolSupervisor = context.schoolSupervisors.Where(x => x.Id == Id).SingleOrDefault();
            return schoolSupervisor;
        }
        public async Task Update(SchoolSupervisor schoolSupervisor)
        {
            var oldSchoolSupervisor = context.schoolSupervisors.Where(x => x.Id == schoolSupervisor.Id).SingleOrDefault();

            oldSchoolSupervisor.firstName = schoolSupervisor.firstName;
            oldSchoolSupervisor.secondName = schoolSupervisor.secondName;
            oldSchoolSupervisor.thirdName = schoolSupervisor.thirdName;
            oldSchoolSupervisor.lastName = schoolSupervisor.lastName;
            oldSchoolSupervisor.PhoneNumber = schoolSupervisor.PhoneNumber;
            oldSchoolSupervisor.schoolId = schoolSupervisor.schoolId;

            await context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            var schoolSupervisor = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(schoolSupervisor);

        }

    }
}
