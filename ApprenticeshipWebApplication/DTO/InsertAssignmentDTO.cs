namespace ApprenticeshipWebApplication.DTO
{
    public class InsertAssignmentDTO
    {
        public int trainingId { get; set; }
        public string assignmentTitle { get; set; }
        public string assignmentDescription { get; set; }
        public string assignmentNotes { get; set; }
        public List<int> objectiveIds { get; set; }
        public List<InsertDocumentDTO> documents { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
}
