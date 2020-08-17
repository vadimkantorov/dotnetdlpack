using System;
using System.Runtime.InteropServices;
using DLPack;

namespace DLPackTest
{
	class Test
	{
		[DllImport("dlpack")]
		public static extern DLManagedTensor create_and_allocate_dlpack_tensor();
		
		public static void Main(string[] args)
		{
			var dl_managed_tensor = create_and_allocate_dlpack_tensor();
			Console.WriteLine("ndom", dl_managed_tensor.dl_tensor.ndim);
			Console.WriteLine("shape", dl_managed_tensor.dl_tensor.shape[0], dl_managed_tensor.dl_tensor.shape[1]);
		}
	}
}
