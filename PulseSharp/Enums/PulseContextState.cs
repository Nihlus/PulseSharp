//
//  PulseContextState.cs
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

namespace PulseSharp.Enums
{
	/// <summary>
	/// The state of a connection context.
	/// </summary>
	public enum PulseContextState
	{
		/// <summary>
		/// The context hasn't been connected yet.
		/// </summary>
		Disconnected,

		/// <summary>
		/// A connection is being established.
		/// </summary>
		Connecting,

		/// <summary>
		/// The client is authorizing to the daemon.
		/// </summary>
		Authorizing,

		/// <summary>
		/// The client is passing the application name to the daemon.
		/// </summary>
		SettingName,

		/// <summary>
		/// The connection is established, and the context is ready to execute operations.
		/// </summary>
		Ready,

		/// <summary>
		/// The connection failed or was disconnected.
		/// </summary>
		Failed,

		/// <summary>
		/// The connection was terminated cleanly.
		/// </summary>
		Terminated
	}
}
