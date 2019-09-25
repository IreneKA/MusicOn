using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            FavouriteCompositions = new List<CompositionViewModel>();
            FavouriteArtists = new List<ArtistsViewModel>();
            LoadedSongs = new List<LoadedSongs>();
        }
        public User User { get; set; }
        public List<CompositionViewModel> FavouriteCompositions { get; set; }
        public List<ArtistsViewModel> FavouriteArtists { get; set; }
        public List<LoadedSongs> LoadedSongs { get; set; }
    }
}
