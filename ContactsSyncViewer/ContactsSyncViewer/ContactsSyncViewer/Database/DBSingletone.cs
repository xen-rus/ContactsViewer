using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContactsSyncViewer.Database
{
    class DBSingletone
    {
        public const string DATABASE_NAME = "Contacts.db";

        private static ContactsRepository database;
        public static ContactsRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new ContactsRepository(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME));
                }
                return database;
            }
        }
    }
}
