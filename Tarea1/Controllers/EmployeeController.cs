using Microsoft.AspNetCore.Mvc;
using Tarea1.DAL;

namespace Tarea1.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index(string? name, string? department, string? jobTitle, string? shift, bool onlyActive = false)
        {
            EmployeeDAL dal = new EmployeeDAL();

            var allEmployees = dal.GetEmployees(null, null, null, null, false);

            ViewBag.Departments = allEmployees.Select(e => e.DepartmentName).Distinct().OrderBy(d => d).ToList();
            ViewBag.Shifts = allEmployees.Select(e => e.ShiftName).Distinct().OrderBy(s => s).ToList();

                  ViewBag.Name = name;
            ViewBag.Department = department;
            ViewBag.JobTitle = jobTitle;
            ViewBag.Shift = shift;

            var employees = dal.GetEmployees(name, null, jobTitle, null, onlyActive);

            if (!string.IsNullOrEmpty(department))
                employees = employees.Where(e => e.DepartmentName == department).ToList();

            if (!string.IsNullOrEmpty(shift))
                employees = employees.Where(e => e.ShiftName == shift).ToList();

            return View(employees);
        }
    }
}