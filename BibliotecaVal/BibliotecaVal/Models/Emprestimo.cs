using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models
{
    public class Emprestimo
    {
        public Guid Id { get; set; }

        [Required]
        public Guid LivroId { get; set; }

        public DateTime DataEmprestimo { get; set; }

        public DateTime? DataDevolucao { get; set; }

        public Livro? Livro { get; set; }
    }
}