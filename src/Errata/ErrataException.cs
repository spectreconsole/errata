using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Errata
{
    /// <summary>
    /// Represents errors that occur within Errata.
    /// </summary>
    public sealed class ErrataException : Exception
    {
        /// <summary>
        /// Gets any contextual information.
        /// </summary>
        public Dictionary<string, object?> Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrataException"/> class.
        /// </summary>
        public ErrataException()
        {
            Context = new Dictionary<string, object?>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrataException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ErrataException(string message)
            : base(message)
        {
            Context = new Dictionary<string, object?>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrataException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.
        /// </param>
        public ErrataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Context = new Dictionary<string, object?>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrataException"/> class
        /// with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or a null reference if no inner exception is specified.
        /// </param>
        public ErrataException(string? message, Exception? innerException)
            : base(message, innerException)
        {
            Context = new Dictionary<string, object?>();
        }

        internal ErrataException WithContext(string key, object? value)
        {
            Context[key] = value;
            return this;
        }
    }
}
