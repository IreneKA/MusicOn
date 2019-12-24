using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class Artist
    {
        public Artist()
        {
            AlbumsArtists = new HashSet<AlbumsArtists>();
            FavouriteArtists = new HashSet<FavouriteArtists>();
        }

        [Column("ArtistID")]
        public Guid ArtistId { get; set; }
        [Required]
        [StringLength(255)]
        [Display (Name = "Исполнитель")]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        [Display(Name = "Информация")]
        public string Information { get; set; }

        [InverseProperty("Artist")]
        [Display(Name = "Альбомы")]
        public virtual ICollection<AlbumsArtists> AlbumsArtists { get; set; }
        [InverseProperty("Artist")]
        public virtual ICollection<FavouriteArtists> FavouriteArtists { get; set; }
    }
}
