using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities;
using ApprenticeshipWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipWebApplication.Repositories
{
    public class ReportRepository : IReportRepository
    {
        ApplicationDbContext context;
        public ReportRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Report> GetAssignmentsReports(int assignmentId)
        {
           return context.reports
                .Where(x =>x.assignmentId== assignmentId)
                .Include(x=> x.reportStatus).ToList();
        }
        public async Task AddReport(Report report)
        {
            context.reports.Add(report);
            await context.SaveChangesAsync();
        }
        public Report GetReport(int reportId)
        {
            return context.reports.SingleOrDefault(x => x.reportId== reportId);
        }
        public async Task UpdateTheSelectedReport(Report report)
        {
            var oldReport = GetReport(report.reportId);

           oldReport.reportName= report.reportName;
           oldReport.reportDescription= report.reportDescription;
           oldReport.reportNotes= report.reportNotes;
           oldReport.reportStatusId= report.reportStatusId;
            context.Update(oldReport);
            await context.SaveChangesAsync();
        }
      
        public async Task ApproveReport(int reportId)
        {
            var report = context.reports.Find(reportId);
            report.reportStatusId = 1;

            await AddReportLog(report, 1); 
            await context.SaveChangesAsync();
        }

        public async Task RejectReport(int reportId)
        {
            var report = context.reports.Find(reportId);
            report.reportStatusId = 3;

            await AddReportLog(report, 3); // Log status code 3 for rejected
            await context.SaveChangesAsync();
        }
        public async Task AddReportLog(Report report, int logStatusId)
        {
            var reportLogEntry = new ReportLog
            {
                reportId = report.reportId,
                reportStatusId = logStatusId,
                reportName = report.reportName,
                reportDescription = report.reportDescription,
                reportNotes = report.reportNotes,
                logDate = DateTime.Now
            };

            context.reportLogs.Add(reportLogEntry);
            await context.SaveChangesAsync();
        }
        public List<ReportLog> GetReportLogs(int assignmentId)
        {
            var reportIdsForAssignment = context.reports
                .Where(r => r.assignmentId == assignmentId)
                .Select(r => r.reportId)
                .ToList();

            return context.reportLogs
                .Where(x => reportIdsForAssignment.Contains(x.reportId))
                .OrderByDescending(x => x.logDate)
                .ToList();
        }
        public async Task DeleteReport(int reportId)
        {
            var report = await context.reports.FindAsync(reportId);
                context.reports.Remove(report);
                await context.SaveChangesAsync();
           
        }
        public ReportLog GetReportLog(int reportLogId)
        {
            return context.reportLogs.SingleOrDefault(x => x.reportLogId == reportLogId);
        }


        public List<Document> GetReportDocuments(int reportId)
        {
            return context.documents
                .Where(x => x.report.reportId == reportId)
                .OrderByDescending(x => x.createdAt)
                .ToList();
        }


        public ReportLog GetReportLogByReportId(int reportId)
        {
            return context.reportLogs
            .Where(x => x.reportId == reportId)
            .OrderByDescending(x => x.logDate)
            .FirstOrDefault();

           


    }
        public  List<Report> GetAllReports()
        {
            return context.reports.ToList();
        }

        public List<Report> GetApprovedReportsForStudent(string studentId)
        {
            return context.reports
                .Include(r => r.assignment)
                    .ThenInclude(a => a.training)
                .Include(r => r.reportStatus)
                .Where(r => r.assignment.training.studentId == studentId && r.reportStatus.reportStatusId == 1).ToList();
            
        }

        public List<Report> GetRejectedReportsForStudent(string studentId)
        {
            return context.reports
                .Include(r => r.assignment)
                    .ThenInclude(a => a.training)
                .Include(r => r.reportStatus)
                .Where(r => r.assignment.training.studentId == studentId && r.reportStatus.reportStatusId == 3).ToList();
        }

        public List<Report> GetPendingReportsForStudent(string studentId)
        {
            return context.reports
                .Include(r => r.assignment)
                    .ThenInclude(a => a.training)
                .Include(r => r.reportStatus)
                .Where(r => r.assignment.training.studentId == studentId && r.reportStatus.reportStatusId == 2).ToList();
        }





    }
}
