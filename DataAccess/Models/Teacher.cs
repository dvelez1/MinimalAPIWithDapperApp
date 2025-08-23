using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class Teacher
{
    public int TeacherId { get; set; }
    public string Name { get; set; }           // Last name
    public string Firstname { get; set; }      // First name
    public DateTime CreateDate { get; set; }   // Hire date
    public DateTime EndDate { get; set; }      // Contract end date
}
