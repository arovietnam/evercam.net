using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NodaTime;

namespace EvercamV1
{
    public class Utility
    {
        /// <summary>
        /// Convet Windows DateTime to equivalent Unix Timestamp
        /// </summary>
        /// <param name="datetime">Windows DateTime</param>
        /// <returns>Unix Timestamp</returns>
        public static long ToUnixTimestamp(DateTime datetime)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            var unixTimestamp = System.Convert.ToInt64((datetime - date).TotalSeconds);

            return unixTimestamp;
        }

        /// <summary>
        /// Convert Unix Timestamp to equivalent Windows DateTime
        /// </summary>
        /// <param name="timestamp">Unix Timestamp</param>
        /// <returns>MS .Net DateTime</returns>
        public static DateTime ToWindowsDateTime(long timestamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

            return dateTime.AddSeconds(timestamp);
        }

        /// <summary>
        /// Converts the Unix timezone to Windows, if matches.
        /// </summary>
        /// <param name="unixTimezone">Unix Timezone ID e.g. Europe/Dublin</param>
        /// <returns>Windows Timezone ID e.g. GMT Standard Time</returns>
        public static string ToWindowsTimezone(string unixTimezone)
        {
            var utcZones = new[] { "Etc/UTC", "Etc/UCT" };
            if (utcZones.Contains(unixTimezone, StringComparer.OrdinalIgnoreCase))
                return "UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

            // resolve any link, since the CLDR doesn't necessarily use canonical IDs
            var links = tzdbSource.CanonicalIdMap
              .Where(x => x.Value.Equals(unixTimezone, StringComparison.OrdinalIgnoreCase))
              .Select(x => x.Key);

            var mappings = tzdbSource.WindowsMapping.MapZones;
            var item = mappings.FirstOrDefault(x => x.TzdbIds.Any(links.Contains));
            if (item == null) return "";
            return item.WindowsId;
        }

        /// <summary>
        /// Converts the Windows timezone to Unix. 
        /// If the primary zone is a link, it then resolves it to the canonical ID.
        /// </summary>
        /// <param name="windowsTtimezone">Windows Timezone ID e.g. GMT Standard Time</param>
        /// <returns>Unix Timezone ID e.g. Europe/Dublin</returns>
        public static string ToUnixTimezone(string windowsTtimezone)
        {
            if (windowsTtimezone.Equals("UTC", StringComparison.OrdinalIgnoreCase))
                return "Etc/UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;
            try
            {
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(windowsTtimezone);
                var tzid = tzdbSource.MapTimeZoneId(tzi);
                return tzdbSource.CanonicalIdMap[tzid];
            }
            catch { return ""; }
        }

        /// <summary>
        /// Decode a base64 image stream to bytes
        /// </summary>
        /// <param name="base64Image">Base 64 image stream</param>
        /// <returns>byte[]</returns>
        public static byte[] ToBytes(string base64Image)
        {
            return System.Convert.FromBase64String(base64Image.Replace("data:image/jpeg;base64,", ""));
        }
    }
}
