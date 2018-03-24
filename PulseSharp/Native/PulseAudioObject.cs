//
//  PulseAudioObject.cs
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
using PulseSharp.Native;

namespace PulseSharp.Native
{
	/// <summary>
	/// Represents a native PulseAudio object.
	/// </summary>
	public abstract class PulseAudioObject : IDisposable
	{
		/// <summary>
		/// Gets the handle to the underlying object.
		/// </summary>
		protected SafeHandle Handle { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PulseAudioObject"/> class.
		/// </summary>
		/// <param name="createFunc">The function used to create a new object.</param>
		/// <param name="freeAction">The function used to free an allocated object.</param>
		protected PulseAudioObject(Func<IntPtr> createFunc, Action<IntPtr> freeAction)
		{
			this.Handle = new PulseAudioSafeHandle(createFunc, freeAction);
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			this.Handle?.Dispose();
		}
	}
}
