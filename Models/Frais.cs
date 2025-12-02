using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class Frais
{
    public int Id { get; set; }
    public string Type { get; set; }
    public double Montant { get; set; }
    public string? Description { get; set; }
    public DateTime DateFrais { get; set; } = DateTime.Now;

    // ✅ المستخدم الذي أضاف هذا المصروف
    public string? Utilisateur { get; set; }
}
}