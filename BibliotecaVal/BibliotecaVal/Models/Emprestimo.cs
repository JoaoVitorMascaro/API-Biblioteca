using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotecaAPI.Models
{
    public class Emprestimo
    {
        public Guid Id { get; set; }

        [Required]
        public Guid LivroId { get; set; }

        // Guarda a data em que o livro foi emprestado
        public DateTime DataEmprestimo { get; set; }

        // Guarda a data de devolução, Pode ficar vazio enquanto o livro ainda estiver emprestado
        public DateTime? DataDevolucao { get; set; }

        // Evita que o objeto Livro seja retornado novamente dentro do empréstimo
        [JsonIgnore]
        public Livro? Livro { get; set; }
    }
}
