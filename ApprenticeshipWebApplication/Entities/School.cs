namespace ApprenticeshipWebApplication.Entities
{
    public class School
    {
        public int schoolId { get; set; }
        public string schoolName { get; set; }
        public string schoolAddress { get; set;}
        public List<SchoolSupervisor> schoolSupervisors { get; set; }
        public List<Student> students { get; set; }


    }


}
