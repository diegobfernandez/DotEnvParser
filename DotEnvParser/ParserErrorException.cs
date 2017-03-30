using System;
using System.Collections.Generic;

namespace DotEnvParser
{
    public class ParserErrorException<T> : Exception
    {
        public ParserErrorException(IEnumerable<T> errors)
        {
            Errors = errors;
        }

        public IEnumerable<T> Errors { get; }
    }
}
