using Assets.Server.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server
{
    public class Command
    {
        public string Verb { get; set; }
        public Item Target { get; set; }
        public string[] Tail { get; set; }

        public Command(string verb = "", Item target = null, string[] tail = null)
        {
            Verb = verb;
            Target = target;
            Tail = tail;
        }
    }
}
