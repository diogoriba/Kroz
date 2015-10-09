using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Key : HoldableItem
    {
        public Key(string name, string description)
            : base(name, description)
        {
            Consumable = true;
        }

        public override void Use(Command command, Player player)
        {
            string tail = string.Join("", command.Tail);
            Item targetItem = player.Find(tail) ?? player.Room.Find(tail);
            if (targetItem != null)
            {
                if (player.Items.Contains(this))
                {
                    if (targetItem.UseOn(this, player) && Consumable)
                    {
                        player.Items.Remove(this);
                    }
                }
                else
                {
                    player.Talk(global::Server.Instance.ServerPlayer, string.Format("Do que você está falando? Não há nada chamado {0}", tail));
                }
            }
            else
            {
                player.Talk(global::Server.Instance.ServerPlayer, string.Format("Do que você está falando? Não há nada chamado {0}", tail));
            }
        }
    }
}
