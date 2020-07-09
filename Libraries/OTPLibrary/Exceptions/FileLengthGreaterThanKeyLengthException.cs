﻿using System;

namespace OTPLibrary.Exceptions
{
    public class FileLengthGreaterThanKeyLengthException : Exception
    {
        public FileLengthGreaterThanKeyLengthException() { }

        public FileLengthGreaterThanKeyLengthException(string message) : base(message) { }

        public FileLengthGreaterThanKeyLengthException(string message, Exception innerException) : base(message, innerException) { }
    }
}