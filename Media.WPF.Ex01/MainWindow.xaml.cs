using Media.Controller.Ex01;
using Media.DataModel;
using Media.Player;
using Media.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Media.WPF.Ex01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicController _musicController;
        private MovieController _movieController;
        private MediaController _activeController;

        private byte[] _newFile;

        public MainWindow()
        {
            InitializeComponent();

            _musicController = new MusicController(new AudioPlayer(), new AudioPlaylist());
            _movieController = new MovieController();

            _musicController.Player.IsStarted += SetMusicPlay;
            _musicController.Player.IsFinished += SetMusicPlay;

            musicListBox.ItemsSource = _musicController.List;
            movieListBox.ItemsSource = _movieController.List;
        }

        #region Events
        private void OnCloseEvent(object sender, RoutedEventArgs e)
        {
            _musicController.Dispose();
            _movieController.Dispose();

            this.Close();
        }

        private void Cleanup(object sender, EventArgs e)
        {
            _musicController.Dispose();
            _movieController.Dispose();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (musicTabItem.IsSelected)
            {
                _activeController = _musicController;
                movieListBox.SelectedIndex = -1;
                textBoxMovieTitle.Clear();
                textBoxDirector.Clear();

                this.LoadMusicData();
            }
            else if (moviesTabItem.IsSelected)
            {
                _activeController = _movieController;
                movieListBox.SelectedIndex = -1;
                textBoxSongTitle.Clear();
                textBoxSinger.Clear();

                this.LoadMovieData();
            }

            this.ClearSelected();
        }

        private void MusicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var song = (Song)musicListBox.SelectedItem;

            if (song != null)
            {
                this.SelectMusicItem(song);
                this.SetMusicForm();

                buttonDeleteSong.IsEnabled = true;
            }

            e.Handled = true;
        }

        private void MovieListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var movie = (Movie)movieListBox.SelectedItem;

            if (movie != null)
            {
                this.SelectMovieItem(movie);
                this.SetMovieForm();

                buttonDeleteMovie.IsEnabled = true;
            }

            e.Handled = true;
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = _activeController.FileFilter();
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                var file = LoadConvert.ImportFile(dialog.FileName);
                _newFile = file;
            }

            if (_activeController.GetType() == typeof(MusicController))
            {
                checkBoxMusicFilePresent.IsChecked = _newFile != null;
            }
            else
            {
                checkBoxMovieFilePresent.IsChecked = _newFile != null;
            }
        }

        private void MusicSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSinger.Text != "" && textBoxSongTitle.Text != "")
            {
                if (_activeController.Selected == null)
                {
                    var newSong = new Song()
                    {
                        Singer = textBoxSinger.Text,
                        Title = textBoxSongTitle.Text,
                        File = _newFile
                    };

                    _activeController.AddMedia(newSong);
                }
                else
                {
                    var selectedSong = (Song)_activeController.Selected;

                    if (_newFile == null)
                    {
                        _newFile = selectedSong.File;
                    }

                    selectedSong.Singer = textBoxSinger.Text;
                    selectedSong.Title = textBoxSongTitle.Text;
                    selectedSong.File = _newFile;
                }

                _activeController.ClearSelected();
                textBoxSinger.Text = "";
                textBoxSongTitle.Text = "";

                musicListBox.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please fill in all the fields");
            }
        }

        private void MovieSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxDirector.Text != "" && textBoxMovieTitle.Text != "")
            {
                if (_activeController.Selected == null)
                {
                    var newMovie = new Movie()
                    {
                        Director = textBoxDirector.Text,
                        Title = textBoxMovieTitle.Text,
                        File = _newFile
                    };

                    _activeController.AddMedia(newMovie);
                }
                else
                {
                    var selectedMovie = (Movie)_activeController.Selected;

                    if (_newFile == null)
                    {
                        _newFile = selectedMovie.File;
                    }

                    selectedMovie.Director = textBoxDirector.Text;
                    selectedMovie.Title = textBoxMovieTitle.Text;
                    selectedMovie.File = _newFile;
                }

                _activeController.ClearSelected();
                textBoxDirector.Text = "";
                textBoxMovieTitle.Text = "";

                movieListBox.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please fill in all the fields");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelected();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _activeController.RemoveMedia(_activeController.Selected);

            if (_activeController.GetType() == typeof(MusicController))
            {
                _musicController.RemoveSongFromPlaylist((Song) _musicController.Selected);
                _musicController.RemoveMedia((Song)_musicController.Selected);

                textBoxSinger.Clear();
                textBoxSongTitle.Clear();
            }
            else
            {
                _movieController.RemoveMedia((Movie)_movieController.Selected);

                textBoxDirector.Clear();
                textBoxMovieTitle.Clear();
            }

            buttonDeleteSong.IsEnabled = false;
            buttonDeleteMovie.IsEnabled = false;

            musicListBox.Items.Refresh();
            movieListBox.Items.Refresh();
        }

        private void MusicAddToPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            _musicController.AddSelectedToPlaylist();
            this.SetButtons();
        }

        private void MusicPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_musicController.HasSongsInPlaylist)
            {
                var song = _musicController.PlayFromPlaylist();
                labelNowPlaying.Content = $"Now playing: {song.Singer} - {song.Title}";
            }

            this.SetMusicPlay();
        }

        private void MusicNextButton_Click(object sender, RoutedEventArgs e)
        {
            _musicController.StopPlaying();
            MusicPlayButton_Click(null, null);
            this.SetMusicPlay();
        }

        private void MusicPauseButton_Click(object sender, RoutedEventArgs e)
        {
            _musicController.Pause();
        }

        private void MusicStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_musicController.IsPlaying)
            {
                _musicController.StopPlaying();
                labelNowPlaying.Content = "Now playing:";
                this.SetMusicPlay();
            }
        }

        private void MusicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_musicController.IsPlaying)
            {
                _musicController.Volume = float.Parse(sliderVolume.Value.ToString());
            }
        }

        private void MoviePlayButton_Click(object sender, RoutedEventArgs e)
        {
            var videoPlayer = new VideoPlayer(_activeController.Selected.File);
            videoPlayer.ShowDialog();
        }
        #endregion

        #region Other methods
        private void SetMusicPlay()
        {
            if (_musicController.IsPlaying)
            {
                buttonPlay.IsEnabled = false;
                buttonPause.IsEnabled = true;
                buttonNext.IsEnabled = _musicController.HasSongsInPlaylist;
                buttonStop.IsEnabled = true;
                sliderVolume.IsEnabled = true;
            }
            else
            {
                buttonPlay.IsEnabled = _musicController.HasSongsInPlaylist;
                buttonPause.IsEnabled = false;
                buttonNext.IsEnabled = _musicController.HasSongsInPlaylist;
                buttonStop.IsEnabled = false;
                sliderVolume.IsEnabled = false;
            }
        }

        private void LoadMusicData()
        {
            musicListBox.Items.Refresh();
        }

        private void LoadMovieData()
        {
            movieListBox.Items.Refresh();
        }

        private void ClearSelected()
        {
            _activeController.ClearSelected();
            musicListBox.SelectedIndex = -1;
            movieListBox.SelectedIndex = -1;

            textBoxSinger.Clear();
            textBoxSongTitle.Clear();
            textBoxDirector.Clear();
            textBoxMovieTitle.Clear();

            buttonDeleteSong.IsEnabled = false;
            buttonDeleteMovie.IsEnabled = false;

            this.SetMusicPlay();
        }

        private void SelectMusicItem(Song song)
        {
            textBoxSinger.Text = song.Singer;
            textBoxSongTitle.Text = song.Title;

            _musicController.ChangeSelected(song);
        }

        private void SetMusicForm()
        {
            if (_musicController.Selected != null)
            {
                buttonAddMusicFile.IsEnabled = true;
                checkBoxMusicFilePresent.IsChecked = _musicController.Selected.File != null || _newFile != null;
                buttonDeleteSong.IsEnabled = true;
                buttonAddToPlaylist.IsEnabled = _musicController.Selected.File != null;
            }
            else
            {
                musicListBox.SelectedItem = null;
                textBoxSinger.Text = null;
                textBoxSongTitle.Text = null;
                buttonAddMusicFile.IsEnabled = true;
                checkBoxMusicFilePresent.IsChecked = false;
                buttonDeleteSong.IsEnabled = false;
                buttonAddToPlaylist.IsEnabled = false;
            }
        }

        private void SelectMovieItem(Movie movie)
        {
            textBoxDirector.Text = movie.Director;
            textBoxMovieTitle.Text = movie.Title;

            _movieController.ChangeSelected(movie);
        }

        private void SetMovieForm()
        {
            if (_movieController.Selected != null)
            {
                buttonAddMovieFile.IsEnabled = true;
                checkBoxMovieFilePresent.IsChecked = _movieController.Selected.File != null || _newFile != null;
                buttonDeleteMovie.IsEnabled = true;
                buttonPlayMovie.IsEnabled = _movieController.Selected.File != null;
            }
            else
            {
                buttonAddMovieFile.IsEnabled = true;
                checkBoxMovieFilePresent.IsChecked = false;
                buttonDeleteMovie.IsEnabled = false;
                buttonPlayMovie.IsEnabled = false;

                textBoxDirector.Text = "";
                textBoxSongTitle.Text = "";
                movieListBox.SelectedItem = null;
            }
        }

        private void SetButtons()
        {
            if (_activeController.GetType() == typeof(MusicController))
            {
                this.SetMusicForm();
                this.SetMusicPlay();
            }
            else
            {
                this.SetMovieForm();
            }
        }
        #endregion
    }
}
