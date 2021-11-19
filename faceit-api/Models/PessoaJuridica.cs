using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class PessoaJuridica
    {
        [StringLength(150)]
        public string RazaoSocial { get; set; }

        [StringLength(150)]
        public string NomeFantasia { get; set; }

        [Required]
        [StringLength(18)]
        public string CNPJ { get; set; }

        [StringLength(45)]
        public string IE { get; set; }

        [Key]
        public int IDPessoa { get; set; }

        [ForeignKey(nameof(IDPessoa))]
        [InverseProperty(nameof(Pessoa.PessoaJuridica))]
        public virtual Pessoa IDPessoaNavigation { get; set; }
    }
}