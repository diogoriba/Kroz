using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual string Parse(string command)
        {
            string output = "";
            switch (command)
            {
                case "examinar":
                    output = Examine();
                    break;
                default:
                    output = string.Format("Você não pode {0} {1}", command, Name);
                    break;
            }
            return output;
        }

        public string Examine()
        {
            return Description;
        }
    }
}
