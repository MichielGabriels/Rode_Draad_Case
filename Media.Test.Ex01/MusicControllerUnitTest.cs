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
    }
}
