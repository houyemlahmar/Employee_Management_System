namespace Employee_Management_System.Models
{
    public class SalaryBonus
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public ICollection<Employe>? Employes { get; set; }

    }

}
