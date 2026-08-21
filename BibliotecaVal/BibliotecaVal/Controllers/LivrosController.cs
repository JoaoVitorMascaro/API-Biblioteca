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

        public LivrosController(LivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var livros = await _livroService.GetTodosAsync();

            return Ok(livros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            var livro = await _livroService.GetPorIdAsync(id);

            if (livro == null)
                return NotFound(new { mensagem = "Livro não encontrado." });

            return Ok(livro);
        }


        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            try
            {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, Livro livro)
        {
            try
            {
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
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