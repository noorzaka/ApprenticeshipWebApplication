namespace ApprenticeshipWebApplication.DTO
{
    public class InsertReportDTO
    {
        public int assignmentId { get; set; }
        public string reportName { get; set; }
        public string reportDescription { get; set; }
        public string reportNotes { get; set; }
        public List<InsertDocumentDTO> documents { get; set; }
        public int reportLogId { get; set; }
    }
}
