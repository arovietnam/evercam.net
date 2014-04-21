using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using EvercamV1;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Evercam evercam = new Evercam("api_id", "api_key");
            var cams = evercam.GetCameras("joeyb");
            string s = evercam.GetCredentials("", "");
        }
    }
}
