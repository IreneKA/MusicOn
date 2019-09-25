using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    [Table("Mixtapes_Compositions")]
    public partial class MixtapesCompositions
    {
        [Column("MixTapeID")]
        public Guid MixTapeId { get; set; }
        [Column("CompositionID")]
        [Display(Name = "Трек")]
        public Guid CompositionId { get; set; }

        [ForeignKey("CompositionId")]
        [InverseProperty("MixtapesCompositions")]
        public virtual Composition Composition { get; set; }
        [ForeignKey("MixTapeId")]
        [InverseProperty("MixtapesCompositions")]
        public virtual MixTape MixTape { get; set; }
    }
}
