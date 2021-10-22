/////////////////////////////////////////////////////////////////////////////////
//
//  FST_GlobalEvent.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This class allows the sending of generic events to and from
//					any class with generic listeners which register and unregister
//					from the events. Events can have 0-3 arguments.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;

public delegate void FST_GlobalCallback();									// 0 Arguments
public delegate void FST_GlobalCallback<T>(T arg1);							// 1 Argument
public delegate void FST_GlobalCallback<T, U>(T arg1, U arg2);				// 2 Arguments
public delegate void FST_GlobalCallback<T, U, V>(T arg1, U arg2, V arg3);	// 3 Arguments

public delegate R FST_GlobalCallbackReturn<R>();									// 0 Arguments and return
public delegate R FST_GlobalCallbackReturn<T, R>(T arg1);						// 1 Argument and return
public delegate R FST_GlobalCallbackReturn<T, U, R>(T arg1, U arg2);				// 2 Arguments and return
public delegate R FST_GlobalCallbackReturn<T, U, V, R>(T arg1, U arg2, V arg3);	// 3 Arguments and return



public enum FST_GlobalEventMode {
	DONT_REQUIRE_LISTENER,
	REQUIRE_LISTENER
}

static internal class FST_GlobalEventInternal
{

	public static Hashtable Callbacks = new Hashtable();
	
	public static UnregisterException ShowUnregisterException(string name)
	{
	
		return new UnregisterException(string.Format("Attempting to Unregister the event {0} but FST_GlobalEvent has not registered this event.", name));
		
	}
	
	public static SendException ShowSendException(string name)
	{
	
		return new SendException(string.Format("Attempting to Send the event {0} but FST_GlobalEvent has not registered this event.", name));
		
	}
 
	public class UnregisterException : Exception { public UnregisterException(string msg) : base(msg){} }
	public class SendException : Exception { public SendException(string msg) : base(msg){} }

}


// Event with no arguments
public static class FST_GlobalEvent
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallback callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallback> callbacks = (List<FST_GlobalCallback>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallback>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallback callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallback> callbacks = (List<FST_GlobalCallback>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event
	/// </summary>
	public static void Send(string name)
	{
	
		Send(name, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
		
	}
	
	
	/// <summary>
	/// sends an event
	/// </summary>
	public static void Send(string name, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    List<FST_GlobalCallback> callbacks = (List<FST_GlobalCallback>)m_Callbacks[name];
	    if(callbacks != null)
	        foreach(FST_GlobalCallback c in callbacks)
	            c();
		else if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
			throw FST_GlobalEventInternal.ShowSendException(name);
	
	}
	
}


// Accepts 1 Argument
public static class FST_GlobalEvent<T>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallback<T> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallback<T>> callbacks = (List<FST_GlobalCallback<T>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallback<T>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallback<T> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallback<T>> callbacks = (List<FST_GlobalCallback<T>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 1 argument
	/// </summary>
	public static void Send(string name, T arg1)
	{
	
		Send(name, arg1, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
		
	}
	
	
	/// <summary>
	/// sends an event with 1 argument
	/// </summary>
	public static void Send(string name, T arg1, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(arg1 == null)
	        throw new ArgumentNullException("arg1");
	        
	    List<FST_GlobalCallback<T>> callbacks = (List<FST_GlobalCallback<T>>)m_Callbacks[name];
	    if(callbacks != null)
	        foreach(FST_GlobalCallback<T> c in callbacks)
	            c(arg1);
		else if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
			throw FST_GlobalEventInternal.ShowSendException(name);
	
	}
	
}


// Accepts 2 arguments
public static class FST_GlobalEvent<T, U>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallback<T, U> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallback<T, U>> callbacks = (List<FST_GlobalCallback<T, U>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallback<T, U>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallback<T, U> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallback<T, U>> callbacks = (List<FST_GlobalCallback<T, U>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 2 arguments
	/// </summary>
	public static void Send(string name, T arg1, U arg2)
	{
	
		Send(name, arg1, arg2, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
		
	}
	
	
	/// <summary>
	/// sends an event with 2 arguments
	/// </summary>
	public static void Send(string name, T arg1, U arg2, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(arg1 == null)
	        throw new ArgumentNullException("arg1");
	        
		if(arg2 == null)
	        throw new ArgumentNullException("arg2");
	        
	    List<FST_GlobalCallback<T, U>> callbacks = (List<FST_GlobalCallback<T, U>>)m_Callbacks[name];
	    if(callbacks != null)
	        foreach(FST_GlobalCallback<T, U> c in callbacks)
	            c(arg1, arg2);
		else if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
			throw FST_GlobalEventInternal.ShowSendException(name);
	
	}
	
}


// Accepts 3 Arguments
public static class FST_GlobalEvent<T, U, V>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallback<T, U, V> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallback<T, U, V>> callbacks = (List<FST_GlobalCallback<T, U, V>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallback<T, U, V>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallback<T, U, V> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallback<T, U, V>> callbacks = (List<FST_GlobalCallback<T, U, V>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 3 arguments
	/// </summary>
	public static void Send(string name, T arg1, U arg2, V arg3)
	{
	
		Send(name, arg1, arg2, arg3, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
	
	}
	
	
	/// <summary>
	/// sends an event with 3 arguments
	/// </summary>
	public static void Send(string name, T arg1, U arg2, V arg3, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(arg1 == null)
	        throw new ArgumentNullException("arg1");
	        
		if(arg2 == null)
	        throw new ArgumentNullException("arg2");
	        
		if(arg3 == null)
	        throw new ArgumentNullException("arg3");
	        
	    List<FST_GlobalCallback<T, U, V>> callbacks = (List<FST_GlobalCallback<T, U, V>>)m_Callbacks[name];
	    if(callbacks != null)
	        foreach(FST_GlobalCallback<T, U, V> c in callbacks)
	            c(arg1, arg2, arg3);
		else if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
			throw FST_GlobalEventInternal.ShowSendException(name);
	
	}
	
}


// Event with no arguments and a return value
public static class FST_GlobalEventReturn<R>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallbackReturn<R> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallbackReturn<R>> callbacks = (List<FST_GlobalCallbackReturn<R>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallbackReturn<R>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallbackReturn<R> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallbackReturn<R>> callbacks = (List<FST_GlobalCallbackReturn<R>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 1 argument and returns a value
	/// </summary>
	public static R Send(string name)
	{
	
		return Send(name, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
	
	}
	
	
	/// <summary>
	/// sends an event with 1 argument and returns a value
	/// </summary>
	public static R Send(string name, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    List<FST_GlobalCallbackReturn<R>> callbacks = (List<FST_GlobalCallbackReturn<R>>)m_Callbacks[name];
	    if(callbacks != null)
	    {
	    	R val = default(R);
	        foreach(FST_GlobalCallbackReturn<R> c in callbacks)
	            val = c();
	        return val;
		}
		else
		{
			if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
				throw FST_GlobalEventInternal.ShowSendException(name);
			return default(R);
		}
	
	}
	
}


// Accepts 1 argument with a return value
public static class FST_GlobalEventReturn<T, R>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallbackReturn<T, R> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallbackReturn<T, R>> callbacks = (List<FST_GlobalCallbackReturn<T, R>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallbackReturn<T, R>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallbackReturn<T, R> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallbackReturn<T, R>> callbacks = (List<FST_GlobalCallbackReturn<T, R>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 1 argument and returns a value
	/// </summary>
	public static R Send(string name, T arg1)
	{
	
		return Send(name, arg1, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
		
	}
	
	
	/// <summary>
	/// sends an event with 1 argument and returns a value
	/// </summary>
	public static R Send(string name, T arg1, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(arg1 == null)
	        throw new ArgumentNullException("arg1");
	        
	    List<FST_GlobalCallbackReturn<T, R>> callbacks = (List<FST_GlobalCallbackReturn<T, R>>)m_Callbacks[name];
	    if(callbacks != null)
	    {
	    	R val = default(R);
	        foreach(FST_GlobalCallbackReturn<T, R> c in callbacks)
	            val = c(arg1);
	        return val;
		}
		else
		{
			if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
				throw FST_GlobalEventInternal.ShowSendException(name);
			return default(R);
		}
	
	}
	
}


// Accepts 2 arguments with a return value
public static class FST_GlobalEventReturn<T, U, R>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallbackReturn<T, U, R> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallbackReturn<T, U, R>> callbacks = (List<FST_GlobalCallbackReturn<T, U, R>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallbackReturn<T, U, R>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallbackReturn<T, U, R> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallbackReturn<T, U, R>> callbacks = (List<FST_GlobalCallbackReturn<T, U, R>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 2 arguments and returns a value
	/// </summary>
	public static R Send(string name, T arg1, U arg2)
	{
	
		return Send(name, arg1, arg2, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
	
	}
	
	
	/// <summary>
	/// sends an event with 2 arguments and returns a value
	/// </summary>
	public static R Send(string name, T arg1, U arg2, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(arg1 == null)
	        throw new ArgumentNullException("arg1");
	        
		if(arg2 == null)
	        throw new ArgumentNullException("arg2");
	        
	    List<FST_GlobalCallbackReturn<T, U, R>> callbacks = (List<FST_GlobalCallbackReturn<T, U, R>>)m_Callbacks[name];
	    if(callbacks != null)
	    {
	    	R val = default(R);
	        foreach(FST_GlobalCallbackReturn<T, U, R> c in callbacks)
	            val = c(arg1, arg2);
	        return val;
		}
		else
		{
			if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
				throw FST_GlobalEventInternal.ShowSendException(name);
			return default(R);
		}
	
	}
	
}


// Accepts 3 Arguments with a return value
public static class FST_GlobalEventReturn<T, U, V, R>
{

	private static Hashtable m_Callbacks = FST_GlobalEventInternal.Callbacks;
	
	/// <summary>
	/// Registers the event specified by name
	/// </summary>
	public static void Register(string name, FST_GlobalCallbackReturn<T, U, V, R> callback)
	{
	
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(@"name");
			
    	if(callback == null)
        	throw new ArgumentNullException("callback");
        
    	List<FST_GlobalCallbackReturn<T, U, V, R>> callbacks = (List<FST_GlobalCallbackReturn<T, U, V, R>>)m_Callbacks[name];
    	if(callbacks == null)
    	{
        	callbacks = new List<FST_GlobalCallbackReturn<T, U, V, R>>();
        	m_Callbacks.Add(name, callbacks);
    	}
   		callbacks.Add(callback);
   		
	}
	
	
	/// <summary>
	/// Unregisters the event specified by name
	/// </summary>
	public static void Unregister(string name, FST_GlobalCallbackReturn<T, U, V, R> callback)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(callback == null)
	        throw new ArgumentNullException("callback");
	        
	    List<FST_GlobalCallbackReturn<T, U, V, R>> callbacks = (List<FST_GlobalCallbackReturn<T, U, V, R>>)m_Callbacks[name];
	    if(callbacks != null)
	        callbacks.Remove(callback);
		else
			throw FST_GlobalEventInternal.ShowUnregisterException(name);
	    
	}
	
	
	/// <summary>
	/// sends an event with 3 arguments and returns a value
	/// </summary>
	public static R Send(string name, T arg1, U arg2, V arg3)
	{
	
		return Send(name, arg1, arg2, arg3, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
		
	}
	
	
	/// <summary>
	/// sends an event with 3 arguments and returns a value
	/// </summary>
	public static R Send(string name, T arg1, U arg2, V arg3, FST_GlobalEventMode mode)
	{
	
	    if(string.IsNullOrEmpty(name))
	        throw new ArgumentNullException(@"name");
	        
	    if(arg1 == null)
	        throw new ArgumentNullException("arg1");
	        
		if(arg2 == null)
	        throw new ArgumentNullException("arg2");
	        
		if(arg3 == null)
	        throw new ArgumentNullException("arg3");

		List<FST_GlobalCallbackReturn<T, U, V, R>> callbacks = (List<FST_GlobalCallbackReturn<T, U, V, R>>)m_Callbacks[name];
	    if(callbacks != null)
	    {
    		R val = default(R);
	        foreach(FST_GlobalCallbackReturn<T, U, V, R> c in callbacks)
            	val = c(arg1, arg2, arg3);
            return val;
		}
		else
	    {
	    	if(mode == FST_GlobalEventMode.REQUIRE_LISTENER)
				throw FST_GlobalEventInternal.ShowSendException(name);
			return default(R);
		}
	
	}
	
}
