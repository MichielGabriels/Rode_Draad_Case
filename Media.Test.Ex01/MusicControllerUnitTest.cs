using Media.Controller.Ex01;
using Media.DataModel;
using Media.Player;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Test._04
{
    [TestFixture]
    public class MusicControllerUnitTest
    {
        private MusicController _sut; //System under test
        private Mock<IPlayer> _mockPlayer;
        private Mock<IPlaylist> _mockPlaylist;

        [SetUp]
        public void TestInit()
        {
            _mockPlayer = new Mock<IPlayer>();
            _mockPlaylist = new Mock<IPlaylist>();
            _sut = new MusicController(_mockPlayer.Object, _mockPlaylist.Object);
        }

        [Test]
        public void Constructor_WhenInitilized_ShouldHaveDataInOnlyTheList()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual(2, _sut.List.Count);
            Assert.IsFalse(_sut.IsPlaying);
            Assert.IsFalse(_sut.HasSongsInPlaylist);
        }

        [Test]
        public void IsPlaying_WhenPlayerIsPlaying_ShouldBeTrue()
        {
            //Arrange
            _mockPlayer.Setup(p => p.IsPlaying).Returns(true);

            //Act
            var result = _sut.IsPlaying;

            //Assert
            Assert.IsTrue(result);
            _mockPlayer.Verify(x => x.IsPlaying, Times.Once);
        }

        [Test]
        public void HasSongInPlaylist_WhenSongIsInPlaylist_ShouldBeTrue()
        {
            // Arrange
            _mockPlaylist.Setup(p => p.IsEmpty).Returns(false);

            // Act
            var result = _sut.HasSongsInPlaylist;

            // Assert
            Assert.IsTrue(result);
            _mockPlaylist.Verify(x => x.IsEmpty, Times.Once);
        }

        [Test]
        public void FileFilter_ShouldBeCorrectFilter()
        {
            // Arrange
            String expected = "Music Files(*.mp3) | *.mp3;";
            String actual;

            // Act
            actual = _sut.FileFilter();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InitializeData_ShouldLoadDataInList()
        {
            // Arrange

            // Act

            // Assert
            Assert.IsNotNull(_sut.List);
        }

        [Test]
        public void Pause_WhenPlayerIsPlaying_ShouldPausePlayer()
        {
            // Arrange

            // Act

            // Assert
            Assert.IsFalse(_sut.IsPlaying);
        }

        [Test]
        public void StopPlaying_ShouldStopPlayer()
        {
            // Arrange

            // Act

            // Assert
            Assert.IsFalse(_sut.IsPlaying);
        }

        [Test]
        public void PlayFromPlaylist_WhenSongIsInPlaylist_ShouldPlaySongFromPlaylist()
        {
            // Arrange

            // Act

            // Assert
            Assert.IsTrue(_sut.IsPlaying);
        }
        
        /*
        [Test]
        public void RemoveSongFromPlaylist_ShouldRemoveSongFromPlaylist(Song oldSong)
        {
            // Arrange

            // Act

            // Assert

        }

        [Test]
        public void AddSelectedToPlaylist_ShouldAddSelectedToPlaylist()
        {
            // Arrange

            // Act

            // Assert
            
        }
        */
    }
}
