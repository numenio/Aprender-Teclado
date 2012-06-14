using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Globalization;
using System.Speech.Synthesis;

namespace Voz
{
    public class voz
    {
        private static SpeechSynthesizer sintetizador = new SpeechSynthesizer();

        public static void hablar(string texto)
        {
            sintetizador.Speak(texto);
        }

        public static void hablarAsync(string texto)
        {
            sintetizador.SpeakAsyncCancelAll();
            sintetizador.SpeakAsync(texto);
        }

        public static void callar()
        {
            if (sintetizador.State == SynthesizerState.Speaking)
                sintetizador.SpeakAsyncCancelAll();
        }

        public static List<string> listarVoces()
        {
            List<string> listaNombreVoces = new List<string>();
            ReadOnlyCollection<InstalledVoice> lista = sintetizador.GetInstalledVoices();
            foreach (InstalledVoice voz in lista)
            {
                listaNombreVoces.Add(voz.VoiceInfo.Name);
            }
            return listaNombreVoces;
        }

        public static List<string> listarVocesPorIdioma(string idioma)
        {
            List<string> listaNombreVoces = new List<string>();
            ReadOnlyCollection<InstalledVoice> lista = sintetizador.GetInstalledVoices();
            foreach (InstalledVoice voz in lista)
            {
                //CultureInfo cultura = new CultureInfo("es-ES");
                if (voz.VoiceInfo.Culture.Parent.DisplayName == idioma) // cultura.Parent.DisplayName)
                    listaNombreVoces.Add(voz.VoiceInfo.Name);
            }
            return listaNombreVoces;
        }

        public static List<string> listarIdiomasInstalados()
        {
            List<string> listaIdiomaVoces = new List<string>();
            ReadOnlyCollection<InstalledVoice> lista = sintetizador.GetInstalledVoices();
            foreach (InstalledVoice voz in lista)
            {
                if (!listaIdiomaVoces.Contains(voz.VoiceInfo.Culture.Parent.DisplayName)) //si no está repetida se añade
                    listaIdiomaVoces.Add(voz.VoiceInfo.Culture.Parent.DisplayName);
            }
            return listaIdiomaVoces;
        }

        public static void cambiarVoz(string quéVoz)
        {
            //int sintetizadorEstá = listarIdiomasInstalados().IndexOf(quéVoz);
            voz.callar();
            if (listarVocesPorIdioma("Español").IndexOf(quéVoz) == -1) //si no está el sintetizador instalado, se pone el primero que haya
                sintetizador.SelectVoice(listarVocesPorIdioma("Español")[0]);
            else
                sintetizador.SelectVoice(quéVoz);
        }

        public static void cambiarVelocidad(int valor)
        {
            valor += -10;
            if (sintetizador.Rate + valor <= 10 && sintetizador.Rate + valor >= -10)
                sintetizador.Rate = valor;
        }
    }
}
