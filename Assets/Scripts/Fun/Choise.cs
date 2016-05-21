//utf-8。
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Fun {
	public class Choice<T> {
		public T value;
		public int prob;
		public Choice(T value, int prob) {
			this.value = value;
			this.prob = prob;
		}

		public static Random random = new Random((int)DateTime.Now.Ticks - 10086);
		public static T Choose(List<Choice<T>> choices) {
			int total = 0;
			foreach (Choice<T> choice in choices) {
				total += choice.prob;
			}
			int rand = random.Next(0, total);
			foreach (Choice<T> choice in choices) {
				rand -= choice.prob;
				if (rand < 0)
					return choice.value;
			}
			Assert.IsTrue(false);
			return default(T);
		}
	}

}
