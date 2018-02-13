using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Comlib.Common.Helpers.Middlewares
{
    /// <summary>
    /// A <see cref="Stream"/> which wraps around another <see cref="Stream"/> and copies all data to a <see cref="MemoryStream"/>.
    /// Used by the logging framework.
    /// </summary>
    public class ResponseLoggerStream : Stream
    {
        private readonly Stream _inner;
        private readonly MemoryStream _tracerStream;
        private readonly bool _ownsParent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseLoggerStream"/> class.
        /// </summary>
        /// <param name="inner">
        /// The stream which is being wrapped.
        /// </param>
        /// <param name="ownsParent">
        /// A value indicating whether the <paramref name="inner"/> should be disposed when the
        /// <see cref="RequestLoggerStream"/> is disposed.
        /// </param>
        public ResponseLoggerStream(Stream inner, bool ownsParent)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            if (inner is ResponseLoggerStream)
            {
                throw new InvalidOperationException("nesting of ResponseLoggerStream objects is not allowed");
            }

            _inner = inner;
            _tracerStream = new MemoryStream();
            _ownsParent = ownsParent;
        }

        /// <summary>
        /// Gets a <see cref="MemoryStream"/>, which holds a copy of all data which was written to the inner
        /// stream.
        /// </summary>
        public MemoryStream TracerStream => _tracerStream;

        /// <summary>
        /// Gets the <see cref="Stream"/> around which this <see cref="ResponseLoggerStream"/> wraps.
        /// </summary>
        public Stream Inner => _inner;

        /// <inheritdoc/>
        public override bool CanRead => _inner.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => _inner.CanSeek;

        /// <inheritdoc/>
        public override bool CanWrite => _inner.CanWrite;

        /// <inheritdoc/>
        public override long Length => _inner.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => _inner.Position;

            set => _inner.Position = value;
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            _inner.Flush();
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            _tracerStream.Read(buffer, offset, count);
            return _inner.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            _tracerStream.Seek(offset, origin);
            return _inner.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            _tracerStream.SetLength(value);
            _inner.SetLength(value);
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _tracerStream.Write(buffer, offset, count);
            _inner.Write(buffer, offset, count);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            _tracerStream.Dispose();

            if (_ownsParent)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
