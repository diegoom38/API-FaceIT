using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class Skill
    {
        public Skill()
        {
            PessoaSkill = new HashSet<PessoaSkill>();
            PropostaSkill = new HashSet<PropostaSkill>();
        }

        [Key]
        public int IDSkill { get; set; }

        [Key]
        public int IDTipoSkill { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; }

        [ForeignKey(nameof(IDTipoSkill))]
        [InverseProperty(nameof(TipoSkill.Skill))]
        public virtual TipoSkill IDTipoSkillNavigation { get; set; }

        [InverseProperty("ID")]
        public virtual ICollection<PessoaSkill> PessoaSkill { get; set; }

        [InverseProperty("ID")]
        public virtual ICollection<PropostaSkill> PropostaSkill { get; set; }
    }
}