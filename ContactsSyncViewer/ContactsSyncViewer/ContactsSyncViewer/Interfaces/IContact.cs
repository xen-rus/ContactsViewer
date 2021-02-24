using System;
using System.Collections.Generic;
using System.Text;


namespace ContactsSyncViewer.Interfaces
{

    public interface IContact
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Phone { get; set; }
        string AdditionalSynchData { get; set; }

    }



}
