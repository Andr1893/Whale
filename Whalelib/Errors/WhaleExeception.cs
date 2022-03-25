using System;

namespace Whalelib.Errors;

public class WhaleExeception : SystemException
{
    public readonly string MessageError;
    public readonly int Code;

    public WhaleExeception(string MessageError, int Code)
    {
        this.MessageError = MessageError;
        this.Code = Code;
    }
}
