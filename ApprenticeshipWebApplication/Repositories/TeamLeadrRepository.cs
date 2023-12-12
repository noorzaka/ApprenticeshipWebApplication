using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApprenticeshipWebApplication.Repositories
{
    public class TeamLeadrRepository : ITeamLeaderRepository
    { ApplicationDbContext context;
      private readonly UserManager<Person> _userManager;

        public TeamLeadrRepository(ApplicationDbContext context, UserManager<Person> _userManager)
        {
            this.context = context;
           this._userManager = _userManager;
        }
        public List <TeamLeader> GetAllTeamLeaders()
        {
            return context.teamLeaders.Include(x => x.company).ToList();
        }
        public async Task  AddTeamLeader(TeamLeader teamLeader , string password)
        {
            await _userManager.CreateAsync(teamLeader, password);
        }
        public TeamLeader GetTeamLeader(string Id)
        {
            var teamLeader = context.teamLeaders.Where(x => x.Id == Id).SingleOrDefault();
            return teamLeader;
        }
        public async Task Update(TeamLeader teamLeader)
        {
           var oldTeamLeader = context.teamLeaders.Where(x => x.Id == teamLeader.Id).SingleOrDefault();

            oldTeamLeader.firstName = teamLeader.firstName;
            oldTeamLeader.secondName = teamLeader.secondName;
            oldTeamLeader.thirdName = teamLeader.thirdName;
            oldTeamLeader.lastName = teamLeader.lastName;
            oldTeamLeader.PhoneNumber= teamLeader.PhoneNumber;
            oldTeamLeader.companyId=teamLeader.companyId;

            await context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            var teamLeader = await _userManager.FindByIdAsync(id);
            
                await _userManager.DeleteAsync(teamLeader);
            
        }

    }
}
