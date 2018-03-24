//
//  BufferAttributes.cs
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

namespace PulseSharp.Structures
{
	/// <summary>
	/// Playback and record buffer metrics.
	/// </summary>
	public struct BufferAttributes
	{
		/// <summary>
		/// Maximum length of the buffer in bytes.
		///
		/// Setting this to (uint32_t) -1 will initialize this to the maximum value supported by server, which is
		/// recommended.
		///
		/// In strict low-latency playback scenarios you might want to set this to a lower value, likely together with
		/// the PA_STREAM_ADJUST_LATENCY flag. If you do so, you ensure that the latency doesn't grow beyond what is
		/// acceptable for the use case, at the cost of getting more underruns if the latency is lower than what the
		/// server can reliably handle.
		/// </summary>
		public uint MaxLength;

		/// <summary>
		/// Playback only.
		///
		/// The server tries to assure that at least tlength bytes are always available in the per-stream server-side
		/// playback buffer. It is recommended to set this to (uint32_t) -1, which will initialize this to a value that
		/// is deemed sensible by the server. However, this value will default to something like 2s, i.e. for
		/// applications that have specific latency requirements this value should be set to the maximum latency that
		/// the application can deal with. When PA_STREAM_ADJUST_LATENCY is not set this value will influence only the
		/// per-stream playback buffer size. When PA_STREAM_ADJUST_LATENCY is set the overall latency of the sink plus
		/// the playback buffer size is configured to this value. Set PA_STREAM_ADJUST_LATENCY if you are interested in
		/// adjusting the overall latency. Don't set it if you are interested in configuring the server-side per-stream
		/// playback buffer size.
		/// </summary>
		public uint TargetLength;

		/// <summary>
		/// Playback only.
		///
		/// The server does not start with playback before at least prebuf bytes are available in the buffer. It is
		/// recommended to set this to (uint32_t) -1, which will initialize this to the same value as tlength,
		/// whatever that may be. Initialize to 0 to enable manual start/stop control of the stream. This means that
		/// playback will not stop on underrun and playback will not start automatically. Instead pa_stream_cork() needs
		/// to be called explicitly. If you set this value to 0 you should also set PA_STREAM_START_CORKED.
		/// </summary>
		public uint PreBuffering;

		/// <summary>
		/// Playback only.
		///
		/// The server does not request less than minreq bytes from the client, instead waits until the buffer is free
		/// enough to request more bytes at once. It is recommended to set this to (uint32_t) -1, which will initialize
		/// this to a value that is deemed sensible by the server. This should be set to a value that gives PulseAudio
		/// enough time to move the data from the per-stream playback buffer into the hardware playback buffer.
		/// </summary>
		public uint MinimumRequest;

		/// <summary>
		/// Recording only.
		///
		/// The server sends data in blocks of fragsize bytes size. Large values diminish interactivity with other
		/// operations on the connection context but decrease control overhead. It is recommended to set this to
		/// (uint32_t) -1, which will initialize this to a value that is deemed sensible by the server. However, this
		/// value will default to something like 2s, i.e. for applications that have specific latency requirements this
		/// value should be set to the maximum latency that the application can deal with. If PA_STREAM_ADJUST_LATENCY
		/// is set the overall source latency will be adjusted according to this value. If it is not set the source
		/// latency is left unmodified.
		/// </summary>
		public uint FragmentSize;
	}
}
