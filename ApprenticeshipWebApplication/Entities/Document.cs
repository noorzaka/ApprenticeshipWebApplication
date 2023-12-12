namespace ApprenticeshipWebApplication.Entities
{
    public class Document
    {
        public int documentId { get; set; }
        public string documentName { get; set; }
        public string contentType { get; set; }
        // Add any other properties related to documents
        public byte[] content { get; set; }
        public int? reportId { get; set; }
        public int? assignmentId { get; set; }
        public Assignment assignment { get; set; }
        public Report report { get; set; }
        public int? reportLogId { get; set; }
        public DateTime createdAt { get; set; }
        public ReportLog reportLog { get; set; }

    }
}
