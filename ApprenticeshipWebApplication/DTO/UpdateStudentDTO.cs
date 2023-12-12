namespace ApprenticeshipWebApplication.DTO
{
    public class UpdateStudentDTO
    {
        public string Id { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string thirdName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmedPassword { get; set; }
        public string phoneNumber { get; set; }
        public int schoolId { get; set; }
    }
}
