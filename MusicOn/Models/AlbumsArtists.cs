using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    [Table("Albums_Artists")]
    public partial class AlbumsArtists
    {
        [Column("ArtistID")]
        [Display(Name = "Исполнитель")]
        public Guid ArtistId { get; set; }
        [Column("AlbumID")]
        [Display(Name = "Альбом")]
        public Guid AlbumId { get; set; }

        [ForeignKey("AlbumId")]
        [InverseProperty("AlbumsArtists")]
        public virtual Album Album { get; set; }
        [ForeignKey("ArtistId")]
        [InverseProperty("AlbumsArtists")]
        public virtual Artist Artist { get; set; }
    }
}
