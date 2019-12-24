using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class FavouriteSongs
    {
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Column("CompositionID")]
        public Guid CompositionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [ForeignKey("CompositionId")]
        [InverseProperty("FavouriteSongs")]
        public virtual Composition Composition { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("FavouriteSongs")]
        public virtual User User { get; set; }
    }
}
