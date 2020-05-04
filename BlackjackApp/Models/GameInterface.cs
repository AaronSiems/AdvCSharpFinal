using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Models
{
    public interface GameInterface
    {
        Player player { get; set; }
        Dealer dealer { get; set; }

        Game.Result Deal();
        Game.Result Hit();
        Game.Result Stand();
    }
}
