using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlSimpleClient.Exceptions
{
    public sealed class DataParserRequiredColumnMissingException : ArgumentException
    {
        public DataParserRequiredColumnMissingException()
        {
        }

        public DataParserRequiredColumnMissingException(string? message) : base(message)
        {
        }

        public DataParserRequiredColumnMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DataParserRequiredColumnMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public DataParserRequiredColumnMissingException(string? message, string? paramName) : base(message, paramName)
        {
        }

        public DataParserRequiredColumnMissingException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException)
        {
        }
    }
}
