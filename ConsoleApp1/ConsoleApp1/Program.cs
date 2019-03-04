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

            List<string> weekList = MySplit(weeks, new string[] {"</TABLE>"});

            string week1 = weekList[0];
            string week2 = weekList[1];

            List<string> daysOfWeek1 = MySplit(week1, new string[] {"</TR>"});
            List<string> daysOfWeek2 = MySplit(week2, new string[] {"</TR>"});

            List<string> lessonOfWeek1 = Clean(daysOfWeek1);
            List<string> lessonOfWeek2 = new List<string>();



            while (lessonOfWeek1.Count > 0)
            {
                List<string> today = new List<string>();
                if (lessonOfWeek1.Count > 0)
                {
                    for (int i=0;i<9;i++)
                    {
                        if(lessonOfWeek1[0]=="" && i==0 || i==0 && lessonOfWeek1[0] == null || i==0 && lessonOfWeek1[0] == "_")
                        {
                            lessonOfWeek1.Remove(lessonOfWeek1[0]);
                            break;
                        } else
                        {
                            today.Add(lessonOfWeek1[0]);
                            lessonOfWeek1.Remove(lessonOfWeek1[0]);
                            Console.WriteLine(i + ") " + today[i]);
                        }
                    
                    }
                }
                
                //foreach(var les in today)
                //{
                    
                //}
                //Console.WriteLine("\n new");
            }

            


            Console.ReadKey();
        }

        static string Parse()
        {
            WebClient webClient = new WebClient();
            string response = webClient.DownloadString("http://www.ulstu.ru/schedule/students/part1/55.htm");
            int start = response.IndexOf("<TABLE");
            int end = response.LastIndexOf("</TABLE>");
            string weeks = response.Substring(start, end+1 - start);
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
                        }
                        else
                            skobka = false;
                        
                    }
                    lessonOfWeek.Add(newLes.Trim());
                }
            }

            return lessonOfWeek;
        }
    }
}
