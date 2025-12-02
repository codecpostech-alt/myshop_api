using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Models
{
    [Table("Produits")]
    public class Produit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire.")]
        [StringLength(100)]
        public string Nom { get; set; }

        [StringLength(50)]
        public string Categorie { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive.")]
        public int Quantite { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le prix d'achat doit être un nombre positif.")]
        public int PrixAchat { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le prix de vente doit être un nombre positif.")]
        public int PrixVente { get; set; }

        [StringLength(50)]
        [Display(Name = "Barcode")]
        public string Barcode { get; set; } // <-- الحقل الجديد للباركود
    }
}