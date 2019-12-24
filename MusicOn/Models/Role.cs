using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        [Column("RoleID")]
        public int RoleId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<User> User { get; set; }
    }
}
