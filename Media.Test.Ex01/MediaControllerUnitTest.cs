using Media.Controller.Ex01;
using Media.DataModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Test.Ex01
{
    [TestFixture]
    public class MediaControllerUnitTest
    {
        private MediaController _sut;
        
        [Test]
        public void ClearSelected_WhenSelectedIsSet_ShouldClearSelected()
        {
            //Arrange
            _sut = new TestMediaController(0);

            //Act
            _sut.ClearSelected();

            //Assert
            Assert.IsNull(_sut.Selected);
        }

        [Test]
        public void ChangeSelected_WhenItemInList_ShouldChangeSelected()
        {
            //Arrange
            _sut = new TestMediaController();

            //Act
            _sut.ChangeSelected(_sut.List[0]);

            //Assert
            Assert.AreEqual(_sut.Selected, _sut.List[0]);
        }
    }
}
