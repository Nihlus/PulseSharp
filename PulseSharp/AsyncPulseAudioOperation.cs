//
//  AsyncPulseAudioOperation.cs
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
using PulseSharp.Interfaces;
using PulseSharp.Native;

namespace PulseSharp
{
	/// <summary>
	/// Represents an asynchronous PulseAudio operation.
	/// </summary>
	public class AsyncPulseAudioOperation : PulseAudioObject
	{
		private static readonly IPulseAudioOperation API;

		static AsyncPulseAudioOperation()
		{
			API = NativeLibraryBuilder.Default.ActivateInterface<IPulseAudioOperation>("pulse");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncPulseAudioOperation"/> class.
		/// </summary>
		/// <param name="operationPtr">A handle to the native operation.</param>
		public AsyncPulseAudioOperation(IntPtr operationPtr)
			: base(() => operationPtr, API.Unref)
		{
		}

		/// <summary>
		/// Sets the callback to call if the operation's state changes.
		/// </summary>
		/// <param name="callback">The callback.</param>
		public void SetStateChangedCallback(PulseAudioStateChanged callback)
		{
			API.SetStateCallback
			(
				this.Handle.DangerousGetHandle(),
				(operation, userData) => { callback(GetState()); },
				IntPtr.Zero
			);
		}

		/// <summary>
		/// Cancels the operation.
		/// </summary>
		public void Cancel()
		{
			API.Cancel(this.Handle.DangerousGetHandle());
		}

		/// <summary>
		/// Gets the current state of the operation.
		/// </summary>
		/// <returns>The state.</returns>
		public PulseAudioOperationState GetState()
		{
			return API.GetState(this.Handle.DangerousGetHandle());
		}
	}
}
