using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evercam.V1
{
    enum Status 
    {
        OK = 200,
        Created = 201,
        BadRequest = 400,
        NotFound = 404
    }

    class Common
    {
        public const string API_VENDOR_URL = "http://api.evercam.io/v1/vendors/";
    }
}
