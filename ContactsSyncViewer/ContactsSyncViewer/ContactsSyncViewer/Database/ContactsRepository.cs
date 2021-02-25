using ContactsSyncViewer.Interfaces;
using ContactsSyncViewer.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsSyncViewer.Database
{
    public class ContactsRepository
    {
        SQLiteConnection database;
        public ContactsRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Contact>();
        }
        public IEnumerable<Contact> GetItems()
        {
            return database.Table<Contact>().ToList();
        }

        public int DeleteItem(int id)
        {
            return database.Delete<Contact>(id);
        }

        // Synchronize contact Dictionary to SQLite DB
        public bool SyncronizeContacts(Dictionary<int, IContact> contDictionary)
        {
            try
            {
                var contactsDBList = database.Table<Contact>().ToList();

                bool isEmpty = contactsDBList.Count == 0;

                // Remove Contacts from DB
                if (contactsDBList.Count > contDictionary.Count)
                    for (int i = 0; i < contactsDBList.Count; i++)
                    {
                        if (!contDictionary.ContainsKey(contactsDBList[i].Id))
                            DeleteItem(contactsDBList[i].Id);
                    }


                // Add or Update contacts to DB
                foreach (var key in contDictionary.Keys)
                {

                    Contact oneContact = (contDictionary[key] as Contact);

                    if (!isEmpty)
                    {

                        var infoID = contactsDBList.Where(item => item.Id == key);

                        if (infoID.ToList().Count > 0)
                        {
                            if (infoID.First().AdditionalSynchData != contDictionary[key].AdditionalSynchData)
                                database.Update(oneContact, typeof(Contact));
                            else
                                continue;
                        }
                        else
                            database.Insert(oneContact, typeof(Contact));
                    }
                    else
                        database.Insert(oneContact, typeof(Contact));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public void DeleteAll()
        {
            database.DeleteAll<Contact>();
        }
    }

}
