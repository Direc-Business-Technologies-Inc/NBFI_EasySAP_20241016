using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class ValidateInput
    {
        public static string String(object value)
        {
            return value == null ? "" : DataAccess.replace(value.ToString());
        }
        
        public static double Double(object value)
        {
            return value == null || value.ToString() == string.Empty? 0D : Convert.ToDouble(value);
        }

        public static int Int(object value)
        {
            return value == null ? 0 : Convert.ToInt32(value);
        }

        public static string ConvertToDate(string value, string format)
        {
            string result = string.Empty;

            if (value != string.Empty && value != "")
            {
                var d = Convert.ToDateTime(value);
                result = d.ToString(format);
            }

            return result;
        }
    }
}
