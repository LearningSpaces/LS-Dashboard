using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class WEPAModel
    {
        public int PSNumber { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string PrinterText { get; set; }
        public PrintColors Toner { get; set; }
        public PrintColors Drum { get; set; }
        public int Belt { get; set; }
        public int Fuser { get; set; }
        public Filters Filter { get; set; }

        public enum Filters : byte
        {
            RED=1,
            YELLOW=2,
            GREEN=4
        }

        public class PrintColors
        {
            public int Black { get; set; }
            public int Cyan { get; set; }
            public int Magenta { get; set; }
            public int Yellow { get; set; }
        }
    }
}