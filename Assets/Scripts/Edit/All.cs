using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Edit {
	public abstract class Meta {
		public int ID;
		public virtual void AfterLoad() { }
	}

	public class Table : CsvTable.Loader {
		private SortedList<int, Meta> data = new SortedList<int, Meta>();
		private HashSet<int> empty = new HashSet<int>();

		public Meta Get(int id) {
			Meta value = null;
			data.TryGetValue(id, out value);
			return value;
		}

		public Meta GetOrAddEmpty(int id, Type type) {
			Meta value = null;
			if (!data.TryGetValue(id, out value)) {
				value = (Meta)Activator.CreateInstance(type);
				value.ID = id;
				data.Add(id, value);
				empty.Add(id);
				return value;
			}
			if (!type.IsAssignableFrom(value.GetType()))
				return null;
			return value;
		}

		public void Add(int id, Meta d) {
			d.ID = id;
			data.Add(d.ID, d);
		}

		object CsvTable.Loader.Allocate(Type type, int id) {
			if (empty.Remove(id)) {
				Meta value = Get(id);
				if (value.GetType() != type) {
					throw new GameResourceException(string.Format("type mismatch. empty:{0} table:{1}", value.GetType(), type));
				}
				return value;
			} else {
				Meta value = (Meta)Activator.CreateInstance(type);
				value.ID = id;
				data.Add(id, value);
				return value;
			}
		}
	}

	public class All {
		public static All all = null;


		private All() {
			if (all != null) {
				throw new GameScriptException("Should not new All twice!");
			}
		}

		private Dictionary<Type, Table> tables = new Dictionary<Type, Table>();

		public Meta Get(int id, Type type) {
			Type origin = TypeHelper.GetOriginType(type, typeof(Meta));
			Table table;
			if (!tables.TryGetValue(origin, out table)) {
				throw new GameResourceException(string.Format("Table not found! type:{0} table:{1}", type, origin));
			}
			Meta value = table.Get(id);
			if (value == null) {
				throw new GameResourceException(string.Format("Data not found! id:{0} type:{1}", id, type));
			}
			if (!type.IsAssignableFrom(value.GetType())) {
				throw new GameResourceException(string.Format("Data type mismatch! id:{0} type:{1} valueType:{2}", id, type, value.GetType()));
			}
			return value;
		}

		public T Get<T>(int id) where T : Meta {
			return (T)Get(id, typeof(T));
		}

		public Meta GetOrAddEmpty(int id, Type type) {
			Type origin = TypeHelper.GetOriginType(type, typeof(Meta));
			Table table;
			if (!tables.TryGetValue(origin, out table)) {
				table = new Table();
				tables.Add(origin, table);
			}
			return table.GetOrAddEmpty(id, type);
		}

		public T GetOrAddEmpty<T>(int id) where T : Meta {
			return (T)GetOrAddEmpty(id, typeof(T));
		}

		public static void Init() {
			all = new All();
			all.LoadFiles();
		}

		public void LoadFiles() {
			TextAsset[] texts = Resources.LoadAll<TextAsset>("csv");
			foreach (TextAsset ass in texts) {
				if (ass.name[0] == '~')
					continue;
				string name = ass.name.Split('#')[0];
				string typename = "Schema.Edit" + name;
                Type type = Type.GetType(typename);
				if (type == null || !type.IsSealed || !typeof(Meta).IsAssignableFrom(type)) {
					throw new GameResourceException(string.Format("fail to load schema data {0} : no such type.", ass.name));
				}
				Type origin = TypeHelper.GetOriginType(type, typeof(Meta));
				Table table;
				if (!tables.TryGetValue(origin, out table)) {
					table = new Table();
					tables.Add(origin, table);
				}
				try {
					using (StringReader reader = new StringReader(ass.text)) {
						CsvTable.Load(reader, type, table);
					}
				} catch (Exception e) {
					throw new GameResourceException(string.Format("fail to load schema data {0}", ass.name), e);
				}
			}
			foreach (TextAsset ass in texts) {
				Resources.UnloadAsset(ass);
			}
		}

	}
}

