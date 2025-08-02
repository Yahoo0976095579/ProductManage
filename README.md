# ProductManage - 產品管理系統

此專案是一個基於 ASP.NET Core MVC 開發的產品管理系統，實作產品資訊的新增、查詢、更新與刪除（CRUD），同時支援圖片上傳、分類與排序等功能。介面採用 Bootstrap 5 設計，具備良好的響應式體驗，適用於桌機、平板與手機。

### 專案特色

- 產品資料管理：完整的 CRUD 操作介面，包含產品名稱、價格、描述、分類等欄位。

- 圖片上傳與預覽：支援上傳產品圖片，並即時於前端顯示預覽。

- 分類與排序：依分類篩選商品，並可依價格升冪/降冪排序。

- 響應式設計：介面適配各種裝置，確保良好使用者體驗。

- 開發結構清晰：使用 Razor View + ViewModel 搭配 Controller 驅動，維護性高。

---

## 技術棧

- **後端** ASP.NET Core MVC（.NET 8）
- **資料存取** Entity Framework Core
- **前端** Razor View + Bootstrap 5 + HTML/CSS
- **開發工具** Visual Studio 2022
- **資料庫** SQL Server（LocalDB）

---

## 功能特色

- **產品 CRUD**：新增、編輯、刪除、查詢 產品資訊。
- **圖片上傳與預覽**：支援上傳產品圖片，並提供即時預覽功能。
- **產品篩選與排序**：可依據產品分類進行篩選，並依價格進行排序。
- **響應式佈局**：頁面在桌機、平板和手機上均能完美呈現。

---

## 專案預覽

<br>

<h3 align="center">產品列表</h3>
<p align="center">
  <img src="assets/images/readme/index.png" alt="產品列表頁面預覽" width="800">
  <br>
  <em>產品列表頁面，使用卡片式佈局展示產品資訊。</em>
</p>

<br>

<h3 align="center">建立新產品</h3>
<p align="center">
  <img src="assets/images/readme/create.png" alt="創建產品頁面預覽" width="800">
  <br>
  <em>新增產品資料。</em>
</p>

<br>

<h3 align="center">產品詳細資料</h3>
<p align="center">
  <img src="assets/images/readme/details.png" alt="產品詳細資料頁面預覽" width="800">
  <br>
  <em>詳細資料頁面，圖片與文字資訊並排呈現。</em>
</p>

---

##  資料夾架構
```
│
├── Controllers/ → 控制器（ProductController）
├── Models/ → 資料模型（Product.cs、Category.cs）
├── ViewModels/ → 專屬頁面資料傳輸模型
├── Views/ → Razor 視圖（Create, Edit, Index, Details）
├── wwwroot/ → 靜態資源（CSS、圖片）
├── Data/ → DbContext 與資料庫初始化
└── Program.cs → 專案進入點與服務註冊
```
---

## 開發重點

使用 ViewModel 與 Model 分離資料處理與顯示邏輯

圖片檔案上傳儲存於伺服器本地（wwwroot）並提供路徑讀取

Razor 語法撰寫 View，支援 Model Binding 與資料驗證

使用 EF Core 完成資料庫操作與遷移
