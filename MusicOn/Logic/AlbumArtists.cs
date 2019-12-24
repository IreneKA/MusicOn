using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.Logic
{
    public class AlbumArtists
    {
        public AlbumArtists()
        {
            Title = "";
        }
        public AlbumArtists(Album album) : this()
        {
            AlbumId = album.AlbumId;
            Title = album.Title;
            foreach(var aartist in album.AlbumsArtists)
            {
                Title += " '" + aartist.Artist.Name + "'";
            }
        }
        public Guid AlbumId { get; set; }
        public string Title { get; set; }
    }
    public class AlbumArtistss
    {
        public AlbumArtistss()
        {
            Albums = new List<AlbumArtists>();
        }
        public AlbumArtistss(List<Album> albums) : this()
        {
            foreach (var album in albums)
            {
                Albums.Add(new AlbumArtists(album));
            }
        }
        public List<AlbumArtists> Albums { get; set; }
    }
}
