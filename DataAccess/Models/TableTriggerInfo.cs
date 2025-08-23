using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class TableTriggerInfo
{
    public string TriggerName { get; set; }              // trigger_name
    public bool IsDisabled { get; set; }                 // is_disabled
    public string Definition { get; set; }               // trigger SQL code
}