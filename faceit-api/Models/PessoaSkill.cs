using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class PessoaSkill
    {
        [Key]
        public int IDPessoa { get; set; }

        [Key]
        public int IDSkill { get; set; }

        [Key]
        public int IDTipoSkill { get; set; }

        [ForeignKey("IDSkill,IDTipoSkill")]
        [InverseProperty(nameof(Skill.PessoaSkill))]
        public virtual Skill ID { get; set; }

        [ForeignKey(nameof(IDPessoa))]
        [InverseProperty(nameof(Pessoa.PessoaSkill))]
        public virtual Pessoa IDPessoaNavigation { get; set; }
    }
}