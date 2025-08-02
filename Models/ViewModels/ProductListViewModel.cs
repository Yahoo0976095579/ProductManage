using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductManage.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public int? SelectedCategoryId { get; set; } // 用於保存目前選取的分類ID
        public string? SearchName { get; set; } // 用於保存搜尋的商品名稱
        public string? PriceSortParam { get; set; } // 新增：用於保存排序狀態
        // --- 新增：分頁屬性 ---
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        // --- 新增：總筆數屬性 ---
        public int TotalItems { get; set; }
    }
}
