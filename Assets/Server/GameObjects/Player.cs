using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Server.GameObjects
{
    public class Player : Item
    {
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

        public Player(string name, string description = "Um ser humano sem características extraordinárias") :
            base(name, description) 
        {

        }

        public override void Talk(Player author, string message)
        {
            global::Server.Instance.Send(author, this, message);
        }

        //public override void Use(string command, Item target)
        //{

        //}
    }
}
