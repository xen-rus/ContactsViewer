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
        public bool SynchronizeContacts(Dictionary<int, IContact> contDictionary)
        {
            try
            {
                var contactsDBList = database.Table<Contact>().ToList();

                // Remove Contacts from DB
                bool isComplete =  RemoveFromDB(contactsDBList, contDictionary);
                if (!isComplete)
                    throw (new Exception("Can't remove"));

                // Add or Update contacts to DB
                isComplete = AddOrUpdateDB(contactsDBList, contDictionary);

                if (!isComplete)
                    throw (new Exception("Can't add"));


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private bool RemoveFromDB(List<Contact> _contactsDBList, Dictionary<int, IContact> _contDictionary)
        {
            try
            { 
            if (_contactsDBList.Count > _contDictionary.Count)
                for (int i = 0; i < _contactsDBList.Count; i++)
                {
                    if (!_contDictionary.ContainsKey(_contactsDBList[i].Id))
                        DeleteItem(_contactsDBList[i].Id);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        

        private bool AddOrUpdateDB(List<Contact> _contactsDBList, Dictionary<int, IContact> _contDictionary)
        {
            bool isEmpty = _contactsDBList.Count == 0;

            try
            {
                foreach (var key in _contDictionary.Keys)
                {

                    Contact oneContact = (_contDictionary[key] as Contact);

                    if (!isEmpty)
                    {

                        var infoID = _contactsDBList.Where(item => item.Id == key);

                        if (infoID.ToList().Count > 0)
                        {
                            if (infoID.First().AdditionalSynchData != _contDictionary[key].AdditionalSynchData)
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
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void DeleteAll()
        {
            database.DeleteAll<Contact>();
        }
    }

}
