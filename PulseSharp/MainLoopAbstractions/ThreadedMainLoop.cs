//
//  ThreadedMainLoop.cs
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
using System.Threading;
using AdvancedDLSupport;
using PulseSharp.Native;

namespace PulseSharp.MainLoopAbstractions
{
	/// <summary>
	/// Represents a threaded mainloop implementation in PulseAudio.
	/// </summary>
	public class ThreadedMainLoop : PulseAudioObject
	{
		private static readonly IThreadedMainLoop API;

		static ThreadedMainLoop()
		{
			API = NativeLibraryBuilder.Default.ActivateInterface<IThreadedMainLoop>("pulse");
		}

		private bool IsRunning { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ThreadedMainLoop"/> class.
		/// </summary>
		public ThreadedMainLoop()
			: base(API.New, API.Free)
		{
			Start();
		}

		/// <summary>
		/// Starts the main loop thread.
		/// </summary>
		public void Start()
		{
			if (this.IsRunning)
			{
				return;
			}

			API.Start(this.Handle.DangerousGetHandle());
			this.IsRunning = true;
		}

		/// <summary>
		/// Stops the main loop thread.
		/// </summary>
		public void Stop()
		{
			if (!this.IsRunning)
			{
				return;
			}

			API.Stop(this.Handle.DangerousGetHandle());
			this.IsRunning = false;
		}

		/// <summary>
		/// Acquires a lock on the main loop. This lock has to be disposed to unlock the loop.
		/// </summary>
		/// <returns>A locking object.</returns>
		public ThreadedMainLoopLock AcquireLock()
		{
			return new ThreadedMainLoopLock(this);
		}

		/// <summary>
		/// Wait for an event to be signalled by the event loop thread.
		/// </summary>
		public void Wait()
		{
			API.Wait(this.Handle.DangerousGetHandle());
		}

		/// <summary>
		/// Locks the main loop.
		/// </summary>
		public void Lock()
		{
			API.Lock(this.Handle.DangerousGetHandle());
		}

		/// <summary>
		/// Unlocks the main loop.
		/// </summary>
		public void Unlock()
		{
			API.Unlock(this.Handle.DangerousGetHandle());
		}

		/// <summary>
		/// Signal all threads waiting for a signalling event in <see cref="Wait"/>.
		///
		/// If waitForAccept is nonzero, does not return until the signal is accepted by an <see cref="Accept"/> call.
		/// While waiting for that condition, the event loop is unlocked.
		/// </summary>
		/// <param name="waitForAccept">Whether or not to wait for the signal to be accepted.</param>
		public void Signal(bool waitForAccept = false)
		{
			API.Signal(this.Handle.DangerousGetHandle(), waitForAccept);
		}

		/// <summary>
		/// Accept a signal from the event thread issued with <see cref="Signal"/>.
		/// </summary>
		public void Accept()
		{
			API.Accept(this.Handle.DangerousGetHandle());
		}

		/// <summary>
		/// Gets a pointer to a vtable of API functions.
		/// </summary>
		/// <returns>The pointer.</returns>
		public IntPtr GetAPI()
		{
			return API.GetAPI(this.Handle.DangerousGetHandle());
		}

		/// <inheritdoc />
		public override void Dispose()
		{
			Stop();
			base.Dispose();
		}
	}
}
