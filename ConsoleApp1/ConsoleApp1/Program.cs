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
            string weeks = Parse();

            List<string> daysOfWeek1 = MySplit(weeks, new string[] {"</TR>"});

            List<string> lessonOfWeek1 = Clean(daysOfWeek1);

            ShowSchedule(lessonOfWeek1);

            Console.ReadKey();
        }

        static string Parse()
        {
            WebClient webClient = new WebClient();
            string response = webClient.DownloadString("http://www.ulstu.ru/schedule/students/part1/55.htm");
            int start = response.IndexOf("<TABLE");
            int end = response.LastIndexOf("</TABLE>");
            string weeks = response.Substring(start, end - start);
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

        static void ShowSchedule(List<string> lessonOfWeek)
        {
            while (lessonOfWeek.Count > 0)
            {
                List<string> today = new List<string>();
                if (lessonOfWeek.Count > 0)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (lessonOfWeek[0] == "" && i == 0 || i == 0 && lessonOfWeek[0] == null || i == 0 && lessonOfWeek[0] == "_")
                        {
                            lessonOfWeek.Remove(lessonOfWeek[0]);
                            break;
                        } else
                        {
                            today.Add(lessonOfWeek[0]);
                            lessonOfWeek.Remove(lessonOfWeek[0]);
                            Console.WriteLine(i + ") " + today[i]);
                        }
                    }
                }
            }
        }
    }
}
