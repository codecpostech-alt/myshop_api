namespace MyShop.Models
{
    public class LivraisonProduit
    {
        public int Id { get; set; }
        public int LivraisonId { get; set; }
        public string Produit { get; set; }
        public int QteDemandee { get; set; }
        public int QteLivree { get; set; }

        public Livraison Livraison { get; set; }
    }
}
