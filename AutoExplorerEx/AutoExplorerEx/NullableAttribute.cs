using System;
using System.Runtime.CompilerServices;

namespace Elin_AutoExplore
{

	[CompilerGenerated]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		public readonly byte[] NullableFlags;

		public NullableAttribute(byte A_1) {
			NullableFlags = new byte[1] { A_1 };
		}

		public NullableAttribute(byte[] A_1) {
			NullableFlags = A_1;
		}
	}
}
