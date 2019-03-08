using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> weeks = Parse();

            Week week1 = new Week();
            Week week2 = new Week();

            List<string> daysOfWeek1 = MySplit(weeks[0], new string[] {"</TR>"});
            List<string> daysOfWeek2 = MySplit(weeks[1], new string[] { "</TR>" });

            List<string> lessonOfWeek1 = Clean(daysOfWeek1);
            List<string> lessonOfWeek2 = Clean(daysOfWeek2);

            week1 = FillWeek(week1, lessonOfWeek1);
            week2 = FillWeek(week2, lessonOfWeek2);

            ShowSchedule(week1);
            ShowSchedule(week2);

            Console.ReadKey();
        }

        static Week FillWeek(Week week, List<string> lessonOfWeek)
        {
            week.Monday = FillSchedule( lessonOfWeek, "Пнд");
            week.Tuesday = FillSchedule( lessonOfWeek, "Втр");
            week.Wednesday = FillSchedule( lessonOfWeek, "Срд");
            week.Thurstday = FillSchedule( lessonOfWeek, "Чтв");
            week.Friday = FillSchedule( lessonOfWeek, "Птн");
            week.Saturday = FillSchedule( lessonOfWeek, "Сбт");
            return week;
        }

        static void ShowSchedule(Week week)
        {
            ShowDay(week.Monday);
            ShowDay(week.Tuesday);
            ShowDay(week.Wednesday);
            ShowDay(week.Thurstday);
            ShowDay(week.Friday);
            ShowDay(week.Saturday);
        }

        static void ShowDay(List<string> day)
        {
            foreach (var item in day)
            {
                Console.WriteLine(item);
            }
        }

        static List<string> Parse()
        {
            List<string> weeks = new List<string>();

            WebClient webClient = new WebClient();
            string response = webClient.DownloadString("http://www.ulstu.ru/schedule/students/part1/55.htm");
            int start = response.IndexOf("<TABLE");
            int end = response.IndexOf("</TABLE>");
            string week1 = response.Substring(start, end - start);
            weeks.Add(week1);
            response = response.Remove(start, end+1 - start);
            start = response.IndexOf("<TABLE");
            end = response.IndexOf("</TABLE>");
            string week2 = response.Substring(start, end - start);
            weeks.Add(week2);

            return weeks;
        }

        static List<string> MySplit(string week, string[] separator)
        {
            string[] days = week.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            List<string> daysOfWeek = days.ToList();
            return daysOfWeek;
        }

        static List<string> Clean(List<string> daysOfWeek)
        {
            List<string> lessonOfWeek = new List<string>();
            foreach (var day in daysOfWeek)
            {
                List<string> lesson = MySplit(day, new string[] {"</TD>"});

                foreach(var les in lesson)
                {
                    string newLes = les;
                    bool skobka = true;
                    while (skobka)
                    {
                        int start = newLes.IndexOf("<");
                        if(start>=0)
                        {
                            int end = newLes.IndexOf(">", start);
                            if (end > 0)
                            {
                                newLes = newLes.Remove(start, end+1 - start);
                            } 
                        } else
                        {
                            skobka = false;
                        }
                    }
                    lessonOfWeek.Add(newLes.Trim());
                }
            }

            return lessonOfWeek;
        }

        static List<string> FillSchedule(List<string> lessonOfWeek, string day)
        {
            List<string> today = new List<string>();
            for(int i = 0; i < lessonOfWeek.Count; i++)
            {
                if (lessonOfWeek[i].Contains(day))
                {
                    for (int j = 0; j < 9; j++)
                    {
                        today.Add(j + ") " + lessonOfWeek[i]);
                        i++;
                    }
                }
            }
            return today;
        }
    }
}
