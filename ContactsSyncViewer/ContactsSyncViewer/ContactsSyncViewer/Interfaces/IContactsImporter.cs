using ContactsSyncViewer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsSyncViewer.Interfaces
{
    public interface IContactsImporter
    {
        Dictionary <int, IContact> GetContacts();
        bool Synch { get ;}
    }
}
