using BlackjackApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Components
{
    public class WinningsDisplay : ViewComponent
    {
        private GameInterface game { get; set; }
        public WinningsDisplay(GameInterface g)
        {
            game = g;
        }

        public IViewComponentResult Invoke() => View(game.player.winnings);
    }
}
