using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EvercamV1
{
    public class Utils
    {
        /// <summary>
        /// Returns Microsoft Windows Standard Format timezone name against given tz database ID. e.g. (Greenwich Standard Time, Pacific Standard Time, etc.)  
        /// Queries [http://unicode.org/repos/cldr/trunk/common/supplemental/windowsZones.xml] to get Tz IDs to Timezone mapping.
        /// </summary>
        /// <param name="tzId">Tz database ID, e.g. America/Los_Angeles</param>
        /// <returns></returns>
        public static string GetTimezoneName(string tzId)
        {
            string timeZone = "";
            try
            {
                XElement xelement = XElement.Load("http://unicode.org/repos/cldr/trunk/common/supplemental/windowsZones.xml");
                IEnumerable<XElement> zones = from z in xelement.Descendants("mapZone")
                                              where (string)z.Attribute("type") == tzId
                                              select z;
                timeZone = zones.FirstOrDefault().Attribute("other").Value;
            }
            catch (Exception x)
            {
                //throw x;
            }
            return timeZone;
        }
    }
}
