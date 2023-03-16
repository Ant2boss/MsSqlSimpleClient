using MsSqlSimpleClient.Attributes.SqlTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Test.Models
{
    public sealed class DbState
    {
        [SqlIdentity]
        public int IdState { get; set; }
        public string StateName { get; set; } = "";

        [SqlExtendedCollection]
        public IEnumerable<DbCity> Cities { get; set; } = Enumerable.Empty<DbCity>();
    }
}
