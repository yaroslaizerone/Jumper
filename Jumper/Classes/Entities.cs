using Jumper.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumper.Classes
{
    public class Entities
    {
        public static JumpDB ent;
        internal static bool flag = false;
        internal static int prioritet = 0;

        public static JumpDB GetContext()
        {
            if (ent == null)
            {
                ent = new JumpDB();
            }
            return ent;
        }
    }
}
