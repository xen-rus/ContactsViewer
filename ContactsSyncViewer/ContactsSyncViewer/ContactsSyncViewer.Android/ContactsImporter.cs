using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using ContactsSyncViewer.Interfaces;
using Android.Provider;
using Android.Database;
using System.Diagnostics;

using ContactsSyncViewer.Models;
using System.Threading.Tasks;


[assembly: Xamarin.Forms.Dependency(typeof(ContactsSyncViewer.Droid.ContactsImporter))]
namespace ContactsSyncViewer.Droid
{
  
    class ContactsImporter : IContactsImporter
    {     
        // Task locker object
        object locker = new object();

        //Main Dictionary
        Dictionary<int, IContact> internalDictionary = new Dictionary<int, IContact>();

        //Const It change to use multitast function
        private const int MultyTaskContact = 50;


        // Sync commplite or failt
        public bool Synch { get => bSynch; }

        private bool bSynch;


        public Dictionary <int, IContact> GetContacts()
        {
            bSynch = true;

            internalDictionary.Clear();

            var cr = Android.App.Application.Context.ContentResolver;

#if DEBUG
            Stopwatch stopWach = new Stopwatch();
            stopWach.Start();
#endif
            int proc = System.Environment.ProcessorCount / 2;

            ICursor cur = cr.Query(ContactsContract.Contacts.ContentUri, ContConst.projContacts,
                   ContConst.ContcatsHasNumber + " = 1 " , null, null);

            var chunk = cur.Count / proc + 1;

            if(proc > 0 && cur.Count > MultyTaskContact) //FastMode MultyTask
            {
                Task<bool>[] cursorTasks = new Task<bool>[proc];

                for (int i = 0; i < proc; i++)
                {
                    int index = i;
                    cursorTasks[i] = Task.Run(async() => await  CoursorMultiTask(cr, index * chunk, (index + 1) * chunk));
                }

                Task.WaitAll(cursorTasks);

                foreach (var result in cursorTasks)
                {
                    bSynch = result.Result;
                    if (bSynch == false)
                        break;
                }            

            }
            else 
            {
                bSynch =  Coursor(cur, cr);
            }

            cur.Close();

#if DEBUG
            stopWach.Stop();
            Console.WriteLine(stopWach.ElapsedMilliseconds);
#endif
            return internalDictionary;

        }

        //Cycle to fill contact for 1 Task
        private bool Coursor(ICursor coursor,ContentResolver cr)
        {
            try
            {
                while (coursor.MoveToNext())
                     if (coursor.GetInt(coursor.GetColumnIndex(ContConst.ContcatsHasNumber)) > 0)
                    {
                        if (!CreateContact(coursor, cr)) break;
                    }


                coursor.Close();
                return true;
            }
            catch 
            {
                coursor.Close();
                return false;
            }
        }


        //Cycle to fill contact for multy Tasks
        private Task<bool> CoursorMultiTask( ContentResolver cr, int posStart = 0, int posEnd = 0)
        {
            ICursor coursor = cr.Query(ContactsContract.Contacts.ContentUri, ContConst.projContacts,
                       ContConst.ContcatsHasNumber + " = 1 ", null, null);

            try
            {
                coursor.MoveToPosition(posStart);

                while (coursor.Position < coursor.Count && coursor.Position < posEnd )
                { 
                    if (coursor.GetInt(coursor.GetColumnIndex(ContConst.ContcatsHasNumber)) > 0)
                    {
                        if (!CreateContact(coursor, cr)) break;
                    }
                    coursor.MoveToNext();
                }

                coursor.Close();
                return Task.FromResult(true);
            }
            catch
            {
                coursor.Close();
                return Task.FromResult(false);
            }
        }

        private bool CreateContact(ICursor coursor, ContentResolver cr)
        {
                InternalContacts contact = new InternalContacts();

                int iD = coursor.GetInt(coursor.GetColumnIndex(ContConst.ContcatsID));
                contact.Id = iD;
                string _id = iD.ToString();

                ICursor pCur = cr.Query(ContactsContract.Data.ContentUri,
                ContConst.projData,
                ContConst.selector,
                new String[] { _id },
                null);

                bool hasNum = false;
                bool hasName = false;

                while (pCur.MoveToNext())
                {
                    FillContact(contact, pCur, ref hasNum, ref hasName);

                    if (hasNum && hasName)
                        break;
                }

                pCur.Close();

                lock (locker)
                {
                    try
                    {
                        internalDictionary.Add(iD, contact);
                        return true;
                    }
                    catch
                    {
                        return true;

                    }
                }
        }

        private void FillContact(InternalContacts contact, ICursor pCur, ref bool num, ref bool name )
        {
            switch (pCur.GetString(pCur.GetColumnIndex(ContConst.CommonDataKindsMimetype)))
            {
                case ContConst.MimeName:
                    contact.LastName = pCur.GetString(pCur.GetColumnIndex(ContConst.CommonDataKindsFamilyName));
                    contact.FirstName = pCur.GetString(pCur.GetColumnIndex(ContConst.CommonDataKindsGivenName));
                    contact.AdditionalSynchData = pCur.GetString(pCur.GetColumnIndex(ContConst.CommonDataKindsContactLastUpdatedTimestamp));
                    name = true;
                    break;

                case ContConst.MimeNum:
                    contact.Phone = pCur.GetString(pCur.GetColumnIndex(ContConst.CommonDataKindsNumber));
                    num = true;
                    break;

                default:
                    break;
            }
        }

    }
}