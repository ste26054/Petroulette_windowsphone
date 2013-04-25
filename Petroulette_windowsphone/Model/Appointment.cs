using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petroulette.model
{
    class Appointment //Class that will handle appointments in the future
    {

        #region appointment_attributes

        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_phoneNumber { get; set; }
        public DateTime appointment_creationDate { get; set; }
        public DateTime requested_date { get; set; }
        public Pet appointment_pet { get; set; }

        #endregion

        #region appointment_methods

        public Appointment(string _name, string _email, string _phoneNumber, DateTime _requestedDate, Pet _pet)
        {
            this.user_name = _name;
            this.user_email = _email;
            this.user_phoneNumber = _phoneNumber;
            this.requested_date = _requestedDate;
            this.appointment_pet = _pet;
        }

        #endregion
    }
}
