using System;
using System.Collections.Generic;

public interface EntityCore {
	void Update (int time);
	bool CanIact (WorldEntity src, EntityIact ia);
}