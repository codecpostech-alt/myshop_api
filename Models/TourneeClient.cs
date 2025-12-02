namespace MyShop.Models
{
    public class TourneeClient
    {
        public int Id { get; set; }
        public int TourneeId { get; set; }
        public string ClientNom { get; set; }
        public string Adresse { get; set; }
        public int Ordre { get; set; }

        // Navigation
        public Tournee Tournee { get; set; }
    }
}
