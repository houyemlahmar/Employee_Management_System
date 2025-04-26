namespace Employee_Management_System.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public ICollection<Employe>? Employes { get; set; }
    }

}
