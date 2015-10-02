using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Room
    {
        public string description;
        public Dictionary<string, Room> neighbors;

        public Room(string description)
        {
            this.description = description;
            neighbors = new Dictionary<string, Room>();
        }

        public void Describe(Player player)
        {
            player.Send("SERVER", description);
        }

        public void Go(string target, Player player)
        {
            if (neighbors.ContainsKey(target))
            {
                player.Room = neighbors[target];
                player.Room.Describe(player);
            }
            else
            {
                player.Send("SERVER", "Não há saída na direção " + target);
            }
        }
    }
}
