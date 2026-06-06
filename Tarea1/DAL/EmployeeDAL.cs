using Microsoft.Data.SqlClient;
using Tarea1.DatabaseHelper;
using Tarea1.Models;

namespace Tarea1.DAL
{
    public class EmployeeDAL
    {
        public List<Employee> GetEmployees(
            string? name,
            int? departmentId,
            string? jobTitle,
            int? shiftId,
            bool onlyActive)
        {
            List<Employee> list = new();

            string sql = @"
                SELECT
                    e.BusinessEntityID,
                    p.FirstName + ' ' + p.LastName AS FullName,
                    d.Name AS DepartmentName,
                    e.JobTitle,
                    s.Name AS ShiftName,
                    CASE WHEN edh.EndDate IS NULL THEN 1 ELSE 0 END AS IsActive
                FROM HumanResources.Employee e
                INNER JOIN Person.Person p ON e.BusinessEntityID = p.BusinessEntityID
                INNER JOIN HumanResources.EmployeeDepartmentHistory edh ON e.BusinessEntityID = edh.BusinessEntityID
                INNER JOIN HumanResources.Department d ON edh.DepartmentID = d.DepartmentID
                INNER JOIN HumanResources.Shift s ON edh.ShiftID = s.ShiftID
                WHERE 1=1
                AND (@Name IS NULL OR (p.FirstName + ' ' + p.LastName) LIKE '%' + @Name + '%')
                AND (@JobTitle IS NULL OR e.JobTitle LIKE '%' + @JobTitle + '%')
                AND (@DepartmentId IS NULL OR d.DepartmentID = @DepartmentId)
                AND (@ShiftId IS NULL OR s.ShiftID = @ShiftId)
                AND (@OnlyActive = 0 OR edh.EndDate IS NULL)";

            using var connection = DatabaseSql.GetConnection();
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            command.Parameters.AddWithValue("@JobTitle", (object?)jobTitle ?? DBNull.Value);
            command.Parameters.AddWithValue("@DepartmentId", (object?)departmentId ?? DBNull.Value);
            command.Parameters.AddWithValue("@ShiftId", (object?)shiftId ?? DBNull.Value);
            command.Parameters.AddWithValue("@OnlyActive", onlyActive ? 1 : 0);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Employee
                {
                    BusinessEntityID = reader.GetInt32(reader.GetOrdinal("BusinessEntityID")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                    JobTitle = reader.GetString(reader.GetOrdinal("JobTitle")),
                    ShiftName = reader.GetString(reader.GetOrdinal("ShiftName")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1
                });
            }

            return list;
        }
    }
}