using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MusicOn.Models
{
    public partial class MusicOnContext : DbContext
    {
        public MusicOnContext()
        {
        }

        public MusicOnContext(DbContextOptions<MusicOnContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<AlbumsArtists> AlbumsArtists { get; set; }
        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<Composition> Composition { get; set; }
        public virtual DbSet<FavouriteArtists> FavouriteArtists { get; set; }
        public virtual DbSet<FavouriteSongs> FavouriteSongs { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<LoadedSongs> LoadedSongs { get; set; }
        public virtual DbSet<MixTape> MixTape { get; set; }
        public virtual DbSet<MixtapesCompositions> MixtapesCompositions { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Song> Song { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-L08T9KH\\SQLEXPRESS;Database=MusicOn;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.AlbumId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Album_Genre");
            });

            modelBuilder.Entity<AlbumsArtists>(entity =>
            {
                entity.HasKey(e => new { e.ArtistId, e.AlbumId });

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumsArtists)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_Albums_Artists_Album");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.AlbumsArtists)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("FK_Albums_Artists_Artist");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.Property(e => e.ArtistId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Composition>(entity =>
            {
                entity.Property(e => e.CompositionId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_Composition_Album");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.SongId)
                    .HasConstraintName("FK_Composition_Song");
            });

            modelBuilder.Entity<FavouriteArtists>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ArtistId });

                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.FavouriteArtists)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("FK_FavouriteArtists_Artist");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavouriteArtists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_FavouriteArtists_User");
            });

            modelBuilder.Entity<FavouriteSongs>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CompositionId });

                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Composition)
                    .WithMany(p => p.FavouriteSongs)
                    .HasForeignKey(d => d.CompositionId)
                    .HasConstraintName("FK_FavouriteSongs_Composition");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavouriteSongs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_FavouriteSongs_User");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreId).ValueGeneratedNever();
            });

            modelBuilder.Entity<LoadedSongs>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CompositionId, e.Date });

                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Composition)
                    .WithMany(p => p.LoadedSongs)
                    .HasForeignKey(d => d.CompositionId)
                    .HasConstraintName("FK_LoadedSongs_Composition");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LoadedSongs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_LoadedSongs_User");
            });

            modelBuilder.Entity<MixTape>(entity =>
            {
                entity.Property(e => e.MixTapeId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<MixtapesCompositions>(entity =>
            {
                entity.HasKey(e => new { e.MixTapeId, e.CompositionId });

                entity.HasOne(d => d.Composition)
                    .WithMany(p => p.MixtapesCompositions)
                    .HasForeignKey(d => d.CompositionId)
                    .HasConstraintName("FK_Mixtapes_Compositions_Composition");

                entity.HasOne(d => d.MixTape)
                    .WithMany(p => p.MixtapesCompositions)
                    .HasForeignKey(d => d.MixTapeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mixtapes_Compositions_MixTape");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.Property(e => e.SongId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.RoleId).HasDefaultValueSql("((3))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });
        }
    }
}
