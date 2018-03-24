//
//  SimplePulseStream.cs
//
//  Copyright (c) 2018 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using AdvancedDLSupport;
using JetBrains.Annotations;
using PulseSharp.Enums;
using PulseSharp.Interfaces;
using PulseSharp.Structures;

namespace PulseSharp.Simple
{
	/// <summary>
	/// A simple connection to the PulseAudio server, equivalent to pa_simple.
	/// </summary>
	[PublicAPI]
	public class SimplePulseStream : Stream
	{
		private const string LibraryName = "pulse-simple";
		private static readonly ISimplePulse SimplePulse;
		private static readonly IPulseError PulseError;

		static SimplePulseStream()
		{
			SimplePulse = NativeLibraryBuilder.Default.ActivateInterface<ISimplePulse>(LibraryName);
			PulseError = NativeLibraryBuilder.Default.ActivateInterface<IPulseError>(LibraryName);
		}

		private readonly StreamDirection Direction;
		private readonly IntPtr Connection;

		/// <summary>
		/// Gets the latency of the connection.
		/// </summary>
		[PublicAPI]
		public ulong Latency
		{
			get
			{
				var latency = SimplePulse.GetLatency(this.Connection, out int error);

				if (error > 0)
				{
					throw new InvalidOperationException(PulseError.GetErrorString(error));
				}

				return latency;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SimplePulseStream"/> class.
		/// </summary>
		/// <param name="connectionName">The name of the connection.</param>
		/// <param name="streamName">The name of the stream.</param>
		/// <param name="direction">The direction of the stream.</param>
		/// <param name="sampleSpec">The sample format.</param>
		/// <param name="serverName">The name of the server to connect to.</param>
		/// <param name="deviceName">The name of the device to use.</param>
		/// <param name="channelMap">The channel map to use.</param>
		/// <param name="bufferAttributes">The buffering attributes.</param>
		/// <exception cref="InvalidOperationException">Thrown if a connection could not be established.</exception>
		[PublicAPI]
		public SimplePulseStream
		(
			string connectionName,
			string streamName,
			StreamDirection direction,
			SampleSpecification sampleSpec,
			string serverName = null,
			string deviceName = null,
			ChannelMap? channelMap = null,
			BufferAttributes? bufferAttributes = null
		)
		{
			this.Connection = SimplePulse.New
			(
				serverName,
				connectionName,
				direction,
				deviceName,
				streamName,
				ref sampleSpec,
				ref channelMap,
				ref bufferAttributes,
				out int error
			);

			if (this.Connection == IntPtr.Zero)
			{
				throw new InvalidOperationException
				(
					$"Failed to initialize the connection to the PulseAudio server: {PulseError.GetErrorString(error)}"
				);
			}

			this.Direction = direction;
		}

		/// <summary>
		/// Writes some data to the server.
		/// </summary>
		/// <param name="data">The data to write.</param>
		[PublicAPI]
		public void Write(byte[] data)
		{
			unsafe
			{
				fixed (void* ptr = data)
				{
					var numBytes = new UIntPtr((uint)data.Length);

					var success = SimplePulse.Write(this.Connection, ptr, numBytes, out int error) > -1;
					if (!success)
					{
						throw new InvalidOperationException(PulseError.GetErrorString(error));
					}
				}
			}
		}

		/// <summary>
		/// Wait until all data already written is played by the daemon.
		/// </summary>
		[PublicAPI]
		public void Drain()
		{
			var success = SimplePulse.Drain(this.Connection, out int error) > -1;
			if (!success)
			{
				throw new InvalidOperationException(PulseError.GetErrorString(error));
			}
		}

		/// <summary>
		/// Flush the playback or audio buffer.
		///
		/// This discards any audio in the buffer.
		/// </summary>
		[PublicAPI]
		public override void Flush()
		{
			var success = SimplePulse.Flush(this.Connection, out int error) > -1;
			if (!success)
			{
				throw new InvalidOperationException(PulseError.GetErrorString(error));
			}
		}

		/// <inheritdoc />
		[PublicAPI]
		public override int Read(byte[] buffer, int offset, int count)
		{
			count = count == 0
				? count
				: buffer.Length;

			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			int bytesRead;
			unsafe
			{
				fixed (byte* basePtr = buffer)
				{
					var targetPtr = basePtr + offset;

					var numBytes = new UIntPtr((uint)count);

					bytesRead = SimplePulse.Read(this.Connection, targetPtr, numBytes, out var error);
					var success = bytesRead > -1;
					if (!success)
					{
						throw new InvalidOperationException(PulseError.GetErrorString(error));
					}
				}
			}

			return bytesRead;
		}

		/// <inheritdoc />
		[PublicAPI]
		public override void Write(byte[] buffer, int offset, int count)
		{
			count = count == 0
				? count
				: buffer.Length;

			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			unsafe
			{
				fixed (byte* basePtr = buffer)
				{
					var targetPtr = basePtr + offset;

					var numBytes = new UIntPtr((uint)count);
					var success = SimplePulse.Write(this.Connection, targetPtr, numBytes, out var error) > -1;
					if (!success)
					{
						throw new InvalidOperationException(PulseError.GetErrorString(error));
					}
				}
			}
		}

		/// <inheritdoc />
		[PublicAPI]
		public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

		/// <inheritdoc />
		[PublicAPI]
		public override void SetLength(long value) => throw new NotSupportedException();

		/// <inheritdoc />
		[PublicAPI]
		public override bool CanRead => this.Direction == StreamDirection.Record;

		/// <inheritdoc />
		[PublicAPI]
		public override bool CanWrite =>
			this.Direction == StreamDirection.Playback ||
			this.Direction == StreamDirection.Upload;

		/// <inheritdoc />
		[PublicAPI]
		public override bool CanSeek => false;

		/// <inheritdoc />
		[PublicAPI]
		public override long Length => throw new NotSupportedException();

		/// <inheritdoc />
		[PublicAPI]
		public override long Position
		{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}

		/// <inheritdoc />
		protected override void Dispose(bool disposing)
		{
			if (this.Connection != IntPtr.Zero)
			{
				SimplePulse.Free(this.Connection);
			}

			base.Dispose(disposing);
		}
	}
}
