namespace FoundationDB.Client.Native
{
	using System;
	using System.Runtime.ConstrainedExecution;
	using System.Runtime.InteropServices;
	using System.Security;

	[SuppressUnmanagedCodeSecurity]
	internal static class NativeWinLoader
	{
		const string KERNEL = "kernel32";

		[DllImport(KERNEL, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
		public static extern SafeWinLibraryHandle LoadLibrary(string fileName);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport(KERNEL, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);

		[DllImport(KERNEL, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		public static extern IntPtr GetProcAddress(SafeWinLibraryHandle hModule, String procname);
	}

}