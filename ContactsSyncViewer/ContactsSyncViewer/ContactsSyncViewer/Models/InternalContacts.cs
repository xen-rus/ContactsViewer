using ContactsSyncViewer.Database;
using ContactsSyncViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsSyncViewer.Models
{
    public class InternalContacts : Contact, IContact
    {
        public override int Id { get; set; }

        public override string FirstName { get; set; }

        public override string LastName { get; set; }

        public override string Phone { get; set; }

        /*
        Identifire for IOS
        TimeStamp for Android
        */
        public override string AdditionalSynchData { get; set; }

    }
}
