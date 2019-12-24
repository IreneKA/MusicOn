using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class SongTitleComposition
    {
        public SongTitleComposition()
        {
            Title = "";
        }
        public Guid AlbumId { get; set; }
        public TimeSpan Duration { get; set; }
        public string Title { get; set; }
    }
}
