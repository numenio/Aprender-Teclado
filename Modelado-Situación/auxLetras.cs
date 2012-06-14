using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace LógicaAplicación
{
    public class auxLetras
    {
        public static bool esCarácterLegible(string carácter)
        {
            bool esLegible = true;
            string[] caracteresLegibles = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "<", ">", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "-", ",", ";", ":", "_", "\"", "!", "¡", "¿", "?", "/", "%", "$", "space", "+", "-", "*", "/", "`", "^", "[", "]", "(", ")", "´", "¨", "{", "}", "|", "\\", "@", "#", "~", "€", "¬", "º", "ª", "\"", "·", "&", "=" }; //"d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "d0",

            if (caracteresLegibles.Contains(carácter))
                esLegible = true;
            else
                esLegible = false;

            return esLegible;
        }

        public static string corregirCarácter(string carácter, bool shiftActivo, bool altActivo, bool swAcentoArriba)
        {
            string carácterCorregido=carácter;
            if (carácter != "rightshift" && carácter != "rightalt" && carácter != "leftctrl")
            {
                switch (carácter)
                {
                    case "d1":
                        if (altActivo)
                            carácterCorregido = "|";
                        else if (shiftActivo)
                            carácterCorregido = "!";
                        else
                            carácterCorregido = "1";
                        break;
                    case "d2":
                        if (altActivo)
                            carácterCorregido = "@";
                        else if (shiftActivo)
                            carácterCorregido = "\"";
                        else
                            carácterCorregido = "2";
                        break;
                    case "d3":
                        if (altActivo)
                            carácterCorregido = "#";
                        else if (shiftActivo)
                            carácterCorregido = "·";
                        else
                            carácterCorregido = "3";
                        break;
                    case "d4":
                        if (altActivo)
                            carácterCorregido = "~";
                        else if (shiftActivo)
                            carácterCorregido = "$";
                        else
                            carácterCorregido = "4";
                        break;
                    case "d5":
                        if (altActivo)
                            carácterCorregido = "€";
                        else if (shiftActivo)
                            carácterCorregido = "%";
                        else
                            carácterCorregido = "5";
                        break;
                    case "d6":
                        if (altActivo)
                            carácterCorregido = "¬";
                        else if (shiftActivo)
                            carácterCorregido = "&";
                        else
                            carácterCorregido = "6";
                        break;
                    case "d7":
                        if (shiftActivo)
                            carácterCorregido = "/";
                        else
                            carácterCorregido = "7";
                        break;
                    case "d8":
                        if (shiftActivo)
                            carácterCorregido = "(";
                        else
                            carácterCorregido = "8";
                        break;
                    case "d9":
                        if (shiftActivo)
                            carácterCorregido = ")";
                        else
                            carácterCorregido = "9";
                        break;
                    case "d0":
                        if (shiftActivo)
                            carácterCorregido = "=";
                        else
                            carácterCorregido = "0";
                        break;
                    case "oemplus":
                        if (shiftActivo)
                            carácterCorregido = "*";
                        else if (altActivo)
                            carácterCorregido = "]";
                        else
                            carácterCorregido = "+";
                        break;
                    case "oemminus":
                        if (shiftActivo)
                            carácterCorregido = "_";
                        else
                            carácterCorregido = "-";
                        break;
                    case "oemcomma":
                        if (shiftActivo)
                            carácterCorregido = ";";
                        else
                            carácterCorregido = ",";
                        break;
                    case "oem1":
                        if (shiftActivo)
                            carácterCorregido = "^";
                        else if (altActivo)
                            carácterCorregido = "[";
                        else
                            carácterCorregido = "`";
                        break;
                    case "oem3":
                        carácterCorregido = "ñ";
                        break;
                    case "oemperiod":
                        if (shiftActivo)
                            carácterCorregido = ":";
                        else
                            carácterCorregido = ".";
                        break;
                    case "oemquotes":
                        if (shiftActivo)
                            carácterCorregido = "¨";
                        else if (altActivo)
                            carácterCorregido = "{";
                        else
                            carácterCorregido = "´";
                        break;
                    case "oemquestion":
                        if (shiftActivo)
                            carácterCorregido = "Ç";
                        else if (altActivo)
                            carácterCorregido = "}";
                        else
                            carácterCorregido = "ç";
                        break;
                    case "oem5":
                        if (shiftActivo)
                            carácterCorregido = "ª";
                        else if (altActivo)
                            carácterCorregido = "\\";
                        else
                            carácterCorregido = "º";
                        break;
                    case "oemopenbrackets":
                        if (shiftActivo)
                            carácterCorregido = "?";
                        else
                            carácterCorregido = "'";
                        break;
                    case "oem6":
                        if (shiftActivo)
                            carácterCorregido = "¿";
                        else
                            carácterCorregido = "¡";
                        break;
                    case "oembackslash":
                        if (shiftActivo)
                            carácterCorregido = ">";
                        else
                            carácterCorregido = "<";
                        break;
                        
                }

                if (swAcentoArriba) //si el acento está arriba, se cambia el mapeo del teclado
                {
                    switch (carácter)
                    {
                        case "oemquotes":
                            if (shiftActivo)
                                carácterCorregido = "^";
                            else if (altActivo)
                                carácterCorregido = "{"; //este queda igual que antes
                            else
                                carácterCorregido = "`";
                            break;
                        case "oem1":
                            if (shiftActivo)
                                carácterCorregido = "¨";
                            else if (altActivo)
                                carácterCorregido = "["; //este queda igual que antes
                            else
                                carácterCorregido = "´";
                            break;
                    }
                }
            }
            return carácterCorregido;
        }

        public static string traducirCarácter(string carácter)
        {
            string carácterTraducido = "";
            switch (carácter)
            {
                case "a":
                    carácterTraducido = "a";
                    break;
                case "b":
                    carácterTraducido = "be larga";
                    break;
                case "c":
                    carácterTraducido = "ce";
                    break;
                case "d":
                    carácterTraducido = "de";
                    break;
                case "e":
                    carácterTraducido = "e";
                    break;
                case "f":
                    carácterTraducido = "efe";
                    break;
                case "g":
                    carácterTraducido = "je";
                    break;
                case "h":
                    carácterTraducido = "ache";
                    break;
                case "i":
                    carácterTraducido = "i";
                    break;
                case "j":
                    carácterTraducido = "jota";
                    break;
                case "k":
                    carácterTraducido = "ca";
                    break;
                case "l":
                    carácterTraducido = "ele";
                    break;
                case "m":
                    carácterTraducido = "eme";
                    break;
                case "n":
                    carácterTraducido = "ene";
                    break;
                case "ñ":
                    carácterTraducido = "eñe";
                    break;
                case "o":
                    carácterTraducido = "o";
                    break;
                case "p":
                    carácterTraducido = "pe";
                    break;
                case "q":
                    carácterTraducido = "cu";
                    break;
                case "r":
                    carácterTraducido = "erre";
                    break;
                case "s":
                    carácterTraducido = "ese";
                    break;
                case "t":
                    carácterTraducido = "te";
                    break;
                case "u":
                    carácterTraducido = "u";
                    break;
                case "v":
                    carácterTraducido = "be corta";
                    break;
                case "w":
                    carácterTraducido = "doble ve";
                    break;
                case "x":
                    carácterTraducido = "equis";
                    break;
                case "y":
                    carácterTraducido = "i griega";
                    break;
                case "z":
                    carácterTraducido = "seta";
                    break;
                case "space":
                    carácterTraducido = "espacio";
                    break;
                case " ":
                    carácterTraducido = "espacio";
                    break;
                case "1":
                    carácterTraducido = "uno";
                    break;
                case "2":
                    carácterTraducido = "dos";
                    break;
                case "3":
                    carácterTraducido = "tres";
                    break;
                case "4":
                    carácterTraducido = "cuatro";
                    break;
                case "5":
                    carácterTraducido = "cinco";
                    break;
                case "6":
                    carácterTraducido = "seis";
                    break;
                case "7":
                    carácterTraducido = "siete";
                    break;
                case "8":
                    carácterTraducido = "ocho";
                    break;
                case "9":
                    carácterTraducido = "nueve";
                    break;
                case "0":
                    carácterTraducido = "cero";
                    break;
                case "+":
                    carácterTraducido = "más";
                    break;
                case "-":
                    carácterTraducido = "menos, o guión";
                    break;
                case "*":
                    carácterTraducido = "asterisco, o multiplicado por";
                    break;
                case ".":
                    carácterTraducido = "punto";
                    break;
                case "·":
                    carácterTraducido = "punto superior";
                    break;
                case ":":
                    carácterTraducido = "dos puntos";
                    break;
                case "/":
                    carácterTraducido = "dividido, o barra diagonal";
                    break;
                case "!":
                    carácterTraducido = "cierra admiración";
                    break;
                case "¡":
                    carácterTraducido = "abre admiración";
                    break;
                case "¿":
                    carácterTraducido = "abre pregunta";
                    break;
                case "?":
                    carácterTraducido = "cierra pregunta";
                    break;
                case "=":
                    carácterTraducido = "igual";
                    break;
                case "(":
                    carácterTraducido = "abre paréntesis";
                    break;
                case ")":
                    carácterTraducido = "cierra paréntesis";
                    break;
                case "\"":
                    carácterTraducido = "comillas";
                    break;
                case "%":
                    carácterTraducido = "porciento";
                    break;
                case "$":
                    carácterTraducido = "pesos";
                    break;
                case "#":
                    carácterTraducido = "numeral";
                    break;
                case "º":
                    carácterTraducido = "o volada, o grados";
                    break;
                case "\\":
                    carácterTraducido = "barra diagonal inversa";
                    break;
                case ",":
                    carácterTraducido = "coma";
                    break;
                case ";":
                    carácterTraducido = "punto y coma";
                    break;
                case "_":
                    carácterTraducido = "guión bajo";
                    break;
                case "<":
                    carácterTraducido = "menor que";
                    break;
                case ">":
                    carácterTraducido = "mayor que";
                    break;
                case "|":
                    carácterTraducido = "barra vertical";
                    break;
                case "@":
                    carácterTraducido = "arroba";
                    break;
                case "€":
                    carácterTraducido = "euro";
                    break;
                case "ª":
                    carácterTraducido = "a volada";
                    break;
                case "~":
                    carácterTraducido = "equivalencia";
                    break;
                case "¬":
                    carácterTraducido = "negación lógica";
                    break;
                case "[":
                    carácterTraducido = "abre corchete";
                    break;
                case "]":
                    carácterTraducido = "cierra corchete";
                    break;
                case "{":
                    carácterTraducido = "abre llave";
                    break;
                case "}":
                    carácterTraducido = "cierra llave";
                    break;
                case "´":
                    carácterTraducido = "acento agudo";
                    break;
                case "`":
                    carácterTraducido = "acento grave";
                    break;
                case "¨":
                    carácterTraducido = "diéresis";
                    break;
                case "^":
                    carácterTraducido = "acento circunflejo";
                    break;
                case "ç":
                    carácterTraducido = "ce cerilla";
                    break;
                case "á":
                    carácterTraducido = "a con acento agudo";
                    break;
                case "à":
                    carácterTraducido = "a con acento grave";
                    break;
                case "â":
                    carácterTraducido = "a con acento circunflejo";
                    break;
                case "é":
                    carácterTraducido = "e con acento agudo";
                    break;
                case "è":
                    carácterTraducido = "e con acento grave";
                    break;
                case "ê":
                    carácterTraducido = "e con acento circunflejo";
                    break;
                case "í":
                    carácterTraducido = "i con acento agudo";
                    break;
                case "ì":
                    carácterTraducido = "i con acento grave";
                    break;
                case "î":
                    carácterTraducido = "i con acento circunflejo";
                    break;
                case "ó":
                    carácterTraducido = "o con acento agudo";
                    break;
                case "ò":
                    carácterTraducido = "o con acento grave";
                    break;
                case "ô":
                    carácterTraducido = "o con acento circunflejo";
                    break;
                case "ú":
                    carácterTraducido = "u con acento agudo";
                    break;
                case "ù":
                    carácterTraducido = "u con acento grave";
                    break;
                case "û":
                    carácterTraducido = "u con acento circunflejo";
                    break;
                case "ü":
                    carácterTraducido = "u con diéresis";
                    break;
                default:
                    carácterTraducido = carácter;
                    break;
            }
            return carácterTraducido;
        }

        public static bool esLetraConAcento(string letra)
        {
            string[] acentos = { "á", "é", "í", "ó", "ú", "à", "è", "ì", "ò", "ù", "â", "ê", "î", "ô", "û", "ü", "`", "´", "^", "¨"};
            if (acentos.Contains(letra))
                return true;
            else
                return false;
        }

        public static bool esAcento(string letra)
        {
            string[] acentos = { "`", "´", "^", "¨" };
            if (acentos.Contains(letra))
                return true;
            else
                return false;
        }

        public static string obtenerVocalSinAcento(string vocalAcentuada)
        {
            string vocalLimpia = "";

            switch (vocalAcentuada)
            {
                case "á":
                    vocalLimpia = "a";
                    break;
                case "é":
                    vocalLimpia = "e";
                    break;
                case "í":
                    vocalLimpia = "i";
                    break;
                case "ó":
                    vocalLimpia = "o";
                    break;
                case "ú":
                    vocalLimpia = "u";
                    break;
                case "à":
                    vocalLimpia = "a";
                    break;
                case "è":
                    vocalLimpia = "e";
                    break;
                case "ì":
                    vocalLimpia = "i";
                    break;
                case "ò":
                    vocalLimpia = "o";
                    break;
                case "ù":
                    vocalLimpia = "u";
                    break;
                case "â":
                    vocalLimpia = "a";
                    break;
                case "ê":
                    vocalLimpia = "e";
                    break;
                case "î":
                    vocalLimpia = "i";
                    break;
                case "ô":
                    vocalLimpia = "o";
                    break;
                case "û":
                    vocalLimpia = "u";
                    break;
                case "ü":
                    vocalLimpia = "u";
                    break;
            }

            return vocalLimpia;
        }

        public static string obtenerAcentoDeVocal(string vocalAcentuada)
        {
            string acento = "";

            switch (chequearTipoAcento(vocalAcentuada))
            {
                case tipoAcento.agudo:
                    acento = "´";
                    break;
                case tipoAcento.grave:
                    acento = "`";
                    break;
                case tipoAcento.circunflejo:
                    acento = "^";
                    break;
                case tipoAcento.diéresis:
                    acento = "¨";
                    break;
            }

            return acento;
        }

        public static tipoAcento chequearTipoAcento(string letraAcentuada)
        {
            string[] acentosAgudos = { "á", "é", "í", "ó", "ú", "´" };
            string[] acentosGraves = { "à", "è", "ì", "ò", "ù", "`" };
            string[] acentosCircunflejos = { "â", "ê", "î", "ô", "û", "^" };
            string[] diéresis = { "ü", "¨" };

            if (acentosAgudos.Contains(letraAcentuada)) return tipoAcento.agudo;
            if (acentosGraves.Contains(letraAcentuada)) return tipoAcento.grave;
            if (acentosCircunflejos.Contains(letraAcentuada)) return tipoAcento.circunflejo;
            if (diéresis.Contains(letraAcentuada)) return tipoAcento.diéresis;

            return tipoAcento.sinAcento; //si llegó hasta acá es que no tiene acento
        }
    }
}
