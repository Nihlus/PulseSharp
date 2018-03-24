//
//  StreamDirection.cs
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

using JetBrains.Annotations;

#pragma warning disable 1591, SA1602
namespace PulseSharp.Enums
{
	/// <summary>
	/// The direction of a pa_stream object.
	/// </summary>
	[PublicAPI]
	public enum StreamDirection
	{
		None,
		Playback,
		Record,
		Upload
	}
}
