//utf-8。
using System;
using Vector2 = UnityEngine.Vector2;

namespace Play {
	public enum Direction {
		None = 0,
		SouthWest = 1,
		South = 2,
		SouthEast = 3,
		West = 4,
		Center = 5,
		East = 6,
		NorthWest = 7,
		North = 8,
		NorthEast = 9,
	}

	[Serializable]
	public struct Coord : IComparable<Coord> {
		public readonly int x;
		public readonly int y;

		public static Coord O = new Coord(0, 0);

		public Coord (int x, int y) {
			this.x = x;
			this.y = y;
		}

		public Coord (Vector2 v) {
			this.x = (int)(v.x + 0.5);
			this.y = (int)(v.y + 0.5);
		}

		public Coord (Direction dir) {
			if (dir == Direction.None) {
				this.x = 0;
				this.y = 0;
			} else {
				int d = (int)dir;
				this.x = (d - 1) % 3 - 1;
				this.y = (d - 1) / 3 - 1;
			}
		}

		public Direction ToDirection () {
			return (Direction)(Math.Sign (y) * 3 + Math.Sign (x) + 5);
		}

		public override string ToString () {
			return string.Format ("{0}_{1}", x, y);
		}

		public override int GetHashCode () {
			return x + y << 5;
		}

		public override bool Equals (Object ob) {
			if (ob is Coord) {
				Coord b = (Coord)ob;
				return x == b.x && y == b.y;
			}
			return false;
		}
		public int CompareTo (Coord b) {
			if (x < b.x)
				return -2;
			else if (x > b.x)
				return 2;
			else if (y < b.y)
				return -1;
			else if (y > b.y)
				return 1;
			else
				return 0;
		}

		static public bool operator ==(Coord a, Coord b) {
			return a.x == b.x && a.y == b.y;
		}
		static public bool operator !=(Coord a, Coord b) {
			return a.x != b.x || a.y != b.y;
		}

		public Coord Grid () {
			return new Coord (x & ~World.GRID_MASK, y & ~World.GRID_MASK);
		}

		public Coord Local () {
			return new Coord (x & World.GRID_MASK, y & World.GRID_MASK);
		}

		public Rect Area (int dist) {
			return new Rect (this - dist, this + dist);
		}

		public bool In(Rect rc) {
			return x >= rc.bl.x && x <= rc.tr.x && y >= rc.bl.y && y <= rc.tr.y;
		}

		public bool On(Rect rc) {
			bool onx = x >= rc.bl.x && x <= rc.tr.x && (y == rc.bl.y-1 || y == rc.tr.y+1);
			bool ony = (x == rc.bl.x-1 || x == rc.tr.x+1) && y >= rc.bl.y && y <= rc.tr.y;
			return onx || ony;
		}

		public Coord Step (Direction dir) {
			return Add (new Coord (dir));
		}

		public Coord Add (int bx, int by) {
			return new Coord (x + bx, y + by);
		}
		public Coord Add (int b) {
			return new Coord (x + b, y + b);
		}
		public Coord Add (Coord b) {
			return new Coord (x + b.x, y + b.y);
		}
		static public Coord operator +(Coord a, Coord b) {
			return new Coord (a.x + b.x, a.y + b.y);
		}
		static public Coord operator +(Coord a, int b) {
			return new Coord (a.x + b, a.y + b);
		}

		public Coord Sub (int bx, int by) {
			return new Coord (x - bx, y - by);
		}
		public Coord Sub (int b) {
			return new Coord (x - b, y - b);
		}
		public Coord Sub (Coord b) {
			return new Coord (x - b.x, y - b.y);
		}
		static public Coord operator -(Coord a, Coord b) {
			return new Coord (a.x - b.x, a.y - b.y);
		}
		static public Coord operator -(Coord a, int b) {
			return new Coord (a.x - b, a.y - b);
		}
		public int Manhattan (Coord b) {
			int dx = x - b.x;
			int dy = y - b.y;
			dx = dx >= 0 ? dx : -dx;
			dy = dy >= 0 ? dy : -dy;
			return dx + dy;
		}
	}

	[Serializable]
	public struct Rect {
		public Coord bl;
		public Coord tr;

		public Rect (Coord bl, Coord tr) {
			this.bl = bl;
			this.tr = tr;
		}

		public Rect Grid () {
			return new Rect (this.bl.Grid (), this.tr.Grid ());
		}
	}
}
