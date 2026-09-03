using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services
{
    // Aqui ficam as regras relacionadas aos livros
    public class LivroService
    {
        // Repository usado para acessar os livros no banco
        private readonly LivroRepository _repository;

        public LivroService(LivroRepository repository)
        {
            _repository = repository;
        }

        // Busca todos os livros cadastrados
        public async Task<List<Livro>> GetTodosAsync()
        {
            return await _repository.GetTodosAsync();
        }

        // Busca um livro específico pelo ID
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

            // Gera um ID para o livro e deixa ele disponível
            livro.Id = Guid.NewGuid();
            livro.Disponivel = true;

            await _repository.AdicionarAsync(livro);

            return livro;
        }

        // Atualiza os dados de um livro existente
        public async Task<Livro?> AtualizarAsync(Guid id, Livro livro)
        {
            // Procura o livro que será atualizado
            var existente = await _repository.GetPorIdAsync(id);

            if (existente == null)
            {
                return null;
            }

            // Verifica se outro livro já possui o mesmo ISBN
            var livroComMesmoISBN =
                await _repository.GetPorISBNAsync(livro.ISBN);

            if (livroComMesmoISBN != null &&
                livroComMesmoISBN.Id != id)
            {
                throw new Exception("Já existe outro livro com este ISBN.");
            }

            // Atualiza apenas os dados principais do livro
            existente.Titulo = livro.Titulo;
            existente.Autor = livro.Autor;
            existente.ISBN = livro.ISBN;

            await _repository.AtualizarAsync(existente);

            return existente;
        }

        // Exclui um livro pelo ID
        public async Task<bool> ExcluirAsync(Guid id)
        {
            // Procura o livro antes de tentar excluir
            var livro = await _repository.GetPorIdAsync(id);

            if (livro == null)
            {
                return false;
            }

            // Não permite excluir um livro que está emprestado
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
