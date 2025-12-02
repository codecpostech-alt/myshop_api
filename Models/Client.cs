using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; }

        public string Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Email invalide")]
        public string Email { get; set; }

        public string Adresse { get; set; }
        public string Activite { get; set; }
        public string Rc { get; set; }
        public string Ai { get; set; }
        public string Nif { get; set; }
        public string Nis { get; set; }
    }
}