using Tarea1.DatabaseHelper;
using System.Data;

namespace Tarea1.Models
{
    public class Employee
    {
        public int BusinessEntityID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public static class EmployeeService
    {
        public static List<Employee> getAll()
        {
            List<Employee> list = new();

            DataTable dt = DatabaseSql.executeStoredProcedure("[dbo].[uspGetEmployees]");

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Employee employee = new Employee();

                    employee.BusinessEntityID = (int)dr["BusinessEntityID"];
                    employee.FullName = dr["FullName"].ToString() ?? string.Empty;
                    employee.DepartmentName = dr["DepartmentName"].ToString() ?? string.Empty;
                    employee.JobTitle = dr["JobTitle"].ToString() ?? string.Empty;
                    employee.ShiftName = dr["ShiftName"].ToString() ?? string.Empty;
                    employee.IsActive = dr["IsActive"] != DBNull.Value && (int)dr["IsActive"] == 1; list.Add(employee);
                }
            }

            return list;
        }
    }
}