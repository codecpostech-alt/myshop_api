namespace MyShop.Models
{
    public class RetourProduit
    {
        public int Id { get; set; }
        public int TourneeId { get; set; }
        public string Produit { get; set; }
        public int Quantite { get; set; }
        public string TypeRetour { get; set; } // Casse, Perime, RetourClient
        public DateTime DateRetour { get; set; } = DateTime.Now;

        public Tournee Tournee { get; set; }
    }
}
