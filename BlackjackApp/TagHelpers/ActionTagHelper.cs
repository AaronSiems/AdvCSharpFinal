using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlackjackApp.TagHelpers
{
    public class ActionTagHelper : TagHelper
    {
        private readonly LinkGenerator linkGen;
        public ActionTagHelper(LinkGenerator l) => linkGen = l;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext VC { get; set; }

        public string Action { get; set; }
        public bool IsDisabled { get; set; }


        //Creates form for buttons
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;

            string control = VC.RouteData.Values["controller"].ToString();
            string url = linkGen.GetPathByAction(Action, control);

            output.Attributes.SetAttribute("action", url);
            output.Attributes.SetAttribute("method", "post");
            output.Attributes.SetAttribute("class", "col"); //Style

            TagBuilder button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.Attributes.Add("class", "btn btn-primary"); //Style
            button.InnerHtml.Append(Action);

            if (IsDisabled) { button.Attributes.Add("disabled", "disabled"); } //Disable if needed

            output.Content.AppendHtml(button);
        }
    }
}
