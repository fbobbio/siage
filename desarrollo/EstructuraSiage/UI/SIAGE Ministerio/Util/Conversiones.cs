using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAGE_Ministerio.Util
{
    public class Conversiones
    {
        public static string ConvertirEnumeracionATextoPresentacion(string item)
        {
            if (!string.IsNullOrEmpty(item) && item.ToCharArray().Length >= 1)
                return item.Substring(0, 1) + item.Substring(1, item.Length - 1).Replace('_', ' ');
            return string.Empty;
        }

        public static string ConvertirTextoPresentacionAEnumeracion(string item)
        {
            return (item.Substring(0, 1) + item.Substring(1, item.Length - 1).Replace(' ', '_')).ToUpper();
        }

        public static string GetEnumClearName(Enum value)
        {
            var functionValueReturn = value.ToString().Replace("_", " ");
            return functionValueReturn;
        }
    }
}