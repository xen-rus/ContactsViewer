using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContactsSyncViewer.Database;
using ContactsSyncViewer.Interfaces;
using ContactsSyncViewer.Models;
using Xamarin.Forms;

namespace ContactsSyncViewer.Services
{
    class Synchronization
    {
        public async Task<bool> SynchronizeAsync()
        {
           var internalContacts = await DependencyService.Get<IContactsImporter>().GetContactsAsync();

            var synchComplete = DependencyService.Get<IContactsImporter>().Synch;

            if (synchComplete)
                return DBSingletone.Database.SynchronizeContacts(internalContacts);
            else
                return false;
        }
    }
}
