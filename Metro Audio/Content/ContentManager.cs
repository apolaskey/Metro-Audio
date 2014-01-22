using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Experia.Framework.Content
{
    public class ContentManager
    {
        public static String GetFileExtension(String text)
        {
            return RegexHelper.GetMatchedValue(@"\.([A-Za-z0-9]+)$", text);
        }
    }
}
