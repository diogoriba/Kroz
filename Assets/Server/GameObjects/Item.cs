﻿using System;
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
            switch (command.Verb)
            {
                case "examinar":
                    Describe(player);
                    break;
                case "falar":
                    string message = String.Join(" ", command.Tail.ToArray());
                    Talk(player, message);
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
                //case "abrir":
                default:
                    global::Server.Instance.Send(global::Server.Instance.ServerPlayer, player, string.Format("Você não pode {0} {1}", command, Name));
                    break;
            }
        }

        public virtual Item Find(string target)
        {
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

        public virtual void Talk(Player author, string message)
        {
            author.Talk(global::Server.Instance.ServerPlayer, string.Format("Você fala \"{0}\" para {1}, mas a falta de consciência de {1} parece ser um problema para a compreensão de sua mensagem", message, Name));
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

    public class HoldableItem : Item
    {
        public HoldableItem(string name, string description)
            : base(name, description)
        {

        }

        public override void Take(Player player)
        {
            if (player.Items.Contains(this))
            {
                player.Talk(global::Server.Instance.ServerPlayer, string.Format("Você já possui {0}", Name));
            }
            else if (player.Room.Items.Contains(this))
            {
                player.Room.Items.Remove(this);
                player.Items.Add(this);
                player.Talk(global::Server.Instance.ServerPlayer, string.Format("Você pegou {0}", Name));
            }
            else
            {
                global::Server.Instance.LogWindow.Log(string.Format("{0} tentou pegar um item {1} que não está nem no inventário nem na sala", player.Name, Name));
            }
        }

        public override void Leave(Player player)
        {
            if (player.Items.Contains(this))
            {
                player.Items.Remove(this);
                player.Room.Items.Add(this);
                player.Talk(global::Server.Instance.ServerPlayer, string.Format("Você largou {0} no chão", Name));
            }
            else
            {
                global::Server.Instance.LogWindow.Log(string.Format("{0} tentou largar um item {1} que ele não possui", player.Name, Name));
            }
        }
    }
}
