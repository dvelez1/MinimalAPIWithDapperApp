using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }           // Last name
    public string Firstname { get; set; }      // First name
    public DateTime CreateDate { get; set; }   // Enrollment date
    public DateTime EndDate { get; set; }      // Graduation date
}