using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshkatEnterprise.Booklet.Entity
{
    public class UserPreference
    {
        public List<StateItem> State { get; set; }
        public long PersonId { get; set; }
    }
}
