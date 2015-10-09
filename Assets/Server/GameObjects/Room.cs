using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Room : Item
    {
        private Dictionary<string, Room> neighbors;

        public Dictionary<string, Room> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }
        private Dictionary<string, Door> doors;

        public Dictionary<string, Door> Doors
        {
            get { return doors; }
            set { doors = value; }
        }
        public string DetailedDescription { get; set; }

        public Room()
            : base("", "")
        {
            neighbors = new Dictionary<string, Room>();
            doors = new Dictionary<string, Door>();
        }

        public Room(string description) 
            : base("", description)
        {
            neighbors = new Dictionary<string, Room>();
            doors = new Dictionary<string, Door>();
        }

        public override Item Find(string target)
        {
            target = target.ToLower();
            if (doors.ContainsKey(target))
            {
                return doors[target];
            }
            return base.Find(target);
        }

        public override void Go(string target, Player player)
        {
            target = target.ToLower();
            if (neighbors.ContainsKey(target))
            {
                if (!doors.ContainsKey(target) || !doors[target].Locked)
                {
                    player.Scream(player, "", "deixa a sala pela saída " + target);
                    player.Room = neighbors[target];
                    player.Scream(player, "", "entra na sala");
                    player.Room.Describe(player);
                }
                else
                {
                    player.Talk(global::Server.Instance.ServerPlayer, doors[target].Description + " impede sua passagem");
                }
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
