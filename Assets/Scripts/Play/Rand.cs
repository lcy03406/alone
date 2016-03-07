using UnityEngine;
using System.Collections.Generic;

namespace Play {
	public static class Rand {
		public static int BigCave(int x, int y, int z) {
			int i1 = SimpleNoise3(x / 20, y / 20, z * 19);
			int i2 = SimpleNoise3(x / 14, y / 14, z * 23);
			int ix1 = SimpleNoise3(x / 13, y / 2, z * 11);
			int ix2 = SimpleNoise3(x / 33, y / 2, z * 13);
			int iy1 = SimpleNoise3(x / 2, y / 13, z * 7);
			int iy2 = SimpleNoise3(x / 2, y / 33, z * 17);
			int ixy1 = SimpleNoise3(x / 2, y / 2, z * 5);
			//return (i1 + i2) >= 320 || (ix + iy) >= 256 ? 1 : 0;
			int i = (i1 + i2) / 2;
			int ix = (ix1 + ix2) / 2;
			int iy = (iy1 + iy2) / 2;
			return i >= 200 || (ix >= 170 || iy >= 170) && ixy1 > 32 ? 1 : 0;
		}

		private static int[] hash = {
			151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
			140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
			247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
			 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
			 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
			 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
			 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
			200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
			 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
			207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
			119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
			129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
			218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
			 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
			184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
			222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
		};

		private const int hash_size = 256;

		public static int SimpleNoise2(int x, int y) {
			int ix = x % hash_size;
			int iy = y % hash_size;
			int ixy = (hash[ix] + iy) % hash_size;
			return hash[ixy];
		}
		public static int SimpleNoise3(int x, int y, int z) {
			int ix = x % hash_size;
			int iy = y % hash_size;
			int iz = z % hash_size;
			int ixy = (hash[ix] + iy) % hash_size;
			int ixyz = (hash[ixy] + iz) % hash_size;
			return hash[ixyz];
		}
	}
}
