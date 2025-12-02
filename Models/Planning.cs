using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Planning
    {
        public int Id { get; set; }

        [Required]
        public int ChauffeurId { get; set; }
        public Chauffeur Chauffeur { get; set; }

        [Required]
        public DateTime DatePlanning { get; set; }

        [Required]
        public string Camion { get; set; }

        [Required]
        public string Etat { get; set; } // Prévue / EnTournee / Terminee
    }
}