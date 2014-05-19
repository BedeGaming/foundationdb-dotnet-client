namespace FoundationDB.Client.Native
{
	using System;
	using System.Runtime.InteropServices;

	internal static class NativeLinuxLoader
	{
		/// <summary>
		/// All addresses in the loaded library are not able to be relocated
		/// after first load.
		/// </summary>
		/// <remarks>
		/// From: http://pubs.opengroup.org/onlinepubs/009695399/functions/dlopen.html
		/// All necessary relocations shall be performed when the object is first loaded.
		/// This may waste some processing if relocations are performed for functions that are
		/// never referenced. This behavior may be useful for applications that need to know
		/// as soon as an object is loaded that all symbols referenced during execution are available.
		/// </remarks>
		private const int RTLD_NOW = 2;

		public static SafeLinuxLibraryHandle LoadLibrary(string fileName)
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
		private static extern SafeLinuxLibraryHandle dlopen(string fileName, int flags);

		[DllImport("libdl.so")]
		private static extern void dlclose(IntPtr handle);

		[DllImport("libdl.so")]
		private static extern IntPtr dlsym(IntPtr dllHandle, string name);

		[DllImport("libdl.so")]
		private static extern IntPtr dlerror();
	}

}