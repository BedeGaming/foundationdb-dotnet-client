namespace FoundationDB.Client.Native
{
	using System.Security;

	using Microsoft.Win32.SafeHandles;

	// See http://msdn.microsoft.com/msdnmag/issues/05/10/Reliability/ for more about safe handles.
	[SuppressUnmanagedCodeSecurity]
	public sealed class SafeLinuxLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeLinuxLibraryHandle() : base(true) { }

		protected override bool ReleaseHandle()
		{
			NativeLinuxLoader.FreeLibrary(handle);
			return true;
		}
	}
}