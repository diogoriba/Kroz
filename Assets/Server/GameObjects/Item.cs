using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public abstract class Item
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Item> Items { get; set; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
            Items = new List<Item>();
        }

        public virtual void Parse(Command command, Player player)
        {
            switch (command.Verb)
            {
                case "examinar":
                    Describe(player);
                    break;
                case "falar":
                    string message = String.Join(" ", command.Tail.Skip(1).ToArray());
                    Talk(player, message);
                    break;
                default:
                    global::Server.Instance.Send(global::Server.Instance.ServerPlayer, player, string.Format("Você não pode {0} {1}", command, Name));
                    break;
            }
        }

        public virtual void Describe(Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, Description);
        }

        public virtual void Go(string target, Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, string.Format("\"{0}\" não é um lugar onde você possa ir", Name));
        }

        public virtual void Talk(Player author, string message)
        {
            author.Talk(global::Server.Instance.ServerPlayer, string.Format("Você fala \"{0}\" para {1}, mas a falta de consciência de {1} parece ser um problema para a compreensão de sua mensagem", message, Name));
        }
    }
}
