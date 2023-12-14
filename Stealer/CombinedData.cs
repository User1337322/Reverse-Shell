using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Stealer.Common;

namespace Stealer
{
    public class CombinedData
    {
        public List<Password> Passwords { get; set; }
        public List<Bookmark> Bookmarks { get; set; }
        public List<AutoFill> Autofill { get; set; }
    }
}
