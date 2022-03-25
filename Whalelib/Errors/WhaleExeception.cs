using System;

namespace Whalelib.Errors;

public class WhaleExeception : Exception
{
    public WhaleExeception() : base() { }
    public WhaleExeception(string message) : base(message) { }
}
