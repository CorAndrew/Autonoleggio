using System;

namespace Autonoleggio
{
    public abstract class Veicolo
    {
        public string Targa { get; set; }
        public string Modello { get; set; }
        public string Tipo { get; set; }
        public double TariffaGiornaliera { get; set; }

        protected Veicolo(string targa, string modello, string tipo, double tariffaGiornaliera)
        {
            Targa = targa;
            Modello = modello;
            Tipo = tipo;
            TariffaGiornaliera = tariffaGiornaliera;
        }

        public virtual double CalcolaCosto(int giorni)
        {
            return TariffaGiornaliera * giorni;
        }

        public abstract override string ToString();
    }
}