using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class FavouriteArtists
    {
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Column("ArtistID")]
        public Guid ArtistId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [ForeignKey("ArtistId")]
        [InverseProperty("FavouriteArtists")]
        public virtual Artist Artist { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("FavouriteArtists")]
        public virtual User User { get; set; }
    }
}
