namespace ApprenticeshipWebApplication.DTO
{
    public class UpdateReportDTO
    {
        public int assignmentId { get; set; }
        public int reportId { get; set; }
        //public int reportStatusId{ get; set; }

        public string reportName { get; set; }
        public string reportDescription { get; set; }
        public string reportNotes { get; set; }

    }
}
