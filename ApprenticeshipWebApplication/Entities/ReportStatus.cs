namespace ApprenticeshipWebApplication.Entities
{
    public class ReportStatus //look up table (Static Information that will not change)
    {
        public int reportStatusId { get; set; }
        public string reportStatusName { get; set; }
        public List <Report> reports  { get; set; }


    }
}
