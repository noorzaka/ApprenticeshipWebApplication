using ApprenticeshipWebApplication.DTO;
using ApprenticeshipWebApplication.Entities;
using ApprenticeshipWebApplication.Models;
using ApprenticeshipWebApplication.ViewModels;
using ApprenticeshipWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Security.Claims;

namespace ApprenticeshipWebApplication.Controllers
{
    public class SchoolSupervisorController : Controller
    {
        ISchoolSupervisorRepository schoolSupervisorRepository;
        ISchoolRepository schoolRepository;
        ITrainingRepository trainingRepository;
        IAssignmentRepository assignmentRepository;
        IReportRepository reportRepository;
        IDocumentRepository documentRepository;
        ISkillRepository skillRepository;
        public SchoolSupervisorController(ISchoolSupervisorRepository schoolSupervisorRepository, ISchoolRepository schoolRepository, ITrainingRepository trainingRepository, IAssignmentRepository assignmentRepository, IReportRepository reportRepository, IDocumentRepository documentRepository, ISkillRepository skillRepository)
        {
            this.schoolSupervisorRepository = schoolSupervisorRepository;
            this.schoolRepository = schoolRepository;
            this.trainingRepository = trainingRepository;
            this.assignmentRepository = assignmentRepository;
            this.reportRepository = reportRepository;
            this.documentRepository = documentRepository;
            this.skillRepository = skillRepository;
        }
        // CRUD
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.schoolSupervisors = schoolSupervisorRepository.GetAllSchoolSupervisors();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Add()
        {
            ViewBag.schools = schoolRepository.GetAllSchools();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(InsertSchoolSupervisorDTO schoolSupervisorDTO)
        {
            SchoolSupervisor schoolSupervisor = new SchoolSupervisor();
            schoolSupervisor.firstName = schoolSupervisorDTO.firstName;
            schoolSupervisor.secondName = schoolSupervisorDTO.secondName;
            schoolSupervisor.thirdName = schoolSupervisorDTO.thirdName;
            schoolSupervisor.lastName = schoolSupervisorDTO.lastName;
            schoolSupervisor.UserName = schoolSupervisorDTO.email;
            schoolSupervisor.Email = schoolSupervisorDTO.email;
            schoolSupervisor.NormalizedUserName = schoolSupervisorDTO.email.ToUpper();
            schoolSupervisor.NormalizedEmail = schoolSupervisorDTO.email.ToUpper();
            schoolSupervisor.schoolId = schoolSupervisorDTO.schoolId;
            schoolSupervisor.PhoneNumber = schoolSupervisorDTO.phoneNumber;
            schoolSupervisor.PhoneNumberConfirmed = true;
            schoolSupervisor.EmailConfirmed = true;
            await schoolSupervisorRepository.AddSchoolSupervisor(schoolSupervisor, schoolSupervisorDTO.password);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(string Id)
        {
            var schoolSupervisor = schoolSupervisorRepository.GetSchoolSupervisor(Id);
            var updateSchoolSupervisorDTO = new UpdateSchoolSupervisorDTO
            {
                firstName = schoolSupervisor.firstName,
                secondName = schoolSupervisor.secondName,
                thirdName = schoolSupervisor.thirdName,
                lastName = schoolSupervisor.lastName,
                schoolId = schoolSupervisor.schoolId,
                phoneNumber = schoolSupervisor.PhoneNumber
            };

            ViewBag.schools = schoolRepository.GetAllSchools();
            return View(updateSchoolSupervisorDTO);
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            await schoolSupervisorRepository.Delete(id);
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Update(UpdateSchoolSupervisorDTO schoolSupervisorDTO)
        {
            SchoolSupervisor schoolSupervisor = new SchoolSupervisor();
            schoolSupervisor.Id = schoolSupervisorDTO.Id;
            schoolSupervisor.firstName = schoolSupervisorDTO.firstName;
            schoolSupervisor.secondName = schoolSupervisorDTO.secondName;
            schoolSupervisor.thirdName = schoolSupervisorDTO.thirdName;
            schoolSupervisor.lastName = schoolSupervisorDTO.lastName;
            schoolSupervisor.schoolId = schoolSupervisorDTO.schoolId;
            schoolSupervisor.PhoneNumber = schoolSupervisorDTO.phoneNumber;

            await schoolSupervisorRepository.Update(schoolSupervisor);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]

        public IActionResult ViewStudentTrainings()
        {
            string loggedInId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewBag.trainings = trainingRepository.GetAllTrainingsForSchoolSupervisor(loggedInId);


            return View();
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]

        public IActionResult ViewTrainingAssignments(int id)
        {
            ViewBag.assignments = assignmentRepository.GetAssignmentsForTraining(id);
            ViewBag.trainingId = id;


            return View();
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]

        public IActionResult ViewAssignmentReports(int assignmentId)
        {
            var reportLogs = reportRepository.GetReportLogs(assignmentId);
            ViewBag.reportLogs = reportLogs;
            ViewBag.assignmentId = assignmentId;
            return View();
        }
        [Authorize(Roles = "SCHOOLSUPERVISOR")]
        public async Task<IActionResult> ViewReportDetails(int reportId)
        {
            var report = reportRepository.GetReport(reportId);

            if (report == null)
            {
                // Handle the case where the Report is not found
                return NotFound();
            }

            var reportDocuments = reportRepository.GetReportDocuments(reportId);


            ViewBag.reportDoc = reportDocuments;

            return View(report);
        }
        public IActionResult Dashboard()
        {
            var reports = reportRepository.GetAllReports();
            int approvedCount = 0;
            int pendingCount = 0;
            int rejectedCount = 0;

            foreach (var report in reports)
            {
                if (report.reportStatusId == 1)
                {
                    approvedCount++;
                }
                else if (report.reportStatusId == 2)
                {
                    pendingCount++;
                }
                else
                {
                    rejectedCount++;
                }
            }

            // Total count for calculating percentages
            int totalCount = approvedCount + pendingCount + rejectedCount;

            // Calculate percentages and round to one decimal place
            double approvedPercentage = Math.Round((double)approvedCount / totalCount * 100, 1);
            double pendingPercentage = Math.Round((double)pendingCount / totalCount * 100, 1);
            double rejectedPercentage = Math.Round((double)rejectedCount / totalCount * 100, 1);

            ViewBag.ApprovedPercentage = approvedPercentage;
            ViewBag.PendingPercentage = pendingPercentage;
            ViewBag.RejectedPercentage = rejectedPercentage;
            ViewBag.RejectedCount = rejectedCount;
            ViewBag.PendingCount = pendingCount;
            ViewBag.ApprovedCount = approvedCount;

            string loggedInId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Training> allTrainings = trainingRepository.GetAllTrainingsForSchoolSupervisor(loggedInId);
            List<TrainingViewModel> viewModels = new List<TrainingViewModel>();

            foreach (var training in allTrainings)
            {
                List<Skill> achievedSkills = skillRepository.GetAchievedSkillsForTraining(training.trainingId);

                var viewModel = new TrainingViewModel
                {
                    Training = training,
                    AchievedSkills = achievedSkills
                };

                viewModels.Add(viewModel);
            }

            ViewBag.TrainingViewModels = viewModels;

            // Count the number of tasks for school supervisor trainings
            int totalTasks = allTrainings.Sum(training => assignmentRepository.GetTaskCountForTraining(training.trainingId));

            ViewBag.TotalTasks = totalTasks;

            Dictionary<string, int> skillDistribution = new Dictionary<string, int>();

            foreach (var training in allTrainings)
            {
                // Fetch the achieved skills for the training (when the assignment report is approved)
                List<Skill> achievedSkills = skillRepository.GetAchievedSkillsForTraining(training.trainingId);

                // Increment the count for each skill in the dictionary
                foreach (var skill in achievedSkills)
                {
                    if (skillDistribution.ContainsKey(skill.skillName))
                    {
                        skillDistribution[skill.skillName]++;
                    }
                    else
                    {
                        skillDistribution[skill.skillName] = 1;
                    }
                }
            }

            // Convert the dictionary to a format suitable for the chart
            var skillsData = skillDistribution.Select(pair => new { label = pair.Key, value = pair.Value }).ToList();

            ViewBag.SkillsData = skillsData;

            return View();
        }






    }

}

