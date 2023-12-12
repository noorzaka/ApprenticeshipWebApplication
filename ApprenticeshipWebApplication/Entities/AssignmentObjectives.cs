namespace ApprenticeshipWebApplication.Entities
{
    public class AssignmentObjectives
    {
        public int assignmentId { get; set; }
        public int objectiveId { get; set; }
        public Assignment assignment { get; set; }
        public Objective objective { get; set; }
    }
}
