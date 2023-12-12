using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Models
{
    public interface IReportRepository
    {
       public List<Report> GetAssignmentsReports(int assignmentId);
       public Task  AddReport(Report report);
       public Report GetReport(int reportId);
       public Task UpdateTheSelectedReport(Report report);
       public Task DeleteReport(int reportId);
       public Task ApproveReport(int reportId);
       public Task RejectReport(int reportId);
        public Task AddReportLog(Report report, int logStatusId);
        public List<ReportLog> GetReportLogs(int assignmentId);
        public ReportLog GetReportLog(int reportLogId);
        public List<Document> GetReportDocuments(int reportLogId);
        public ReportLog GetReportLogByReportId(int reportId);
        public List<Report> GetAllReports();
        public List<Report> GetApprovedReportsForStudent(string studentId);
        public List<Report> GetRejectedReportsForStudent(string studentId);
        public List<Report> GetPendingReportsForStudent(string studentId);







        //public Task AddReportLog(Report report, int logStatusId);
        // public List<ReportLog> GetReportLogs(int assignmentId);




    }
}
