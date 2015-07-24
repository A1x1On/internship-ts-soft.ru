using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TaskManager.Realizations
{
    public class Crypt
    {
        Dictionary<char, int> alph = new Dictionary<char, int>();
        Dictionary<int, char> alph_r = new Dictionary<int, char>();
        public Crypt(IEnumerable<char> Alphabet)
        {
            int i = 0;
            foreach (char c in Alphabet)
            {
                alph.Add(c, i);
                alph_r.Add(i++, c);
            }
        }
        public string CryptDecrypt(string Text, string Key, bool Crypt)
        {
            char[] key = Key.ToCharArray();
            char[] text = Text.ToCharArray();
            var sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                int idx;
                if (alph.TryGetValue(text[i], out idx))
                {
                    int r = alph.Count + idx;
                    r += (Crypt ? 1 : -1) * alph[key[i % key.Length]];
                    sb.Append(alph_r[r % alph.Count]);
                }
            }
            return sb.ToString();
        }
    }
}