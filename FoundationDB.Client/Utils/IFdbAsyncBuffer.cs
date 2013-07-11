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
	* Neither the name of Doxense nor the
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

namespace FoundationDB.Client.Utils
{
	using System;
#if !NET_4_0
	using System.Runtime.ExceptionServices;
#endif
	using System.Threading;
	using System.Threading.Tasks;


	interface IFdbAsyncBuffer<T>
	{
		int Count { get; }
		int Capacity { get; }

		Task OnNextAsync(T value, CancellationToken ct = default(CancellationToken));
		Task OnNextBatchAsync(T[] batch, CancellationToken ct = default(CancellationToken));
		void OnError(Exception error);
#if !NET_4_0
		void OnError(ExceptionDispatchInfo error);
#endif
		void OnCompleted();
		Task DrainAsync();

		Task<Maybe<T>> ReceiveAsync(CancellationToken ct = default(CancellationToken));
		Task<Maybe<T>[]> ReceiveBatchAsync(int count, CancellationToken ct = default(CancellationToken));
	}

}