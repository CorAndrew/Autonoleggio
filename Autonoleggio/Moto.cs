using System;

namespace Autonoleggio
{
    public class Moto : Veicolo
    {
        public Moto(string t, string m, string ti, double tg) : base(t, m,ti, 30.0)
        {
        }

        public override string ToString()
        {
            return $"Targa {Targa} | modello {Modello} | tipo {Tipo} " + TariffaGiornaliera.ToString();
        }

        public override double CalcolaCosto(int giorni)
        {
            double costoBase = base.CalcolaCosto(giorni);
            if (giorni >= 7)
            {
                costoBase = costoBase * 0.80;
            }
            return costoBase;
        }
    }
}