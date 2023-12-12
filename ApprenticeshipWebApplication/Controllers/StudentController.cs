using ApprenticeshipWebApplication.Data.Migrations;
using ApprenticeshipWebApplication.DTO;
using ApprenticeshipWebApplication.Entities;
using ApprenticeshipWebApplication.Models;
using ApprenticeshipWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Composition;
using System.Data;
using System.Security.Claims;

namespace ApprenticeshipWebApplication.Controllers
{
    public class StudentController : Controller
    { 
        IStudentRepository studentRepository;
        ISchoolRepository schoolRepository;
        ITrainingRepository trainingRepository;
        IAssignmentRepository assignmentRepository;
        IReportRepository reportRepository;
        IDocumentRepository documentRepository;
        ISkillRepository skillRepository;
        public StudentController(IStudentRepository studentRepository, ISchoolRepository schoolRepository, ITrainingRepository trainingRepository, IAssignmentRepository assignmentRepository,IReportRepository reportRepository, IDocumentRepository documentRepository, ISkillRepository skillRepository)
        {
            this.studentRepository = studentRepository;
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
            ViewBag.students = studentRepository.GetAllStudents();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Add()
        {
            ViewBag.schools = schoolRepository.GetAllSchools();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(InsertStudentDTO studentDTO)
        {
            Student student = new Student();
            student.firstName = studentDTO.firstName;
            student.secondName = studentDTO.secondName;
            student.thirdName = studentDTO.thirdName;
            student.lastName = studentDTO.lastName;
            student.UserName = studentDTO.email;
            student.Email = studentDTO.email;
            student.NormalizedUserName = studentDTO.email.ToUpper();
            student.NormalizedEmail = studentDTO.email.ToUpper();
            student.schoolId = studentDTO.schoolId;
            student.PhoneNumber = studentDTO.phoneNumber;
            student.PhoneNumberConfirmed = true;
            student.EmailConfirmed = true;
            await studentRepository.AddStudent(student, studentDTO.password);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(string Id)
        {
            var student = studentRepository.GetStudent(Id);

            // Assuming you have a mapper, map student to UpdatestudentDTO
            var updateStudentDTO = new UpdateStudentDTO
            {
                firstName = student.firstName,
                secondName = student.secondName,
                thirdName = student.thirdName,
                lastName = student.lastName,
                schoolId = student.schoolId,
                phoneNumber = student.PhoneNumber
                // Map other properties as needed
            };

            ViewBag.schools = schoolRepository.GetAllSchools();
            return View(updateStudentDTO);
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            await studentRepository.Delete(id);
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(UpdateStudentDTO studentDTO)
        {
            Student student = new Student();
            student.Id = studentDTO.Id;
            student.firstName = studentDTO.firstName;
            student.secondName = studentDTO.secondName;
            student.thirdName = studentDTO.thirdName;
            student.lastName = studentDTO.lastName;
            student.schoolId = studentDTO.schoolId;
            student.PhoneNumber = studentDTO.phoneNumber;

            await studentRepository.Update(student);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "STUDENT")]
        public IActionResult ViewStudentAssignments()
        {
            string loggedInStudent = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var assignments = assignmentRepository.GetAssignmentsForStudent(loggedInStudent);
            ViewBag.assignments = assignments;
            var documentsByAssignment = new Dictionary<int, List<Document>>(); // Dictionary to store reportId and its documents

            foreach (var assignment in assignments)
            {
                var documents = documentRepository.GetAssignmentDocuments(assignment.assignmentId);
                documentsByAssignment.Add(assignment.assignmentId, documents);
            }
            ViewBag.documentsByAssignment = documentsByAssignment;
            int approvedReportsCount = reportRepository.GetApprovedReportsForStudent(loggedInStudent).Count;
            int rejectedReportsCount = reportRepository.GetRejectedReportsForStudent(loggedInStudent).Count;
            int pendingReportsCount = reportRepository.GetPendingReportsForStudent(loggedInStudent).Count;

            ViewBag.ApprovedReportsCount = approvedReportsCount;
            ViewBag.RejectedReportsCount = rejectedReportsCount;
            ViewBag.PendingReportsCount = pendingReportsCount;

            ViewBag.trainings = trainingRepository.GetTrainingsForStudent(loggedInStudent);

            // Assuming you have a way to retrieve trainingIds for the logged-in student
            var trainingId = trainingRepository.GetTrainingIdForStudent(loggedInStudent); // Adjust this based on your actual implementation

            var allSkillsForTrainingObjectives = skillRepository.GetAllSkillsForTrainingObjectives(loggedInStudent);
            ViewBag.AllSkillsForTrainingObjectives = allSkillsForTrainingObjectives;
            ViewBag.AchievedSkills = skillRepository.GetAchievedSkillsForTraining(trainingId);

            return View();
        }




        [Authorize(Roles = "STUDENT")]

        public IActionResult ViewReports(int assignmentId)
        {
            // Retrieve reports for the given assignment
            var reports = reportRepository.GetAssignmentsReports(assignmentId);

            var documentsByReport = new Dictionary<int, List<Document>>(); // Dictionary to store reportId and its documents

            foreach (var report in reports)
            {
                var documents = documentRepository.GetReportDocuments(report.reportId);
                documentsByReport.Add(report.reportId, documents);
            }

             ViewBag.reports = reports;
             ViewBag.documentsByReport = documentsByReport;
             ViewBag.assignmentId = assignmentId;

            return View();
        }

        [Authorize(Roles = "STUDENT")]

        public IActionResult SubmitNewReport(int assignmentId)
        {
            ViewBag.assignmentId = assignmentId;
            return View();
        }
        [Authorize(Roles = "STUDENT")]
        [HttpPost]
        public async Task<IActionResult> CreateNewReport(InsertReportDTO reportDTO, List<IFormFile> formFiles)
        {
            try
            {
                // Create the initial report
                Report report = new Report
                {
                    assignmentId = reportDTO.assignmentId,
                    reportName = reportDTO.reportName,
                    reportDescription = reportDTO.reportDescription,
                    reportNotes = reportDTO.reportNotes,
                    reportStatusId = 2 // 2 represents pending approval
                };

                // Add the report and get the created report entity
                await reportRepository.AddReport(report);

                // Create a report log for the initial report status
                await reportRepository.AddReportLog(report, 2); // 2 for pending approval

                // Get the created report log entity
                var reportLog = reportRepository.GetReportLogByReportId(report.reportId); // Await here

                int reportId = report.reportId;

                // Process each uploaded file
                foreach (var formFile in formFiles)
                {
                    if (formFile.Length > 0)
                    {
                        using (Stream stream = formFile.OpenReadStream())
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            byte[] content = reader.ReadBytes((int)stream.Length);

                            // Create a document associated with the report log
                            Document document = new Document
                            {
                                documentName = formFile.FileName,
                                contentType = formFile.ContentType,
                                content = content,
                                reportLogId = reportLog.reportLogId,
                                reportId=reportId
                            };

                            // Add the document
                            await documentRepository.AddDocument(document);
                        }
                    }
                }

                return RedirectToAction("ViewReports", new { assignmentId = report.assignmentId });
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., logging, displaying an error message)
                return RedirectToAction("ViewReports", new { assignmentId = reportDTO.assignmentId });
            }
        }


        public IActionResult GetDocument(int documentId)
        {
            var file = documentRepository.GetDoc(documentId);
            Stream stream = new MemoryStream(file.content);
            return new FileStreamResult(stream, file.contentType);
        }


        [Authorize(Roles = "STUDENT")]

        public IActionResult EditReport(int reportId)
        { var report=reportRepository.GetReport(reportId);

            UpdateReportDTO reportDTO = new UpdateReportDTO();
            reportDTO.assignmentId = report.assignmentId;
            reportDTO.reportName = report.reportName;
            reportDTO.reportDescription = report.reportDescription;
            reportDTO.reportNotes = report.reportNotes;
            ViewBag.documents = documentRepository.GetReportDocuments(report.reportId);
            ViewBag.assignmentId = report.assignmentId;
            ViewBag.reportId = report.reportId;

            return View(reportDTO);



        }
        [Authorize(Roles = "STUDENT")]
        public async Task<IActionResult> UpdateReport(UpdateReportDTO reportDTO, List<IFormFile> formFiles)
        {
            var report = reportRepository.GetReport(reportDTO.reportId);
            report.reportName = reportDTO.reportName;
            report.reportDescription = reportDTO.reportDescription;
            report.reportNotes = reportDTO.reportNotes;
            report.reportStatusId = 2; // Assuming status code 2 for pending approval

            await reportRepository.UpdateTheSelectedReport(report);

            // Get the latest report log entry for the updated report
            var latestReportLog =  reportRepository.GetReportLogByReportId(report.reportId);

            int reportId = report.reportId;

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
                            reportId = reportId,
                            reportLogId = latestReportLog.reportLogId // Pass the latest report log Id
                        };

                        await documentRepository.AddDocument(document);
                    }
                }
            }

            return RedirectToAction("ViewReports", new { assignmentId = report.assignmentId });
        }

        [Authorize(Roles = "STUDENT")]
       /* public async Task<IActionResult> DeleteReport(int reportId, int assignmentId)
        {
            var report = reportRepository.GetReport(reportId);
            var documents = documentRepository.GetReportDocuments(reportId);
            foreach (var document in documents)
            {
                await documentRepository.DeleteDoc(document.documentId);
            }
            await reportRepository.DeleteReport(reportId);
            return RedirectToAction("ViewReports", new { assignmentId = assignmentId });
        }*/

        public async Task<IActionResult> DeleteDocument(int documentId, int assignmentId,int reportId)
        {
            await documentRepository.DeleteDoc(documentId);
            return RedirectToAction("EditReport", new { reportId = reportId });
        }
        public IActionResult ViewApprovedReports()
        {
            string loggedInStudent = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var approvedReports = reportRepository.GetApprovedReportsForStudent(loggedInStudent);
            var assignments = assignmentRepository.GetAssignmentsForStudent(loggedInStudent);
            ViewBag.assignments = assignments;
            var documentsByAssignment = new Dictionary<int, List<Document>>(); // Dictionary to store reportId and its documents

            foreach (var assignment in assignments)
            {
                var documents = documentRepository.GetAssignmentDocuments(assignment.assignmentId);
                documentsByAssignment.Add(assignment.assignmentId, documents);
            }
            ViewBag.documentsByAssignment = documentsByAssignment;
            return View(approvedReports);
        }

        // Action to view reports with status "Rejected"
        public IActionResult ViewRejectedReports()
        {
            string loggedInStudent = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var rejectedReports = reportRepository.GetRejectedReportsForStudent(loggedInStudent);
            var assignments = assignmentRepository.GetAssignmentsForStudent(loggedInStudent);
            ViewBag.assignments = assignments;
            var documentsByAssignment = new Dictionary<int, List<Document>>(); // Dictionary to store reportId and its documents

            foreach (var assignment in assignments)
            {
                var documents = documentRepository.GetAssignmentDocuments(assignment.assignmentId);
                documentsByAssignment.Add(assignment.assignmentId, documents);
            }
            ViewBag.documentsByAssignment = documentsByAssignment;
            return View(rejectedReports);
        }

        // Action to view reports with status "Pending"
        public IActionResult ViewPendingReports()
        {
            string loggedInStudent = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var pendingReports = reportRepository.GetPendingReportsForStudent(loggedInStudent);
            var assignments = assignmentRepository.GetAssignmentsForStudent(loggedInStudent);
            ViewBag.assignments = assignments;
            var documentsByAssignment = new Dictionary<int, List<Document>>(); // Dictionary to store reportId and its documents

            foreach (var assignment in assignments)
            {
                var documents = documentRepository.GetAssignmentDocuments(assignment.assignmentId);
                documentsByAssignment.Add(assignment.assignmentId, documents);
            }
            ViewBag.documentsByAssignment = documentsByAssignment;
            return View(pendingReports);
        }
    }

}



