using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmLight4.Model.Parser
{
    class Generic_Appointment
    {
        public class modelAppointmentQuery
        {
            public string time { get; set; }
            public string sql { get; set; }
        }

        public class modelAppointmentData
        {
            public List<modelAppointmentQuery> queries { get; set; }
            public string appointment_result { get; set; }
        }

        public class modelAppointmentMessage
        {
            public string message { get; set; }
            public string type { get; set; }
        }

        public class modelAppointmentRootObject
        {
            public modelAppointmentData data { get; set; }
            public string html { get; set; }
            public List<modelAppointmentMessage> messages { get; set; }
            public bool success { get; set; }
        }


                public modelAppointmentRootObject genericAppointment;

        public Generic_Appointment(string toParse)
        {
            genericAppointment = JsonConvert.DeserializeObject<modelAppointmentRootObject>(toParse);
       //     System.Diagnostics.Debug.WriteLine(genericPetDetails.data.pet.description);

        }
    }
}
