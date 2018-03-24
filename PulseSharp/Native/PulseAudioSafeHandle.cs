//
//  PulseAudioSafeHandle.cs
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
using Microsoft.Win32.SafeHandles;

namespace PulseSharp.Native
{
	/// <summary>
	/// Represents a safe handle to a PulseAudio object.
	/// </summary>
	public class PulseAudioSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private readonly Action<IntPtr> FreeAction;

		/// <summary>
		/// Initializes a new instance of the <see cref="PulseAudioSafeHandle"/> class.
		/// </summary>
		/// <param name="createFunc">A function which creates a handle to an object.</param>
		/// <param name="freeAction">An action which frees a handle to an object.</param>
		public PulseAudioSafeHandle(Func<IntPtr> createFunc, Action<IntPtr> freeAction)
			: base(true)
		{
			this.FreeAction = freeAction;
			SetHandle(createFunc());
		}

		/// <inheritdoc />
		protected override bool ReleaseHandle()
		{
			this.FreeAction(this.handle);
			return true;
		}
	}
}
