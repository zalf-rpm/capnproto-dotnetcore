using System;

namespace CapnpC.CSharp.Generator.Model;

internal class InvalidSchemaException : Exception
{
    public InvalidSchemaException(string message) : base(message)
    {
    }
}