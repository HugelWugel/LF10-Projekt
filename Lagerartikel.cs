using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF10_Lager_Projekt
{
    public class Lagerartikel
    {
        public int Materialnummer { get; set; }
        public string Materialname { get; set; }
        public string Warengruppe { get; set; }
        public int Menge { get; set; }
        public int Grenzwert { get; set; }
    }
    public class kritArtikel
    {
        public int Materialnummer { get; set; }
        public int Menge { get; set; }
        public int Grenzwert { get; set; }
    }
}
