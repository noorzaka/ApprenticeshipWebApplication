namespace ApprenticeshipWebApplication.Entities
{
    public class Company
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public List <TeamLeader> teamLeaders { get; set; }
    }
}
