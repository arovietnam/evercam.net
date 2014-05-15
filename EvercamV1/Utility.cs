using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EvercamV1
{
    public class Utility
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

        /// <summary>
        /// Convet MS DateTime to equivalent Unix Timestamp
        /// </summary>
        /// <param name="datetime">MS .Net DateTime</param>
        /// <returns>Unix Timestamp</returns>
        public static long ToUnixTimestamp(DateTime datetime)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            var unixTimestamp = System.Convert.ToInt64((datetime - date).TotalSeconds);

            return unixTimestamp;
        }

        /// <summary>
        /// Convert Unix Timestamp to equivalent MS DateTime
        /// </summary>
        /// <param name="timestamp">Unix Timestamp</param>
        /// <returns>MS .Net DateTime</returns>
        public static DateTime ToMsDateTime(long timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

            return dateTime.AddSeconds(timestamp);
        }
    }
}
