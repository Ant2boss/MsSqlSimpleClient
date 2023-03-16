using MsSqlSimpleClient.Attributes.SqlTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Test.Models
{
    public sealed class DbCity
    {
        [SqlIdentity]
        public int Id { get; set; }

        [SqlRequired]
        public string Name { get; set; } = "";

        [SqlColumnName("Population")]
        public int Papilation { get; set; }
    }
}
