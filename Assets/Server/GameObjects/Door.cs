using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Door : Item
    {
        public string UnlockDescription { get; set; }
        public string LockDescription { get; set; }
        public bool Locked { get; set; }
        public string KeyName { get; set; }

        public Door(string name, string description)
            : base(name, description)
        {
            if (string.IsNullOrEmpty(description))
            {
                Description = "Uma porta de madeira um pouco mofada";
            }
            UnlockDescription = "Com um clank, a porta pesada se abre lentamente. As dobradiças estão enferrujadas, e soam como se você não fosse bem vindo.";
            LockDescription = "Após virar a chave, a porta volta a ficar trancada.";
            Locked = true;
            KeyName = "chave";
        }

        public override bool UseOn(Item source, Player player)
        {
            if (source.Name.Equals(KeyName))
            {
                Locked = !Locked;
                string text;
                if (Locked)
                {
                    text = LockDescription;
                }
                else
                {
                    text = UnlockDescription;
                }
                player.Talk(global::Server.Instance.ServerPlayer, text);
                return true;
            }

            return false;
        }
    }
}
