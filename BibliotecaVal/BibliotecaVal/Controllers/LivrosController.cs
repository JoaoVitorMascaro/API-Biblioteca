using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/livros")]
    public class LivrosController : ControllerBase
    {
        // Guarda o Service responsável pelas operações dos livros
        private readonly LivroService _livroService;

        // Recebe o Service para poder usar seus métodos
        public LivrosController(LivroService livroService)
        {
            _livroService = livroService;
        }

        // GET: api/livros
        // Retorna todos os livros cadastrados
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var livros = await _livroService.GetTodosAsync();

            return Ok(livros);
        }

        // GET: api/livros/{id}
        // Busca um livro específico pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            var livro = await _livroService.GetPorIdAsync(id);

            // Se não encontrar o livro, retorna erro 404
            if (livro == null)
                return NotFound(new { mensagem = "Livro não encontrado." });

            return Ok(livro);
        }


        // POST: api/livros
        // Cadastra um novo livro
        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            try
            {
                // Envia o livro para o Service fazer o cadastro
                var novoLivro = await _livroService.CriarAsync(livro);

                // Retorna 201 informando que o livro foi criado
                return CreatedAtAction(
                    nameof(GetPorId),
                    new { id = novoLivro.Id },
                    novoLivro
                );
            }
            catch (Exception ex)
            {
                // Se acontecer algum erro, retorna 400 com a mensagem
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PUT: api/livros/{id}
        // Atualiza as informações de um livro
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, Livro livro)
        {
            try
            {
                // Manda o ID e os novos dados para o Service atualizar
                var atualizado = await _livroService.AtualizarAsync(id, livro);

                // Se o livro não existir, retorna 404
                if (atualizado == null)
                    return NotFound(new { mensagem = "Livro não encontrado." });

                return Ok(atualizado);
            }
            catch (Exception ex)
            {
                // Caso dê algum erro, retorna 400 com a mensagem
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // DELETE: api/livros/{id}
        // Exclui um livro pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                // Tenta excluir o livro através do Service
                var excluido = await _livroService.ExcluirAsync(id);

                // Se não encontrar o livro, retorna 404
                if (!excluido)
                    return NotFound(new { mensagem = "Livro não encontrado." });

                // Retorna 204, indicando que foi excluído com sucesso
                return NoContent();
            }
            catch (Exception ex)
            {
                // Se acontecer algum erro, retorna 400
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
