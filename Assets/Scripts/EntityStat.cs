using System;
using System.Collections.Generic;

[Serializable]
public class EntityStat {
	private int[] meters = new int[(int)Schema.StatMeter.Size];
	private int[] stats = new int[(int)Schema.StatPoint.Size];

	//TODO
	public int hp;

	public int GetMeter(Schema.StatMeter id) {
		return meters[(int)id];
	}
	public void SetMeter(Schema.StatMeter id, int value) {
		meters[(int)id] = value;
	}
	public int GetPoint(Schema.StatPoint id) {
		return stats[(int)id];
	}
	public void SetPoint(Schema.StatPoint id, int value) {
		stats[(int)id] = value;
	}
}
