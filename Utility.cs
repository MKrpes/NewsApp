using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace NewsApp
{
    public class Utility
    {
        public static string Hash(string tekst)
        {
            //SHA hashing algorithm 
            SHA256Managed algoritam = new SHA256Managed();
            //Get bytes from string
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(tekst);
            //Execute hashing
            byte[] resultBytes = algoritam.ComputeHash(bytes);
            //return as string
            return Convert.ToBase64String(resultBytes);

        }
    }
}