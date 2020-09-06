using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NetCore.Core.Extensions
{
    public static class DateTimeExtensions
    {

        public static string ToString2(this DateTime dt)
        {
            return (dt == new DateTime() ? string.Empty : dt.ToString("HH:mm"));
        }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static string ToString3(this DateTime dt)
        {
            return (dt == new DateTime() ? string.Empty : dt.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        public static string ToString5(this DateTime dt)
        {
            return (dt == new DateTime() ? string.Empty : dt.ToString("yyyy-MM-dd HH:mm"));
        }
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string ToString6(this DateTime dt)
        {
            return (dt == new DateTime() ? string.Empty : dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss:fff
        /// </summary>
        public static string ToString7(this DateTime dt)
        {
            return (dt == new DateTime() ? string.Empty : dt.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public static bool IsNull(this DateTime? dt)
        {
            var flag = true;
            if (dt != null)
            {
                flag = false;
            }
            return flag;
        }

        public static string GetChineseDate(this DateTime date)
        {
            var cnDate = new ChineseLunisolarCalendar();
            string[] arrMonth = { "", "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "冬月", "腊月" };
            string[] arrDay = { "", "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" };
            string[] arrYear = { "", "甲子", "乙丑", "丙寅", "丁卯", "戊辰", "己巳", "庚午", "辛未", "壬申", "癸酉", "甲戌", "乙亥", "丙子", "丁丑", "戊寅", "己卯", "庚辰", "辛己", "壬午", "癸未", "甲申", "乙酉", "丙戌", "丁亥", "戊子", "己丑", "庚寅", "辛卯", "壬辰", "癸巳", "甲午", "乙未", "丙申", "丁酉", "戊戌", "己亥", "庚子", "辛丑", "壬寅", "癸丑", "甲辰", "乙巳", "丙午", "丁未", "戊申", "己酉", "庚戌", "辛亥", "壬子", "癸丑", "甲寅", "乙卯", "丙辰", "丁巳", "戊午", "己未", "庚申", "辛酉", "壬戌", "癸亥" };

            var lYear = cnDate.GetYear(date);
            var sYear = arrYear[cnDate.GetSexagenaryYear(date)];
            var lMonth = cnDate.GetMonth(date);
            var lDay = cnDate.GetDayOfMonth(date);

            //获取第几个月是闰月,等于0表示本年无闰月
            var leapMonth = cnDate.GetLeapMonth(lYear);
            var sMonth = arrMonth[lMonth];
            //如果今年有闰月   
            if (leapMonth > 0)
            {
                //闰月数等于当前月份   
                sMonth = lMonth == leapMonth ? string.Format("闰{0}", arrMonth[lMonth - 1]) : sMonth;
                sMonth = lMonth > leapMonth ? arrMonth[lMonth - 1] : sMonth;
            }
            return string.Format("{0}年{1}{2}", sYear, sMonth, arrDay[lDay]);
        }

        public static int DateDiff(this DateTime d1, DateTime d2, string flag)
        {
            var iR = 0;
            switch (flag.ToLower())
            {
                case "year":
                    iR = d2.Year - d1.Year;
                    break;
                case "month":
                    iR = (d2.Year - d1.Year) * 12 + (d2.Month - d1.Month);
                    break;
                case "day":
                    iR = (int)(d2 - d1).TotalDays;
                    break;
                case "hour":
                    iR = (int)(d2 - d1).TotalHours;
                    break;
                case "minute":
                    iR = (int)(d2 - d1).TotalMinutes;
                    break;
                case "second":
                    iR = (int)(d2 - d1).TotalSeconds;
                    break;
            }
            return iR;
        }

        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime ? DateTimeOffset.MinValue
                  : new DateTimeOffset(dateTime);
        }
    }
}
