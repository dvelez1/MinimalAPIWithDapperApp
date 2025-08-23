using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class StoredProcedureInfo
{
    public string ProcedureName { get; set; }            // procedure_name
    public string Definition { get; set; }               // procedure SQL code
}