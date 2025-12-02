using System;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(50)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateTime? DateNaissance { get; set; }

        [StringLength(100)]
        [Display(Name = "Adresse")]
        public string Adresse { get; set; }

        [StringLength(50)]
        [Display(Name = "Poste occupé")]
        public string Poste { get; set; }

        [StringLength(20)]
        [Display(Name = "N° Sécurité Sociale")]
        public string NSS { get; set; } // رقم الضمان الاجتماعي

        [DataType(DataType.Date)]
        [Display(Name = "Date de recrutement")]
        public DateTime DateRecrutement { get; set; } = DateTime.Now;

        [Range(0, 1000000, ErrorMessage = "Le salaire doit être positif.")]
        [Display(Name = "Salaire de base (DA)")]
        [DataType(DataType.Currency)]
        public double SalaireBase { get; set; }

        [EmailAddress(ErrorMessage = "Adresse email invalide.")]
        [Display(Name = "Email professionnel")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide.")]
        [Display(Name = "Téléphone")]
        public string Telephone { get; set; }
    }
}