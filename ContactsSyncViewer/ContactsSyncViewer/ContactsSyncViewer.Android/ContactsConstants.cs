using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ContactsSyncViewer.Droid
{
    public class ContConst
    {
        public const string ContcatsID = ContactsContract.Contacts.InterfaceConsts.Id;
        public const string ContcatsHasNumber = ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber;

        public const string CommonDataKindsId = ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId;
        public const string CommonDataKindsNumber = ContactsContract.CommonDataKinds.Phone.Number;
        public const string CommonDataKindsFamilyName = ContactsContract.CommonDataKinds.StructuredName.FamilyName;
        public const string CommonDataKindsGivenName = ContactsContract.CommonDataKinds.StructuredName.GivenName;
        public const string CommonDataKindsContactLastUpdatedTimestamp = ContactsContract.CommonDataKinds.StructuredName.InterfaceConsts.ContactLastUpdatedTimestamp;
        public const string CommonDataKindsMimetype = ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Mimetype;

        public const string MimeName = "vnd.android.cursor.item/name";
        public const string MimeNum = "vnd.android.cursor.item/phone_v2";

        public readonly static string[]  projContacts = {
                          ContcatsID,
                          ContcatsHasNumber
                                };

        public readonly static string[] projData = {
                            CommonDataKindsNumber,
                            CommonDataKindsFamilyName,
                            CommonDataKindsGivenName,
                            CommonDataKindsContactLastUpdatedTimestamp,
                            CommonDataKindsMimetype
                            };

        public const string selector = CommonDataKindsId + " = ?" + " AND (" +
                  CommonDataKindsMimetype + " = 'vnd.android.cursor.item/phone_v2' "
                  + " OR " +
                  CommonDataKindsMimetype + " = 'vnd.android.cursor.item/name')";

    }
}