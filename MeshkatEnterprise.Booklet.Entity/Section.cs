using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class Section
    {
        public long ParagraphId{ get; set; }
        public int StartOffset{ get; set; }
        public int EndOffset{ get; set; }
    }
}
