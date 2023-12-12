namespace ApprenticeshipWebApplication.Entities
{
    public class ReportLog
    {
        public int reportLogId { get; set; }
        public int reportId { get; set; }
        public int reportStatusId { get; set; }
        public string reportName { get; set; }
        public string reportDescription { get; set; }
        public string reportNotes { get; set; }
        public DateTime logDate { get; set; }
        public Report report { get; set; }
        public List<Document> documents { get; set; }



    }
}
