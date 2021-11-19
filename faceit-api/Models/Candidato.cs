using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class Candidato
    {
        [Key]
        public int IDProposta { get; set; }

        [Key]
        public int IDPessoa { get; set; }

        [ForeignKey(nameof(IDPessoa))]
        [InverseProperty(nameof(Pessoa.Candidato))]
        public virtual Pessoa IDPessoaNavigation { get; set; }

        [ForeignKey(nameof(IDProposta))]
        [InverseProperty(nameof(Proposta.Candidato))]
        public virtual Proposta IDPropostaNavigation { get; set; }
    }
}