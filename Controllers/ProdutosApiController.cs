
using DW01.Data;
using DW01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DW01.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/produtosapi
        [HttpGet]
        public async Task<IActionResult> GetProdutos()
        {
            var produtos = await _context.Produtos
                .AsNoTracking()
                .ToListAsync();

            return Ok(produtos);
        }

        // GET: api/produtos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduto(int id)
        {
            var produto = await _context.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return Ok(produto);
        }


        // POST: api/produtos
        [HttpPost]
        public async Task<IActionResult> CriarProduto(
            [FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Produtos.Add(produto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProduto),
                new { id = produto.Id },
                produto
            );
        }


        // PUT: api/produtos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> AtualizarProduto(
            int id,
            [FromBody] Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest(new
                {
                    mensagem = "O ID da URL é diferente do ID do produto."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtoExistente = await _context.Produtos
                .FindAsync(id);

            if (produtoExistente == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Preco = produto.Preco;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/produtos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            _context.Produtos.Remove(produto);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
