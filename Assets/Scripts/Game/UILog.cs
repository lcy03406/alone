//utf-8。
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

using Stat = Play.Attrs.Stat;
using System.Text;

public class UILog : MonoBehaviour {
	public Text text;

	private int update_time = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (update_time < Game.game.world_update_time) {
			ShowLog();
		}
	}

	public void Hide() {
		text.text = "";
		gameObject.SetActive(false);
	}

	public void ShowLog() {
		update_time = Game.game.world_update_time;
		List<string> logs = Game.game.world.logs;
		int begin = logs.Count - 100;
		if (begin < 0)
			begin = 0;
		StringBuilder sb = new StringBuilder();
		for (int i = begin; i < logs.Count; ++i) {
			sb.AppendLine(logs[i]);
		}
		text.text = sb.ToString();
		gameObject.SetActive(true);
	}
}
