using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Door : Item
    {
        public string UnlockDescription { get; set; }

        public Door(string name, string description)
            : base(name, description)
        {
            UnlockDescription = "Com um clank, a porta pesada se abre lentamente. As dobradiças estão enferrujadas, e soam como se você não fosse bem vindo.";
        }

        public override bool UseOn(Item source, Player player)
        {
            if (source.Name.Equals("chave"))
            {
                // faz coisas
                player.Talk(global::Server.Instance.ServerPlayer, UnlockDescription);
                return true;
            }

            return false;
        }
    }
}
