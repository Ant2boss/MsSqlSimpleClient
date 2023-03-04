using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Exceptions
{
    public sealed class DataParserIdentityMissingException : ArgumentException
    {
        public DataParserIdentityMissingException()
        {
        }

        public DataParserIdentityMissingException(string? message) : base(message)
        {
        }

        public DataParserIdentityMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DataParserIdentityMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public DataParserIdentityMissingException(string? message, string? paramName) : base(message, paramName)
        {
        }

        public DataParserIdentityMissingException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException)
        {
        }
    }
}
