using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class Footnote
    {
        public long Id { set; get; }
        public String Text { set; get; }
        public Section Section { set; get; }
    }
}
