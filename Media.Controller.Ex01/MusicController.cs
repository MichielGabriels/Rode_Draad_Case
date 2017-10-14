using Media.DataModel;
using Media.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Controller.Ex01
{
    /// <summary>
    /// A controller for media items of the type Music.
    /// </summary>
    public class MusicController : MediaController
    {
        /// <summary>
        /// A private variable that will hold the player that is currently in use.
        /// </summary>
        private IPlayer _player;
        /// <summary>
        /// A private variable that will hold the playlist that is currently in use.
        /// </summary>
        private IPlaylist _playlist;

        /// <summary>
        /// Constructor that calls the base constructor and initializes the player and the playlist.
        /// </summary>
        /// <param name="player">A player that you want to use and that is already initialized.</param>
        /// <param name="playlist">A playlist that you want to use and that is already initialized.</param>
        public MusicController(IPlayer player, IPlaylist playlist) : base()
        {
            _player = player;
            _playlist = playlist;
        }

        /// <summary>
        /// Read only property that returns the currently used player.
        /// </summary>
        public IPlayer Player
        {
            get { return _player; }
        }

        /// <summary>
        /// Read only property that returns the currently used playlist.
        /// </summary>
        public IPlaylist PlayList
        {
            get { return _playlist; }
        }

        /// <summary>
        /// Read only property that returns a boolean which represents if the player is playing a song.
        /// </summary>
        public bool IsPlaying
        {
            get { return _player.IsPlaying; }
        }

        /// <summary>
        /// Property that gets or sets the volume at which the player is playing.
        /// </summary>
        public float Volume
        {
            get { return _player.Volume; }
            set { _player.Volume = value; }
        }

        /// <summary>
        /// Read only property that returns a boolean which represents if there are songs in the playlist.
        /// </summary>
        public bool HasSongsInPlaylist
        {
            get { return _playlist.Count > 0; }
        }

        /// <summary>
        /// Builds a string that can be used in an OpenFileDialog as a filter for the type of files (extensions) you want to load.
        /// </summary>
        /// <returns>A string that can be passed to an OpenFileDialog as filter for the type of files you can add.</returns>
        public override string FileFilter()
        {
            return "Music Files(*.mp3) | *.mp3;";
        }

        /// <summary>
        /// Disposes objects, that implement IDisposable, used in the controller.
        /// </summary>
        public override void Dispose()
        {
            _player.Dispose();
        }

        /// <summary>
        /// Loads data in the memory (List).
        /// </summary>
        protected override void InitializeData()
        {
            List = new List<DataModel.Media>();
            List.AddRange(new Song[]
            {
                new Song()
                {
                    Title = "Summer of 69",
                    Singer = "Bryan Adams"
                },
                new Song()
                {
                    Title = "Title",
                    Singer = "Singer"
                }
            });
        }

        /// <summary>
        /// Pauses a song if there is a song playing.
        /// </summary>
        public void Pause()
        {
            if (_player.IsPlaying)
            {
                _player.Pause();
            }
        }

        /// <summary>
        /// Stops a song.
        /// </summary>
        public void StopPlaying()
        {
            _player.Stop();
        }

        /// <summary>
        /// Loads the first song from the playlist.
        /// </summary>
        /// <returns>A song (if there is one in the playlist) from the playlist.</returns>
        public Song PlayFromPlaylist()
        {
            if (_playlist.Count > 0)
            {
                var fileToPlay = (Song)_playlist.PlayMedia();
                _player.Play(fileToPlay.File);
                return fileToPlay;
            }
            return null;
        }

        /// <summary>
        /// Removes a song from the playlist.
        /// </summary>
        /// <param name="oldSong">A song from the playlist that you want to remove.</param>
        public void RemoveSongFromPlaylist(Song oldSong)
        {
            _playlist.RemoveMedia(oldSong);
        }

        /// <summary>
        /// Adds the selected song (from Selected) to the playlist.
        /// </summary>
        public void AddSelectedToPlaylist()
        {
            _playlist.AddMedia(Selected);
        }
    }
}
