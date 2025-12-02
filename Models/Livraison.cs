namespace MyShop.Models
{
    public class Livraison
    {
        public int Id { get; set; }
        public int TourneeId { get; set; }
        public string ClientNom { get; set; }
        public DateTime DateLivraison { get; set; }
        public bool EstLivre { get; set; }
        public string SignatureClient { get; set; } // Base64 image
        public decimal MontantPaye { get; set; }
        public string ModePaiement { get; set; } // Especes, Cheque, Terme

        // Navigation
        public Tournee Tournee { get; set; }
        public ICollection<LivraisonProduit> Produits { get; set; }
    }
}
