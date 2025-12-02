using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // جدول المستخدمين
        public DbSet<User> Users { get; set; }

        // جدول العملاء
        public DbSet<Client> Clients { get; set; }

        public DbSet<StoreSettings> StoreSettings { get; set; }
        public DbSet<Chauffeur> Chauffeurs { get; set; }
        public DbSet<PreVendeur> PreVendeurs { get; set; }
        public DbSet<Tournee> Tournees { get; set; }
        public DbSet<TourneeClient> TourneeClients { get; set; }

        public DbSet<Planning> Plannings { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }
        public DbSet<LivraisonProduit> LivraisonProduits { get; set; }
        public DbSet<RetourProduit> RetourProduits { get; set; }
        public DbSet<PaymentRecord> Payments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        // ✅ جدول المصاريف
        public DbSet<Frais> Frais { get; set; }


        public DbSet<PaiementEmploye> PaiementEmployes { get; set; }

        // جدول الموردين
        public DbSet<Fournisseur> Fournisseurs { get; set; }

        // جدول المنتجات
        public DbSet<Produit> Produits { get; set; }

        // جدول المبيعات وتفاصيلها
        public DbSet<Vente> Ventes { get; set; }
        public DbSet<VenteDetail> VenteDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // العلاقة بين Vente و VenteDetail
            modelBuilder.Entity<Vente>()
                .HasMany(v => v.VenteDetails)
                .WithOne(d => d.Vente)
                .HasForeignKey(d => d.VenteId)
                .OnDelete(DeleteBehavior.Cascade);

            // جعل اسم المورد إلزامي
            modelBuilder.Entity<Fournisseur>()
                        .Property(f => f.Nom)
                        .IsRequired()
                        .HasMaxLength(200);

            // جعل اسم العميل إلزامي
            modelBuilder.Entity<Client>()
                        .Property(c => c.Nom)
                        .IsRequired()
                        .HasMaxLength(200);
        }
    }
}