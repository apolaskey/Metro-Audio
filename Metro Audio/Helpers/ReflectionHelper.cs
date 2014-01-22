using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Experia.Framework
{
    /// <summary>Responsible for providing assistance in using Reflection in the .NET Environment</summary>
    public static class ReflectionHelper
    {
        public static MethodBase GetCallingMethod()
        {
            StackFrame frame = new StackFrame(2);
            return frame.GetMethod();
        }

        public static Type GetCallingMethodType()
        {
            StackFrame frame = new StackFrame(2);
            var type = frame.GetMethod().DeclaringType;
            return type;
        }

        public static Assembly GetFrameworkAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
        public static Assembly GetGameAssembly()
        {
            return Assembly.GetEntryAssembly();
        }
    }
}
