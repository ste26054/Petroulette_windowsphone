using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Petroulette_windowsphone;
using petroulette.model;
using petroulette.parser;
using petroulette.model.parser;



namespace Petroulette_windowsphone_unitTests
{
    [TestClass]
    public class Petroulette_UnitTests
    {
      
        
        [TestMethod]
        public void VideoUrlTest()
        {
            Video video = new Video("http://www.youtube.com/watch?v=plWnm7UpsXk");
            Assert.IsTrue(video.getYoutubeId().Equals("plWnm7UpsXk"));
        }

        [TestMethod]
        public void BirthdayTest()
        {
            DateTime birthday = new DateTime(2008,10,25);
            Assert.IsTrue(Checker.check_pet_birthDate("2008-10-25") == birthday);
        }

        [TestMethod]
        public void PetCreatedDateTest()
        {
            DateTime birthday = new DateTime(2013, 04, 11);
            Assert.IsTrue(Checker.check_pet_createdDate("2013-04-11 06:57:21.278656+00:00") == birthday);
        }

        [TestMethod]
        public void ShelterCreatedDateTest()
        {
            DateTime birthday = new DateTime(2013, 03, 01);
            Assert.IsTrue(Checker.check_shelter_creationDate("2013-03-01 09:01:45.353121+00:00") == birthday);
        }


    }
}
