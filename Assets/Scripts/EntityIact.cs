using System;
using System.Collections.Generic;

public interface EntityIact {
	void Interact (WorldEntity src, WorldEntity dst);
}

public struct IactDst {
	public readonly EntityIact ia;
	public readonly WUID dst;
}