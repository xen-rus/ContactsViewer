using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using ContactsSyncViewer.Interfaces;
using Contacts;
using System.Collections;
using ContactsSyncViewer.Models;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(ContactsSyncViewer.iOS.ContactsImporter))]
namespace ContactsSyncViewer.iOS
{

    class ContactsImporter : IContactsImporter
    {
        public bool Synch { get => bSynch; }

        private bool bSynch;


        public async Task<Dictionary<int, IContact>> GetContactsAsync()
        {
            var Dictionary = new Dictionary<int, IContact>();

            bSynch = true;

            try
            {
                return await Task.Run(() => { 
                var keysToFetch = new[] {
                        CNContactKey.PhoneNumbers, CNContactKey.GivenName, CNContactKey.FamilyName
                        };


                var containerId = new CNContactStore().DefaultContainerIdentifier;

                using (var predicate = CNContact.GetPredicateForContactsInContainer(containerId))
                {
                    CNContact[] contactList;
                    using (var store = new CNContactStore())
                    {
                        contactList = store.GetUnifiedContacts(predicate, keysToFetch, out
                            var error);
                    }
                    
                    for (int i = 0; i < contactList.Length; i++)
                    {

                        var hash = (contactList[i].PhoneNumbers.First().Value.StringValue + contactList[i].GivenName + contactList[i].FamilyName).GetHashCode().ToString();
                        Dictionary.Add(Convert.ToInt32(i), new InternalContacts
                        {
                            Phone = contactList[i].PhoneNumbers.First().Value.StringValue,
                            FirstName = contactList[i].GivenName,
                            LastName = contactList[i].FamilyName,
                            Id = i,
                            AdditionalSynchData = hash

                        }) ;

                        
                    }

                }
                    return Dictionary;
                });
            }
            catch 
            {
                bSynch = false;
            }
            return Dictionary;
        }
    }
}