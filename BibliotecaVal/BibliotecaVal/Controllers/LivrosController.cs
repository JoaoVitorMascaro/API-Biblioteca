using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/livros")]
    public class LivrosController : ControllerBase
    {
        private readonly LivroService _livroService;

        // Recebe o Service para poder usar seus métodos
        public LivrosController(LivroService livroService)
        {
            _livroService = livroService;
        }

        // GET: api/livros
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var livros = await _livroService.GetTodosAsync();

            return Ok(livros);
        }

        // GET: api/livros/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            var livro = await _livroService.GetPorIdAsync(id);

            if (livro == null)
                return NotFound(new { mensagem = "Livro não encontrado." });

            return Ok(livro);
        }


        // POST: api/livros
        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            try
            {
                // Envia o livro para o Service fazer o cadastro
                var novoLivro = await _livroService.CriarAsync(livro);

                return CreatedAtAction(
                    nameof(GetPorId),
                    new { id = novoLivro.Id },
                    novoLivro
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PUT: api/livros/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, Livro livro)
        {
            try
            {
                // Manda o ID e os novos dados para o Service atualizar
                var atualizado = await _livroService.AtualizarAsync(id, livro);

                if (atualizado == null)
                    return NotFound(new { mensagem = "Livro não encontrado." });

                return Ok(atualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // DELETE: api/livros/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                // Tenta excluir o livro através do Service
                var excluido = await _livroService.ExcluirAsync(id);

                if (!excluido)
                    return NotFound(new { mensagem = "Livro não encontrado." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
