namespace ApprenticeshipWebApplication.Entities
{
    public class TrainingObjectives
    {
       public int trainingId { get; set; }
       public int objectiveId { get; set; }
       public Training training { get; set; }
        public Objective objective { get; set; }



    }
}
