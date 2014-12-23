using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneNetCzechSupport.Tests.Extensions
{
    public static class StringExtensions
    {
        public static StreamReader AsStreamReader(this string s)
        {
            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(s)), Encoding.UTF8);
        }
    }
}
