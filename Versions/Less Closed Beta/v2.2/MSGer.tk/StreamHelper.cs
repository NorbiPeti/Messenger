using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace HdSystemLibrary.IO
{
    /// <summary>
    /// A delegate for reporting binary progress
    /// </summary>
    /// <param name="bytesRead">The amount of bytes already read</param>
    /// <param name="totalBytesToRead">The amount of total bytes to read. Can be -1 if unknown.</param>
    public delegate void ProgressChange(long bytesRead, long totalBytesToRead);


    /// <summary>
    /// The arguments for StreamHelper.CopyFrom(Stream, Stream, CopyFromArguments)
    /// </summary>
    public sealed class CopyFromArguments
    {
        /// <summary>
        /// Creates the default arguments
        /// </summary>
        public CopyFromArguments()
        {
        }

        /// <summary>
        /// Creates arguments with a progress change callback.
        /// </summary>
        /// <param name="progressChangeCallback">The progress change callback (see <see cref="ProgressChangeCallback"/>)</param>
        public CopyFromArguments(ProgressChange progressChangeCallback)
        {
            ProgressChangeCallback = progressChangeCallback;
        }

        /// <summary>
        /// Creates arguments with a progress change callback and an interval between to progress changes.
        /// </summary>
        /// <param name="progressChangeCallback">The progress change callback (see <see cref="ProgressChangeCallback"/>)</param>
        /// <param name="progressChangeCallbackInterval">The interval between to progress change callbacks (see <see cref="ProgressChangeCallbackInterval"/>)</param>
        public CopyFromArguments(ProgressChange progressChangeCallback,
            TimeSpan progressChangeCallbackInterval)
        {
            ProgressChangeCallback = progressChangeCallback;
            ProgressChangeCallbackInterval = progressChangeCallbackInterval;
        }

        /// <summary>
        /// Creates arguments with a progress change callback, an interval between to progress changes and a total length
        /// </summary>
        /// <param name="progressChangeCallback">The progress change callback (see <see cref="ProgressChangeCallback"/>)</param>
        /// <param name="progressChangeCallbackInterval">The interval between to progress change callbacks (see <see cref="ProgressChangeCallbackInterval"/>)</param>
        /// <param name="totalLength">The total bytes to read (see <see cref="TotalLength"/>)</param>
        public CopyFromArguments(ProgressChange progressChangeCallback,
            TimeSpan progressChangeCallbackInterval, long totalLength)
        {
            ProgressChangeCallback = progressChangeCallback;
            ProgressChangeCallbackInterval = progressChangeCallbackInterval;
            TotalLength = totalLength;
        }

        private long totalLength = -1;
        /// <summary>
        /// Gets or sets the total length of stream. Set to -1 if the value has to be determined by stream.Length.
        /// If the stream is not seekable, the total length in the progress report will be stay -1.
        /// </summary>
        public long TotalLength { get { return totalLength; } set { totalLength = value; } }

        private int bufferSize = 4096;
        /// <summary>
        /// Gets or sets the size of the buffer used for copying bytes. Default is 4096.
        /// </summary>
        public int BufferSize { get { return bufferSize; } set { bufferSize = value; } }

        /// <summary>
        /// Gets or sets the callback for progress-report. Default is null.
        /// </summary>
        public ProgressChange ProgressChangeCallback { get; set; }

        /// <summary>
        /// Gets or sets the event for aborting the operation. Default is null.
        /// </summary>
        public WaitHandle StopEvent { get; set; }

        private TimeSpan progressCallbackInterval = TimeSpan.FromSeconds(0.2);
        /// <summary>
        /// Gets or sets the time interval between to progress change callbacks. Default is 200 ms.
        /// </summary>
        public TimeSpan ProgressChangeCallbackInterval
        {
            get { return progressCallbackInterval; }
            set { progressCallbackInterval = value; }
        }
    }

    /// <summary>
    /// A static class for basic stream operations.
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// Copies the source stream into the current while reporting the progress.
        /// The copying process is done in a separate thread, therefore the stream has to 
        /// support reading from a different thread as the one used for construction.
        /// Nethertheless, the separate thread is synchronized with the calling thread.
        /// The callback in arguments is called from the calling thread.
        /// </summary>
        /// <param name="target">The current stream</param>
        /// <param name="source">The source stream</param>
        /// <param name="arguments">The arguments for copying</param>
        /// <returns>The number of bytes actually copied.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either target, source of arguments is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if arguments.BufferSize is less than 128 or arguments.ProgressChangeCallbackInterval is less than 0</exception>
        public static long CopyFrom(this Stream target, Stream source, CopyFromArguments arguments)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (source == null)
                throw new ArgumentNullException("source");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            if (arguments.BufferSize < 128)
                throw new ArgumentOutOfRangeException("arguments.BufferSize",
                    arguments.BufferSize, "BufferSize has to be greater or equal than 128.");
            if (arguments.ProgressChangeCallbackInterval.TotalSeconds < 0)
                throw new ArgumentOutOfRangeException("arguments.ProgressChangeCallbackInterval",
                    arguments.ProgressChangeCallbackInterval,
                    "ProgressChangeCallbackInterval has to be greater or equal than 0.");

            long length = 0;

            bool runningFlag = true;

            Action<Stream, Stream, int> copyMemory = (Stream _target, Stream _source, int bufferSize) =>
                //Raw copy-operation, "length" and "runningFlag" are enclosed as closure
                {
                    int count;
                    byte[] buffer = new byte[bufferSize];

                    while ((count = _source.Read(buffer, 0, bufferSize)) != 0 && runningFlag)
                    {
                        _target.Write(buffer, 0, count);
                        long newLength = length + count;
                        //"length" can be read as this is the only thread which writes to "length"
                        Interlocked.Exchange(ref length, newLength);
                    }
                };

            IAsyncResult asyncResult = copyMemory.BeginInvoke(target, source, arguments.BufferSize, null, null);

            long totalLength = arguments.TotalLength;
            if (totalLength == -1 && source.CanSeek)
                totalLength = (long)source.Length;

            DateTime lastCallback = DateTime.Now;
            long lastLength = 0;

            while (!asyncResult.IsCompleted)
            {
                if (arguments.StopEvent != null && arguments.StopEvent.WaitOne(0))
                    runningFlag = false; //to indicate that the copy-operation has to abort

                Thread.Sleep((int)(arguments.ProgressChangeCallbackInterval.TotalMilliseconds / 10));

                if (arguments.ProgressChangeCallback != null
                    && DateTime.Now - lastCallback > arguments.ProgressChangeCallbackInterval)
                {
                    long currentLength = Interlocked.Read(ref length); //Since length is 64 bit, reading is not an atomic operation.

                    if (currentLength != lastLength)
                    {
                        lastLength = currentLength;
                        lastCallback = DateTime.Now;
                        arguments.ProgressChangeCallback(currentLength, totalLength);
                    }
                }
            }

            if (arguments.ProgressChangeCallback != null && lastLength != length)
                //to ensure that the callback is called once with maximum progress
                arguments.ProgressChangeCallback(length, totalLength);

            copyMemory.EndInvoke(asyncResult);

            return length;
        }

        /// <summary>
        /// Copies the source stream into the current
        /// </summary>
        /// <param name="stream">The current stream</param>
        /// <param name="source">The source stream</param>
        /// <param name="bufferSize">The size of buffer used for copying bytes</param>
        /// <returns>The number of bytes actually copied.</returns>
        public static long CopyFrom(this Stream stream, Stream source, int bufferSize = 4096)
        {
            int count = 0;
            byte[] buffer = new byte[bufferSize];
            long length = 0;

            while ((count = source.Read(buffer, 0, bufferSize)) != 0)
            {
                length += count;
                stream.Write(buffer, 0, count);
            }

            return length;
        }
    }

}
