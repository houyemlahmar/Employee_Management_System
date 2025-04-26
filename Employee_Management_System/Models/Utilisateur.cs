using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class Utilisateur
    {
        
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Username { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            [StringLength(50)]
            public string Role { get; set; } 
        
    }


}

