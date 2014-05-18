﻿namespace FoundationDB.Client.Native
{
	using System.Security;

	using Microsoft.Win32.SafeHandles;

	// See http://msdn.microsoft.com/msdnmag/issues/05/10/Reliability/ for more about safe handles.
	[SuppressUnmanagedCodeSecurity]
	public sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeLibraryHandle() : base(true) { }

		protected override bool ReleaseHandle()
		{
			return NativeWinLoader.FreeLibrary(handle);
		}
	}

}