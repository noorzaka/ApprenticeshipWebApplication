namespace ApprenticeshipWebApplication.Entities
{
    public class Skill
    {
        public int skillId { get; set; }
        public string skillName { get; set; }
        public List<ObjectiveSkills> objectiveSkills { get; set; }
    }
}
