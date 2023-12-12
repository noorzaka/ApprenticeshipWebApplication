using ApprenticeshipWebApplication.DTO;
using ApprenticeshipWebApplication.Entities;
using ApprenticeshipWebApplication.Helper;
using ApprenticeshipWebApplication.Models;
using ApprenticeshipWebApplication.Repositories;
using ApprenticeshipWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;

namespace ApprenticeshipWebApplication.Controllers
{
    public class TeamLeaderController : Controller
    {
        ITeamLeaderRepository teamLeaderRepository;
        ICompanyRepository companyRepository;
        ITrainingRepository trainingRepository;
        IAssignmentRepository assignmentRepository;
        IObjectiveRepository objectiveRepository;
        IReportRepository reportRepository;
        IDocumentRepository documentRepository;
        IStudentRepository studentRepository;
        public TeamLeaderController(ITeamLeaderRepository teamLeaderRepository, ICompanyRepository companyRepository, ITrainingRepository trainingRepository, IAssignmentRepository assignmentRepository, IObjectiveRepository objectiveRepository, IReportRepository reportRepository, IDocumentRepository documentRepository, IStudentRepository studentRepository)
        {
            this.teamLeaderRepository = teamLeaderRepository;
            this.companyRepository = companyRepository;
            this.trainingRepository = trainingRepository;
            this.assignmentRepository = assignmentRepository;
            this.objectiveRepository = objectiveRepository;
            this.reportRepository = reportRepository;
            this.documentRepository = documentRepository;
            this.studentRepository = studentRepository;
        }
        // CRUD
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.teamLeaders = teamLeaderRepository.GetAllTeamLeaders();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Add()
        {
            ViewBag.companies = companyRepository.GetAllCompanies();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(InsertTeamLeaderDTO teamLeaderDTO)
        {
            TeamLeader teamLeader = new TeamLeader();
            teamLeader.firstName = teamLeaderDTO.firstName;
            teamLeader.secondName = teamLeaderDTO.secondName;
            teamLeader.thirdName = teamLeaderDTO.thirdName;
            teamLeader.lastName = teamLeaderDTO.lastName;
            teamLeader.UserName = teamLeaderDTO.email;
            teamLeader.Email = teamLeaderDTO.email;
            teamLeader.NormalizedUserName = teamLeaderDTO.email.ToUpper();
            teamLeader.NormalizedEmail = teamLeaderDTO.email.ToUpper();
            teamLeader.companyId = teamLeaderDTO.companyId;
            teamLeader.PhoneNumber = teamLeaderDTO.phoneNumber;
            teamLeader.PhoneNumberConfirmed = true;
            teamLeader.EmailConfirmed = true;
            await teamLeaderRepository.AddTeamLeader(teamLeader, teamLeaderDTO.password);
            return RedirectToAction("Index");
        }
        /*public IActionResult Edit(string Id)
        {
            var teamLeader = teamLeaderRepository.GetTeamLeader(Id);
            

            ViewBag.companies = companyRepository.GetAllCompanies();

            return View(teamLeader);
        }*/
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(string Id)
        {
            var teamLeader = teamLeaderRepository.GetTeamLeader(Id);

            // Assuming you have a mapper, map TeamLeader to UpdateTeamLeaderDTO
            var updateTeamLeaderDTO = new UpdateTeamLeaderDTO
            {
                firstName = teamLeader.firstName,
                secondName = teamLeader.secondName,
                thirdName = teamLeader.thirdName,
                lastName = teamLeader.lastName,
                companyId = teamLeader.companyId,
                phoneNumber = teamLeader.PhoneNumber
                // Map other properties as needed
            };

            ViewBag.Companies = companyRepository.GetAllCompanies();
            return View(updateTeamLeaderDTO);
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            await teamLeaderRepository.Delete(id);
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(UpdateTeamLeaderDTO teamLeaderDTO)
        {
            TeamLeader teamLeader = new TeamLeader();
            teamLeader.Id = teamLeaderDTO.Id;
            teamLeader.firstName = teamLeaderDTO.firstName;
            teamLeader.secondName = teamLeaderDTO.secondName;
            teamLeader.thirdName = teamLeaderDTO.thirdName;
            teamLeader.lastName = teamLeaderDTO.lastName;
            teamLeader.companyId = teamLeaderDTO.companyId;
            teamLeader.PhoneNumber = teamLeaderDTO.phoneNumber;

            await teamLeaderRepository.Update(teamLeader);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult ViewTrainings()
        {
            string loggedInTeamLeaderId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewBag.trainings = trainingRepository.GetAllTrainingForTeamLeader(loggedInTeamLeaderId);


            return View();
        }
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult ViewAssignments(int id)
        {
            ViewBag.assignments = assignmentRepository.GetAssignmentsForTraining(id);
            ViewBag.trainingId = id;
            var training = trainingRepository.GetTraining(id);
            var trainingAssignments=assignmentRepository.GetAssignmentsForTraining(id);
            var documentsByAssignment = new Dictionary<int, List<Document>>(); // Dictionary to store assignmentId and its documents
          
                foreach (var assignment in trainingAssignments)
                {
                    var documents = documentRepository.GetAssignmentDocuments(assignment.assignmentId);
                    documentsByAssignment.Add(assignment.assignmentId, documents);
                }

                ViewBag.documentsByAssignment = documentsByAssignment;
            
            return View();
        }
        [Authorize(Roles = "TEAMLEADER")]
       public IActionResult GetDocument(int documentId)
        {
            var file = documentRepository.GetDoc(documentId);
            Stream stream =new MemoryStream(file.content);
            return new FileStreamResult(stream,file.contentType);
        }


        [Authorize(Roles = "TEAMLEADER")]

        public IActionResult AddAssignment(int id)
        {
            ViewBag.objectives = objectiveRepository.GetObjectivesForTraining(id);
            ViewBag.trainingId = id;
            return View();
        }
        [Authorize(Roles = "TEAMLEADER")]
        [HttpPost]

        public async Task<IActionResult> CreateAssignment(InsertAssignmentDTO assignmentDTO, List<IFormFile> formFiles)
        {
            Assignment assignment = new Assignment
            {
                trainingId = assignmentDTO.trainingId,
                assignmentTitle = assignmentDTO.assignmentTitle,
                assignmentDescription = assignmentDTO.assignmentDescription,
                assignmentNotes = assignmentDTO.assignmentNotes,
                startDate=assignmentDTO.startDate,
                endDate=assignmentDTO.endDate

            };

            await assignmentRepository.AddAssignment(assignment, assignmentDTO.objectiveIds);
            int assignmentId = assignment.assignmentId;

            foreach (var formFile in formFiles)
            {
                // Process each uploaded file
                if (formFile.Length > 0)
                {
                    using (Stream stream = formFile.OpenReadStream())
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        byte[] content = reader.ReadBytes((int)stream.Length);

                        Document document = new Document
                        {
                            documentName = formFile.FileName,
                            contentType = formFile.ContentType,
                            content = content,
                            assignmentId = assignmentId
                        };

                        await documentRepository.AddDocument(document);
                    }
                }
            }
            //SendEmail.NewEmail(studentRepository.GetStudentEmailByTrainingId(assignmentDTO.trainingId), "This Email to let you to know that there is anew assignment assigned to you,Please go to the Apprenticrship portal to cheak it.", assignmentDTO.assignmentTitle);

            return RedirectToAction("ViewAssignments", new { id = assignment.trainingId });
        }


        [Authorize(Roles = "TEAMLEADER")]
        
            public IActionResult EditAssignment(int id)
            {
                var assignment = assignmentRepository.GetAssignment(id);

                UpdateAssignmentDTO assignmentDTO = new UpdateAssignmentDTO();
                assignmentDTO.assignmentId = assignment.assignmentId;
                assignmentDTO.assignmentTitle = assignment.assignmentTitle;
                assignmentDTO.assignmentDescription = assignment.assignmentDescription;
                assignmentDTO.assignmentNotes = assignment.assignmentNotes;
                assignmentDTO.objectiveIds = assignment.assignmentObjectives.Select(x => x.objectiveId).ToList();
                assignmentDTO.startDate=assignment.startDate;
                assignmentDTO.endDate=assignment.endDate;
                ViewBag.trainingId = assignment.trainingId; 
                ViewBag.objectives = objectiveRepository.GetObjectivesForTraining(assignment.trainingId);
                ViewBag.documents = documentRepository.GetAssignmentDocuments(assignment.assignmentId);

            return View(assignmentDTO);



            }
            [Authorize(Roles = "TEAMLEADER")]

            public async Task<IActionResult> UpdateAssignment(UpdateAssignmentDTO assignmentDTO, List<IFormFile> formFiles)
            {
                var assignment = assignmentRepository.GetAssignment(assignmentDTO.assignmentId);

                if (assignment == null)
                {
                    // Handle the case where training is not found, e.g., return a not found response
                    return NotFound();
                }

                assignment.assignmentTitle = assignmentDTO.assignmentTitle;
                assignment.assignmentDescription = assignmentDTO.assignmentDescription;
                assignment.assignmentNotes = assignmentDTO.assignmentNotes;
                assignment.startDate= assignmentDTO.startDate;
                assignment.endDate= assignmentDTO.endDate;

                await assignmentRepository.Update(assignment, assignmentDTO.objectiveIds);
            int assignmentId = assignment.assignmentId;

            foreach (var formFile in formFiles)
            {
                // Process each uploaded file
                if (formFile.Length > 0)
                {
                    using (Stream stream = formFile.OpenReadStream())
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        byte[] content = reader.ReadBytes((int)stream.Length);

                        Document document = new Document
                        {
                            documentName = formFile.FileName,
                            contentType = formFile.ContentType,
                            content = content,
                            assignmentId = assignmentId
                        };

                        await documentRepository.AddDocument(document);
                    }
                }
            }
            return RedirectToAction("ViewAssignments", new { id = assignment.trainingId });
        }
        [Authorize(Roles = "TEAMLEADER")]

            public IActionResult ViewAssignmentReports(int assignmentId)
            {
                // Retrieve reports for the given assignment
                ViewBag.reports = reportRepository.GetAssignmentsReports(assignmentId);
                ViewBag.assignmentId = assignmentId;
                return View();
            }
        [Authorize(Roles = "TEAMLEADER")]

        [Authorize(Roles = "TEAMLEADER")]
        public async Task<IActionResult> ViewReportDetails(int reportId, int assignmentId)
        {
            var report = reportRepository.GetReport(reportId);

            if (report == null)
            {
                // Handle the case where the Report is not found
                return NotFound();
            }

            var reportDocuments = reportRepository.GetReportDocuments(reportId);

            ViewBag.assignmentId = assignmentId;
            ViewBag.reportId = reportId;
            ViewBag.reportDoc = reportDocuments;

            return  View(report);
        }




        [Authorize(Roles = "TEAMLEADER")]

            [HttpPost]
            public async Task<IActionResult> ApproveReport(int reportId, int assignmentId)
            {
                // Implement logic to approve the report
                await reportRepository.ApproveReport(reportId);
                //await reportRepository.AddReportLog(reportRepository.GetReport(assignmentId),1);
                return RedirectToAction("ViewAssignmentReports", new { assignmentId = assignmentId });
            }
            [Authorize(Roles = "TEAMLEADER")]

            [HttpPost]
            public async Task<IActionResult> RejectReport(int reportId, int assignmentId)
            {
                // Implement logic to reject the report
                await reportRepository.RejectReport(reportId);
                // await reportRepository.AddReportLog(reportRepository.GetReport(assignmentId),3);
                return RedirectToAction("ViewAssignmentReports", new { assignmentId = assignmentId });
            }
        [Authorize(Roles = "TEAMLEADER")]
        public async Task<IActionResult> DeleteDocument(int documentId,int assignmentId,int trainingId)
        {

            
            await documentRepository.DeleteDoc(documentId);
            return RedirectToAction("EditAssignment", new { id = assignmentId });

        }
        [Authorize(Roles = "TEAMLEADER")]
        public async Task<IActionResult> DeleteAssignment( int assignmentId, int trainingId)
        {
            var assignment= assignmentRepository.GetAssignment(assignmentId);
            var documents = documentRepository.GetAssignmentDocuments(assignmentId);
            foreach (var document in documents)
            {
                await documentRepository.DeleteDoc(document.documentId);
            }
            await assignmentRepository.DeleteAssignment(assignmentId);
            return RedirectToAction("ViewAssignments", new { trainingId = trainingId });
        }
        [Authorize(Roles = "TEAMLEADER")]

        public IActionResult Dashboard()
        {
            string loggedInId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Training> allTrainings = trainingRepository.GetAllTrainingForTeamLeader(loggedInId);
            List<TrainingViewModel> viewModels = new List<TrainingViewModel>();

            foreach (var training in allTrainings)
            {
                List<Objective> achievedObjectives = objectiveRepository.GetAchievedObjectivesForTraining(training.trainingId);

                var viewModel = new TrainingViewModel
                {
                    Training = training,
                    AchievedObjectives = achievedObjectives
                };

                viewModels.Add(viewModel);
            }

            ViewBag.TrainingViewModels = viewModels;

            return View();
        }
      /*  [Authorize(Roles = "TEAMLEADER")]

        public List<Report> GetPendingApprovalReportsForTraining(int trainingId)
        {
            var reports = context.reports
                .Where(r => r.assignment.training.trainingId == trainingId && r.reportStatus.reportStatusId == 2) // Assuming 2 represents the "Pending Approval" status
                .ToList();

            return reports;
        }*/









    }
}


