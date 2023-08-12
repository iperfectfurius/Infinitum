using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.UI
{
    internal class ColorText
    {
        public const string Pink = "c/ff1493:"; 
        public const string Blue = "c/0070FF:";

        public static string SetTextColor(string text, string color)
        {
            if (text == null || color == "") return "";

            return $"[{color}{text}]";
        }

        public static string SetTextColor(int? text, string color)
        {
            if (text == null || color == "") return "";

            return $"[{color}{text}]";
        }
    }
}
