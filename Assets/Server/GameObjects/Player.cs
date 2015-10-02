using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Server.GameObjects
{
    public class Player
    {
        private string playerName;

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        private Room room;

        public Room Room
        {
            get { return room; }
            set { room = value; }
        }
        private NetworkPlayer networkPlayer;

        public NetworkPlayer NetworkPlayer
        {
            get { return networkPlayer; }
            set { networkPlayer = value; }
        }

        public void Send(string authorName, string message)
        {
            global::Server.NetworkView.RPC("ApplyGlobalChatText", networkPlayer, authorName, message);
        }
    }
}
