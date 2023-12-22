using System.ComponentModel.DataAnnotations;
using VbApi.Controllers;

namespace VbApi;

public class Employee2
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public double HourlySalary { get; set; }
}