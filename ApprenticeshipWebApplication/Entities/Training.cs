namespace ApprenticeshipWebApplication.Entities
{
    public class Training
    {  public int trainingId { get; set; } 
        public string trainingName { get; set; }
        public Student student { get; set; }
        public string studentId  { get; set; }
        public TeamLeader teamLeader { get; set; }
        public string teamLeaderId { get; set; }
        public SchoolSupervisor schoolSupervisor { get; set; }
        public string schoolSupervisorId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List <Assignment> assignments { get; set; } //كم اسايمنت انجو وكم واحد ضايل
        public int? estimatedNumberOfTasks { get; set; } = 0;

        public List<TrainingObjectives> trainingObjectives { get; set; }



    }
}
