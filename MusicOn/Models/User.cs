using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class User
    {
        public User()
        {
            FavouriteArtists = new HashSet<FavouriteArtists>();
            FavouriteSongs = new HashSet<FavouriteSongs>();
            LoadedSongs = new HashSet<LoadedSongs>();
        }

        [Column("UserID")]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(16)]
        public string Password { get; set; }
        [Column("RoleID")]
        [Display(Name ="Роль")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("User")]
        [Display(Name ="Роль")]
        public virtual Role Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<FavouriteArtists> FavouriteArtists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<FavouriteSongs> FavouriteSongs { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<LoadedSongs> LoadedSongs { get; set; }
    }
}
