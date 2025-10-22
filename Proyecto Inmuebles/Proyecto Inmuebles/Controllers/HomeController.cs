using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Filters;
using Proyecto_Inmuebles.Models;

namespace Proyecto_Inmuebles.Controllers
{
    //[RequireUserType(2)]
    [Authorize] 
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            OracleDBConnection con = new OracleDBConnection();

           var data = await con.SelectAsync("SELECT * FROM TIPOUSUARIO");

            foreach (var r in data)
            {
                System.Diagnostics.Debug.WriteLine($"#{r["IDTIPOUSUARIO"]}: {r["NOMBRETIPO"]}  {r["ELIMINADO"]}");
            }
            return View();
        }
    }
}


