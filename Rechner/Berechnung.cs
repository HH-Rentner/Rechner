using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rechner
{
    class Berechnung
    {
        public double Berechnungen(int prioIndex, int zahlenIndex, int[] _prio, double[] _zahlen)
        {
            double resultat = 0.0;
            double fehler = resultat / 0.0; // bei einem Abbruch soll keine echte Zahl zurückgegeben werden.
            int klammerGefunden = 0; // 0 = nichts gefunden, 1 = Klammer "(" gefunden, 2 = passende Klammer ")" gefunden.
            int start, ende, i0, i1, iX, anzLoeschung, klammerNR;
            prioIndex = prioIndex - 1;
            for (i0 = 0; i0 < prioIndex; i0++)
            {//Suche nach Klammern

                klammerGefunden = 0; klammerNR = 0; start = 0; ende = prioIndex;
                for (iX = 0; iX < prioIndex && klammerGefunden < 2 && _prio[iX] != 0; iX++)
                {
                    if (_prio[iX] >= 50 && _prio[iX] < 100)
                    {
                        if (klammerGefunden == 0)
                        {
                            klammerNR = _prio[iX];
                            klammerGefunden = 1;
                            start = iX;
                        }
                        else if (_prio[iX] > klammerNR)
                        {
                            klammerNR = _prio[iX];
                            start = iX;
                        }
                        else if (_prio[iX] == klammerNR)
                        {
                            ende = iX;
                            klammerGefunden = 2;
                        }
                    }
                }
                if (klammerGefunden == 1) { MessageBox.Show("Klammern wurden nicht korrekt eingesetzt!"); return fehler; }
                int rzMax = 0; int rzPos = 0;
                for (i1 = start; i1 <= ende && _prio[i1] != 0; i1++)
                {
                    anzLoeschung = 2;
                    for (iX = start; iX < ende && _prio[iX] != 0; iX++)
                    { //sucht den Wert mit der höhsten Rechenpriorität
                        if (_prio[iX] > rzMax && _prio[iX] < 50)
                        { rzMax = _prio[iX]; rzPos = iX; }
                    }
                    if (rzPos > 0 && prioIndex >= 3)
                    {
                        switch (_prio[rzPos])
                        {
                            case 1:
                                resultat = (_zahlen[_prio[rzPos - 1] - 100]) + (_zahlen[_prio[rzPos + 1] - 100]);
                                break;
                            case 2:
                                resultat = (_zahlen[_prio[rzPos - 1] - 100]) - (_zahlen[_prio[rzPos + 1] - 100]);
                                break;
                            case 3:
                                resultat = (_zahlen[_prio[rzPos - 1] - 100]) * (_zahlen[_prio[rzPos + 1] - 100]);
                                break;
                            case 4:
                                resultat = (_zahlen[_prio[rzPos - 1] - 100]) / (_zahlen[_prio[rzPos + 1] - 100]);
                                break;
                            /* case 5: // Case 5 ist im Case 7 enthalten.
                                resultat = (_zahlen[_prio[rzPos - 1] - 100]) * (_zahlen[_prio[rzPos - 1] - 100]);
                                anzLoeschung = 1;
                                break;
                            case 6: // Case 6 ist im Case 7 enthalten.
                                if (_zahlen[_prio[rzPos - 1] - 100] < 0)
                                { MessageBox.Show("Wurzelziehen aus negativen Zahlen nicht möglich");  return fehler; }
                                resultat = Math.Sqrt(_zahlen[_prio[rzPos - 1] - 100]);
                                anzLoeschung = 1;
                                break; */
                            case 7:
                                double basis_2 = 0;
                                if (_zahlen[_prio[rzPos - 1] - 100] < 0)
                                {
                                    basis_2 = Math.Round(_zahlen[_prio[rzPos + 1] - 100], MidpointRounding.ToEven);
                                    if (_zahlen[_prio[rzPos + 1] - 100] != basis_2)
                                    {
                                        MessageBox.Show("Wurzelziehen aus negativen Zahlen nicht möglich bzw. " +
                                          "\nPotenzieren einer negativen Basis ist nur mit einem ganzzahligen Exponenten möglich"); return fehler;
                                    }
                                }
                                resultat = Math.Pow((_zahlen[_prio[rzPos - 1] - 100]), (_zahlen[_prio[rzPos + 1] - 100]));
                                break;
                            default:
                                //MessageBox.Show("_prio[prioPos] " + _prio[prioPos]);
                                MessageBox.Show("keine Rechenart gefunden");
                                break;
                        }
                        _zahlen[_prio[rzPos - 1] - 100] = resultat;
                        for (iX = rzPos; iX < prioIndex && _prio[iX] != 0; iX++)
                        { _prio[iX] = _prio[iX + anzLoeschung]; }      // lösche berechnte Werte durch "nachrücken".
                        prioIndex = prioIndex - anzLoeschung;  // Anzahl belegter Worte - 2.

                        if (klammerGefunden == 2 && _prio[start + 2] == klammerNR)
                        { // alle Werte innerhalb der Klammer wurden berechnet.
                            _prio[start] = _prio[start + 1];
                            for (int ix = start + 1; ix < prioIndex && _prio[ix] != 0; ix++)
                            { _prio[ix] = _prio[ix + 2]; }      // lösche berechnte Werte durch "nachrücken".
                            prioIndex = prioIndex - 2;  // Anzahl belegter Worte - 2.
                            i1 = ende + 100; // STOP Schleife "i1"
                        }
                        else if (prioIndex > 2)
                        { i1 = -1; rzPos = 0; rzMax = 0; ende = ende - 2; }
                    }
                    else { MessageBox.Show("keine Rechenart gefunden!"); return fehler; }
                    if (prioIndex > 2) { i0 = 0; }
                } // Ende Schleife i1 (Bearbeitung der Werte innerhalb einer Klammer)
            } // Ende Schleife i0 (Bearbeitung aller Klammern)    }
            return resultat;
        }
    }
}
