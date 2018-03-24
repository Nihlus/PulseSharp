//
//  MinimalMainLoop.cs
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
	/// Represents a minimal mainloop implementation in PulseAudio.
	/// </summary>
	public class MinimalMainLoop : PulseAudioObject
	{
		private static readonly IMinimalMainLoopAPI API;

		static MinimalMainLoop()
		{
			API = NativeLibraryBuilder.Default.ActivateInterface<IMinimalMainLoopAPI>("pulse");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MinimalMainLoop"/> class.
		/// </summary>
		public MinimalMainLoop()
			: base(API.New, API.Free)
		{
			API.Run(this.Handle.DangerousGetHandle(), out _);
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
			API.Quit(this.Handle.DangerousGetHandle(), 0);
			base.Dispose();
		}
	}
}
