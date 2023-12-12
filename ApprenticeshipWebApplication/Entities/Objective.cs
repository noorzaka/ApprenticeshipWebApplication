namespace ApprenticeshipWebApplication.Entities
{
    public class Objective //look up table
    {
        public int objectiveId { get; set; }
        public string objectiveName { get; set;}
        public List<ObjectiveSkills> objectiveSkills { get; set; }
        public List<TrainingObjectives> trainingObjectives { get; set; }
        public List<AssignmentObjectives> assignmentObjectives{ get; set; }



    }
}
