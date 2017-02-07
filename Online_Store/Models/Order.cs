using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Online_Store.Models
{
    public enum Delivery
    {
        [Display(Name= "پست معمولی")]
        post=1,
        [Display(Name = "پست پیشتاز")]
        pishtaz=2,
        [Display(Name = "پیک")]
        peyk=3
    }
    public enum Payment
    {
        [Display(Name = "پرداخت با حضوری به پیک")]
        peyk = 1,
        [Display(Name = "آنلاین با درگاه بانک ملت")]
        melat = 2,
        [Display(Name = "واریز فیش بانکی")]
        fishe = 3
    }
    
   
    //public class Cart
    //{
    //    [Key]
    //    public int RecordId { get; set; }
    //    public string CartId { get; set; }
    //    [Required(AllowEmptyStrings = true, ErrorMessage = " ")]
    //    [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100")]
    //    [DisplayName("Quantity")]
    //    public int Count { get; set; }
    //    public System.DateTime DateCreated { get; set; }
    //    public int productId { get; set; }
    //    public virtual product product{ get; set; }
        
    //}
    public partial class Order
    {
        public int Id { get; set; }
        public string Username { get; set; }
        //[Required(ErrorMessage = "وارد کردن نام الزامی است")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی است")]
        public string LastName { get; set; }
        //[Required(ErrorMessage = "وارد کردن آدرس است")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "وارد کردن شهر الزامی است")]
        public string City { get; set; }
       // [Required(ErrorMessage = "وارد کردن استان الزامی است")]
        public string State { get; set; }
       // [Required(ErrorMessage = "وارد کردن کد پستی الزامی است")]
        public string PostalCode { get; set; }
       // [Required(ErrorMessage = "وارد کردن تلفن الزامی است")]
        public string Phone { get; set; }
      //  [Required(ErrorMessage = "وارد کردن ایمیل الزامی است")]
        public string Email { get; set; }
        [Required]
        public decimal Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        //public List<OrderDetail> OrderDetails { get; set; }
    }
    //public class OrderDetail
    //{
    //    public long OrderDetailId { get; set; }
    //    public decimal Quantity { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public long OrderProudctId { get; set; }
    //    public virtual product product { get; set; }
    //    public int OrderId { get; set; }
    //    public virtual Order Order { get; set; }

    //}

    public class DevliveryMethod
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        public Delivery DeliveryEnum { get; set; }
       
    }

    public class PaymentMethod
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        public Payment PaymentEnum { get; set; }
    }
    
    
}