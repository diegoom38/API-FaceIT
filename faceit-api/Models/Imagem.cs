using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class Imagem
    {
        [Key]
        public int IDPessoa { get; set; }

        [StringLength(150)]
        public string Nome { get; set; }

        public byte[] Bytes { get; set; }

        [ForeignKey(nameof(IDPessoa))]
        [InverseProperty(nameof(Pessoa.Imagem))]
        public virtual Pessoa IDPessoaNavigation { get; set; }
    }
}