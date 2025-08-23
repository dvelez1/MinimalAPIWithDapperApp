using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public DateTime CreateDate { get; set; }   // Course start date
    public DateTime EndDate { get; set; }      // Course end date
}