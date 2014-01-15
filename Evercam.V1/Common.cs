using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evercam.V1
{
    public class Common
    {
        public const int MaxJsonLength = 999999999;

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
            return base64decoded;
        }
    }
}
