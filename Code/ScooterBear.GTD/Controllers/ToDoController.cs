using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ScooterBear.GTD.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        [Route("/todo/main")]
        public async Task<IActionResult> Main()
        {
            //TODO:  Return the current Projects, Labels, and ToDo Items
            throw new NotImplementedException();
        }
    }
}
