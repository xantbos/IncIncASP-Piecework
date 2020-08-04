// IncIncEnumerables.cs
// Last Modified: 2018-11-30
// Modified By:   Adrian Kriz
// 
// Enumerable class, designed to make it more 'dynamic' when
// addressing worker types. Easy expansion and modification.
// Referencing this instead of raw int values just seems like
// a better option.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncIncASP
{
    public class IncIncEnumerables
    {
        public enum WorkerTypes { Regular, Senior, Hourly };
    }
}