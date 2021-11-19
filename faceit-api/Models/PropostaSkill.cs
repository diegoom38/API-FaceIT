using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class PropostaSkill
    {
        [Key]
        public int IDProposta { get; set; }

        [Key]
        public int IDSkill { get; set; }

        [Key]
        public int IDTipoSkill { get; set; }

        [ForeignKey("IDSkill,IDTipoSkill")]
        [InverseProperty(nameof(Skill.PropostaSkill))]
        public virtual Skill ID { get; set; }

        [ForeignKey(nameof(IDProposta))]
        [InverseProperty(nameof(Proposta.PropostaSkill))]
        public virtual Proposta IDPropostaNavigation { get; set; }
    }
}