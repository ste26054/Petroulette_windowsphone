using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petroulette.model;
using petroulette.model.api;
using petroulette.model.parser;

namespace petroulette.model.parser
{
   public static class Checker
    {
        #region checker : set of methods that control and format attributes we got from Jsons
        // pet attributes


        public static uint check_pet_id(string _id)
        {
            return uint.Parse(_id);
        }
        public static uint check_pet_nextCounts(string _next_counts)
        { return uint.Parse(_next_counts); } 

        public static string check_pet_name(string _pet_name)
        {
            return _pet_name;

        }
        public static string check_pet_race(string _pet_race)
        {
            return _pet_race;
        }
        public static string check_pet_specie(string _specie_name)
        {
            return _specie_name;
        }
        public static string check_pet_description(string _description)
        {
            return _description;
        }
        public static DateTime check_pet_birthDate(string _birthdate)
        {
            DateTime date;
            string date_string = _birthdate;
            try
            {
                date = DateTime.ParseExact(date_string, "yyyy-MM-dd", null);
                return date;
            }
            catch (FormatException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to convert '{0}' in BirthDate", date_string);
                return DateTime.ParseExact("2013-01-01", "yyyy-MM-dd", null);
            }


        }
        public static DateTime check_pet_createdDate(string _created_datetime)
        {
            DateTime date;
            string date_string = _created_datetime;

            try
            {
                date = DateTime.ParseExact(date_string.Split(' ')[0], "yyyy-MM-dd", null); //THROW EXCEPTION
                return date;
            }
            catch (FormatException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to convert '{0}' in CreatedDate", date_string);
                return DateTime.ParseExact("2013-01-01", "yyyy-MM-dd", null);
            }

        } //pet announcement creation date
        public static Video check_pet_currentVideo(string _videolink, string _videotitle)
        {

            string url = _videolink;
            Video v = new Video(url, _videotitle);

            return v;

        }
        public static List<Video> check_pet_videoList(List<petroulette.model.parser.Generic_PetDetails.modelDetailVideo> videos)
        {
            List<Video> list = new List<Video>();

            foreach (petroulette.model.parser.Generic_PetDetails.modelDetailVideo video in videos)
            {
                list.Add(new Video(video.video_link, video.title));
            }


            return list;
        }
        public static uint check_shelter_id(string _shelterId)
        {
            return uint.Parse(_shelterId);
        }
        public static string check_shelter_phoneNumber(string _phonNumber)
        {
            return _phonNumber;
        }
        public static string check_shelter_name(string _shelterName)
        {
            return _shelterName;
        }
        public static string check_shelter_adress(string _shelterAdress)
        {
            return _shelterAdress;
        }
        public static string check_shelter_email(string _organizationEmail)
        {
            return _organizationEmail;
        }
        public static DateTime check_shelter_creationDate(string _shelterCreationDate)
        {
            DateTime date;
            string date_string = _shelterCreationDate;

            try
            {
                date = DateTime.ParseExact(date_string.Split(' ')[0], "yyyy-MM-dd", null); //THROW EXCEPTION
                return date;
            }
            catch (FormatException)
            {
                System.Diagnostics.Debug.WriteLine("Unable to convert '{0}' in shelter_creationDate", date_string);
                return DateTime.ParseExact("2013-01-01", "yyyy-MM-dd", null);
            }
        }

        #endregion 


       public static Pet createPet( Generic_RandomPet random, Generic_PetDetails details)
        {

            Pet currentPet = new Pet(// Pet Creation 
                   check_pet_id(random.genericPet.data.video.pet_id), 
                   check_pet_name(random.genericPet.data.video.pet.name), 
                   check_pet_race(details.genericPetDetails.data.pet.race_name),
                   check_pet_specie(details.genericPetDetails.data.pet.species_name), 
                   check_pet_description(random.genericPet.data.video.pet.description),
                   check_pet_nextCounts(random.genericPet.data.video.pet.next_count),
                   check_pet_birthDate(random.genericPet.data.video.pet.date_of_birth), 
                   check_pet_createdDate(random.genericPet.data.video.pet.created_datetime),
                   check_pet_currentVideo(random.genericPet.data.video.video_link, random.genericPet.data.video.title));

            currentPet.setDetails(// Pet details fill
                check_shelter_id(details.genericPetDetails.data.pet.organization_id),
                check_shelter_phoneNumber(details.genericPetDetails.data.pet.organization.contact_number),
                check_shelter_name(details.genericPetDetails.data.pet.organization.name),
                check_shelter_adress(details.genericPetDetails.data.pet.organization.address),
                check_shelter_email(details.genericPetDetails.data.pet.organization.email),
                check_shelter_creationDate(details.genericPetDetails.data.pet.organization.created_datetime),
                check_pet_videoList(details.genericPetDetails.data.pet.videos));

            return currentPet;
        }
    }
}
