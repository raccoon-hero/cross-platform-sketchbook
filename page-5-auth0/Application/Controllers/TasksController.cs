using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KPWebApp5.Controllers
{
    public class TasksController : Controller
    {
        //TASK1

        [Authorize]
        public IActionResult Task1()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public void Task1(string input_file, string output_file)
        {
            KPCore5.KPCore5.Lab1StartProcess(input_file, output_file);
        }

        //TASK2

        [Authorize]
        public IActionResult Task2()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public void Task2(string input_file, string output_file)
        {
            KPCore5.KPCore5.Lab2StartProcess(input_file, output_file);
        }

        //TASK3

        [Authorize]
        public IActionResult Task3()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public void Task3(string input_file, string output_file)
        {
            KPCore5.KPCore5.Lab3StartProcess(input_file, output_file);
        }

    }
}
