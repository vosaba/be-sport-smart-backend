﻿namespace Bss.Infrastructure.Errors.Abstractions;

public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified name of the queried object and its key.
    /// </summary>
    /// <param name="objectName">Name of the queried object.</param>
    /// <param name="key">The value by which the object is queried.</param>
    public NotFoundException(string key, string objectName)
        : base($"Queried object {objectName} was not found, Key: {key}")
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified name of the queried object, its key,
    /// and the exception that is the cause of this exception.
    /// </summary>
    /// <param name="objectName">Name of the queried object.</param>
    /// <param name="key">The value by which the object is queried.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NotFoundException(string key, string objectName, Exception innerException)
        : base($"Queried object {objectName} was not found, Key: {key}", innerException)
    {
    }
}

public class NotFoundException<TObject> : NotFoundException
{
    public NotFoundException(string key)
        : base(key, typeof(TObject).Name)
    {
    }

    public NotFoundException(object key)
        : this(key.ToString()!)
    {
    }

    public NotFoundException(string key, Exception innerException)
        : base(key, typeof(TObject).Name, innerException)
    {
    }

    public NotFoundException(object key, Exception innerException)
        : this(key.ToString()!, innerException)
    {
    }
}