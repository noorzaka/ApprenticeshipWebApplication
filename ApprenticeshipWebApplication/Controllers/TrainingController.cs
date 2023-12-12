using ApprenticeshipWebApplication.DTO;
using ApprenticeshipWebApplication.Entities;
using ApprenticeshipWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApprenticeshipWebApplication.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class TrainingController : Controller
    {
        ITrainingRepository trainingRepository;
        IObjectiveRepository objectiveRepository;
        IStudentRepository studentRepository;
        ISchoolSupervisorRepository schoolSupervisorRepository;
        ITeamLeaderRepository teamLeaderRepository;
        public TrainingController(ITrainingRepository trainingRepository, IObjectiveRepository objectiveRepository, IStudentRepository studentRepository, ISchoolSupervisorRepository schoolSupervisorRepository, ITeamLeaderRepository teamLeaderRepository)
        {
            this.trainingRepository = trainingRepository;
            this.objectiveRepository = objectiveRepository;
            this.studentRepository = studentRepository;
            this.schoolSupervisorRepository = schoolSupervisorRepository;
            this.teamLeaderRepository = teamLeaderRepository;
        }

        public IActionResult Index()
        {
            ViewBag.trainings = trainingRepository.GetAllTrainings();
           
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.objectives = objectiveRepository.GetAllObjectives();
            ViewBag.students = studentRepository.GetAllStudents();
            ViewBag.teamLeaders = teamLeaderRepository.GetAllTeamLeaders();
            ViewBag.schoolSupervisors =schoolSupervisorRepository.GetAllSchoolSupervisors();



            return View();
        }
        

        public async Task<IActionResult> Create(InsertTrainingDTO trainingDTO)
        {
            
                Training training = new Training();
                training.trainingName = trainingDTO.trainingName;
                training.studentId = trainingDTO.studentId;
                training.schoolSupervisorId = trainingDTO.schoolSupervisorId;
                training.teamLeaderId = trainingDTO.teamLeaderId;
                training.startDate = trainingDTO.startDate;
                training.endDate = trainingDTO.endDate;
                await trainingRepository.AddTraining(training, trainingDTO.objectiveIds);
                return RedirectToAction("Index");
            

        }

        public IActionResult Edit(int id)
         {
            var training = trainingRepository.GetTraining(id);

             UpdateTrainingDTO trainingDTO = new UpdateTrainingDTO();
             trainingDTO.id = training.trainingId;
             trainingDTO.trainingName = training.trainingName;
             trainingDTO.studentId=training.studentId;
             trainingDTO.schoolSupervisorId=training.schoolSupervisorId;
             trainingDTO.teamLeaderId=training.teamLeaderId;
             trainingDTO.startDate = training.startDate;
             trainingDTO.endDate = training.endDate;
             trainingDTO. objectiveIds = training.trainingObjectives.Select(x => x.objectiveId).ToList();

             ViewBag.objectives = objectiveRepository.GetAllObjectives();
             ViewBag.students = studentRepository.GetAllStudents();
             ViewBag.teamLeaders = teamLeaderRepository.GetAllTeamLeaders();
             ViewBag.schoolSupervisors = schoolSupervisorRepository.GetAllSchoolSupervisors();
             return View(trainingDTO);



         }
        public async Task<IActionResult> Update(UpdateTrainingDTO trainingDTO)
        {
            var training = trainingRepository.GetTraining(trainingDTO.id);

            if (training == null)
            {
                // Handle the case where training is not found, e.g., return a not found response
                return NotFound();
            }

            training.studentId = trainingDTO.studentId;
            training.trainingName = trainingDTO.trainingName;
            training.schoolSupervisorId = trainingDTO.schoolSupervisorId;
            training.teamLeaderId = trainingDTO.teamLeaderId;
            training.startDate = trainingDTO.startDate;
            training.endDate = trainingDTO.endDate;

            await trainingRepository.UpdateTraining(training, trainingDTO.objectiveIds);
            return RedirectToAction("Index");
        }


     
        public async Task<IActionResult> Delete(int id)
        {
            await trainingRepository.DeleteTraining(id);
            return RedirectToAction("Index");

        }
     
        }
}
