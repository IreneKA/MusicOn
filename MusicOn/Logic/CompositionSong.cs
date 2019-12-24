using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.Logic
{
    public class CompositionSong
    {
        public CompositionSong()
        {
            Song = "";
        }
        public CompositionSong(Composition composition):this()
        {
            CompositionId = composition.CompositionId;
            Song = composition.Song.Title + " " + composition.Duration;
        }
        public Guid CompositionId { get; set; }
        public string Song { get; set; }
    }
    public class CompositionSongs
    {
        public CompositionSongs()
        {
            Compositions = new List<CompositionSong>();
        }
        public CompositionSongs(List<Composition> compositions) : this()
        {
            foreach(var composition in compositions)
            {
                Compositions.Add(new CompositionSong(composition));
            }
        }
        public List<CompositionSong> Compositions { get; set; }
    }
}
