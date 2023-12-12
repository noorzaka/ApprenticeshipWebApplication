namespace ApprenticeshipWebApplication.Entities
{
    public class Student : Person
    { public List<Training> apprenticeships { get; set; }
        public int schoolId { get; set; }
        public School school { get; set; }

    }
}
