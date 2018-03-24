//
//  IPulseContext.cs
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
using PulseSharp.Enums;
using PulseSharp.Structures;
using pa_context = System.IntPtr;
using pa_operation = System.IntPtr;

namespace PulseSharp.Interfaces
{
	/// <summary>
	/// Native API for the PulseAudio context functions.
	/// </summary>
	public interface IPulseContext
	{
		/// <summary>
		/// Creates a new PulseAudio onctext with the given name.
		/// </summary>
		/// <param name="mainloopApi">A pointer to an abstract mainloop implementation.</param>
		/// <param name="applicationName">A descriptive application name.</param>
		/// <returns>A new context.</returns>
		[NativeSymbol("pa_context_new")]
		pa_context New(IntPtr mainloopApi, string applicationName);

		/// <summary>
		/// Connect to the PulseAudio daemon.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="server">The name of the server to connect to, or null.</param>
		/// <param name="flags">Optional flags.</param>
		/// <param name="spawnApi">Optional process spawning API.</param>
		/// <returns>Unknown.</returns>
		[NativeSymbol("pa_context_connect")]
		int Connect(pa_context context, string server, PulseContextFlags flags, IntPtr spawnApi);

		/// <summary>
		/// Immediately terminate the connection to the server.
		/// </summary>
		/// <param name="context">The context.</param>
		[NativeSymbol("pa_context_disconnect")]
		void Disconnect(pa_context context);

		/// <summary>
		/// Decrements the reference count on the given context.
		/// </summary>
		/// <param name="context">The context.</param>
		[NativeSymbol("pa_context_unref")]
		void Unref(pa_context context);

		/// <summary>
		/// Increments the reference count on the given context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>The context, with the reference count incremented.</returns>
		[NativeSymbol("pa_context_new")]
		pa_context Ref(pa_context context);

		/// <summary>
		/// Gets some information about the server.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="callback">The result callback.</param>
		/// <param name="userData">Some arbitrary data.</param>
		/// <returns>An asynchronous operation handle.</returns>
		[NativeSymbol("pa_context_get_server_info")]
		pa_operation GetServerInfo(pa_context context, PulseAudioServerInfoCallback callback, IntPtr userData);

		/// <summary>
		/// Sets the callback function that is called whenever the context status changes.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="userData">Arbitrary user data.</param>
		[NativeSymbol("pa_context_set_state_callback")]
		void SetStateCallback(pa_context context, PulseAudioContextNotifyCallback callback, IntPtr userData);

		/// <summary>
		/// Gets the current state of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>The state.s</returns>
		[NativeSymbol("pa_context_get_state")]
		PulseContextState GetState(pa_context context);
	}

	/// <summary>
	/// Generic notification callback prototype.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="userData">Arbitrary user data.</param>
	public delegate void PulseAudioContextNotifyCallback(pa_context context, IntPtr userData);

	/// <summary>
	/// Callback prototype for <see cref="IPulseContext.GetServerInfo"/>.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="info">The retrieved <see cref="ServerInfo"/>.</param>
	/// <param name="userData">Some arbitrary data.</param>
	public delegate void PulseAudioServerInfoCallback(pa_context context, in ServerInfo info, IntPtr userData);
}
