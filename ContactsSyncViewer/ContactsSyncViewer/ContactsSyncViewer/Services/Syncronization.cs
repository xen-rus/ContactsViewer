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
        public static bool Synchronize()
        {
           var InternalContacts = DependencyService.Get<IContactsImporter>().GetContacts();

            bool SyncComlete = DependencyService.Get<IContactsImporter>().Synch;

            if (SyncComlete)
                return DBSingletone.Database.SyncronizeContacts(InternalContacts);
            else
                return false;
        }
    }
}
