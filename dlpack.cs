using System;
using System.Runtime.InteropServices;

namespace DLPack
{
	public enum DLDeviceType : Int32
	{
		kDLCPU = 1,
		kDLGPU = 2,
		kDLCPUPinned = 3,
		kDLOpenCL = 4,
		kDLVulkan = 7,
		kDLMetal = 8,
		kDLVPI = 9,
		kDLROCM = 10,
		kDLExtDev = 12,
	}

	public enum DLDataTypeCode : Byte
	{
		kDLInt = 0,
		kDLUInt = 1,
		kDLFloat = 2,
		kDLBfloat = 4,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DLDataType
	{
		public DLDataTypeCode type_code;
		public Byte bits;
		public UInt16 lanes;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DLContext
	{
		public DLDeviceType device_type;
		public Int32 device_id;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DLTensor
	{
		public IntPtr data;
		public DLContext ctx;
		public Int32 ndim;
		public DLDataType dtype;
		public IntPtr shape;
		public IntPtr strides;
		public UInt64 byte_offset;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DLManagedTensor
	{
		public DLTensor dl_tensor;
		public IntPtr manager_ctx;
		public DLDeleterFunc deleter;
	}
	
	public delegate void DLDeleterFunc(IntPtr self);
}
