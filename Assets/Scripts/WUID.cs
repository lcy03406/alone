using System;

using T = System.UInt64;

[Serializable]
public struct WUID
{
	public readonly T value;

	public WUID(ulong value)
	{
		this.value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
			return false;
		if (!(obj is WUID))
			return false;
		WUID b = (WUID) obj;
		return this.value == b.value;
	}
	public bool Equals(WUID b)
	{
		if ((object)b == null)
			return false;
		return this.value == b.value;
	}

	public override int GetHashCode()
	{
		return this.value.GetHashCode();
	}
	
	public override string ToString()
	{
		return this.value.ToString();
	}

	public static explicit operator WUID(ulong v)
	{
		return new WUID(v);
	}

	public static bool operator ==(WUID a, WUID b)
	{
		return a.value == b.value;
	}
	
	public static bool operator !=(WUID a, WUID b)
	{
		return a.value != b.value;
	}
}

