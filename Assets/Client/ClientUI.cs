using UnityEngine;
using System.Collections;

public class ClientUI : MonoBehaviour {

    public string connectToIP = "";
    public int connectPort = 53105;
    public string name = "";

    void Awake()
    {
        name = PlayerPrefs.GetString("playerName");
        if (string.IsNullOrEmpty(name))
        {
            name = "RandomName" + Random.Range(1, 1000);
        }
        
    }
    void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");

            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString()));
            name = GUILayout.TextField(name, GUILayout.MinWidth(100));

            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect as client"))
            {
                PlayerPrefs.DeleteKey("playerName");
                PlayerPrefs.SetString("playerName", name);
                //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                Network.Connect(connectToIP, connectPort);
            }

            GUILayout.EndVertical();


        }
        else
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Connection status: Connecting");

            }
            else if (Network.peerType == NetworkPeerType.Client)
            {

                GUILayout.Label("Player name: " + name);
                GUILayout.Label("Ping to server: " + Network.GetAveragePing(Network.connections[0]));

            }

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect(200);
            }
        }
    }
}
