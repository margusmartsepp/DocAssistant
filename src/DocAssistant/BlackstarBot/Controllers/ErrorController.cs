using System.Diagnostics;
using DocAssistant.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocAssistant.Controllers
{
	public class ErrorController : Controller
	{
		[Route("Error")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
