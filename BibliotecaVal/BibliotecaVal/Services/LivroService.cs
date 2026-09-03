using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services
{
    public class LivroService
    {
        private readonly LivroRepository _repository;

        public LivroService(LivroRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Livro>> GetTodosAsync()
        {
            return await _repository.GetTodosAsync();
        }

        public async Task<Livro?> GetPorIdAsync(Guid id)
        {
            return await _repository.GetPorIdAsync(id);
        }

        // Cadastra um novo livro
        public async Task<Livro> CriarAsync(Livro livro)
        {
            // Verifica se já existe um livro com esse ISBN
            var existente = await _repository.GetPorISBNAsync(livro.ISBN);

            if (existente != null)
            {
                throw new Exception("Já existe um livro com este ISBN.");
            }

            livro.Id = Guid.NewGuid();
            livro.Disponivel = true;

            await _repository.AdicionarAsync(livro);

            return livro;
        }

        // Atualiza os dados de um livro existente
        public async Task<Livro?> AtualizarAsync(Guid id, Livro livro)
        {
            var existente = await _repository.GetPorIdAsync(id);

            if (existente == null)
            {
                return null;
            }


            var livroComMesmoISBN =
                await _repository.GetPorISBNAsync(livro.ISBN);

            if (livroComMesmoISBN != null &&
                livroComMesmoISBN.Id != id)
            {
                throw new Exception("Já existe outro livro com este ISBN.");
            }

            existente.Titulo = livro.Titulo;
            existente.Autor = livro.Autor;
            existente.ISBN = livro.ISBN;

            await _repository.AtualizarAsync(existente);

            return existente;
        }

        // Exclui um livro pelo ID
        public async Task<bool> ExcluirAsync(Guid id)
        {
            var livro = await _repository.GetPorIdAsync(id);

            if (livro == null)
            {
                return false;
            }

            if (!livro.Disponivel)
            {
                throw new Exception(
                    "Não é possível excluir um livro que está emprestado.");
            }

            await _repository.ExcluirAsync(livro);

            return true;
        }
    }
}
