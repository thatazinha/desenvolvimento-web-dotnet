using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DW01.Models;

namespace DW01.Controllers
{
    public class DapperController : Controller
    {
        private readonly IConfiguration _configuration;

        public DapperController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            using var connection =
                new SqlConnection(connectionString);

            var sql = "SELECT Id, Nome, Preco FROM Produtos";

            var produtos = await connection.QueryAsync<Produto>(sql);

            return View(produtos);
        }
    }
}
