using Media.DataModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Media.Controller.Ex01;

namespace Media.Test._04
{
    [TestFixture]
    public class MovieControllerUnitTest
    {
        private MovieController _sut; //System under test

        [SetUp]
        public void TestInit()
        {
            _sut = new MovieController();
        }

        [Test]
        public void Constructor_WhenInitilized_ShouldHaveDataInOnlyTheList()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual(1, _sut.List.Count);
        }

        [Test]
        public void FileFilter_ShouldBeCorrectFilter()
        {
            //Arrange
            string expected = "Video Files(*.avi) | *.avi;";
            string actual;

            //Act
            actual = _sut.FileFilter();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
