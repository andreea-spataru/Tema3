using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tema3
{
    public class Movie
    {
        public string Genre {  get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public string Country { get; set; }
        public string Actor { get; set; }
        public double Rating { get; set; }
        public string Characteristics { get; set; }
        public int Budget { get; set; }
        
        public Movie(XElement element) 
        {
            Genre = (string)element.Attribute("genre");
            Title = (string)element.Attribute("title");
            Year = (int)element.Attribute("year");
            Director = (string)element.Attribute("director");
            Country = (string)element.Attribute("country");
            Actor = (string)element.Attribute("actor");
            Rating = (double)element.Attribute("rating");
            Characteristics = (string)element.Attribute("characteristics"); 
            Budget = (int)element.Attribute("budget");
        }
    }
}
