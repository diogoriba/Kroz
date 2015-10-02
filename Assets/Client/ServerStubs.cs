using UnityEngine;
using System.Collections;

public class ServerStubs : MonoBehaviour {
    [RPC]
    void TellServerOurName(string name, NetworkMessageInfo info)
    {
    }
}
