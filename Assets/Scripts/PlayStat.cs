using System;
using System.Collections.Generic;

[Serializable]
public class PlayStat {
	private int[] meters = new int[(int)Scheme.StatMeter.Size];
	private int[] stats = new int[(int)Scheme.StatPoint.Size];

	//TODO
	public int hp;

	public int GetMeter(Scheme.StatMeter id) {
		return meters[(int)id];
	}
	public void SetMeter(Scheme.StatMeter id, int value) {
		meters[(int)id] = value;
	}
	public int GetPoint(Scheme.StatPoint id) {
		return stats[(int)id];
	}
	public void SetPoint(Scheme.StatPoint id, int value) {
		stats[(int)id] = value;
	}
}
