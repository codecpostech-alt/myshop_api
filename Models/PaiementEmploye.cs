using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Models
{
    public class PaiementEmploye
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'employé est obligatoire.")]
        [Display(Name = "Employé")]
        public int EmployeeId { get; set; }  // ✅ فقط هذا الحقل مطلوب في النموذج

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }  // ❌ أزل الـ Required عنها

        [Required(ErrorMessage = "La date de paiement est obligatoire.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de paiement")]
        public DateTime DatePaiement { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0, 100000000, ErrorMessage = "Montant invalide.")]
        [Display(Name = "Montant payé (DA)")]
        public double Montant { get; set; }

        [StringLength(200)]
        [Display(Name = "Remarque")]
        public string? Remarque { get; set; }
    }
}