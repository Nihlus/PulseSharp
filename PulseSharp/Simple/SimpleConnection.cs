//
//  SimpleConnection.cs
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
	public class SimpleConnection : IDisposable
	{
		private const string LibraryName = "pulse-simple";
		private static readonly ISimplePulse SimplePulse;
		private static readonly IPulseError PulseError;

		static SimpleConnection()
		{
			SimplePulse = NativeLibraryBuilder.Default.ActivateInterface<ISimplePulse>(LibraryName);
			PulseError = NativeLibraryBuilder.Default.ActivateInterface<IPulseError>(LibraryName);
		}

		private readonly IntPtr Connection;

		/// <summary>
		/// Gets the latency of the connection.
		/// </summary>
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
		/// Initializes a new instance of the <see cref="SimpleConnection"/> class.
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
		public SimpleConnection
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
		}

		/// <summary>
		/// Writes some data to the server.
		/// </summary>
		/// <param name="data">The data to write.</param>
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
		/// Reads some data from the server. This function blocks until <paramref name="bytes"/> amount of data has been
		/// received from the server, or until an error occurs.
		/// </summary>
		/// <param name="bytes">The number of bytes to read.</param>
		/// <returns>An array containing the data.</returns>
		public byte[] Read(int bytes)
		{
			var buffer = new byte[bytes];
			Read(buffer);

			return buffer;
		}

		/// <summary>
		/// Reads some data from the server. This function blocks until <paramref name="bytes"/> amount of data has been
		/// received from the server, or until an error occurs.
		/// </summary>
		/// <param name="buffer">The buffer to read the data into.</param>
		/// <param name="bytes">The number of bytes to read.</param>
		public void Read([NotNull] byte[] buffer, int? bytes = null)
		{
			bytes = bytes ?? buffer.Length;

			if (bytes > buffer.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(bytes));
			}

			unsafe
			{
				fixed (void* ptr = buffer)
				{
					var numBytes = new UIntPtr((uint)bytes.Value);

					var success = SimplePulse.Read(this.Connection, ptr, numBytes, out int error) > -1;
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
		public void Flush()
		{
			var success = SimplePulse.Flush(this.Connection, out int error) > -1;
			if (!success)
			{
				throw new InvalidOperationException(PulseError.GetErrorString(error));
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if (this.Connection != IntPtr.Zero)
			{
				SimplePulse.Free(this.Connection);
			}
		}
	}
}
