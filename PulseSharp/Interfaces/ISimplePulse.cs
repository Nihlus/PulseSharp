//
//  ISimplePulse.cs
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
using System.Runtime.InteropServices;
using AdvancedDLSupport;
using JetBrains.Annotations;
using PulseSharp.Enums;
using PulseSharp.Structures;

namespace PulseSharp.Interfaces
{
	/// <summary>
	/// Native interface for the simple PulseAudio API.
	/// </summary>
	public interface ISimplePulse
	{
		/// <summary>
		/// Creates a new connection to the server.
		/// </summary>
		/// <param name="server">The server name, or null for default.</param>
		/// <param name="name">A descriptive client name</param>
		/// <param name="direction">The direction of the stream (recording or playback).</param>
		/// <param name="device">The sink or source name, or null for default.</param>
		/// <param name="streamName">A descriptive name for the stream.</param>
		/// <param name="spec">The sample type to use.</param>
		/// <param name="channelMap">The channel map to use, or null for default.</param>
		/// <param name="bufferAttributes">The buffering attributes, or null for default.</param>
		/// <param name="error">The error, if any.</param>
		/// <returns>An opaque connection object.</returns>
		[NativeSymbol("pa_simple_new")]
		IntPtr New
		(
			[CanBeNull] string server,
			[NotNull] string name,
			StreamDirection direction,
			[CanBeNull] string device,
			[NotNull] string streamName,
			ref SampleSpecification spec,
			[CanBeNull] ref ChannelMap? channelMap,
			[CanBeNull] ref BufferAttributes? bufferAttributes,
			out int error
		);

		/// <summary>
		/// Close and free the connection to the server.
		///
		/// The connection object becomes invalid when this is called.
		/// </summary>
		/// <param name="connection">The opaque connection object.</param>
		[NativeSymbol("pa_simple_free")]
		void Free(IntPtr connection);

		/// <summary>
		/// Writes some data to the server.
		/// </summary>
		/// <param name="connection">The connection to the server.</param>
		/// <param name="data">A pointer to the data.</param>
		/// <param name="size">The size in bytes of the data.</param>
		/// <param name="error">The error, if any.</param>
		/// <returns>positive on success; otherwise, negative.</returns>
		[NativeSymbol("pa_simple_write")]
		unsafe int Write(IntPtr connection, void* data, UIntPtr size, out int error);

		/// <summary>
		/// Wait until all data already written is played by the daemon.
		/// </summary>
		/// <param name="connection">The connection to the server.</param>
		/// <param name="error">The error, if any.</param>
		/// <returns>positive on success; otherwise, negative.</returns>
		[NativeSymbol("pa_simple_drain")]
		int Drain(IntPtr connection, out int error);

		/// <summary>
		/// Reads some data from the server. This function blocks until <paramref name="bytes"/> amount of data has been
		/// received from the server, or until an error occurs.
		/// </summary>
		/// <param name="connection">The connection to the server.</param>
		/// <param name="buffer">A pointer to an allocated buffer.</param>
		/// <param name="bytes">The number of bytes to read.</param>
		/// <param name="error">The error, if any.</param>
		/// <returns>positive on success; otherwise, negative.</returns>
		[NativeSymbol("pa_simple_read")]
		unsafe int Read(IntPtr connection, void* buffer, UIntPtr bytes, out int error);

		/// <summary>
		/// Gets the playback or record latency.
		/// </summary>
		/// <param name="connection">The connection to the server.</param>
		/// <param name="error">The error, if any.</param>
		/// <returns>The latency, in µs.</returns>
		[NativeSymbol("pa_simple_get_latency")]
		ulong GetLatency(IntPtr connection, out int error);

		/// <summary>
		/// Flush the playback or audio buffer.
		///
		/// This discards any audio in the buffer.
		/// </summary>
		/// <param name="connection">The connection to the server.</param>
		/// <param name="error">The error, if any.</param>
		/// <returns>positive on success; otherwise, negative.</returns>
		[NativeSymbol("pa_simple_flush")]
		int Flush(IntPtr connection, out int error);
	}
}
