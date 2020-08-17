using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using DLPack;

namespace DLPackTest
{
	class Test
	{
		[DllImport("dlpack")]
		public static extern DLManagedTensor create_and_allocate_dlpack_tensor();
		
		public static unsafe void Main(string[] args)
		{
			var dl_managed_tensor = create_and_allocate_dlpack_tensor();
			
			var shape = dl_managed_tensor.dl_tensor.ShapeSpan();
			Console.WriteLine("ndim {0}", dl_managed_tensor.dl_tensor.ndim);
			Console.WriteLine("shape {0} {1}", shape[0], shape[1]);
			Debug.Assert(dl_managed_tensor.dl_tensor.CheckType<Int32, Int32>());
			for(var r = 0; r < shape[0]; r++)
				for(var c = 0; c < shape[1]; c++)
					Console.WriteLine("({0}, {1}) = {2}", r, c, dl_managed_tensor.dl_tensor.Read<Int32>(r, c));
			
			var data = dl_managed_tensor.dl_tensor.DataSpanLessThan2Gb<byte>();
			using(var writer = new BinaryWriter(new FileStream(args[1], FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)))
				writer.Write(data);

			dl_managed_tensor.CallDeleter();
		}
	}
}
