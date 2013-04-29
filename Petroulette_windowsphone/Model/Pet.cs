using System;
using System.Collections.Generic;

namespace petroulette.model
{
   public class Pet //Class that represents the real and final Pet Object
    {
        //********************* PROPERTIES ********************* //
        #region pet_attributes

        public uint pet_id { get;  set; }
        /*
        Equivalent to : 
         * private uint pet_id_private
         * public uint pet_id
         * {    get
         *          { return pet_id_private;
         *                                    }
         *      set
         *          { pet_id_private = value;
         *                                     }
         * 
         * 
         * }

        */
        public uint pet_nextCounts { get; set; }
        public string pet_name { get; set; }
        public string pet_race { get; set; }
        public string pet_specie { get; set; }
        public string pet_description { get; set; }
        public DateTime pet_birthDate { get; set; }
        public DateTime pet_createdDate { get; set; } //pet announcement creation date
        public Video pet_currentVideo { get; set; }
        public List<Video> pet_videoList { get; set; }
        
        public uint shelter_id { get; set; }
        public string shelter_phoneNumber { get; set; }
        public string shelter_name { get; set; }
        public string shelter_adress { get; set; }
        public string shelter_email { get; set; }
        public DateTime shelter_creationDate { get; set; }

        #endregion 

        #region pet_methods
        //********************* METHODS ********************* //

        //Creates the pet with informations provided by the parser after we call /api/random/
        public Pet(uint pet_id, string _name, string _race, string _specie, string _description, DateTime _birthDate, DateTime _createdDate, Video _video )
        {
            this.pet_id = pet_id;
            this.pet_name = _name; //calls the setter of pet_name property
            this.pet_race = _race;
            this.pet_specie = _specie;
            this.pet_description = _description;
            this.pet_birthDate = _birthDate;
            this.pet_createdDate = _createdDate;
            this.pet_currentVideo = _video;

            this.pet_videoList = new List<Video>();
        }

        //Adds details to the pet thanks to /api/details/
        public void setDetails(uint _shelter_id, string _phoneNumber, string _name, string _adress, string _email, DateTime _creationDate, List<Video> _videoList)
        {
            this.shelter_id = _shelter_id;
            this.shelter_phoneNumber = _phoneNumber;
            this.shelter_name = _name;
            this.shelter_adress = _adress;
            this.shelter_email = _email;
            this.shelter_creationDate = _creationDate;
            
            this.pet_videoList.Clear();
            this.pet_videoList.AddRange(_videoList);

        }


        #endregion

    }
}