using System;

namespace Autonoleggio
{
    public class Auto : Veicolo
    {
        public Auto(string t, string m, string ti, double tg) : base(t, m, ti, 50)
        {
        }

        public override string ToString()
        {
            return $"Targa {Targa} | modello {Modello} | tipo {Tipo} " + TariffaGiornaliera.ToString();
        }
        public override double CalcolaCosto(int giorni)
        {
               return base.CalcolaCosto(giorni);
        }
    }
}