using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProductManage.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "名字必填")]
        public string Name { get; set; }
        [Display(Name = "價格")]
        [Range(0, int.MaxValue, ErrorMessage = "價格必須>=0")]
        [Required(ErrorMessage = "價格必填")]
        public int? Price { get; set; }
        [Display(Name = "描述")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "類別必填")]
        [Range(1, int.MaxValue, ErrorMessage = "請選分類")]
        [Display(Name = "分類")]
        
        public int? CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; }=
        new List<SelectListItem>();
        [Display(Name = "分類名稱")]
        public string CategoryName { get; set; } = string.Empty;


        // --- 新增：用於檔案上傳的屬性 ---
        [Display(Name = "選擇圖片")]
        public IFormFile? ImageFile { get; set; }

        // --- 新增：用於顯示圖片的屬性（路徑） ---
        [Display(Name = "圖片")]
        public string? ImageUrl { get; set; }
    }
}
