using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class WEPAModel
    {
        public string PSNumber { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string PrinterText { get; set; }
        public PrintColors Toner { get; set; }
        public PrintColors Drum { get; set; }
        public byte Belt { get; set; }
        public byte Fuser { get; set; }
        public Filters Filter { get; set; }

        public enum Filters : byte
        {
            RED=1,
            YELLOW=2,
            GREEN=4
        }

        public class PrintColors
        {
            public byte Black { get; set; }
            public byte Cyan { get; set; }
            public byte Magenta { get; set; }
            public byte Yellow { get; set; }
        }
    }
}