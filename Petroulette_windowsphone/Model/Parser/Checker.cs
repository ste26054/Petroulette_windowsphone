using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petroulette.model;
using petroulette.model.api;
using petroulette.model.parser;

namespace petroulette.parser
{
   public static class Checker
    {
        #region checker : set of methods that control and format attributes we got from Jsons
        // pet attributes


        public static uint check_pet_id(Generic_RandomPet generic_randomPet)
        {
            return uint.Parse(generic_randomPet.genericPet.data.video.pet_id);
        }
        public static uint check_pet_nextCounts { get; set; } //TODO

        public static string check_pet_name(Generic_RandomPet generic_randomPet)
        {
            return generic_randomPet.genericPet.data.video.pet.name;

        }
        public static string check_pet_race(Generic_PetDetails generic_petDetails)
        {
            return generic_petDetails.genericPetDetails.data.pet.race_name;
        }
        public static string check_pet_specie(Generic_PetDetails generic_petDetails)
        {
            return generic_petDetails.genericPetDetails.data.pet.species_name;
        }
        public static string check_pet_description(Generic_RandomPet generic_randomPet)
        {
            return generic_randomPet.genericPet.data.video.pet.description;
        }
        public static DateTime check_pet_birthDate(Generic_RandomPet generic_randomPet)
        {
            DateTime date;
            string date_string = generic_randomPet.genericPet.data.video.pet.date_of_birth;
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
        public static DateTime check_pet_createdDate(Generic_RandomPet generic_randomPet)
        {
            DateTime date;
            string date_string = generic_randomPet.genericPet.data.video.pet.created_datetime;

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
        public static Video check_pet_currentVideo(Generic_RandomPet generic_randomPet)
        {

            string url = generic_randomPet.genericPet.data.video.video_link;
            Video v = new Video(url);

            return v;

        }
        public static List<Video> check_pet_videoList(Generic_PetDetails generic_petDetails)
        {
            List<Video> list = new List<Video>();

            foreach (petroulette.model.parser.Generic_PetDetails.modelDetailVideo video in generic_petDetails.genericPetDetails.data.pet.videos)
            {
                list.Add(new Video(video.video_link));
            }


            return list;
        }
        public static uint check_shelter_id(Generic_PetDetails generic_petDetails)
        {
            return uint.Parse(generic_petDetails.genericPetDetails.data.pet.organization_id);
        }
        public static string check_shelter_phoneNumber(Generic_PetDetails generic_petDetails)
        {
            return generic_petDetails.genericPetDetails.data.pet.organization.contact_number;
        }
        public static string check_shelter_name(Generic_PetDetails generic_petDetails)
        {
            return generic_petDetails.genericPetDetails.data.pet.organization.name;
        }
        public static string check_shelter_adress(Generic_PetDetails generic_petDetails)
        {
            return generic_petDetails.genericPetDetails.data.pet.organization.address;
        }
        public static string check_shelter_email(Generic_PetDetails generic_petDetails)
        {
            return generic_petDetails.genericPetDetails.data.pet.organization.email;
        }
        public static DateTime check_shelter_creationDate(Generic_PetDetails generic_petDetails)
        {
            DateTime date;
            string date_string = generic_petDetails.genericPetDetails.data.pet.organization.created_datetime;

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
                   check_pet_id(random), check_pet_name(random), check_pet_race(details),
                   check_pet_specie(details), check_pet_description(random),
                   check_pet_birthDate(random), check_pet_createdDate(random),
                   check_pet_currentVideo(random));

            currentPet.setDetails(// Pet details fill
                check_shelter_id(details), check_shelter_phoneNumber(details),
                check_shelter_name(details), check_shelter_adress(details)
                , check_shelter_email(details), check_shelter_creationDate(details),
                check_pet_videoList(details));

            return currentPet;
        }
    }
}
