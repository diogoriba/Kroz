using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class GenericItem : Item
    {
        public GenericItem(string name, string description)
            : base(name, description)
        {

        }
    }

    public abstract class Item
    {
        public string Name { get; private set; }
        public string Description { get; protected set; }
        public List<Item> Items { get; set; }
        public bool Consumable { get; set; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
            Items = new List<Item>();
        }

        public virtual void Parse(Command command, Player player)
        {
            string message = String.Join(" ", command.Tail.ToArray());
            switch (command.Verb)
            {
                case "examinar":
                    Describe(player);
                    break;
                case "falar":
                    Talk(player, message);
                    break;
                case "gritar":
                    Scream(player, message);
                    break;
                case "inventario":
                    Inventory(player);
                    break;
                case "usar":
                    Use(command, player);
                    break;
                case "pegar":
                    Take(player);
                    break;
                case "largar":
                    Leave(player);
                    break;
                default:
                    player.Talk(global::Server.Instance.ServerPlayer, string.Format("Você não pode {0} {1}", command, Name));
                    break;
            }
        }

        public virtual Item Find(string target)
        {
            target = target.ToLower();
            foreach (Item item in Items)
            {
                if (item.Name.Equals(target))
                {
                    return item;
                }
            }
            return null;
        }

        public virtual void Describe(Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, Description);
        }

        public virtual void Go(string target, Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, string.Format("\"{0}\" não é um lugar onde você possa ir", Name));
        }

        public virtual void Talk(Player author, string message, string action = "fala")
        {
            author.Talk(global::Server.Instance.ServerPlayer, string.Format("Você fala \"{0}\" para {1}, mas a falta de consciência de {1} parece ser um problema para a compreensão de sua mensagem", message, Name));
        }

        public virtual void Scream(Player author, string message, string action = "grita")
        {
            author.Talk(global::Server.Instance.ServerPlayer, string.Format("Após berrar \"{0}\" vigorosamente você se sente mais calmo, mas {1} não parece se importar", message, Name));
        }

        public virtual void Inventory(Player author)
        {
            Dictionary<string, int> inventory = new Dictionary<string, int>();
            foreach (Item item in author.Items)
            {
                if (inventory.ContainsKey(item.Name))
                {
                    inventory[item.Name] += 1;
                }
                else
                {
                    inventory.Add(item.Name, 1);
                }
            }

            StringBuilder result = new StringBuilder();
            result.AppendLine("Lista de itens:");
            foreach (string key in inventory.Keys)
            {
                result.AppendLine(string.Format("{0}\tx{1}", key, inventory[key]));
            }

            author.Talk(global::Server.Instance.ServerPlayer, result.ToString());
        }

        public virtual void Take(Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, string.Format("Não parece possível carregar {0} confortavelmente", Name));
        }

        public virtual void Leave(Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, string.Format("Você tenta, mas {0} não quer deixar suas mãos", Name));
        }

        public virtual void Use(Command command, Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, string.Format("Como assim? Não me parece o momento apropriado para usar {0}", Name));
        }

        public virtual bool UseOn(Item source, Player player)
        {
            player.Talk(global::Server.Instance.ServerPlayer, string.Format("{0} não parece interagir com {1} de nenhuma maneira interessante", source.Name, Name));
            return false;
        }
    }
}
