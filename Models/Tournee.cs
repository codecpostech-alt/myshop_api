namespace MyShop.Models
{
    public class Tournee
    {
        public int Id { get; set; }
        public int ChauffeurId { get; set; }
        public int PreVendeurId { get; set; }
        public string Zone { get; set; }
        public DateTime DateTournee { get; set; }
        public string Statut { get; set; } = "Planifiee"; // Planifiee, EnCours, Terminee

        // Navigation
        public Chauffeur Chauffeur { get; set; }
        public PreVendeur PreVendeur { get; set; }
        public ICollection<TourneeClient> Clients { get; set; }
    }
}
