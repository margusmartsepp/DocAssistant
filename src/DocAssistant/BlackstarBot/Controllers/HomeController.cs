using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DocAssistant.Controllers
{
	public class HomeController : Controller
	{
		public Task<IActionResult> Index()
		{
			return Task.FromResult<IActionResult>(View());
		}
	}
}
