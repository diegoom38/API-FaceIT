using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class PessoaFisica
    {
        [StringLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        [StringLength(45)]
        public string RG { get; set; }

        [Key]
        public int IDPessoa { get; set; }

        [ForeignKey(nameof(IDPessoa))]
        [InverseProperty(nameof(Pessoa.PessoaFisica))]
        public virtual Pessoa IDPessoaNavigation { get; set; }
    }
}