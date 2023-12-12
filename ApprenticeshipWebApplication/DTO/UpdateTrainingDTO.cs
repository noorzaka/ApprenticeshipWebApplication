namespace ApprenticeshipWebApplication.DTO
{
    public class UpdateTrainingDTO
    {
        public int id { get; set; }
        public string studentId { get; set; }
        public string trainingName { get; set; }

        public string teamLeaderId { get; set; }
        public string schoolSupervisorId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<int> objectiveIds { get; set; }
    }
}
