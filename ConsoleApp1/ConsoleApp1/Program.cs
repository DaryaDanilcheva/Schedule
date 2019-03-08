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
            string classesTable = ParseClasses();
            List<string> classes = MySplit(classesTable, new string[] { "</TD>" });

            List<string> groups = FillGroups(classes);
            List<string> hrefs = FillHref(classes);

            Console.WriteLine(groups[5] +" "+hrefs[5]);

            List<string> weeks = Parse(hrefs[5]);

            Week week1 = new Week();
            Week week2 = new Week();

            List<string> daysOfWeek1 = MySplit(weeks[0], new string[] {"</TR>"});
            List<string> daysOfWeek2 = MySplit(weeks[1], new string[] { "</TR>" });

            List<string> lessonOfWeek1 = Clean(daysOfWeek1);
            List<string> lessonOfWeek2 = Clean(daysOfWeek2);

            week1 = FillWeek(week1, lessonOfWeek1);
            week2 = FillWeek(week2, lessonOfWeek2);

            Console.WriteLine("Week1");
            ShowSchedule(week1);
            Console.WriteLine("Week2");
            ShowSchedule(week2);

            Console.ReadKey();
        }

        static List<string> FillHref(List<string> classes)
        {
            List<string> hrefs = new List<string>();
            for (int i = 0; i < classes.Count(); i++)
            {
                if (classes[i].Contains("HREF"))
                {
                    int start = classes[i].IndexOf("<A HREF=") + 9;
                    int end = classes[i].IndexOf('>', start) - 1;
                    string href = classes[i].Substring(start, end - start);
                    hrefs.Add(href);
                }
            }
            return hrefs;
        }

        static List<string> FillGroups(List<string> classes)
        {
            List<string> groups = new List<string>();
            for (int i = 0; i < classes.Count(); i++)
            {
                if (classes[i].Contains("HREF"))
                {
                    string group = classes[i];
                    bool skobka = true;
                    while (skobka)
                    {
                        int start1 = group.IndexOf("<");
                        if (start1 >= 0)
                        {
                            int end1 = group.IndexOf(">", start1);
                            if (end1 > 0)
                            {
                                group = group.Remove(start1, end1 + 1 - start1);
                            }
                        } else
                        {
                            skobka = false;
                        }
                    }
                    groups.Add(group.Trim());
                }
            }
            return groups;
        }

        static string ParseClasses()
        {
            WebClient webClient = new WebClient();
            string response = webClient.DownloadString("http://www.ulstu.ru/schedule/students/part1/raspisan.htm");
            int start = response.IndexOf("<TABLE");
            int end = response.IndexOf("</TABLE>");
            string classesTable = response.Substring(start, end - start);
            return classesTable;
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

        static List<string> Parse(string href)
        {
            List<string> weeks = new List<string>();

            WebClient webClient = new WebClient();
            string response = webClient.DownloadString("http://www.ulstu.ru/schedule/students/part1/"+href);
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
