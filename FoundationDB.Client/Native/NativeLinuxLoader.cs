namespace FoundationDB.Client.Native
{
	using System;
	using System.Runtime.InteropServices;

	internal static class NativeLinuxLoader
	{
		private const int RTLD_NOW = 2;
		public static SafeLibraryHandle LoadLibrary(string fileName)
		{
			return dlopen(fileName, RTLD_NOW);
		}

		public static void FreeLibrary(IntPtr handle)
		{
			dlclose(handle);
		}

		public static IntPtr GetProcAddress(IntPtr dllHandle, string name)
		{
			// clear previous errors if any
			dlerror();
			var res = dlsym(dllHandle, name);
			var errPtr = dlerror();
			if (errPtr != IntPtr.Zero)
			{
				throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
			}
			return res;
		}

		[DllImport("libdl.so")]
		private static extern SafeLibraryHandle dlopen(string fileName, int flags);

		[DllImport("libdl.so")]
		private static extern void dlclose(IntPtr handle);

		[DllImport("libdl.so")]
		private static extern IntPtr dlsym(IntPtr dllHandle, string name);

		[DllImport("libdl.so")]
		private static extern IntPtr dlerror();
	}

}