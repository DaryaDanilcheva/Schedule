using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Week
    {
        private List<string> monday;
        private List<string> tuesday;
        private List<string> wednesday;
        private List<string> thurstday;
        private List<string> friday;
        private List<string> saturday;

        public List<string> Monday
        {
            get
            {
                return monday;
            }
            set
            {
                monday = value;
            }
        }

        public List<string> Tuesday
        {
            get
            {
                return tuesday;
            }
            set
            {
                tuesday = value;
            }
        }

        public List<string> Wednesday
        {
            get
            {
                return wednesday;
            }
            set
            {
                wednesday = value;
            }
        }

        public List<string> Thurstday
        {
            get
            {
                return thurstday;
            }
            set
            {
                thurstday = value;
            }
        }

        public List<string> Friday
        {
            get
            {
                return friday;
            }
            set
            {
                friday = value;
            }
        }

        public List<string> Saturday
        {
            get
            {
                return saturday;
            }
            set
            {
                saturday = value;
            }
        }
    }
}
