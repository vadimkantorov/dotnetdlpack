using System;
using System.Runtime.InteropServices;
using DLPack;

class Test
{
	[DllImport("dlpack")]
	public struct DLManagedTensor create_and_allocate_dlpack_tensor();
	
	public void Main(string[] args)
	{
		var dl_managed_tensor = create_and_allocate_dlpack_tensor();
	}
}
