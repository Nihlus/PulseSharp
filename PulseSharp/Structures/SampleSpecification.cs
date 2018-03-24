//
//  SampleSpecification.cs
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

using PulseSharp.Enums;

namespace PulseSharp.Structures
{
	/// <summary>
	/// A sample format and attribute specification.
	/// </summary>
	public struct SampleSpecification
	{
		/// <summary>
		/// Gets the sample format.
		/// </summary>
		public SampleFormat Format;

		/// <summary>
		/// Gets the sample rate.
		/// </summary>
		public uint Rate;

		/// <summary>
		/// Gets the number of audio channels.
		/// </summary>
		public byte ChannelCount;
	}
}
