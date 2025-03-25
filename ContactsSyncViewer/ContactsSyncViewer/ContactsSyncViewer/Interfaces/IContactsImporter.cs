using ContactsSyncViewer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSyncViewer.Interfaces
{

    public interface IContactsImporter
    {
        Task<Dictionary<int, IContact>> GetContactsAsync();
        bool Synch { get ;}
    }
}
