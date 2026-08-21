using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services
{
    public class EmprestimoService
    {
        private readonly EmprestimoRepository _emprestimoRepository;
        private readonly LivroRepository _livroRepository;

        public EmprestimoService(
            EmprestimoRepository emprestimoRepository,
            LivroRepository livroRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _livroRepository = livroRepository;
        }

        public async Task<List<Emprestimo>> GetTodosAsync()
        {
            return await _emprestimoRepository.GetTodosAsync();
        }

        public async Task<Emprestimo?> GetPorIdAsync(Guid id)
        {
            return await _emprestimoRepository.GetPorIdAsync(id);
        }

        public async Task<Emprestimo> CriarAsync(Emprestimo emprestimo)
        {
            var livro = await _livroRepository.GetPorIdAsync(
                emprestimo.LivroId);

            if (livro == null)
            {
                throw new Exception("Livro não encontrado.");
            }

            if (!livro.Disponivel)
            {
                throw new Exception("Livro já está emprestado.");
            }

            emprestimo.Id = Guid.NewGuid();
            emprestimo.DataEmprestimo = DateTime.UtcNow;
            emprestimo.DataDevolucao = null;

            livro.Disponivel = false;

            await _emprestimoRepository.AdicionarAsync(emprestimo);
            await _livroRepository.AtualizarAsync(livro);

            return emprestimo;
        }

        public async Task<Emprestimo?> DevolverAsync(Guid id)
        {
            var emprestimo =
                await _emprestimoRepository.GetPorIdAsync(id);

            if (emprestimo == null)
            {
                return null;
            }

            if (emprestimo.DataDevolucao != null)
            {
                throw new Exception("Este empréstimo já foi devolvido.");
            }

            emprestimo.DataDevolucao = DateTime.UtcNow;

            if (emprestimo.Livro != null)
            {
                emprestimo.Livro.Disponivel = true;

                await _livroRepository.AtualizarAsync(
                    emprestimo.Livro);
            }

            await _emprestimoRepository.AtualizarAsync(emprestimo);

            return emprestimo;
        }
    }
}