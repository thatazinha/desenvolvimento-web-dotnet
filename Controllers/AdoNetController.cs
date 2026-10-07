using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DW01.Controllers
{
    public class AdoNetController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdoNetController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var produtos = new List<string>();
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            var sql = "SELECT Id, Nome, Preco FROM Produtos";
            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                produtos.Add(
                    $"{reader["Id"]} - {reader["Nome"]} - R$ {reader["Preco"]}"
                );
            }
            return View(produtos);
        }
    }
}

