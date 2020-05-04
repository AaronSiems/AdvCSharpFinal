using BlackjackApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackjackApp.Components
{
    public class HandDisplay : ViewComponent
    {
        public IViewComponentResult Invoke(Hand hand) => View(hand);
    }
}
