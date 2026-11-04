using System;
using System.Collections.Generic;
using System.Text;

namespace Autonoleggio
{
    public class Furgone : Veicolo
    {
        public Furgone(string t, string m, string ti, double tg) : base(t, m, ti, tg)
        {
            Targa = t;
            Modello = m;
            Tipo = ti;
            TariffaGiornaliera = 80;
        }

        public override double CalcolaCosto(int giorni)
        {
            return base.CalcolaCosto(giorni) + 100;
        }
        public override string ToString()
        {
            return $"Targa {Targa} | modello {Modello} | tipo {Tipo} " + TariffaGiornaliera.ToString();
        }
    }
}
