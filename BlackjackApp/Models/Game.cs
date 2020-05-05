using Microsoft.AspNetCore.Http;
using System;

namespace BlackjackApp.Models
{
    public class Game : GameInterface
    {
        private ISession Session { get; set; }
        public Deck Deck { get; set; }
        public Player player { get; set; }
        public Dealer dealer { get; set; }
        public bool NeedsDeal { get; set; }
        public enum Result
        {
            Shuffling, Continue, DealerBust, DealerBlackJack, DealerWin,
            PlayerBust, PlayerBlackJack, PlayerWin, DoubleBlackJack, Push
        }


        public Game(IHttpContextAccessor accessor)
        {
            //Get our session data or create new ones if not exist. Default NeedsDeal to true
            Session = accessor.HttpContext.Session;
            Deck = Session.GetObject<Deck>("Deck") ?? new Deck();
            player = Session.GetObject<Player>("Player") ?? new Player();
            dealer = Session.GetObject<Dealer>("Dealer") ?? new Dealer();
            NeedsDeal = Convert.ToBoolean(Session.GetInt32("needsdeal") ?? 1);
            
        }

        public Result Deal()
        {
            var result = Result.Continue;

            if(Deck.cards == null) { Deck.NewDeck(); } //New game - create new deck

            //If less than 4 cards left we will make new deck otherwise deal the cards
            if(Deck.cards.Count < 4)
            {
                Deck.NewDeck();
                dealer.ShowCard();
                result = Result.Shuffling;
            } else
            {
                player.NewHand(Deck.Deal(), Deck.Deal());
                dealer.NewHand(Deck.Deal(), Deck.Deal());
                dealer.hand.HiddenCard = true;
                NeedsDeal = false;
            }

            //Check for instant wins
            if(player.hand.HasBlackJack && dealer.hand.HasBlackJack)
            {
                Update();
                dealer.ShowCard();
                result = Result.DoubleBlackJack;
            } else if (player.hand.HasBlackJack)
            {
                Update();
                dealer.ShowCard();
                result = Result.PlayerBlackJack;
            } else if (dealer.hand.HasBlackJack)
            {
                Update();
                dealer.ShowCard();
                result = Result.DealerBlackJack;
            }


            Save();
            return result;
        }

        public Result Hit()
        {
            //Attempt to hit player, if deck empty return shuffle else check if hit busts player and return busted
            //Default action = continue
            var result = Result.Continue;
            bool shuffle = HitPlayer();

            if (shuffle)
            {
                Deck.NewDeck();
                result = Result.Shuffling;
            } else if(player.hand.IsBusted) {
                Update();
                dealer.ShowCard();
                result = Result.PlayerBust;
            }
            Save();
            return result;
        }

        public Result Stand()
        {
            dealer.ShowCard();
            var result = Result.Continue;

            //Hit the dealer
            if (dealer.ShouldHit)
            {
                bool shuffle = HitDealer();
                if (shuffle)
                {
                    Deck.NewDeck();
                    result = Result.Shuffling;
                }
                else if (dealer.hand.IsBusted)
                {
                    Update();
                    Save();
                    result = Result.DealerBust;
                }
            }

            //If he needs another hit we will just continue
            if (dealer.ShouldHit)
            {

            } else if (IsDealerHandHigher) //Dealer doesn't need hit so decide who won
            {
                Update();
                result = Result.DealerWin;
            } else if (IsPlayerHandHigher)
            {
                Update();
                result = Result.PlayerWin;
            } else if (IsPush)
            {
                Update();
                result = Result.Push;
            }

            Save();
            return result;
        }

        private bool IsDealerHandHigher => player.hand.Value < dealer.hand.Value;
        private bool IsPlayerHandHigher => player.hand.Value > dealer.hand.Value;
        private bool IsPush => player.hand.Value == dealer.hand.Value;
        private double Bet => 25; //Config values?
        private double Multiplier => 2; //I don't know what common blackjack bets are so just gonna do double

        //Hit player and dealer check for empty deck and if not hit respected party
        //Return true if shuffle is needed
        private bool HitPlayer()
        {
            bool needsShuffle = false;
            if(Deck.cards.Count == 0)
            {
                needsShuffle = true;
            } else
            {
                player.Hit(Deck.Deal());
            }
            return needsShuffle;
        }
        private bool HitDealer()
        {
            bool needsShuffle = false;
            if (Deck.cards.Count == 0)
            {
                needsShuffle = true;
            }
            else
            {
                dealer.Hit(Deck.Deal());
            }
            return needsShuffle;
        }
        private void Update()
        {
            //Check for a win or loss
            if (dealer.hand.IsBusted)
            {
                player.winnings += (Bet * Multiplier) / 2; //I can't figure out why dealer busting makes Update call twice but
                                                           //it does so the player win condition is split into two since player
                                                           //hand being higher doesnt call Update twice
            } else if (IsPlayerHandHigher && !player.hand.IsBusted)
            {
                player.winnings += (Bet * Multiplier);
            } else if(player.hand.IsBusted || (IsDealerHandHigher && !dealer.hand.IsBusted))
            {
                player.winnings -= Bet;
            }
            //Push needs no logic since we dont remove the bet unless they lose.
            NeedsDeal = true; //If conditions are met we will make a new game otherwise, new deal
        }

        private void Save()
        {
            Session.SetObject<Deck>("Deck", Deck);
            Session.SetObject<Player>("Player", player);
            Session.SetObject<Dealer>("Dealer", dealer);
            Session.SetInt32("needsdeal", Convert.ToInt32(NeedsDeal));
        }
    }
}
