using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.TagHelpers
{
    [HtmlTargetElement("info-message")]
    public class MessageTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext VC {get; set;}

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var td = VC.TempData;
            if (td.ContainsKey("msg"))
            {
                output.BuildTag("h4", "text-center");
                output.Content.SetContent(td["msg"].ToString());
            } else
            {
                output.SuppressOutput();
            }
        }
    }
}
