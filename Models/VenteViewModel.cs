using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class VenteProduit
    {
        public int ProduitId { get; set; }
        public string ProduitNom { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantité doit être positive")]
        public int Quantite { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Prix doit être positif")]
        public double PrixUnitaire { get; set; }

        public double Total => Quantite * PrixUnitaire;
    }

    public class VenteViewModel
    {
        public string ClientNom { get; set; }
        public DateTime DateVente { get; set; } = DateTime.Now;
        public List<VenteProduit> Produits { get; set; } = new List<VenteProduit>();

        // **إضافة هذه الخاصية لإظهار جميع المنتجات في View**
        public List<Produit> ProduitsList { get; set; } = new List<Produit>();

        public double MontantTotal
        {
            get
            {
                double total = 0;
                foreach (var p in Produits)
                    total += p.Total;
                return total;
            }
        }
    }
}