//
//  IPulseError.cs
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

using AdvancedDLSupport;

namespace PulseSharp.Interfaces
{
	/// <summary>
	/// Error management.
	/// </summary>
	public interface IPulseError
	{
		/// <summary>
		/// Return a human readable error message for the specified numeric error code.
		/// </summary>
		/// <param name="error">The error code.</param>
		/// <returns>A human readable error message.</returns>
		[NativeSymbol("pa_strerror")]
		string GetErrorString(int error);
	}
}
