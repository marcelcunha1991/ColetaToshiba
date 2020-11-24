using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColetaInjetoraToshiba.util
{
    class UtilsStrings
    {

        public static String adicionaZero(String info, int tamanho)
        {
            int qtd = info.Length;
            for (qtd = info.Length; info.Length < tamanho; qtd++)
            {
                info = "0" + info;
            }
            return info;
        }

        public static string RemoveControlCharacters(string inString)
        {
            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                if (!char.IsControl(ch) || ch.Equals('\x009') || ch.Equals('\x00A') || ch.Equals('\x00D'))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }

    }
}
