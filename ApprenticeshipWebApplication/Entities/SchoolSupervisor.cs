namespace ApprenticeshipWebApplication.Entities
{
    public class SchoolSupervisor : Person
    {
        public List<Training> trainings { get; set; }
        public School school { get; set; }
        public int schoolId { get; set; }

    }
}
