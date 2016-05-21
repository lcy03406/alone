//utf-8。
using System;
using System.IO;
using System.Collections.Generic;

public class GameException : Exception {
	protected GameException() { }
	protected GameException(string message) : base(message) { }
	protected GameException(string message, Exception inner) : base(message, inner) { }
	protected GameException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class GameResourceException : GameException {
	public GameResourceException() { }
	public GameResourceException(string message) : base(message) { }
	public GameResourceException(string message, Exception inner) : base(message, inner) { }
	protected GameResourceException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class GameScriptException : GameException {
	public GameScriptException() { }
	public GameScriptException(string message) : base(message) { }
	public GameScriptException(string message, Exception inner) : base(message, inner) { }
	protected GameScriptException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
