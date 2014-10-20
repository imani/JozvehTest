using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class BookHighlight
    {
        public long HighlightId{ get; set; }
        public Section HighlightSection{ get; set; }
        public String Color{ get; set; }
        public long PersonId{ get; set; }
    }
}
