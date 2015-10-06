using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Room : Item
    {
        public Dictionary<string, Room> neighbors;

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

        public override void Parse(Command command, Player player)
        {
            switch (command.Verb)
            {
                case "ir":
                    string destination = command.Tail.FirstOrDefault();
                    Go(destination, player);
                    break;
                //case "examinar":
                //case "pegar":
                default:
                    base.Parse(command, player);
                    break;
            }
        }
    }
}
