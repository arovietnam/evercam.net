﻿using System;
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

        ///// <summary>
        ///// Returns Microsoft Windows Standard Format timezone name against given tz database ID. e.g. (Greenwich Standard Time, Pacific Standard Time, etc.)  
        ///// Queries [http://unicode.org/repos/cldr/trunk/common/supplemental/windowsZones.xml] to get Tz IDs to Timezone mapping.
        ///// </summary>
        ///// <param name="tzId">Tz database ID, e.g. America/Los_Angeles</param>
        ///// <returns></returns>
        //public static string GetTimezoneName(string tzId)
        //{
        //    string timeZone = "";
        //    try
        //    {
        //        XElement xelement = XElement.Load("http://unicode.org/repos/cldr/trunk/common/supplemental/windowsZones.xml");
        //        IEnumerable<XElement> zones = from z in xelement.Descendants("mapZone")
        //                                      where (string)z.Attribute("type") == tzId
        //                                      select z;
        //        timeZone = zones.FirstOrDefault().Attribute("other").Value;
        //    }
        //    catch (Exception x)
        //    {
        //        //throw x;
        //    }
        //    return timeZone;
        //}

        //private string[] unix_timezones = new string[] {"Africa/Abidjan", "Africa/Accra", "Africa/Addis_Ababa",
        //         "Africa/Algiers", "Africa/Asmara", "Africa/Asmera", "Africa/Bamako",
        //         "Africa/Bangui", "Africa/Banjul", "Africa/Bissau", "Africa/Blantyre",
        //         "Africa/Brazzaville", "Africa/Bujumbura", "Africa/Cairo", "Africa/Casablanca",
        //         "Africa/Ceuta", "Africa/Conakry", "Africa/Dakar", "Africa/Dar_es_Salaam",
        //         "Africa/Djibouti", "Africa/Douala", "Africa/El_Aaiun", "Africa/Freetown",
        //         "Africa/Gaborone", "Africa/Harare", "Africa/Johannesburg", "Africa/Kampala",
        //         "Africa/Khartoum", "Africa/Kigali", "Africa/Kinshasa", "Africa/Lagos",
        //         "Africa/Libreville", "Africa/Lome", "Africa/Luanda", "Africa/Lubumbashi",
        //         "Africa/Lusaka", "Africa/Malabo", "Africa/Maputo", "Africa/Maseru",
        //         "Africa/Mbabane", "Africa/Mogadishu", "Africa/Monrovia", "Africa/Nairobi",
        //         "Africa/Ndjamena", "Africa/Niamey", "Africa/Nouakchott", "Africa/Ouagadougou",
        //         "Africa/Porto-Novo", "Africa/Sao_Tome", "Africa/Timbuktu", "Africa/Tripoli",
        //         "Africa/Tunis", "Africa/Windhoek", "America/Adak", "America/Anchorage",
        //         "America/Anguilla", "America/Antigua", "America/Araguaina",
        //         "America/Argentina/Buenos_Aires", "America/Argentina/Catamarca",
        //         "America/Argentina/ComodRivadavia", "America/Argentina/Cordoba",
        //         "America/Argentina/Jujuy", "America/Argentina/La_Rioja",
        //         "America/Argentina/Mendoza", "America/Argentina/Rio_Gallegos",
        //         "America/Argentina/San_Juan", "America/Argentina/Tucuman",
        //         "America/Argentina/Ushuaia", "America/Aruba", "America/Asuncion",
        //         "America/Atikokan", "America/Atka", "America/Bahia", "America/Barbados",
        //         "America/Belem", "America/Belize", "America/Blanc-Sablon",
        //         "America/Boa_Vista", "America/Bogota", "America/Boise",
        //         "America/Buenos_Aires", "America/Cambridge_Bay", "America/Campo_Grande",
        //         "America/Cancun", "America/Caracas", "America/Catamarca", "America/Cayenne",
        //         "America/Cayman", "America/Chicago", "America/Chihuahua",
        //         "America/Coral_Harbour", "America/Cordoba", "America/Costa_Rica",
        //         "America/Cuiaba", "America/Curacao", "America/Danmarkshavn", "America/Dawson",
        //         "America/Dawson_Creek", "America/Denver", "America/Detroit",
        //         "America/Dominica", "America/Edmonton", "America/Eirunepe",
        //         "America/El_Salvador", "America/Ensenada", "America/Fort_Wayne",
        //         "America/Fortaleza", "America/Glace_Bay", "America/Godthab",
        //         "America/Goose_Bay", "America/Grand_Turk", "America/Grenada",
        //         "America/Guadeloupe", "America/Guatemala", "America/Guayaquil",
        //         "America/Guyana", "America/Halifax", "America/Havana", "America/Hermosillo",
        //         "America/Indiana/Indianapolis", "America/Indiana/Knox",
        //         "America/Indiana/Marengo", "America/Indiana/Petersburg",
        //         "America/Indiana/Vevay", "America/Indiana/Vincennes",
        //         "America/Indiana/Winamac", "America/Indianapolis", "America/Inuvik",
        //         "America/Iqaluit", "America/Jamaica", "America/Jujuy", "America/Juneau",
        //         "America/Kentucky/Louisville", "America/Kentucky/Monticello",
        //         "America/Knox_IN", "America/La_Paz", "America/Lima", "America/Los_Angeles",
        //         "America/Louisville", "America/Maceio", "America/Managua", "America/Manaus",
        //         "America/Martinique", "America/Mazatlan", "America/Mendoza",
        //         "America/Menominee", "America/Merida", "America/Mexico_City",
        //         "America/Miquelon", "America/Moncton", "America/Monterrey",
        //         "America/Montevideo", "America/Montreal", "America/Montserrat",
        //         "America/Nassau", "America/New_York", "America/Nipigon", "America/Nome",
        //         "America/Noronha", "America/North_Dakota/Center",
        //         "America/North_Dakota/New_Salem", "America/Panama", "America/Pangnirtung",
        //         "America/Paramaribo", "America/Phoenix", "America/Port-au-Prince",
        //         "America/Port_of_Spain", "America/Porto_Acre", "America/Porto_Velho",
        //         "America/Puerto_Rico", "America/Rainy_River", "America/Rankin_Inlet",
        //         "America/Recife", "America/Regina", "America/Resolute", "America/Rio_Branco",
        //         "America/Rosario", "America/Santiago", "America/Santo_Domingo",
        //         "America/Sao_Paulo", "America/Scoresbysund", "America/Shiprock",
        //         "America/St_Johns", "America/St_Kitts", "America/St_Lucia",
        //         "America/St_Thomas", "America/St_Vincent", "America/Swift_Current",
        //         "America/Tegucigalpa", "America/Thule", "America/Thunder_Bay",
        //         "America/Tijuana", "America/Toronto", "America/Tortola", "America/Vancouver",
        //         "America/Virgin", "America/Whitehorse", "America/Winnipeg", "America/Yakutat",
        //         "America/Yellowknife", "Antarctica/Casey", "Antarctica/Davis",
        //         "Antarctica/DumontDUrville", "Antarctica/Mawson", "Antarctica/McMurdo",
        //         "Antarctica/Palmer", "Antarctica/Rothera", "Antarctica/South_Pole",
        //         "Antarctica/Syowa", "Antarctica/Vostok", "Arctic/Longyearbyen", "Asia/Aden",
        //         "Asia/Almaty", "Asia/Amman", "Asia/Anadyr", "Asia/Aqtau", "Asia/Aqtobe",
        //         "Asia/Ashgabat", "Asia/Ashkhabad", "Asia/Baghdad", "Asia/Bahrain",
        //         "Asia/Baku", "Asia/Bangkok", "Asia/Beirut", "Asia/Bishkek", "Asia/Brunei",
        //         "Asia/Calcutta", "Asia/Choibalsan", "Asia/Chongqing", "Asia/Chungking",
        //         "Asia/Colombo", "Asia/Dacca", "Asia/Damascus", "Asia/Dhaka", "Asia/Dili",
        //         "Asia/Dubai", "Asia/Dushanbe", "Asia/Gaza", "Asia/Harbin", "Asia/Hong_Kong",
        //         "Asia/Hovd", "Asia/Irkutsk", "Asia/Istanbul", "Asia/Jakarta", "Asia/Jayapura",
        //         "Asia/Jerusalem", "Asia/Kabul", "Asia/Kamchatka", "Asia/Karachi",
        //         "Asia/Kashgar", "Asia/Katmandu", "Asia/Krasnoyarsk", "Asia/Kuala_Lumpur",
        //         "Asia/Kuching", "Asia/Kuwait", "Asia/Macao", "Asia/Macau", "Asia/Magadan",
        //         "Asia/Makassar", "Asia/Manila", "Asia/Muscat", "Asia/Nicosia",
        //         "Asia/Novosibirsk", "Asia/Omsk", "Asia/Oral", "Asia/Phnom_Penh",
        //         "Asia/Pontianak", "Asia/Pyongyang", "Asia/Qatar", "Asia/Qyzylorda",
        //         "Asia/Rangoon", "Asia/Riyadh", "Asia/Saigon", "Asia/Sakhalin",
        //         "Asia/Samarkand", "Asia/Seoul", "Asia/Shanghai", "Asia/Singapore",
        //         "Asia/Taipei", "Asia/Tashkent", "Asia/Tbilisi", "Asia/Tehran",
        //         "Asia/Tel_Aviv", "Asia/Thimbu", "Asia/Thimphu", "Asia/Tokyo",
        //         "Asia/Ujung_Pandang", "Asia/Ulaanbaatar", "Asia/Ulan_Bator", "Asia/Urumqi",
        //         "Asia/Vientiane", "Asia/Vladivostok", "Asia/Yakutsk", "Asia/Yekaterinburg",
        //         "Asia/Yerevan", "Atlantic/Azores", "Atlantic/Bermuda", "Atlantic/Canary",
        //         "Atlantic/Cape_Verde", "Atlantic/Faeroe", "Atlantic/Faroe",
        //         "Atlantic/Jan_Mayen", "Atlantic/Madeira", "Atlantic/Reykjavik",
        //         "Atlantic/South_Georgia", "Atlantic/St_Helena", "Atlantic/Stanley",
        //         "Australia/ACT", "Australia/Adelaide", "Australia/Brisbane",
        //         "Australia/Broken_Hill", "Australia/Canberra", "Australia/Currie",
        //         "Australia/Darwin", "Australia/Eucla", "Australia/Hobart", "Australia/LHI",
        //         "Australia/Lindeman", "Australia/Lord_Howe", "Australia/Melbourne",
        //         "Australia/NSW", "Australia/North", "Australia/Perth", "Australia/Queensland",
        //         "Australia/South", "Australia/Sydney", "Australia/Tasmania",
        //         "Australia/Victoria", "Australia/West", "Australia/Yancowinna", "Brazil/Acre",
        //         "Brazil/DeNoronha", "Brazil/East", "Brazil/West", "CET", "CST6CDT",
        //         "Canada/Atlantic", "Canada/Central", "Canada/East-Saskatchewan",
        //         "Canada/Eastern", "Canada/Mountain", "Canada/Newfoundland", "Canada/Pacific",
        //         "Canada/Saskatchewan", "Canada/Yukon", "Chile/Continental",
        //         "Chile/EasterIsland", "Cuba", "EET", "EST", "EST5EDT", "Egypt", "Eire",
        //         "Etc/GMT", "Etc/GMT+0", "Etc/GMT+1", "Etc/GMT+10", "Etc/GMT+11", "Etc/GMT+12",
        //         "Etc/GMT+2", "Etc/GMT+3", "Etc/GMT+4", "Etc/GMT+5", "Etc/GMT+6", "Etc/GMT+7",
        //         "Etc/GMT+8", "Etc/GMT+9", "Etc/GMT-0", "Etc/GMT-1", "Etc/GMT-10",
        //         "Etc/GMT-11", "Etc/GMT-12", "Etc/GMT-13", "Etc/GMT-14", "Etc/GMT-2",
        //         "Etc/GMT-3", "Etc/GMT-4", "Etc/GMT-5", "Etc/GMT-6", "Etc/GMT-7", "Etc/GMT-8",
        //         "Etc/GMT-9", "Etc/GMT0", "Etc/Greenwich", "Etc/UCT", "Etc/UTC",
        //         "Etc/Universal", "Etc/Zulu", "Europe/Amsterdam", "Europe/Andorra",
        //         "Europe/Athens", "Europe/Belfast", "Europe/Belgrade", "Europe/Berlin",
        //         "Europe/Bratislava", "Europe/Brussels", "Europe/Bucharest", "Europe/Budapest",
        //         "Europe/Chisinau", "Europe/Copenhagen", "Europe/Dublin", "Europe/Gibraltar",
        //         "Europe/Guernsey", "Europe/Helsinki", "Europe/Isle_of_Man", "Europe/Istanbul",
        //         "Europe/Jersey", "Europe/Kaliningrad", "Europe/Kiev", "Europe/Lisbon",
        //         "Europe/Ljubljana", "Europe/London", "Europe/Luxembourg", "Europe/Madrid",
        //         "Europe/Malta", "Europe/Mariehamn", "Europe/Minsk", "Europe/Monaco",
        //         "Europe/Moscow", "Europe/Nicosia", "Europe/Oslo", "Europe/Paris",
        //         "Europe/Podgorica", "Europe/Prague", "Europe/Riga", "Europe/Rome",
        //         "Europe/Samara", "Europe/San_Marino", "Europe/Sarajevo", "Europe/Simferopol",
        //         "Europe/Skopje", "Europe/Sofia", "Europe/Stockholm", "Europe/Tallinn",
        //         "Europe/Tirane", "Europe/Tiraspol", "Europe/Uzhgorod", "Europe/Vaduz",
        //         "Europe/Vatican", "Europe/Vienna", "Europe/Vilnius", "Europe/Volgograd",
        //         "Europe/Warsaw", "Europe/Zagreb", "Europe/Zaporozhye", "Europe/Zurich", "GB",
        //         "GB-Eire", "GMT", "GMT+0", "GMT-0", "GMT0", "Greenwich", "HST", "Hongkong",
        //         "Iceland", "Indian/Antananarivo", "Indian/Chagos", "Indian/Christmas",
        //         "Indian/Cocos", "Indian/Comoro", "Indian/Kerguelen", "Indian/Mahe",
        //         "Indian/Maldives", "Indian/Mauritius", "Indian/Mayotte", "Indian/Reunion",
        //         "Iran", "Israel", "Jamaica", "Japan", "Kwajalein", "Libya", "MET", "MST",
        //         "MST7MDT", "Mexico/BajaNorte", "Mexico/BajaSur", "Mexico/General", "NZ",
        //         "NZ-CHAT", "Navajo", "PRC", "PST8PDT", "Pacific/Apia", "Pacific/Auckland",
        //         "Pacific/Chatham", "Pacific/Easter", "Pacific/Efate", "Pacific/Enderbury",
        //         "Pacific/Fakaofo", "Pacific/Fiji", "Pacific/Funafuti", "Pacific/Galapagos",
        //         "Pacific/Gambier", "Pacific/Guadalcanal", "Pacific/Guam", "Pacific/Honolulu",
        //         "Pacific/Johnston", "Pacific/Kiritimati", "Pacific/Kosrae",
        //         "Pacific/Kwajalein", "Pacific/Majuro", "Pacific/Marquesas", "Pacific/Midway",
        //         "Pacific/Nauru", "Pacific/Niue", "Pacific/Norfolk", "Pacific/Noumea",
        //         "Pacific/Pago_Pago", "Pacific/Palau", "Pacific/Pitcairn", "Pacific/Ponape",
        //         "Pacific/Port_Moresby", "Pacific/Rarotonga", "Pacific/Saipan",
        //         "Pacific/Samoa", "Pacific/Tahiti", "Pacific/Tarawa", "Pacific/Tongatapu",
        //         "Pacific/Truk", "Pacific/Wake", "Pacific/Wallis", "Pacific/Yap", "Poland",
        //         "Portugal", "ROC", "ROK", "Singapore", "Turkey", "UCT", "US/Alaska",
        //         "US/Aleutian", "US/Arizona", "US/Central", "US/East-Indiana", "US/Eastern",
        //         "US/Hawaii", "US/Indiana-Starke", "US/Michigan", "US/Mountain", "US/Pacific",
        //         "US/Pacific-New", "US/Samoa", "UTC", "Universal", "W-SU", "WET", "Zulu",
        //         "posixrules"};
    }
}
