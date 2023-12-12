namespace ApprenticeshipWebApplication.DTO
{
    public class InsertDocumentDTO
    {
        public string documentName { get; set; }
        public string contentType { get; set; }
        public byte[] content { get; set; }
        public int reportId { get; set; }
        public int assignmentId { get; set; }
    }
}
