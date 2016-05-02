using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Edit {
	public abstract class Data {
		public int ID;
	}

	public class All : Table.Loader {
		public static All all = null;

		private SortedList<int, Data> data = new SortedList<int, Data>();
		private HashSet<int> empty = new HashSet<int>();

		private All() {
			if (all != null) {
				throw new Exception("Should not new All twice!");
			}
		}

		public Data Get(int id) {
			Data value = null;
			data.TryGetValue(id, out value);
			return value;
		}

		public Data Get(int id, Type type) {
			Data value = null;
			if (!data.TryGetValue(id, out value))
				return null;
			if (!type.IsAssignableFrom(value.GetType()))
				return null;
			return value;
		}

		public T Get<T>(int id) where T : Data {
			return (T)Get(id, typeof(T));
		}

		public Data GetOrAddEmpty(int id, Type type) {
			Data value = null;
			if (!data.TryGetValue(id, out value)) {
				value = (Data) Activator.CreateInstance(type);
				value.ID = id;
				data.Add(id, value);
				empty.Add(id);
				return value;
			}
			if (!type.IsAssignableFrom(value.GetType()))
				return null;
			return value;
		}

		public static void Init() {
			all = new All();
			Tile.Init(all);
			all.LoadFiles();
		}

		public void Add(int id, Data d) {
			d.ID = id;
			data.Add(d.ID, d);
		}

		public void LoadFiles() {
			TextAsset[] texts = Resources.LoadAll<TextAsset>("csv");
			foreach (TextAsset ass in texts) {
				string name = ass.name.Split('#')[0];
				string typename = "Schema.Edit" + name;
                Type type = Type.GetType(typename);
				if (type == null || !type.IsSealed || !typeof(Data).IsAssignableFrom(type)) {
					throw new InvalidDataException(string.Format("fail to load schema data {0} : no such type.", ass.name));
				}
				try {
					using (StringReader reader = new StringReader(ass.text)) {
						Table.Load(reader, type, this);
					}
				} catch (Exception e) {
					throw new InvalidDataException(string.Format("fail to load schema data {0}", ass.name), e);
				}
			}
			foreach (TextAsset ass in texts) {
				Resources.UnloadAsset(ass);
			}
		}

		object Table.Loader.Allocate(Type type, int id) {
			if (empty.Remove(id)) {
				Data value = Get(id);
				if (value.GetType() != type) {
					throw new InvalidDataException(string.Format("type mismatch empty:{0} table:{1}", value.GetType(), type));
				}
				return value;
			} else {
				Data value = (Data)Activator.CreateInstance(type);
				value.ID = id;
				data.Add(id, value);
				return value;
			}
		}
	}

	interface HasItemDes {
		string ItemDes { get; set; }
	}
	public sealed class EditDropTable : Data, HasItemDes {
		public string Name;
		public Data ItemName;
		public string ItemDes;
		public struct DropItem {
			public int ID;
			public int Num;
		}
		public List<DropItem> Item;

		string HasItemDes.ItemDes { get; set; }
	}
}

