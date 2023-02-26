using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Exceptions
{
    public sealed class SqlConnectionFailedException : Exception
    {
        public SqlConnectionFailedException()
        {
        }

        public SqlConnectionFailedException(string? message) : base(message)
        {
        }

        public SqlConnectionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlConnectionFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
