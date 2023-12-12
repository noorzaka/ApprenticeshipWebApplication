namespace ApprenticeshipWebApplication.Entities
{
    public class Assignment
    {
        public int assignmentId { get; set; }
        public string assignmentTitle { get; set; }
        public string assignmentDescription { get; set; }
        public string assignmentNotes { get; set; }
        public int trainingId { get; set; }
        public Training training { get; set; }
        public List <Report> reports { get; set; }
        public List<AssignmentObjectives> assignmentObjectives { get; set; }
        public List<Document> documents { get; set; } // Documents related to assignments
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public TimeSpan TimeRemaining
        {
            get
            {
                // Calculate time remaining based on current date/time and assignment's end date
                return endDate - DateTime.Now;
            }
        }




    }
}
