using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlackjackApp.Models;

namespace BlackjackApp.Controllers
{
    public class HomeController : Controller
    {
        private GameInterface game { get; set; }
        public HomeController(GameInterface gi) => game = gi;

        public ViewResult Index() => View(game);



        //Deal was ran - return if any blackjacks occured or shuffling needed.
        public RedirectToActionResult Deal()
        {
            var result = game.Deal();

            if(result == Game.Result.Shuffling)
            {
                TempData["msg"] = "Shuffling the deck, please press deal to continue.";
            } 
            else if(result == Game.Result.PlayerBlackJack)
            {
                TempData["msg"] = "You got a blackjack! You win!";
            }
            else if (result == Game.Result.DealerBlackJack)
            {
                TempData["msg"] = "Dealer got a blackjack, you lose.";
            }
            else if (result == Game.Result.DoubleBlackJack)
            {
                TempData["msg"] = "";
            }

            return RedirectToAction("Index");
        }

        //Hit was run - return if shuffling was needed or if player bust
        public RedirectToActionResult Hit()
        {
            var result = game.Hit();

            if(result == Game.Result.Shuffling)
            {
                TempData["msg"] = "Shuffling the deck, please press hit to continue.";
            }
            else if (result == Game.Result.PlayerBust)
            {
                TempData["msg"] = "You went over and lost.";
            }

            return RedirectToAction("Index");
        }

        //Stand was run - return if shuffling was needed or the winner.
        public RedirectToActionResult Stand()
        {
            var result = game.Stand();

            if (result == Game.Result.Shuffling)
            {
                TempData["msg"] = "Shuffling the deck, please press hit to continue.";
            }
            else if (result == Game.Result.Continue)
            {
                //TODO
            }
            else if (result == Game.Result.DealerBust)
            {
                TempData["msg"] = "Dealer went over, you win!";
            }
            else if (result == Game.Result.DealerWin)
            {
                TempData["msg"] = "You lost.";
            }
            else if (result == Game.Result.PlayerWin)
            {
                TempData["msg"] = "You won!";
            }
            else if (result == Game.Result.Push)
            {
                TempData["msg"] = "";
            }

            return RedirectToAction("Index");
        }

    }
}
