using BlackjackApp.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlackjackApp.TagHelpers
{
    [HtmlTargetElement("h3", Attributes = "my-player")]
    [HtmlTargetElement("h3", Attributes = "my-dealer")]
    public class PlayerHeadingTagHelper : TagHelper
    {
        public Dealer MyDealer { get; set; }
        public Player MyPlayer { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string text = "";

            if(MyDealer != null)
            {
                text += (MyDealer.ShowCards) ? $"Dealer: {MyDealer.hand.Value}" : "Dealer";
            }
            //else{ text += "Error: Dealer is null"; }
            if (MyPlayer != null)
            {
                text += (MyPlayer.hand.HasCards) ? $"Player: {MyPlayer.hand.Value}" : "Player";
            } 
            //else{ text += "Error: Player is null"; }

            output.Content.Append(text);
        }
    }
}
