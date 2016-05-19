//utf-8。
using System;
using IList = System.Collections.IList;
using IDictionary = System.Collections.IDictionary;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Utility {
	public interface Field {
		Type ValueType();
		object Get(object parent);
		void Set(object parent, object value);
	}

	public class SingleField : Field {
		public Type dataType;
		public FieldInfo finfo;

		public Type ValueType() {
			return dataType != null ? dataType : finfo.FieldType;
		}

		public object Get(object parent) {
			object field = finfo.GetValue(parent);
			if (field == null) {
				Type fieldType = ValueType();
				field = Activator.CreateInstance(fieldType);
				finfo.SetValue(parent, field);
			}
			return field;
		}

		public void Set(object parent, object value) {
			finfo.SetValue(parent, value);
		}
	}

	public class ListField : Field {
		public Type dataType;
		public FieldInfo finfo;
		public int pos;

		public Type ValueType() {
			return dataType != null ? dataType : finfo.FieldType.GetGenericArguments()[0];
		}

		public object Get(object parent) {
			Type fieldType = finfo.FieldType;
			Type elementType = ValueType();
			object field = finfo.GetValue(parent);
			if (field == null) {
				field = Activator.CreateInstance(fieldType);
				finfo.SetValue(parent, field);
			}
			IList list = field as IList;
			if (list.Count < pos) {
				bool isclass = elementType.IsClass;
				if (isclass) {
					while (list.Count < pos) {
						object newElement = null;
						list.Add(newElement);
					}
				} else {
					while (list.Count < pos) {
						object newElement = Activator.CreateInstance(elementType);
						list.Add(newElement);
					}
				}
			}
			if (list.Count == pos) {
				object newElement = Activator.CreateInstance(elementType);
				list.Add(newElement);
			}
			return list[pos];
		}

		public void Set(object parent, object value) {
			Type fieldType = finfo.FieldType;
			Type elementType = ValueType();
			object field = finfo.GetValue(parent);
			if (field == null) {
				field = Activator.CreateInstance(fieldType);
				finfo.SetValue(parent, field);
			}
			IList list = field as IList;
			if (list.Count < pos) {
				bool isclass = elementType.IsClass;
				if (isclass) {
					while (list.Count < pos) {
						object newElement = null;
						list.Add(newElement);
					}
				} else {
					while (list.Count < pos) {
						object newElement = Activator.CreateInstance(elementType);
						list.Add(newElement);
					}
				}
			}
			if (pos >= 0 && list.Count > pos) {
				list[pos] = value;
			} else {
				list.Add(value);
			}
		}
	}

	public class DictField : Field {
		public Type dataType;
		public FieldInfo finfo;
		public object key;

		public Type ValueType() {
			return dataType != null ? dataType : finfo.FieldType.GetGenericArguments()[1];
		}

		public object Get(object parent) {
			Type fieldType = finfo.FieldType;
			Type valueType = ValueType();
			object field = finfo.GetValue(parent);
			if (field == null) {
				field = Activator.CreateInstance(fieldType);
				finfo.SetValue(parent, field);
			}
			IDictionary dict = field as IDictionary;
			return dict[key];
		}

		public void Set(object parent, object value) {
			Type fieldType = finfo.FieldType;
			Type valueType = ValueType();
			object field = finfo.GetValue(parent);
			if (field == null) {
				field = Activator.CreateInstance(fieldType);
				finfo.SetValue(parent, field);
			}
			IDictionary dict = field as IDictionary;
			dict[key] = value;
		}
	}

	public class PropField : Field {
		public Type dataType;
		public PropertyInfo pinfo;

		public Type ValueType() {
			return dataType != null ? dataType : pinfo.PropertyType;
		}

		public object Get(object parent) {
			object prop = pinfo.GetValue(parent, null);
			if (prop == null) {
				Type fieldType = ValueType();
				prop = Activator.CreateInstance(fieldType);
				pinfo.SetValue(parent, prop, null);
			}
			return prop;
		}

		public void Set(object parent, object value) {
			pinfo.SetValue(parent, value, null);
		}
	}

	public class MethodField : Field {
		public Type dataType;
		public MethodInfo get;
		public MethodInfo set;

		public Type ValueType() {
			return dataType != null ? dataType : get.ReturnType;
		}

		public object Get(object parent) {
			
			object value = get.Invoke(parent, null);
			if (value == null) {
				Type fieldType = ValueType();
				value = Activator.CreateInstance(fieldType);
				set.Invoke(parent, new object[] {value});
				value = get.Invoke(parent, null);
			}
			return value;
		}

		public void Set(object parent, object value) {
			set.Invoke(parent, new object[] { value });
		}
	}

	public class TypeField : Field {
		public Field field;

		public Type ValueType() {
			return typeof(Type);
		}

		public object Get(object parent) {
			return field.Get(parent).GetType();
		}

		public void Set(object parent, object value) {
			Type type = (Type)value;
			object instance = TypeHelper.CreateInstance(type);
			field.Set(parent, instance);
		}
	}

	public class PathField : Field {
		List<Field> path = new List<Field>();

		public PathField(Type type, string str) {
			Type pathType = type;
			int idxtype = str.IndexOf("@@type");
			bool istype = idxtype >= 0;
			if (istype) {
				str = str.Substring(0, idxtype);
			}
			string[] paths = str.Split('.');
			for (int i = 0; i < paths.Length; i++) {
				string field = paths[i];
				Type dataType = null;
				int index = field.IndexOf('@');
				if (index >= 0) {
					string typeName = field.Substring(index + 1);
					dataType = Type.GetType(typeName);
					if (dataType == null) {
						throw new InvalidDataException(string.Format("no type {0} in {1}", typeName, str));
					}
					field = field.Substring(0, index);
				}
				FieldInfo finfo;
				index = field.IndexOf('[');
				if (index >= 0) {
					string elementName = field.Substring(0, index);
					string posName = field.Substring(index + 1, field.Length - index - 2);
					finfo = TypeHelper.GetField(pathType, elementName);
					if (finfo == null) {
						throw new InvalidDataException(string.Format("no field {0} in {1}", elementName, str));
					}
					Type fieldType = finfo.FieldType;
					Type genericType = fieldType.GetGenericTypeDefinition();
					if (genericType == typeof(List<>)) {
						int pos = Convert.ToInt32(posName);
						path.Add(new ListField() { dataType = dataType, finfo = finfo, pos = pos });
						pathType = fieldType.GetGenericArguments()[0];
					} else if (genericType == typeof(SortedList<,>)) {
						Type posType = fieldType.GetGenericArguments()[0];
						object key = JsonHelper.ReadObject(posName, posType);
						path.Add(new DictField() { dataType = dataType, finfo = finfo, key = key });
						pathType = fieldType.GetGenericArguments()[1];
					} else {
						throw new InvalidDataException(string.Format("not support type {0} of {1} in {2}", fieldType, elementName, str));
					}
				} else {
					finfo = TypeHelper.GetField(pathType, field);
					if (finfo != null) {
						path.Add(new SingleField() { dataType = dataType, finfo = finfo });
						pathType = finfo.FieldType;
					} else {
						PropertyInfo pinfo = TypeHelper.GetProp(pathType, field);
						if (pinfo != null) {
							path.Add(new PropField() { dataType = dataType, pinfo = pinfo });
							pathType = pinfo.PropertyType;
						} else {
							MethodInfo get, set;
							if (TypeHelper.GetMethod(pathType, field, out get, out set)) {
								path.Add(new MethodField() { dataType = dataType, get = get, set = set });
								pathType = pinfo.PropertyType;
							} else {
								throw new InvalidDataException(string.Format("no field {0} in {1}", pathType, str));
							}
						}
					}
				}
			}
			if (istype) {
				Field last = path[path.Count - 1];
				path[path.Count - 1] = new TypeField() { field = last };
			}
		}

		public object Get(object parent) {
			object obj = parent;
			for (int i = 0; i < path.Count; ++i) {
				Field field = path[i];
				obj = field.Get(obj);
			}
			return obj;
		}

		public void Set(object parent, object value) {
			object obj = parent;
			for (int i = 0; i < path.Count - 1; ++i) {
				Field field = path[i];
				obj = field.Get(obj);
			}
			Field last = path[path.Count - 1];
			last.Set(obj, value);
		}

		public Type ValueType() {
			Field last = path[path.Count - 1];
			return last.ValueType();
		}
	}

	public static class TypeHelper {
		public static Type GetType(string name) {
			switch (name) {
				case "":
					return null;
				case "bool":
					return typeof(bool);
				case "byte":
					return typeof(byte);
				case "sbyte":
					return typeof(sbyte);
				case "short":
					return typeof(short);
				case "ushort":
					return typeof(ushort);
				case "int":
					return typeof(int);
				case "uint":
					return typeof(uint);
				case "long":
					return typeof(long);
				case "ulong":
					return typeof(ulong);
				case "char":
					return typeof(char);
				case "double":
					return typeof(double);
				case "float":
					return typeof(float);
				case "string":
					return typeof(string);
			}
			Type type = Type.GetType(name);
			if (type != null)
				return type;
			type = Type.GetType("Edit." + name);
			if (type != null)
				return type;
			return null;
		}

		public static FieldInfo GetField(Type type, string name) {
			while (type != null) {
				FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f;
				type = type.BaseType;
			}
			return null;
		}

		public static PropertyInfo GetProp(Type type, string name) {
			while (type != null) {
				PropertyInfo p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (p != null)
					return p;
				type = type.BaseType;
			}
			return null;
		}

		public static bool GetMethod(Type type, string name, out MethodInfo get, out MethodInfo set) {
			for (; type != null; type = type.BaseType) {
				get = type.GetMethod("Get" + name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
					null, Type.EmptyTypes, null);
				if (get == null) {
					continue;
				}
				Type ret = get.ReturnType;
				if (ret == typeof(void)) {
					continue;
				}
				set = type.GetMethod("Set" + name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
					null, new Type[] { ret }, null);
				if (set == null) {
					continue;
				}
				return true;
			}
			get = null;
			set = null;
			return false;
		}

		public static object CreateInstance(Type type) {
			return Activator.CreateInstance(type);
		}
	}
}
