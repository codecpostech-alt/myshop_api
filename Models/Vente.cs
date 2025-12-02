using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Vente
    {
        public int Id { get; set; }
        public DateTime DateVente { get; set; } = DateTime.Now;
        public string ClientNom { get; set; }
        public double MontantTotal { get; set; }

        // تفاصيل البيع
        public List<VenteDetail> VenteDetails { get; set; } = new List<VenteDetail>(); // مهيأة
    }

    public class VenteDetail
    {
        public int Id { get; set; }
        public int VenteId { get; set; } // FK
        public int ProduitId { get; set; } // FK
        public string ProduitNom { get; set; }
        public int Quantite { get; set; }
        public double PrixUnitaire { get; set; }
        public double Total => PrixUnitaire * Quantite;

        // Navigation property (اختياري لكن مفيد)
        public Vente Vente { get; set; }
        }
    }