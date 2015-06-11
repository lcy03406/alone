using System;
using System.Collections.Generic;

[Serializable]
public class PlayInventory
{
	public interface View {
	}

	public List<PlayItem> items = new List<PlayItem> ();
}