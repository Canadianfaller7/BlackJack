using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    // all interfaces are public so no need to write public, interfaces are also like abstract classes where there are no implementation details
    interface IWalkAway // interfaces make it so other classes can inherit more than just one class and an interface always will start with an upper case I to let others know it is in interface
    {
         void WalkAway(Player player); // method we will add to our blackJack game stating players aren't forced to play they can just walk away from a game when they are done playing
    }
}
