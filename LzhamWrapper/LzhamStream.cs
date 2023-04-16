using LzhamWrapper.Enums;

namespace LzhamWrapper
{
    public class LzhamStream : Stream
    {
        internal const int DefaultBufferSize = 8192;
        private Stream _stream;
        private readonly bool _leaveOpen;
        private readonly byte[] _buffer;
        private readonly CompressionHandle _compressionHandle;
        private readonly DecompressionHandle _decompressionHandle;

        private bool _readFinishing;
        private int _inputAvailable;
        private int _readOffset;


        public LzhamStream(Stream stream, CompressionParameters mode) : this(stream, mode, false)
        {
        }

        public LzhamStream(Stream stream, CompressionParameters mode, bool leaveOpen)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite)
                throw new ArgumentException("The base stream is not writeable", nameof(stream));
            _stream = stream;
            _leaveOpen = leaveOpen;
            _buffer = new byte[DefaultBufferSize];
            _compressionHandle = LzhamInterop.CompressInit(mode);
            if (_compressionHandle.IsInvalid)
            {
                throw new ApplicationException("Could not initialize compression stream with specified parameters");
            }
        }

        public LzhamStream(Stream stream, DecompressionParameters mode) : this(stream, mode, false)
        {
        }

        public LzhamStream(Stream stream, DecompressionParameters mode, bool leaveOpen)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("The base stream is not readable", nameof(stream));
            _stream = stream;
            _leaveOpen = leaveOpen;
            _buffer = new byte[DefaultBufferSize];
            _decompressionHandle = LzhamInterop.DecompressInit(mode);
            if (_decompressionHandle.IsInvalid)
            {
                throw new ApplicationException("Could not initialize Decompression stream with specified parameters");
            }
            ReadInput();
        }


        private void EnsureNotDisposed()
        {
            if (_stream == null)
                throw new ObjectDisposedException(null, "Can not access a closed Stream");
        }

        private void EnsureDecompressionMode()
        {
            if (_decompressionHandle == null || _decompressionHandle.IsInvalid)
                throw new InvalidOperationException("Reading from the compression stream is not supported");
        }

        private void EnsureCompressionMode()
        {
            if (_compressionHandle == null || _compressionHandle.IsInvalid)
                throw new InvalidOperationException("Writing to the compression stream is not supported");
        }

        public override void Flush()
        {
            EnsureNotDisposed();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("This operation is not supported");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("This operation is not supported");
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            EnsureDecompressionMode();
            ValidateParameters(buffer, offset, count);
            EnsureNotDisposed();
            int totalWritten = 0;
            int writeOffset = offset;

            do
            {
                DecompressStatus decompressionStatus;
                do
                {
                    int remaining = _inputAvailable;
                    int outSize = count - totalWritten;
                    decompressionStatus = LzhamInterop.Decompress(_decompressionHandle, _buffer, ref remaining, _readOffset, buffer, ref outSize, writeOffset, _readFinishing);
                    if (!(decompressionStatus == DecompressStatus.HasMoreOutput
                          || decompressionStatus == DecompressStatus.NeedsMoreInput
                          || decompressionStatus == DecompressStatus.NotFinished
                          || (decompressionStatus == DecompressStatus.Success))
                        )
                    {
                        throw new InvalidOperationException($"Unexpected Decompress Status Code {decompressionStatus}");
                    }
                    int written = outSize;
                    int read = remaining;
                    totalWritten += written;
                    writeOffset += written;
                    _inputAvailable = _inputAvailable - read;
                    _readOffset += read;
                } while (decompressionStatus == DecompressStatus.HasMoreOutput && totalWritten < count);

                if (_inputAvailable == 0)
                {
                    ReadInput();
                }

            } while (totalWritten < count && _inputAvailable > 0);

            return totalWritten;
        }

        private void ReadInput()
        {
            if (!_readFinishing)
            {
                _inputAvailable = _stream.Read(_buffer, 0, _buffer.Length);
                _readOffset = 0;
            }
            _readFinishing = _readFinishing || _inputAvailable == 0;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Write(buffer, offset, count, false);
        }

        private void Write(byte[] buffer, int offset, int count, bool finishing)
        {
            EnsureCompressionMode();
            ValidateParameters(buffer, offset, count);
            EnsureNotDisposed();
            int outSize = _buffer.Length;
            int remaining = count;
            do
            {
                CompressStatus compressionStatus;
                do
                {
                    compressionStatus = LzhamInterop.Compress(_compressionHandle, buffer, ref remaining, offset, _buffer, ref outSize, 0, finishing);
                    if (!(compressionStatus == CompressStatus.HasMoreOutput
                          || compressionStatus == CompressStatus.NeedsMoreInput
                          || (compressionStatus == CompressStatus.Success && finishing))
                        )
                    {
                        throw new InvalidOperationException($"Unexpected Compress Status Code {compressionStatus}");
                    }

                    int readBytes = remaining;
                    int writtenBytes = outSize;
                    if (writtenBytes > 0)
                    {
                        _stream.Write(_buffer, 0, writtenBytes);
                    }
                    remaining = remaining - readBytes;
                } while (compressionStatus == CompressStatus.HasMoreOutput);
            } while (remaining > 0);
        }

        private void ValidateParameters(byte[] array, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (array.Length - offset < count)
                throw new ArgumentException("Offset plus count is larger than the length of target array");
        }

        public Stream BaseStream => _stream;

        public override bool CanSeek => false;
        public override bool CanRead
        {
            get
            {
                if (_stream == null)
                {
                    return false;
                }

                return (_decompressionHandle != null && !_decompressionHandle.IsInvalid && _stream.CanRead);
            }
        }

        public override bool CanWrite
        {
            get
            {
                if (_stream == null)
                {
                    return false;
                }

                return (_compressionHandle != null && !_compressionHandle.IsInvalid && _stream.CanWrite);
            }
        }
        public override long Length
        {
            get { throw new NotSupportedException("This operation is not supported"); }
        }

        public uint Finish()
        {
            uint? result = null;
            if (_compressionHandle != null && !_compressionHandle.IsInvalid)
            {
                FinishCompression();
                result = _compressionHandle.Finish();
            }
            if (_decompressionHandle != null && !_decompressionHandle.IsInvalid)
            {
                result = _decompressionHandle.Finish();
            }
            if (!_leaveOpen)
            {
                _stream?.Dispose();
            }
            if (!result.HasValue)
            {
                throw new InvalidOperationException("It appears that operation has already finished");
            }
            return result.Value;
        }

        private void FinishCompression()
        {
            Write(new byte[0], 0, 0, true);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                Finish();
            }
            finally
            {
                _stream = null;
                try
                {
                    _compressionHandle?.Dispose();
                    _decompressionHandle?.Dispose();
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }

        public override long Position
        {
            get { throw new NotSupportedException("This operation is not supported"); }
            set { throw new NotSupportedException("This operation is not supported"); }
        }
    }
}
