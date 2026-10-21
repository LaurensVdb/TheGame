using System;

namespace Domain.Exceptions;

public class InvalidMoveException : Exception
{
    public InvalidMoveException(string? message) : base(message)
    {
    }
}
