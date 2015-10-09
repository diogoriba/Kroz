using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Server.GameObjects
{
    public class Map : HoldableItem
    {
        private static string mapString;
        static Map()
        {
            string[] lines = new string[]{
                "",
                "***********                                                ",
                "*    {3}    *                                                ",
                "*         *            ******                              ",
                "********  *            *  {8} *                              ",
                "*         *            *    *                              ",
                "*         *          ****——****            ****************",
                "*         *          *        *            *              *",
                "*    {2}    ********   *        *            *              *",
                "*         *      *****        *            *              *",
                "*         |  {1}   | {4} |   {5}    *            *              *",
                "*         *      *****        *            *              *",
                "*         *      *   *        *            *       {9}      *",
                "*************——***   *        *            *              *",
                "            *  *     ****——****            *              *",
                "            *  *        *  *               *              *",
                "            *  *        * {6}*               *              *",
                "            *  *        *  *****************              *",
                "            *  *        *  ||||||||||||||»{7} |              *",
                "            *  *        ***********************************",
                "            *  *                                           ",
                "            * {0}*                                           ",
                "            *  *                                           ",
                "            *  *                                           "
            };
            mapString = string.Join("\n", lines);
        }

        public Map()
            : base("mapa", "")
        {

        }

        public override void Describe(Player player)
        {
            string[] parameters = global::Server.Instance.Map.Select(room => player.Room == room ? "X" : " ").ToArray();
            Description = string.Format(mapString, parameters);
            base.Describe(player);
        }
    }   
}
