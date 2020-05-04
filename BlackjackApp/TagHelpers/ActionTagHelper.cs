using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlackjackApp.TagHelpers
{
    public class ActionTagHelper : TagHelper
    {
        private LinkGenerator linkGen;
        public ActionTagHelper(LinkGenerator l) => linkGen = l;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext VC { get; set; }

        public string action { get; set; }
        public bool isDisabled { get; set; }


        //Creates form for buttons
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;

            string control = VC.RouteData.Values["controller"].ToString();
            string url = linkGen.GetPathByAction(action, control);

            output.Attributes.SetAttribute("action", url);
            output.Attributes.SetAttribute("method", "post");
            output.Attributes.SetAttribute("class", "col"); //Style

            TagBuilder button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.Attributes.Add("class", "btn btn-primary"); //Style
            button.InnerHtml.Append(action);

            if (isDisabled) { button.Attributes.Add("disabled", "disabled"); } //Disable if needed

            output.Content.AppendHtml(button);
        }
    }
}
