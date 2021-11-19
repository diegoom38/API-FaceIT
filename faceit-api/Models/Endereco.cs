using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class Endereco
    {
        [StringLength(9)]
        public string CEP { get; set; }

        [StringLength(150)]
        public string Pais { get; set; }

        [StringLength(2)]
        public string UF { get; set; }

        [StringLength(150)]
        public string Municipio { get; set; }

        [StringLength(150)]
        public string Logradouro { get; set; }

        [StringLength(50)]
        public string Numero { get; set; }

        [StringLength(150)]
        public string Complemento { get; set; }

        [StringLength(150)]
        public string Bairro { get; set; }

        [Key]
        public int IDPessoa { get; set; }

        [ForeignKey(nameof(IDPessoa))]
        [InverseProperty(nameof(Pessoa.Endereco))]
        public virtual Pessoa IDPessoaNavigation { get; set; }
    }
}