using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class StateItem
    {
        public StateItem(String p, String toc)
        {
            ParagraphId = p;
            TableOfContentId = toc;
        }
        public String ParagraphId { set; get; }

        public String TableOfContentId { set; get; }
    }
}
