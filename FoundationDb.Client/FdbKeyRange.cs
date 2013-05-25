﻿#region BSD Licence
/* Copyright (c) 2013, Doxense SARL
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
	* Redistributions of source code must retain the above copyright
	  notice, this list of conditions and the following disclaimer.
	* Redistributions in binary form must reproduce the above copyright
	  notice, this list of conditions and the following disclaimer in the
	  documentation and/or other materials provided with the distribution.
	* Neither the name of the <organization> nor the
	  names of its contributors may be used to endorse or promote products
	  derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion

using System;
using System.Diagnostics;

namespace FoundationDb.Client
{

	[DebuggerDisplay("Begin={this.Begin}, End={this.end}")]
	public struct FdbKeyRange
	{
		public readonly Slice Begin;
		public readonly Slice End;

		public FdbKeyRange(Slice begin, Slice end)
		{
			this.Begin = begin;
			this.End = end;
		}

		/// <summary>Convert a prefix key into a range "key\x00".."key\xFF"</summary>
		/// <param name="prefix">Key prefix</param>
		/// <returns>Range including all keys with the specified prefix</returns>
		public static FdbKeyRange FromPrefix(Slice prefix)
		{
			int n = prefix.Count + 1;

			var tmp = new byte[n + 2];
			int p = 0;
			// first segment will contain prefix + '\x00'
			prefix.CopyTo(tmp, 0);
			tmp[n - 1] = 0;

			// second segment will contain prefix + '\xFF'
			prefix.CopyTo(tmp, n);
			tmp[(n << 1) - 1] = 0xFF;

			return new FdbKeyRange(
				new Slice(tmp, 0, n),
				new Slice(tmp, n, n)
			);
		}

		public FdbKeySelector BeginIncluded
		{
			get { return FdbKeySelector.FirstGreaterOrEqual(this.Begin); }
		}

		public FdbKeySelector BeginExcluded
		{
			get { return FdbKeySelector.FirstGreaterThan(this.Begin); }
		}

		public FdbKeySelector EndIncluded
		{
			get { return FdbKeySelector.FirstGreaterThan(this.End); }
		}

		public FdbKeySelector EndExcluded
		{
			get { return FdbKeySelector.FirstGreaterOrEqual(this.End); }
		}

		/// <summary>Returns true, if the key is contained in the range</summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(Slice key)
		{
			return key.CompareTo(this.Begin) >= 0 && key.CompareTo(this.End) <= 0;
		}

		public override string ToString()
		{
			return "{\"" + this.Begin.ToString() + "\", \"" + this.End.ToString() + "}";
		}
	}

}