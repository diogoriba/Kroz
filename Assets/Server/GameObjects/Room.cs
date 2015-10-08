using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Room : Item
    {
        public Dictionary<string, Room> neighbors;
        public string DetailedDescription { get; set; }

        public Room()
            : base("", "")
        {
            neighbors = new Dictionary<string, Room>();
        }

        public Room(string description) 
            : base("", description)
        {
            neighbors = new Dictionary<string, Room>();
        }

        public override void Go(string target, Player player)
        {
            if (neighbors.ContainsKey(target))
            {
                player.Room = neighbors[target];
                player.Room.Describe(player);
            }
            else
            {
                player.Talk(global::Server.Instance.ServerPlayer, "Não há saída na direção " + target);
            }
        }

        public void DetailedDescribe(Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, DetailedDescription);
        }

        public override void Parse(Command command, Player player)
        {
            switch (command.Verb)
            {
                case "ir":
                    string destination = command.Tail.FirstOrDefault();
                    Go(destination, player);
                    break;
                case "examinar":
                    DetailedDescribe(player);
                    break;
                default:
                    base.Parse(command, player);
                    break;
            }
        }
    }
}
