namespace MyShop.Models
{
    public class StoreSettings
    {
        public int Id { get; set; }
        public string StoreName { get; set; }   // اسم المحل
        public string BusinessType { get; set; }  // نوع النشاط
        public string CommercialRegister { get; set; } // رقم السجل التجاري
        public string TaxNumber { get; set; }  // الرقم الضريبي
        public string BankAccount { get; set; } // رقم الحساب البنكي
        public string Address { get; set; }  // العنوان
        public string Phone { get; set; }   // الهاتف
        public decimal DefaultTax { get; set; }  // نسبة الضريبة
    }
}