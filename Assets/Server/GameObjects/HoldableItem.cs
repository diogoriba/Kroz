using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
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
                player.Scream(player, Name, "pegou");
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
                player.Scream(player, Name, "largou");
            }
            else
            {
                global::Server.Instance.LogWindow.Log(string.Format("{0} tentou largar um item {1} que ele não possui", player.Name, Name));
            }
        }
    }
}
