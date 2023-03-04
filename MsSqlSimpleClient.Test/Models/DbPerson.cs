using MsSqlSimpleClient.Attributes.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Test.Models
{
    public sealed class DbPerson
    {
        [SqlIdentity]
        [SqlColumnName("PersonId")]
        public int Id { get; set; }

        [SqlRequired]
        [SqlColumnName("first_name")]
        public string FirstName { get; set; } = "";

        [SqlIgnore]
        public string LastName { get; set; } = "";

        [SqlRequired]
        [SqlExtendedCollection]
        public IEnumerable<DbCity> _CityCollection { get; set; } = Enumerable.Empty<DbCity>();

        public DbCity? City => this._CityCollection.SingleOrDefault();
    }
}
