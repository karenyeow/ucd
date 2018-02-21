using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Comlib.Common.Framework.Middlewares
{
    /// <summary>
    /// Traces the request data which is read by the server.
    /// </summary>
    public class RequestLoggerStream : Stream
    {
        private readonly MemoryStream _tracerStream;
        private readonly bool _ownsParent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggerStream"/> class.
        /// </summary>
        /// <param name="inner">
        /// The stream which is being wrapped.
        /// </param>
        /// <param name="ownsParent">
        /// A value indicating whether the <paramref name="inner"/> should be disposed when the
        /// <see cref="RequestLoggerStream"/> is disposed.
        /// </param>
        public RequestLoggerStream(Stream inner, bool ownsParent)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            if (inner is RequestLoggerStream)
            {
                throw new InvalidOperationException("nesting of RequestLoggerStream objects is not allowed");
            }

            this.Inner = inner;
            _tracerStream = new MemoryStream();
            this._ownsParent = ownsParent;
        }

        /// <summary>
        /// Gets a <see cref="MemoryStream"/>, which holds a copy of all data which was written to the inner
        /// stream.
        /// </summary>
        public MemoryStream TracerStream => _tracerStream;

        /// <summary>
        /// Gets the <see cref="Stream"/> around which this <see cref="RequestLoggerStream"/> wraps.
        /// </summary>
        public Stream Inner { get; }

        /// <inheritdoc/>
        public override bool CanRead => Inner.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => Inner.CanSeek;

        /// <inheritdoc/>
        public override bool CanWrite => Inner.CanWrite;

        /// <inheritdoc/>
        public override long Length => Inner.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => Inner.Position;

            set => Inner.Position = value;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            Inner.Flush();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = Inner.Read(buffer, offset, count);
            _tracerStream.Write(buffer, offset, read);
            return read;
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            _tracerStream.Seek(offset, origin);
            return Inner.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            _tracerStream.SetLength(value);
            Inner.SetLength(value);
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _tracerStream.Write(buffer, offset, count);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            _tracerStream.Dispose();

            if (_ownsParent)
            {
                Inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
