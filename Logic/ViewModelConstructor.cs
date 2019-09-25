using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicOn.Models;
using MusicOn.ViewModels;

namespace MusicOn.Logic
{
    public static class ViewModelConstructor
    {
        public static List<CompositionViewModel> FavouriteSongs(List<Composition> compositionsF, MusicOnContext _context, Guid userId)
        {
            List<CompositionViewModel> compositions = new List<CompositionViewModel>();
            foreach (var composition in compositionsF)
            {
                var compositionVM = new CompositionViewModel();
                compositionVM.Composition = composition;
                var favourit =  _context.FavouriteSongs
                    .Where(f => f.UserId == userId)
                    .FirstOrDefault(f => f.CompositionId == composition.CompositionId);
                if (favourit != null)
                {
                    compositionVM.IsFavourite = true;
                }
                compositions.Add(compositionVM);
            }
            return compositions;
        }
        public static List<ArtistsViewModel> FavouriteArtists(List<Artist> artistsF, MusicOnContext _context, Guid userId)
        {
            List<ArtistsViewModel> artists = new List<ArtistsViewModel>();
            foreach (var artist in artistsF)
            {
                var artistVM = new ArtistsViewModel();
                artistVM.Artist = artist;
                var favourit = _context.FavouriteArtists
                    .Where(f => f.UserId == userId)
                    .FirstOrDefault(f => f.ArtistId == artist.ArtistId);
                if (favourit != null)
                {
                    artistVM.IsFavourite = true;
                }
                artists.Add(artistVM);
            }
            return artists;
        }

        public static List<CompositionViewModel> AlbumSongs(List<Composition> compositionsF, MusicOnContext _context, Guid userId)
        {
            List<CompositionViewModel> compositions = new List<CompositionViewModel>();
            foreach (var composition in compositionsF)
            {
                var compositionVM = new CompositionViewModel();
                compositionVM.Composition = composition;
                var favourit = _context.FavouriteSongs
                    .Where(f => f.UserId == userId)
                    .FirstOrDefault(f => f.CompositionId == composition.CompositionId);
                if (favourit != null)
                {
                    compositionVM.IsFavourite = true;
                }
                compositions.Add(compositionVM);
            }
            return compositions;
        }
    }
}
