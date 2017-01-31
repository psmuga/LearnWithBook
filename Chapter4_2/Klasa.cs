using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4_2
{
    /// <summary>
    /// Klasa do sstworzenia metody rozszerzajacej
    /// </summary>
    /// <remarks>
    /// Dodatkowy opis
    /// </remarks>
    public static class Klasa
    {
        //metoda rozszerzajacqa musi być static w kalsie statycznej

        /// <param name="s"> łancuch znakowy</param>
        /// <returns>bool - wartosc zwrotna metody</returns>
        /// <seealso cref="www.google.pl"/>
    public static bool IsCapitalized(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            return char.IsUpper(s[0]);
        }
    }
}
