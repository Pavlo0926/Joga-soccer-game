/////////////////////////////////////////////////////////////////////////////////
//
//	FST_MPDebug.cs
//
//	description:	simple debug message functionality for multiplayer. messages
//					are re-routed to the chat by default, but could also be pushed
//					to a console of any kind. this is work in progress
//
/////////////////////////////////////////////////////////////////////////////////

public class FST_MPDebug
{

    /// <summary>
    /// prints a message to an appropriate multiplayer gui target
    /// </summary>
    public static void Log(string msg)
    {

        FST_GlobalEvent<string, bool>.Send("ChatMessage", msg, false, FST_GlobalEventMode.DONT_REQUIRE_LISTENER);
        //Debug.Log(msg);

    }

}