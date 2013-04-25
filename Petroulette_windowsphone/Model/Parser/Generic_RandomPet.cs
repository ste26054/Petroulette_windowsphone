using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace petroulette.model.parser
{
   public class Generic_RandomPet
   {
       
       #region Generic_RandomPet_attributes

       public class modelRandomPet
       {
           public string species_id { get; set; }
           public string description { get; set; }
           public string created_datetime { get; set; }
           public string available_until { get; set; }
           public string organization_id { get; set; }
           public string date_of_birth { get; set; }
           public string race_id { get; set; }
           public string id { get; set; }
           public string name { get; set; }
       }

       public class modelRandomVideo
       {
           public string title { get; set; }
           public string ordering { get; set; }
           public string published { get; set; }
           public modelRandomPet pet { get; set; }
           public string video_link { get; set; }
           public string created_date { get; set; }
           public string created_by_id { get; set; }
           public string pet_id { get; set; }
           public string id { get; set; }
       }

       public class modelRandomQuery
       {
           public string time { get; set; }
           public string sql { get; set; }
       }

       public class modelRandomData
       {
           public modelRandomVideo video { get; set; }
           public List<modelRandomQuery> queries { get; set; }
       }

       public class modelRandomMessage
       {
           public string message { get; set; }
           public string type { get; set; }
       }

       public class modelRandomRootObject
       {
           public modelRandomData data { get; set; }
           public string html { get; set; }
           public List<modelRandomMessage> messages { get; set; }
           public bool success { get; set; }
       }


       

         public modelRandomRootObject genericPet;

       #endregion

        #region Generic_RandomPet_methods

        public Generic_RandomPet(string toParse)
        {
            genericPet = JsonConvert.DeserializeObject<modelRandomRootObject>(toParse);
            System.Diagnostics.Debug.WriteLine(genericPet.data.video.video_link);


        }

        #endregion

   }
}
