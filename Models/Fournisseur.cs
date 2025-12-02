using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Fournisseur
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du fournisseur est obligatoire")]
        public string Nom { get; set; }

        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string Activite { get; set; }
        public string Rc { get; set; }
        public string Ai { get; set; }
        public string Nif { get; set; }
        public string Nis { get; set; }

        // Remplace Remarques par Solde
        public double? Solde { get; set; } // اختياري ويمكن أن يكون null
    }
}