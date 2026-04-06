using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            // 去抓資料庫裡 customer 表的所有資料
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpPut("{id}")] // 對應前端的 `/Customer/${customeId}`
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            //安全檢查：確認網址上的 ID 跟傳過來的物件 ID 是否一致            
            if (id != customer.CustomerId)
            {
                return BadRequest("ID 不符");
            }
            if (string.IsNullOrWhiteSpace(customer.CTEL))
            {
                return BadRequest("電話號碼不能為空");
            }
            //告訴 Entity Framework：這筆資料已經被動過了，請準備更新它
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                //真正執行 SQL 的 UPDATE 動作
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //防呆
                if (!_context.Customers.Any(e => e.CustomerId == id))
                {
                    return NotFound("找不到該客戶資料");
                }
                else
                {
                    throw;
                }
            }           

            //回傳 204 No Content，代表「成功了，但我不需要回傳額外資料」
            // 前端 Axios 會收到 status 204
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Customer>>> PostCustomers(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

    }
}