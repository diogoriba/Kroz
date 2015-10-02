using UnityEngine;
using System.Collections;

public class LogWindow : MonoBehaviour {

    public bool showChat = true;
    private Vector2 scrollPosition;
    private Rect window;

    private ArrayList chatEntries = new ArrayList();
    public class ChatEntry
    {
	    public string name = "";
	    public string text = "";	
    }

    void OnGUI ()
    {
	    if(!showChat){
		    return;
	    }
	
	    window = GUI.Window(5, new Rect(0, 100, Screen.width, Screen.height - 100), GlobalChatWindow, "");
    }


    void GlobalChatWindow (int id) {
	
	    GUILayout.BeginVertical();
	    GUILayout.Space(10);
	    GUILayout.EndVertical();
	
	    // Begin a scroll view. All rects are calculated automatically - 
        // it will use up any available screen space and make sure contents flow correctly.
        // This is kept small with the last two parameters to force scrollbars to appear.
	    scrollPosition = GUILayout.BeginScrollView(scrollPosition);

	    foreach (ChatEntry entry in chatEntries)
	    {
		    GUILayout.BeginHorizontal();
		    if(entry.name==""){//Game message
			    GUILayout.Label (entry.text);
		    }else{
			    GUILayout.Label (entry.name+": "+entry.text);
		    }
		    GUILayout.EndHorizontal();
		    GUILayout.Space(3);
		
	    }
	    // End the scrollview we began above.
        GUILayout.EndScrollView ();
    }

    public void Log(string message)
    {
        var entry = new ChatEntry();
        entry.name = "";
        entry.text = message;

        chatEntries.Add(entry);
        while (chatEntries.Count > 100)
        {
            chatEntries.RemoveAt(0);
        }
    }
}
