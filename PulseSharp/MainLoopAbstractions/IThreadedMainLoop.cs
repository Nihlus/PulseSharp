//
//  IThreadedMainLoop.cs
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
using pa_mainloop_api = System.IntPtr;
using pa_threaded_mainloop = System.IntPtr;

namespace PulseSharp.MainLoopAbstractions
{
	/// <summary>
	/// Threaded mainloop native API.
	/// </summary>
	public interface IThreadedMainLoop
	{
		/// <summary>
		/// Allocate a new threaded main loop object.
		/// </summary>
		/// <returns>The loop.</returns>
		[NativeSymbol("pa_threaded_mainloop_new")]
		pa_threaded_mainloop New();

		/// <summary>
		/// Free a threaded main loop object.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		[NativeSymbol("pa_threaded_mainloop_free")]
		void Free(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Start the event loop thread.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		/// <returns>true if the loop could not be started; otherwise, false.</returns>
		[NativeSymbol("pa_threaded_mainloop_start")]
		bool Start(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Terminate the event loop cleanly.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		[NativeSymbol("pa_threaded_mainloop_stop")]
		void Stop(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Lock the event loop object, effectively blocking the event loop thread from processing events.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		[NativeSymbol("pa_threaded_mainloop_lock")]
		void Lock(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Unlock the event loop object, inverse of <see cref="Lock"/>
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		[NativeSymbol("pa_threaded_mainloop_unlock")]
		void Unlock(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Wait for an event to be signalled by the event loop thread.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		[NativeSymbol("pa_threaded_mainloop_wait")]
		void Wait(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Signal all threads waiting for a signalling event in <see cref="Wait"/>.
		///
		/// If waitForAccept is nonzero, does not return until the signal is accepted by a <see cref="Accept"/> call.
		/// While waiting for that condition, the event loop is unlocked.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		/// <param name="waitForAccept">Whether or not to wait for the signal to be accepted.</param>
		[NativeSymbol("pa_threaded_mainloop_signal")]
		void Signal(pa_threaded_mainloop mainLoop, bool waitForAccept);

		/// <summary>
		/// Accept a signal from the event thread issued with <see cref="Signal"/>.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		[NativeSymbol("pa_threaded_mainloop_accept")]
		void Accept(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Return the return value as specified with the main loop's <see cref="IMinimalMainLoopAPI.Quit"/> routine.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		/// <returns>The return value.</returns>
		[NativeSymbol("pa_threaded_mainloop_get_retval")]
		int GetRetVal(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Return the main loop abstraction layer vtable for this main loop.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		/// <returns>A vtable pointer.</returns>
		[NativeSymbol("pa_threaded_mainloop_get_api")]
		pa_mainloop_api GetAPI(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Returns nonzero when called from within the event loop thread.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		/// <returns>true if inside the event loop thread; otherwise, false.</returns>
		[NativeSymbol("pa_threaded_mainloop_in_thread")]
		bool InThread(pa_threaded_mainloop mainLoop);

		/// <summary>
		/// Sets the name of the thread.
		/// </summary>
		/// <param name="mainLoop">The loop.</param>
		/// <param name="name">The name</param>
		[NativeSymbol("pa_threaded_mainloop_set_name")]
		void SetName(pa_threaded_mainloop mainLoop, string name);
	}
}
