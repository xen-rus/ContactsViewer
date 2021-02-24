using System;
using System.Collections.Generic;
using System.Text;
using ContactsSyncViewer.Interfaces;
using SQLite;

namespace ContactsSyncViewer.Database
{
    [Table("Contacts")]
    public class Contact : IContact
    {
        [PrimaryKey,Column("_id")]
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Phone { get; set; }

        /*
        Hash for IOS
        TimeStamp for Android
        */
        public virtual string AdditionalSynchData { get; set; }

    }
}
