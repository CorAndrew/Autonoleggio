using System;

namespace Autonoleggio
{
    public class AutoElettrica : Auto, IElettrico
    {
        public int AutonomiaKm { get; }


        public AutoElettrica(string t, string m, string ti, double tg, int autonomia) : base(t, m, ti, tg)
        {
            AutonomiaKm = autonomia;
        }

        public override string ToString()
        {
            return $"Targa {Targa} | modello {Modello} | tipo {Tipo} " + TariffaGiornaliera.ToString() + $"Autonomia in km {AutonomiaKm}";
        }

        public override double CalcolaCosto(int giorni)
        {
            return base.CalcolaCosto(giorni);
        }
    }
}