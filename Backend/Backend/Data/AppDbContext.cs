using Microsoft.EntityFrameworkCore;
using Backend.Models; // 引用你的 Model 資料夾

namespace Backend.Data
{
    
    public class AppDbContext : DbContext
    {
        // 建構子：這是為了接收從 Program.cs 傳進來的連線設定
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 關鍵：這行代表你要在資料庫建立一張叫 Customer 的表
        // 格式參考你的 Customer.cs 類別
        public DbSet<Customer> Customers { get; set; }
    }
}