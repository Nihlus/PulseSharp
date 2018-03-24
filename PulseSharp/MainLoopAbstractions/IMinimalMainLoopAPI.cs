//
//  IMinimalMainLoopAPI.cs
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

namespace PulseSharp.MainLoopAbstractions
{
	/// <summary>
	/// Represents a mainloop creation API from pulseaudio.
	/// </summary>
	public interface IMinimalMainLoopAPI : IDisposable
	{
		/// <summary>
		/// Creates a new mainloop object.
		/// </summary>
		/// <returns>A handle to the object.</returns>
		[NativeSymbol("pa_mainloop_new")]
		IntPtr New();

		/// <summary>
		/// Gets a pointer to a vtable of API functions in the given mainloop object.
		/// </summary>
		/// <param name="handle">A handle to a mainloop object.</param>
		/// <returns>The vtable pointer.</returns>
		[NativeSymbol("pa_mainloop_get_api")]
		IntPtr GetAPI(IntPtr handle);

		/// <summary>
		/// Frees the underlying simple main loop object.
		/// </summary>
		/// <param name="handle">A handle to a mainloop object.</param>
		[NativeSymbol("pa_mainloop_free")]
		void Free(IntPtr handle);

		/// <summary>
		/// Run an unlimited number of iterations until <see cref="Quit"/> is called.
		/// </summary>
		/// <param name="handle">The mainloop object.</param>
		/// <param name="retVal">The return value.</param>
		/// <returns>Unclear.</returns>
		[NativeSymbol("pa_mainloop_run")]
		int Run(IntPtr handle, out int retVal);

		/// <summary>
		/// Quits the main loop iteration.
		/// </summary>
		/// <param name="handle">The mainloop object.</param>
		/// <param name="retVal">The return value.</param>
		[NativeSymbol("pa_mainloop_quit")]
		void Quit(IntPtr handle, int retVal);
	}
}
