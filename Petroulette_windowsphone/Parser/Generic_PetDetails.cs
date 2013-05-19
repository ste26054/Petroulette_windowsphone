using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace petroulette.model.parser
{
   public class Generic_PetDetails //Contains generic attributes generated from JSON obtained via /api/details/
    {
        #region Generic_PetDetails_attributes

        public class modelDetailVideo
        {
            public string title { get; set; }
            public string ordering { get; set; }
            public string published { get; set; }
            public string video_link { get; set; }
            public string created_date { get; set; }
            public string created_by_id { get; set; }
            public string pet_id { get; set; }
            public string id { get; set; }
        }

        public class modelDetailOrganization
        {
            public string youtube_channel { get; set; }
            public string contact_number { get; set; }
            public string user_id { get; set; }
            public string name { get; set; }
            public string created_datetime { get; set; }
            public string address { get; set; }
            public string email { get; set; }
        }

        public class modelDetailPet
        {
            public string species_id { get; set; }
            public string species_name { get; set; }
            public string description { get; set; }
            public List<modelDetailVideo> videos { get; set; }
            public string created_datetime { get; set; }
            public string available_until { get; set; }
            public string organization_id { get; set; }
            public string date_of_birth { get; set; }
            public string race_id { get; set; }
            public modelDetailOrganization organization { get; set; }
            public string race_name { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }

        public class modelDetailQuery
        {
            public string time { get; set; }
            public string sql { get; set; }
        }

        public class modelDetailData
        {
            public modelDetailPet pet { get; set; }
            public List<modelDetailQuery> queries { get; set; }
        }
        
        public class modelDetailMessage
        {
            public string message { get; set; }
            public string type { get; set; }
        }

        public class modelDetailRootObject
        {
            public modelDetailData data { get; set; }
            public string html { get; set; }
            public List<modelDetailMessage> messages { get; set; }
            public bool success { get; set; }
        }

        #endregion

        #region Generic_PetDetails

        public modelDetailRootObject genericPetDetails;

        public Generic_PetDetails(string toParse)
        {
            genericPetDetails = JsonConvert.DeserializeObject<modelDetailRootObject>(toParse);
       //     System.Diagnostics.Debug.WriteLine(genericPetDetails.data.pet.description);

        }

        #endregion
    }
}
