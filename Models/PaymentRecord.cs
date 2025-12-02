namespace MyShop.Models
{
    public class PaymentRecord
    {
        public int Id { get; set; }
        public string Email { get; set; }          // البريد الإلكتروني للمستخدم
        public string Plan { get; set; }           // الخطة المدفوعة
        public decimal Amount { get; set; }        // المبلغ
        public string CheckoutId { get; set; }     // معرف الدفع من Chargily
        public string Status { get; set; }         // pending, success, failed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
