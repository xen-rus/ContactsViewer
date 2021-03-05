using System;
using System.Collections.Generic;
using System.Text;
using ContactsSyncViewer.Database;
using ContactsSyncViewer.Interfaces;
using ContactsSyncViewer.Models;
using Xamarin.Forms;

namespace ContactsSyncViewer.Services
{
    class Synchronization
    {
        public  bool Synchronize()
        {
           var internalContacts = DependencyService.Get<IContactsImporter>().GetContacts();

            var synchComplete = DependencyService.Get<IContactsImporter>().Synch;

            if (synchComplete)
                return DBSingletone.Database.SyncronizeContacts(internalContacts);
            else
                return false;
        }
    }
}
