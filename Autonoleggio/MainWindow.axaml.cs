using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace Autonoleggio
{
    public partial class MainWindow : Window
    {
        private List<Veicolo> Flotta;

        public MainWindow()
        {
            InitializeComponent();

            Flotta = new List<Veicolo>();

            // Disabilita il tasto calcola all'avvio
            BtnCalcola.IsEnabled = false;

            AggiornaGrafica();
            AggiornaVisibilitaAutonomia();
        }

        public void CmbVeicolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AggiornaVisibilitaAutonomia();
        }

        private void AggiornaVisibilitaAutonomia()
        {
            // 
            if (CmbVeicolo == null || TxtAutonomia == null)
            {
                return;
            }

            ComboBoxItem itemSelezionato = CmbVeicolo.SelectedItem as ComboBoxItem;
            if (itemSelezionato != null)
            {
                string tipoSelezionato = itemSelezionato.Content.ToString();
                bool isElettrica = (tipoSelezionato == "Auto elettrica");

                TxtAutonomia.IsVisible = isElettrica;
            }
            else
            {
                TxtAutonomia.IsVisible = false;
            }
        }

        public void BtnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            TxtErrori.Text = "";

            string targa = TxtTarga.Text;
            string modello = TxtModello.Text;

            if (string.IsNullOrWhiteSpace(targa) || string.IsNullOrWhiteSpace(modello))
            {
                TxtErrori.Text = "Targa e Modello sono campi obbligatori";
                return;
            }

            targa = targa.Trim().ToUpper();

            // Controllo targhe duplicate
            for (int i = 0; i < Flotta.Count; i++)
            {
                if (Flotta[i].Targa.ToUpper() == targa)
                {
                    TxtErrori.Text = "Esiste già un veicolo con la targa " + targa;
                    return;
                }
            }

            ComboBoxItem itemSelezionato = CmbVeicolo.SelectedItem as ComboBoxItem;
            if (itemSelezionato == null)
            {
                return;
            }

            string tipoVeicolo = itemSelezionato.Content.ToString();
            Veicolo nuovoVeicolo = null;

            if (tipoVeicolo == "Auto")
            {
                nuovoVeicolo = new Auto(targa, modello, "Auto", 50);
            }
            else if (tipoVeicolo == "Moto")
            {
                nuovoVeicolo = new Moto(targa, modello, "Moto", 30);
            }
            else if (tipoVeicolo == "Furgone")
            {
                nuovoVeicolo = new Furgone(targa, modello, "Furgone", 80);
            }
            else if (tipoVeicolo == "Auto elettrica")
            {
                int autonomia;
                if (!int.TryParse(TxtAutonomia.Text, out autonomia) || autonomia <= 0)
                {
                    TxtErrori.Text = "Errore: L'autonomia deve essere un numero intero positivo.";
                    return;
                }
                nuovoVeicolo = new AutoElettrica(targa, modello, "Auto elettrica", 65.0, autonomia);
            }

            if (nuovoVeicolo != null)
            {
                Flotta.Add(nuovoVeicolo);
                AggiornaGrafica();
                PulisciCampi();
            }
        }

        // Metodo per rimuovere il veicolo selezionato
        public void BtnRimuovi_Click(object sender, RoutedEventArgs e)
        {
            Veicolo veicoloSelezionato = ListaVeicoli.SelectedItem as Veicolo;
            if (veicoloSelezionato == null)
            {
                TxtErrori.Text = "Seleziona un veicolo dalla lista da rimuovere";
                return;
            }

            Flotta.Remove(veicoloSelezionato);
            AggiornaGrafica();
            TxtErrori.Text = "Veicolo rimosso con successo";
        }

        public void BtnCalcola_Click(object sender, RoutedEventArgs e)
        {
            Veicolo veicoloSelezionato = ListaVeicoli.SelectedItem as Veicolo;
            if (veicoloSelezionato == null)
            {
                TxtPreventivo.Text = "Seleziona prima un veicolo dall'elenco.";
                return;
            }

            int giorni = 1;
            if (NumGiorni.Value.HasValue)
            {
                giorni = (int)NumGiorni.Value.Value;
            }

            double costoBase = veicoloSelezionato.CalcolaCosto(giorni);

            double costoAssicurazione = 0.0;
            bool haAssicurazione = false;
            if (ChkAssicurazione.IsChecked == true)
            {
                haAssicurazione = true;
            }
            if (haAssicurazione)
            {
                costoAssicurazione = 15.0 * giorni;
            }

            double totale = costoBase + costoAssicurazione;

            string preventivo = "Preventivo per " + veicoloSelezionato.Modello + " (" + giorni + " giorni):\n";
            preventivo = preventivo + "Noleggio: " + costoBase.ToString() + " €\n";

            if (haAssicurazione)
            {
                preventivo = preventivo + "Assicurazione: " + costoAssicurazione.ToString() + " €\n";
            }
            else
            {
                preventivo = preventivo + "Assicurazione: non richiesta\n";
            }

            preventivo = preventivo + "TOTALE: " + totale.ToString() + " €";

            IElettrico veicoloElettrico = veicoloSelezionato as IElettrico;
            if (veicoloElettrico != null)
            {
                preventivo = preventivo + "\nRicarica inclusa – autonomia " + veicoloElettrico.AutonomiaKm + " km";
            }

            TxtPreventivo.Text = preventivo;
        }

        public void ListaVeicoli_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool veicoloSelezionato = (ListaVeicoli.SelectedItem != null);
            BtnCalcola.IsEnabled = veicoloSelezionato;
        }

        private void AggiornaGrafica()
        {
            ListaVeicoli.ItemsSource = null;
            ListaVeicoli.ItemsSource = Flotta;

            // Calcolo statistiche flotta
            int totaleVeicoli = Flotta.Count;
            int veicoliElettrici = 0;

            for (int i = 0; i < Flotta.Count; i++)
            {
                if (Flotta[i] is IElettrico)
                {
                    veicoliElettrici++;
                }
            }
        }

        private void PulisciCampi()
        {
            TxtTarga.Text = "";
            TxtModello.Text = "";
            TxtAutonomia.Text = "";
            TxtErrori.Text = "";
            CmbVeicolo.SelectedIndex = 0;
        }
    }
}