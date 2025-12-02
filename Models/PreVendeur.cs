namespace MyShop.Models
{
    public class PreVendeur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string Zone { get; set; }
        public decimal ObjectifMensuel { get; set; }
        public decimal RealisationMensuelle { get; set; }

        // Navigation
        public ICollection<Tournee> Tournees { get; set; }
    }
}