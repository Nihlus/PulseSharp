//
//  SampleFormat.cs
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

// ReSharper disable InconsistentNaming

using JetBrains.Annotations;

#pragma warning disable 1591, SA1602

namespace PulseSharp.Enums
{
	/// <summary>
	/// Sample formats.
	/// </summary>
	[PublicAPI]
	public enum SampleFormat
	{
		U8,
		ALAW,
		ULAW,
		S16LE,
		S16BE,
		Float32LE,
		Float32BE,
		S32LE,
		S32BE,
		S24LE,
		S24BE,
		S24_32LE,
		S24_32BE,
		Max,
		Invalid = -1
	}
}
