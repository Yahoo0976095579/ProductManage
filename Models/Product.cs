using System.ComponentModel.DataAnnotations;

namespace ProductManage.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "名字必填")]
        public string Name { get; set; }
        [Display(Name = "價格")]
        [Range(0, int.MaxValue, ErrorMessage = "價格必須>=0")]
        [Required(ErrorMessage = "價格必填")]
        public int Price { get; set; }
        [Display(Name = "描述")]
        public string? Description { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        [Display(Name = "分類名稱")]
        public Category Category { get; set; }

        [Display(Name = "圖片")]
        public string? ImageUrl { get; set; }

    }
}
