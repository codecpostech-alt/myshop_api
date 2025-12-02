using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Chauffeur
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public string NumeroPermis { get; set; }

        [Required]
        public DateTime? ExpirationPermis { get; set; }

        public string? Camion { get; set; }

        public string Statut { get; set; } = "Disponible";
        [ValidateNever]
        public ICollection<Tournee> Tournees { get; set; }
    }
}