using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anses.Director.Session;

namespace MedioUnicoDePago.Ayudante
{
    public static class DirectorHelper
    {
        public static string[] GetTokenListaGroups(SSOToken token)
        {
            var Grupo = new string[token.Operation.Login.Groups.Length];
            var i = 0;
            foreach (var grupo in token.Operation.Login.Groups)
            {
                Grupo[i] = grupo.Name;
                ++i;
            }
            return Grupo;
        }
    }
}
