using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices; 

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
	
		public bool CheckType<T, TT>() where T: unmanaged where TT : unmanaged
		{
			var t = typeof(T);
			var kind = dtype.type_code switch {
				DLDataTypeCode.kDLInt => t == typeof(SByte) || t == typeof(Int16) || t == typeof(Int32) || t == typeof(Int64),
				DLDataTypeCode.kDLUInt => t == typeof(Byte) || t == typeof(UInt16) || t == typeof(UInt32) || t == typeof(UInt64),
				DLDataTypeCode.kDLFloat => t == typeof(Single) || t == typeof(Double), 
				_ => throw new Exception("Only kDLInt, KDLUint and KDLFloat is supported"), 
			};
			return kind && Unsafe.SizeOf<TT>() * 8 == dtype.bits * dtype.lanes;
		}
		
		public unsafe ReadOnlySpan<Int64> ShapeSpan()
		{
			return new ReadOnlySpan<Int64>(shape.ToPointer(), ndim); 
		}

		public unsafe Int64 Numel()
		{
			Int64 numel = 1;
			var shape = ShapeSpan();
			for(var i = 0; i < ndim; i++)
				numel *= shape[i];
			return numel;
		}
		
		public unsafe ReadOnlySpan<Int64> StridesSpan()
		{
			return new ReadOnlySpan<Int64>(strides.ToPointer(), ndim); 
		}

		public unsafe ReadOnlySpan<T> DataSpanLessThan2Gb<T>() where T : unmanaged
		{
			var bits = Numel() * dtype.bits * dtype.lanes;
			Int32 length = (Int32)(bits / (Unsafe.SizeOf<T>() * 8));
			return new ReadOnlySpan<T>(data.ToPointer(), length);
		}
		
		public unsafe T Read<T>(params Int64[] coords) where T : unmanaged
		{
			var strides = StridesSpan();
			Int64 offset = 0;
			for(var i = 0; i < ndim; i++)
				offset += strides[i] * coords[i];
			T* ptr = (T*)data.ToPointer();
			return ptr[offset];
		}
	}
	
	public delegate void DLDeleterFunc(ref DLManagedTensor self);

	[StructLayout(LayoutKind.Sequential)]
	public struct DLManagedTensor
	{
		public DLTensor dl_tensor;
		public IntPtr manager_ctx;
		public DLDeleterFunc deleter;

		public void CallDeleter()
		{
			if(deleter != null)
				deleter(ref this);
		}
	}
}
