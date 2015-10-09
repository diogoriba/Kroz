using UnityEngine;
using System.Collections;

public class Chat : MonoBehaviour {

    public bool usingChat = false;
    bool showChat = false;

    //Private vars used by the script
    private string inputField = "";

    private Vector2 scrollPosition;
    private string playerName;
    private float lastUnfocusTime = 0;
    private Rect window;

    private ArrayList chatEntries = new ArrayList();
    public class ChatEntry
    {
	    public string name = "";
        public string action = "";
	    public string text = "";	
    }

    void Awake(){
    }


    //Client function
    void OnConnectedToServer() {
        window = new Rect(0, 80, Screen.width, Screen.height - 80);
	    ShowChatWindow();
        //We get the name from the masterserver example, if you entered your name there ;).
        playerName = PlayerPrefs.GetString("playerName", "");
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "RandomName" + Random.Range(1, 999);
        }	
	    networkView.RPC ("TellServerOurName", RPCMode.Server, playerName);
    }

    void OnDisconnectedFromServer(){
	    CloseChatWindow();
    }

    void CloseChatWindow ()
    {
	    showChat = false;
	    inputField = "";
	    chatEntries = new ArrayList();
    }

    void ShowChatWindow ()
    {
	    showChat = true;
	    inputField = "";
	    chatEntries = new ArrayList();
    }

    void OnGUI ()
    {
	    if(!showChat){
		    return;
	    }
			
	    if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length <= 0)
	    {
		    if(lastUnfocusTime+0.25<Time.time){
			    usingChat=true;
			    GUI.FocusWindow(5);
			    GUI.FocusControl("Chat input field");
		    }
	    }
	
	    window = GUI.Window(5, window, GlobalChatWindow, "");
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
            string text = entry.name + " " + entry.action;
            if (!string.IsNullOrEmpty(entry.text))
            {
                text = text + ": " + entry.text;
            }
			GUILayout.Label(text);
		    GUILayout.EndHorizontal();
		    GUILayout.Space(3);
		
	    }
	    // End the scrollview we began above.
        GUILayout.EndScrollView ();
	
	    if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length > 0)
	    {
		    HitEnter(inputField);
	    }
	    GUI.SetNextControlName("Chat input field");
	    inputField = GUILayout.TextField(inputField);
	
	
	    if(Input.GetKeyDown("mouse 0")){
		    if(usingChat){
			    usingChat=false;
			    GUI.UnfocusWindow ();//Deselect chat
			    lastUnfocusTime=Time.time;
		    }
	    }
    }

    void HitEnter(string msg){
	    msg = msg.Replace("\n", "");
	    networkView.RPC("ApplyGlobalChatText", RPCMode.Server, playerName, "", msg);
	    inputField = ""; //Clear line
	    //GUI.UnfocusWindow ();//Deselect chat
	    //lastUnfocusTime=Time.time;
	    //usingChat=false;
    }


    [RPC]
    void ApplyGlobalChatText (string name, string action, string msg)
    {
	    var entry = new ChatEntry();
	    entry.name = name;
        entry.action = action;
	    entry.text = msg;

	    chatEntries.Add(entry);
	
	    //Remove old entries
	    if (chatEntries.Count > 100){
		    chatEntries.RemoveAt(0);
	    }

	    scrollPosition.y = 1000000;	
    }
}
