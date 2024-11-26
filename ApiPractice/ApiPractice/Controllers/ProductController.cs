using ApiPractice.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace ApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var products = await connection.QueryAsync<Product>("select * from ProductsDB");
            return Ok(products);
        }

        [HttpGet("{ProductID}")]
        public async Task<ActionResult<Product>> GetProduct(int ProductID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var product = await connection.QueryFirstAsync<Product>("select * from productsDB where ProductID= @Id", new {Id = ProductID });
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into ProductsDB(name, description, price, quantity) values(@name, @description, @price, @quantity)", product);
            return Ok(await GetAllProducts(connection));
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update ProductsDB set name= @name, description = @description, price= @price, quantity = @quantity where ProductID = @ProductID", product);
            return Ok(await GetAllProducts(connection));
        }

        [HttpDelete("{ProductID}")]
        public async Task<ActionResult<Product>> DeleteProduct(int ProductID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from ProductsDB where ProductId = @Id", new { Id = ProductID });
            return Ok(await GetAllProducts(connection));
        }

        private static async Task<IEnumerable<Product>> GetAllProducts(SqlConnection connection)
        {
            return await connection.QueryAsync<Product>("select * from ProductsDB");
        }
    }
}
