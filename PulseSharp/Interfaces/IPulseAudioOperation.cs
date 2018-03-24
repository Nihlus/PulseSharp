//
//  IPulseAudioOperation.cs
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
using pa_operation = System.IntPtr;

namespace PulseSharp.Interfaces
{
	/// <summary>
	/// Native API for asynchronous PulseAudio operation objects.
	/// </summary>
	public interface IPulseAudioOperation
	{
		/// <summary>
		/// Cancels the given operation.
		/// </summary>
		/// <param name="operation">The operation</param>
		[NativeSymbol("pa_operation_cancel")]
		void Cancel(pa_operation operation);

		/// <summary>
		/// Gets the current status of the operation.
		/// </summary>
		/// <param name="operation">The operation</param>
		/// <returns>The status.</returns>
		[NativeSymbol("pa_operation_get_state")]
		PulseAudioOperationState GetState(pa_operation operation);

		/// <summary>
		/// Increases the reference count by one.
		/// </summary>
		/// <param name="operation">The operation</param>
		/// <returns>The operation, with its reference count incremented.</returns>
		[NativeSymbol("pa_operation_ref")]
		pa_operation Ref(pa_operation operation);

		/// <summary>
		/// Sets the callback function that is called when the operation state changes.
		/// </summary>
		/// <param name="operation">The operation</param>
		/// <param name="callback">The callback.</param>
		/// <param name="userData">Arbitrary user data.</param>
		[NativeSymbol("pa_operation_set_state_callback")]
		void SetStateCallback(pa_operation operation, PulseAudioOperationNotify callback, IntPtr userData);

		/// <summary>
		/// Decrements the reference count by one.
		/// </summary>
		/// <param name="operation">The operation</param>
		[NativeSymbol("pa_operation_unref")]
		void Unref(pa_operation operation);
	}

	/// <summary>
	/// State change callback.
	/// </summary>
	/// <param name="operation">The operation.</param>
	/// <param name="userData">Arbitrary user data.</param>
	public delegate void PulseAudioOperationNotify(pa_operation operation, IntPtr userData);

	/// <summary>
	/// Managed state change callback.
	/// </summary>
	/// <param name="state">The current state of the operation.</param>
	public delegate void PulseAudioStateChanged(PulseAudioOperationState state);
}
