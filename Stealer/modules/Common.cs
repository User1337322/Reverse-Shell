/* 
    Author : LimerBoy
    Github : github.com/LimerBoy/Adamantium-Thief
*/

namespace Stealer
{
    public sealed class Common
    {
        public struct Password
        {
            public string hostname { get; set; }
            public string username { get; set; }    
            public string password { get; set; }
        }

        public struct Cookie
        {
            public string hostname { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public string expiresutc { get; set; }
            public string key { get; set; }
            public string value { get; set; }
            public string issecure { get; set; }

            #region Added proper values 
            public long CreationUtc { get; set; }
            public string HostKey { get; set; }
            public string TopFrameSiteKey { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public byte[] EncryptedValue { get; set; }
            public string Path { get; set; }
            public long ExpiresUtc { get; set; }
            public int IsSecure { get; set; }
            public int IsHttpOnly { get; set; }
            public long LastAccessUtc { get; set; }
            public int HasExpires { get; set; }
            public int IsPersistent { get; set; }
            public int Priority { get; set; }
            public int SameSite { get; set; }
            public int SourceScheme { get; set; }
            public int SourcePort { get; set; }
            public int IsSameParty { get; set; }
            public long LastUpdateUtc { get; set; }
            #endregion
        }

        internal struct CreditCard
        {
            public string number { get; set; }
            public string expyear { get; set; }
            public string expmonth { get; set; }
            public string name { get; set; }
        }

        public struct AutoFill
        {
            public string name;
            public string value;
        }

        internal struct Site
        {
            public string hostname { get; set; }
            public string title { get; set; }
            public string date { get; set; }
            public int visits { get; set; }
        }

        public struct Bookmark
        {
            public string hostname { get; set; }
            public string title { get; set; }
            public string added { get; set; }
        }
    }
}
