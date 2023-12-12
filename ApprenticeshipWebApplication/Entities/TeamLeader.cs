namespace ApprenticeshipWebApplication.Entities
{
    public class TeamLeader : Person
    { 
     public List<Training> apprenticeships { get; set; }
        public int companyId { get; set; }
        public Company company { get; set; }

    }
}
