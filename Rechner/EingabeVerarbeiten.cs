using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rechner
{
    class EingabeVerarbeiten
    {
        Berechnung rechne = new Berechnung();

        public string txtEingabeText;
        public string txtFormelText;
        public string lblMemoryText;
        //---------------------------------------------------------------------------------------------------------------------
        private bool firstChar = true;              // wird bei der Zahleneingabe benötigt, um am Anfang der Zahl besonders zu reagieren.
        private bool kommaSperre = false;           // wird benötigt um zu verhindern, dass das Zeichen ',' falsch eingesetzt werden kann.  
        private bool exponentSperre = true;         // wird benötigt um zu verhindern, dass das Zeichen 'e' falsch eingesetzt werden kann.
        private bool sperreZahl = false;            // Zur Kontrolle der richtigen Reihenfolge während der Parametereingabe.
        private bool sperreRA1 = true;              // Zur Kontrolle der richtigen Reihenfolge während der Parametereingabe. Betrifft +, -, * und/.
        private bool sperreRA2 = true;              // Zur Kontrolle der richtigen Reihenfolge während der Parametereingabe. Betrifft zurzeit X^2, X^y und Wurzel.
        private bool sperreKLauf = false;           // Zur Kontrolle der richtigen Reihenfolge während der Parametereingabe.
        private bool sperreKLzu = true;             // Zur Kontrolle der richtigen Reihenfolge während der Parametereingabe.
        private bool sperreBerechnung = true;       // Zur Kontrolle der richtigen Reihenfolge während der Parametereingabe.
        private bool sperreSofortBerechnung = false;// Bei X², Wurzel ist eine Sofortberechnung möglich, wenn nicht direkt vorher ")" eingegeben wurde.
        private bool AnzeigeIstZahl = false;        // Wurde die Zahl in der Eingabe bereits gewandelt, ist keine erneute Wandlung nötig.
        private bool AnzeigeE10 = false;            // Wurde die Zahl in der Eingabe bereits gewandelt, ist keine erneute Wandlung nötig.
        private bool folgeAktion = false;           // wird benötigt um mit dem vorherigen Rechenergebnis weiter zu rechnen.
        private bool prozentFolgeAktion = false;    // wird benötigt um Prozente zu ermitteln.
        private bool winkelUmkehr = false;          // wird zum Ändern der Tastenfunktionen benötigt..
        //---------------------------------------------------------------------------------------------------------------------
        private int prioIndex;
        private int[] prio = new int[50];           //50 Zeichen Maximum sollten mit Sicherheit ausreichen.
        private int zahlenIndex;
        private double[] zahlen = new double[25];   //25 Zahlen  Maximum sollten mit Sicherheit ausreichen.
        //---------------------------------------------------------------------------------------------------------------------
        private double zahlAnzeige, memory, resultat, potenzBasis, dividend;
        private int klammerNR, potenzAbfolge = 0;
        private string potenzRZ;

        public void TastaturVerarbeiten(char tastaturEingabe)
        {
            AnzeigeIstZahl = false;
            switch (tastaturEingabe)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0':
                case ',':
                    Ziffer_prüfen_freigeben(tastaturEingabe);
                    break;
                case '\b':
                    //loescheZeichen();
                    BtnDel_Klick();
                    break;
                case 'e':
                    if (firstChar)
                    { MessageBox.Show("Fehlerhafte Eingabe.\nEin Exponent ohne Basiszahl ist nicht zulässig."); }
                    else if (exponentSperre == false)
                    {
                        exponentSperre = true;
                        txtEingabeText += tastaturEingabe;
                    }
                    else { MessageBox.Show("Fehlerhafte Eingabe.\nDer Exponent kann nur einmal vergeben werden."); }
                    break;
                case '+':
                case '-':
                case '*':
                case '/':
                    string gr_Rart = "";
                    Verarbeite4Grundrechenarten(gr_Rart += tastaturEingabe);
                    break;
                case ':':
                    Verarbeite4Grundrechenarten("/");
                    break;
                case '=':
                case '\r':  // [Enter]
                    BtnGleich_Klick();
                    break;
                default:
                    MessageBox.Show("Es wurde ein falsches Zeichen eingeben\noder eine falsche Taste betätigt.");
                    break;
            }
        }

        public void ButtonVerarbeitungZiffern(string eingabe)
        {   //setzt den Button_Click in ein Zeichen um.
            char eingabeChar = '?';
            AnzeigeIstZahl = false;
            switch (eingabe)
            {
                case "Btn0":
                    eingabeChar = '0';
                    break;
                case "BtnKomma":
                    eingabeChar = ',';
                    break;
                case "Btn1":
                    eingabeChar = '1';
                    break;
                case "Btn2":
                    eingabeChar = '2';
                    break;
                case "Btn3":
                    eingabeChar = '3';
                    break;
                case "Btn4":
                    eingabeChar = '4';
                    break;
                case "Btn5":
                    eingabeChar = '5';
                    break;
                case "Btn6":
                    eingabeChar = '6';
                    break;
                case "Btn7":
                    eingabeChar = '7';
                    break;
                case "Btn8":
                    eingabeChar = '8';
                    break;
                case "Btn9":
                    eingabeChar = '9';
                    break;
                default:
                    MessageBox.Show("Diese Zifferneingabe wird noch nicht verarbeitet!");
                    break;
            }
            Ziffer_prüfen_freigeben(eingabeChar);
            return;
        }

        public void ButtonVerarbeitung(string eingabe, bool winkelFunktion)
        {   
            winkelUmkehr = winkelFunktion;
            switch (eingabe)
            {
                case "BtnPI":
                    BtnPI_Klick();
                    break;
                case "BtnFE":
                    BtnFE_Klick();
                    break;
                case "BtnExp":
                    BtnExp_Klick();
                    break;
                case "BtnPlusMinus":
                    BtnPlusMinus_Klick();
                    break;
                case "BtnAddieren":
                    Verarbeite4Grundrechenarten("+");
                    break;
                case "BtnSubtrahieren":
                    Verarbeite4Grundrechenarten("-");
                    break;
                case "BtnMultiplizieren":
                    Verarbeite4Grundrechenarten("*");
                    break;
                case "BtnDividieren":
                    Verarbeite4Grundrechenarten("/");
                    break;
                case "BtnKehrwert":
                    BtnKehrwert_Klick();
                    break;
                case "BtnQuadrat":
                    BtnQuadrat_Klick();
                    break;
                case "BtnWurzel":
                    BtnWurzel_Klick();
                    break;
                case "BtnPotenz":
                    BtnPotenz_Klick();
                    break;
                case "BtnKlammerAuf":
                    BtnKlammerAuf_Klick();
                    break;
                case "BtnKlammerZu":
                    BtnKlammerZu_Klick();
                    break;
                case "BtnSin":
                    BtnSin_Klick();
                    break;
                case "BtnCos":
                    BtnCos_Klick();
                    break;
                case "BtnTan":
                    BtnTan_Klick();
                    break;
                case "BtnDel":
                    BtnDel_Klick();
                    break;
                case "BtnCE":
                    BtnCE_Klick();
                    break;
                case "BtnC":
                    BtnC_Klick();
                    break;
                case "BtnGleich":
                    BtnGleich_Klick();
                    break;
                case "BtnMC":
                    BtnMC_Klick();
                    break;
                case "BtnMR":
                    BtnMR_Klick();
                    break;
                case "BtnMminus":
                    BtnMminus_Klick();
                    break;
                case "BtnMplus":
                    BtnMplus_Klick();
                    break;
                case "BtnMS":
                    BtnMS_Klick();
                    break;
                default:
                    MessageBox.Show("Dieser Button wird noch nicht verarbeitet!");
                    break;
            }
            return;
        }

        public void Ziffer_prüfen_freigeben(char zeichen)
        {   // Es kommen nur die Zeichen "0-9" und "," in Betracht. Alle anderen wurden bereits ausgeschlossen! 
            // Etwas problematisch war, dass sowohl Tastatur- wie auch die Buttoneingabe auf diese Funktion zugreifen.
            // Dies war besonders bei der Exponenteneingabe (nur mit der Tastatureingabe möglich) ein Problem.
            if (sperreZahl)
            {
                MessageBox.Show("Eingabe_prüfen_freigeben\r\nDie Eingabe einer Zahl ist zurzeit gesperrt.");
                txtEingabeText = "0";
                return;
            }
            else if (firstChar)
            {
                if (folgeAktion) { NormiereBerechnung(false); }
                txtEingabeText = "";
                if (prioIndex == 0) { txtEingabeText = ""; }
                firstChar = false;
                exponentSperre = false;
                if (zeichen != '0')
                {
                    if (zeichen == ',') { kommaSperre = true; txtEingabeText = "0" + zeichen; }
                    else { txtEingabeText = "" + zeichen; }
                }
                exponentSperre = false;
            }
            else if (zeichen == ',')
            {
                if (kommaSperre == false) { kommaSperre = true; txtEingabeText += zeichen; }
                else { MessageBox.Show("Fehlerhafte Eingabe.\nEin Komma kann nur einmal vergeben werden."); }
            }
            else { txtEingabeText += zeichen; }
            sperreZahl = false;
            sperreRA1 = false;
            if (potenzAbfolge == 0) { sperreRA2 = false; }
            sperreKLauf = true;
            sperreKLzu = false;
            sperreBerechnung = false;
            sperreSofortBerechnung = false;
        }

        private void BtnPI_Klick()
        {
            if (!firstChar)
            {
                MessageBox.Show("BtnPI\r\nDieseRechenart ist zurzeit gesperrt.");
                return;
            }
            if (folgeAktion) { NormiereBerechnung(false); }
            zahlAnzeige = Math.PI;
            txtFormelText += "(PI)";
            TrageWertInTxtErgebnisEin(zahlAnzeige);
            ArrEintragZahl(zahlAnzeige);
            sperreZahl = true;
            sperreRA1 = false;
            sperreRA2 = false;
            sperreKLauf = true;
            sperreKLzu = false;
            sperreBerechnung = false;
            sperreSofortBerechnung = false;
        }

        private void BtnFE_Klick()
        {
            if (!AnzeigeIstZahl)
            { if (!Umwandlung("BtnFE_Click")) { return; } }
            if (AnzeigeE10)
            {
                txtEingabeText = zahlAnzeige.ToString("#,##0.##########");
                AnzeigeE10 = false;
            }
            else
            {
                txtEingabeText = zahlAnzeige.ToString("E10");
                AnzeigeE10 = true;
            }
        }

        private void BtnExp_Klick()
        {
            MessageBox.Show("Funktion Exponent ist noch nicht implementiert.");
        }

        private void BtnPlusMinus_Klick()
        {
            if (!AnzeigeIstZahl)
            { if (!Umwandlung("BtnPlusMinus")) { return; } }
            zahlAnzeige = zahlAnzeige * -1;
            TrageWertInTxtErgebnisEin(zahlAnzeige);
            // Falls das +- Zeichen ausgegeben werden soll, hier das Zeichen "\u00B1"
        }

        private void BtnKehrwert_Klick()
        {
            if (!AnzeigeIstZahl)
            { if (!Umwandlung("BtnKehrwert")) { return; } }
            zahlAnzeige = 1 / zahlAnzeige;
            TrageWertInTxtErgebnisEin(zahlAnzeige);
        }

        private void BtnAddieren_Klick() { Verarbeite4Grundrechenarten("+"); }
        private void BtnSubtrahieren_Klick() { Verarbeite4Grundrechenarten("-"); }
        private void BtnMultiplizieren_Klick() { Verarbeite4Grundrechenarten("*"); }
        private void BtnTeilen_Klick() { Verarbeite4Grundrechenarten("/"); }

        private void Verarbeite4Grundrechenarten(string rechenZeichen)
        { // An dieser Stelle sind nur die 4 Grundrechenarten möglich.
            if (sperreRA1 == true)
            {
                MessageBox.Show("Verarbeite4Grundrechenarten\r\nDiese Rechenart ist zurzeit gesperrt.");
                return;
            }
            if (potenzAbfolge > 0)
            {
                if (potenzAbfolge == 2) { potenzRZ = "/"; BtnPotenz_Klick(); return; }
                potenzRZ = "Abschluss"; BtnPotenz_Klick();
            }
            int grArt = 0;
            switch (rechenZeichen)
            {
                case "+":
                    grArt = 1;
                    break;
                case "-":
                    grArt = 2;
                    break;
                case "*":
                    grArt = 3;
                    break;
                case "/":
                    grArt = 4;
                    break;
                default:
                    MessageBox.Show("Verarbeite4Grundrechenarten\r\nfehlerhafter Zustand");
                    break;
            }
            if (prioIndex == 0) { txtFormelText = ""; }
            //if (letzteZeichenArt >= 5 && letzteZeichenArt != 50) // z.B sind hier auch : ")", X^2, X^y, Wurzel u. a.? möglich.
            if (sperreZahl) // z.B sind hier auch : ")", X^2, X^y, Wurzel u. a.? möglich.
            { // in diesem Fall muss keine Zahl übernommen werden.
                txtFormelText += rechenZeichen;
                ArrEintragZeichen(grArt);
            }
            else
            { // Zahl muss verarbeitet werden
                if (folgeAktion) { folgeAktion = false; }
                if (!AnzeigeIstZahl)
                { if (!Umwandlung("Verarbeite4Grundrechenarten")) { return; } }
                ArrEintragZahl(zahlAnzeige);
                ArrEintragZeichen(grArt);
                txtFormelText += zahlAnzeige.ToString("#,##0.#####") + rechenZeichen;
            }
            NormierungEingabe(true); // von 2 Zeilen vorher übernommen
            sperreZahl = false;
            sperreRA1 = true;
            sperreRA2 = true;
            sperreKLauf = false;
            sperreKLzu = true;
            sperreBerechnung = true;
            sperreSofortBerechnung = false;
        }

        private void BtnQuadrat_Klick()
        {   // Kann direkt eine Berechnung erfolgen (z.B.: 5^2), wird diese durchgeführt.
            // In der "Formelanzeige" wird dies entsprechen angezeigt (z.B: 3+5^2=, oder 128,345^2=)
            // Intern wird dann bereits mit dem Ergebnis weitergerechnet.
            // Kann die Berechnung nicht sofort erfolgen (z.B.: (3+6)^2=), weil erst die Klammer ermittelt werden muss,
            // erfolgt die Berechnung später in der Methode "Berechnung
            if (sperreRA2)
            {
                MessageBox.Show("BtnQuadrat\r\nDieseRechenart ist zurzeit gesperrt.");
                return;
            }
            if (sperreSofortBerechnung)
            {
                txtFormelText += "\u00B2";
                ArrEintragZeichen(7);
                ArrEintragZahl(2);
                sperreZahl = true;
            }
            else
            {
                if (!AnzeigeIstZahl) { if (!Umwandlung("BtnQuadrat")) { return; } }
                zahlAnzeige = zahlAnzeige * zahlAnzeige;
                //TxtFormel.Text += TxtEingabe.Text + "\u00B2";
                //ArrEintragZahl(zahlAnzeige);
                TrageWertInTxtErgebnisEin(zahlAnzeige);
                sperreZahl = false;
            }
            sperreRA1 = false;
            sperreRA2 = false;
            sperreKLauf = true;
            sperreKLzu = false;
            sperreBerechnung = false;
            sperreSofortBerechnung = false;
        }

        private void BtnWurzel_Klick()
        {   // Beachte: Das Wurzelzeichen("\u221A") wird zumindest vorerst immer hinter dem Begriff, aus dem die Wurzel zu ziehen ist, aufgeführt.
            // Kann direkt eine Berechnung erfolgen (z.B.: 5Wurzel), wird diese durchgeführt.
            // In der "Formelanzeige" wird dies entsprechen angezeigt (z.B: 8Wurzel= u.a.)
            // Intern wird dann bereits mit dem Ergebnis weitergerechnet.
            // Kann die Berechnung nicht sofort erfolgen (z.B.: (3+6)Wurzel=), weil erst die Klammer ermittelt werden muss,
            // erfolgt die Berechnung später in der Methode "Berechnung
            if (sperreRA2)
            {
                MessageBox.Show("BtnWurzel\r\nDieseRechenart ist zurzeit gesperrt.");
                return;
            }
            if (sperreSofortBerechnung)
            {
                txtFormelText += "\u221A";
                ArrEintragZeichen(7);
                ArrEintragZahl(0.5);
                sperreZahl = true;
            }
            else
            {
                if (!AnzeigeIstZahl) { if (!Umwandlung("BtnWurzel")) { return; } }
                if (zahlAnzeige < 0) { MessageBox.Show("Wurzelziehen aus negativen Zahlen nicht möglich"); return; }
                zahlAnzeige = Math.Sqrt(zahlAnzeige);
                //TxtFormel.Text += TxtErgebnis.Text + "\u221A";
                //ArrEintragZahl(zahlAnzeige);
                TrageWertInTxtErgebnisEin(zahlAnzeige);
                sperreZahl = false;
            }
            sperreRA1 = false;
            sperreRA2 = false;
            sperreKLauf = true;
            sperreKLzu = false;
            sperreBerechnung = false;
            sperreSofortBerechnung = false;
        }

        private void BtnPotenz_Klick()
        {   // Kann direkt eine Berechnung erfolgen (z.B.: 5^5), wird diese durchgeführt.
            // In der "Formelanzeige" wird dies entsprechen angezeigt (z.B: 3+5^5=, 27^(1/3)= oder 2^8=)
            // Intern wird dann bereits mit dem Ergebnis weitergerechnet.
            // Kann die Berechnung nicht sofort erfolgen (z.B.: (3+6)^3=), weil erst die Klammer ermittelt werden muss,
            // erfolgt die Berechnung später in der Methode "Berechnung.
            /* letzteUebergabe:
            1  Basis eingeben und X^y betätigen.    -> potenzAbfolge = 1 
            2a Exponent eingeben.                   -> potenzAbfolge = 0 (Ende)
            --- an hier beginnt das "Wurzelziehen" Beispiel: 27^(1/3)= 3 ---
            2b "(" betätigen.                       -> potenzAbfolge = 2
            3  Zahl1 eingegeben und "/" betätigen.  -> potenzAbfolge = 3
            4  Zahl2 eingegeben und ")" betätigen.  -> potenzAbfolge = 0 (Ende) */

            if (sperreRA2 && potenzRZ == "")
            {
                MessageBox.Show("BtnPotenz\r\nDieseRechenart ist zurzeit gesperrt.");
                return;
            }
            if (potenzAbfolge == 0)
            {
                if (!sperreZahl)
                {
                    //txtErgebnis.Text = "";
                    if (prioIndex == 0) { txtFormelText = ""; }
                    if (!AnzeigeIstZahl) { if (!Umwandlung("BtnPotenz")) { return; } }
                    potenzBasis = zahlAnzeige;
                    txtFormelText += potenzBasis.ToString("#,##0.#####") + "^";
                    AnzeigeIstZahl = false;
                    ArrEintragZahl(zahlAnzeige); // Eintrag Basis
                    NormierungEingabe(true);
                }
                else { txtFormelText += "^"; }
                ArrEintragZeichen(7); // Eintrag Rechenzeichen
                potenzAbfolge = 1;
                sperreZahl = false;
                sperreRA1 = false; // falls Exponent als Bruch eingegeben wird.
                sperreRA2 = true;
                sperreKLauf = false;
                sperreKLzu = true;
                sperreBerechnung = true;
                sperreSofortBerechnung = true;
                return;
            }
            else
            {
                if (potenzRZ == "Abschluss"
                    || potenzRZ == ")" && potenzAbfolge == 1) // normale Klamme zu. Nicht im Zusammenhang mit dem Exponent!
                {
                    if (!AnzeigeIstZahl) { if (!Umwandlung("BtnPotenz")) { return; } }
                    ArrEintragZahl(zahlAnzeige); // Eintrag Exponent
                    txtFormelText += zahlAnzeige.ToString("#,##0.#####");
                    potenzAbfolge = 0;
                    sperreZahl = true;
                    sperreRA1 = false;
                    sperreRA2 = true;
                    sperreKLauf = true;
                    sperreKLzu = false;
                    sperreBerechnung = false;
                    sperreSofortBerechnung = false;
                    potenzRZ = "";
                }
                else
                { // Achtung: ab hier "Wurzelziehen" Beispiel: 27^(1/3) = 27^0,3333 = 3. -------------------------------
                    switch (potenzRZ)
                    {
                        case "(":
                            potenzAbfolge = 2;
                            txtFormelText += "(";
                            sperreKLauf = true;
                            break;
                        case "/":
                            potenzAbfolge = 3;
                            if (!AnzeigeIstZahl) { if (!Umwandlung("BtnPotenz")) { return; } }
                            txtFormelText += zahlAnzeige.ToString() + "/";
                            txtEingabeText = "0";
                            dividend = zahlAnzeige;
                            sperreRA1 = true;
                            sperreKLzu = false;
                            NormierungEingabe(true);
                            break;
                        case ")":
                            potenzAbfolge = 0;
                            if (!AnzeigeIstZahl) { if (!Umwandlung("BtnPotenz")) { return; } }
                            txtFormelText += zahlAnzeige.ToString() + ")";
                            txtEingabeText = "0";
                            zahlAnzeige = dividend / zahlAnzeige;
                            ArrEintragZahl(zahlAnzeige); // Eintrag Exponent
                            sperreZahl = true;
                            sperreRA1 = false;
                            sperreRA2 = true;
                            sperreKLauf = true;
                            sperreKLzu = false;
                            sperreBerechnung = false;
                            sperreSofortBerechnung = false;
                            potenzRZ = "";
                            break;
                        default:
                            MessageBox.Show(" BtnPotenz\r\nfehlerhafter Zustand");
                            break;
                    }
                }
            }
        }

        private void BtnProzent_Klick()
        {
            string prozMeldung = "Die %-Rechnung ist kein Bestandteil der sonst. Berechnungsmöglichkeiten." +
                                 "\nFolgende Vorgehensweise ist bei der Prozentrechnung zu empfehlen" +
                                 "\n1. Zuerst [C] betätigen" +
                                 "\n2. eine Zahl eingeben (Grundwert)" +
                                 "\n3. eine der 3 Rechenarten [+], [-] oder [*] eingeben" +
                                 "\n4. eine zweite Zahl eingeben (Prozentsatz)" +
                                 "\n5. [%] betätigen" +
                                 "\n6. [=] betätigen" +
                                 "\n7. Durch Eingabe einer Rechenart, kann das Ergebnis weiterverwendet werden";
            if (prozentFolgeAktion)
            {
                if (prioIndex != 3) { MessageBox.Show(prozMeldung); resultat = 0; }
                else
                {
                    double prozentwert = zahlen[prio[prioIndex - 3] - 100] / 100 * zahlen[prio[prioIndex - 1] - 100];
                    txtFormelText += "%";
                    switch (prio[prioIndex - 2])
                    {
                        case 1:
                            zahlen[prio[prioIndex - 1] - 100] = prozentwert;
                            break;
                        case 2:
                            zahlen[prio[prioIndex - 1] - 100] = prozentwert;
                            break;
                        case 3:
                            zahlen[prio[prioIndex - 1] - 100] = zahlen[prio[prioIndex - 1] - 100] / 100;
                            break;
                        default:
                            MessageBox.Show(prozMeldung);
                            resultat = 0;
                            break;
                    }
                }
                prozentFolgeAktion = false;
                sperreBerechnung = false;
                sperreZahl = true;
            }
            else
            {
                if (prioIndex != 2) { MessageBox.Show(prozMeldung); resultat = 0; }
                else { prozentFolgeAktion = true; }
            }
        }

        private void BtnSin_Klick()
        {
            if (winkelUmkehr)
            {
                if (!AnzeigeIstZahl)
                { if (!Umwandlung("BtnSin")) { return; } }
                zahlAnzeige = Math.Asin(zahlAnzeige);
                zahlAnzeige = zahlAnzeige * 180 / Math.PI;
                txtEingabeText = zahlAnzeige.ToString();
            }
            else
            {
                if (!AnzeigeIstZahl)
                { if (!Umwandlung("BtnSin")) { return; } }
                if (zahlAnzeige == 90 || zahlAnzeige == 270 ||
                    zahlAnzeige == 180 || zahlAnzeige == 360)
                {
                    if (zahlAnzeige == 90) { txtEingabeText = "1"; zahlAnzeige = 1; }
                    if (zahlAnzeige == 270) { txtEingabeText = "1"; zahlAnzeige = -1; }
                    if (zahlAnzeige == 180 || zahlAnzeige == 360) { txtEingabeText = "0"; zahlAnzeige = 0; }

                    txtEingabeText = "0";
                }
                else
                {
                    double winkel = Math.PI * zahlAnzeige / 180.0;
                    zahlAnzeige = Math.Sin(winkel);
                    txtEingabeText = zahlAnzeige.ToString();
                }
            }
            NormierungEingabe(false);
        }

        private void BtnCos_Klick()
        {
            if (winkelUmkehr)
            {
                if (!AnzeigeIstZahl)
                {
                    if (!Umwandlung("BtnSin")) { return; }
                }
                zahlAnzeige = Math.Acos(zahlAnzeige);
                zahlAnzeige = zahlAnzeige * 180 / Math.PI;
                txtEingabeText = zahlAnzeige.ToString();
            }
            else
            {
                if (!AnzeigeIstZahl)
                { if (!Umwandlung("BtnCos")) { return; } }
                if (zahlAnzeige == 90 || zahlAnzeige == 270 ||
                    zahlAnzeige == 180 || zahlAnzeige == 360)
                {
                    if (zahlAnzeige == 90 || zahlAnzeige == 270) { txtEingabeText = "0"; zahlAnzeige = 0; }
                    if (zahlAnzeige == 180) { txtEingabeText = "1"; zahlAnzeige = -1; }
                    if (zahlAnzeige == 360) { txtEingabeText = "1"; zahlAnzeige = 1; }
                }
                else
                {
                    double winkel = Math.PI * zahlAnzeige / 180.0;
                    zahlAnzeige = Math.Cos(winkel);
                    txtEingabeText = zahlAnzeige.ToString();
                }
            }
            NormierungEingabe(false);
        }

        private void BtnTan_Klick()
        {
            if (winkelUmkehr)
            {
                if (!AnzeigeIstZahl)
                {
                    if (!Umwandlung("BtnSin")) { return; }
                }
                zahlAnzeige = Math.Atan(zahlAnzeige);
                zahlAnzeige = zahlAnzeige * 180 / Math.PI;
                txtEingabeText = zahlAnzeige.ToString();
            }
            else
            {
                if (!AnzeigeIstZahl)
                { if (!Umwandlung("BtnCos")) { return; } }
                if (zahlAnzeige == 90 || zahlAnzeige == 270 ||
                    zahlAnzeige == 180 || zahlAnzeige == 360)
                {
                    if (zahlAnzeige == 90 || zahlAnzeige == 270) { txtEingabeText = "ungültige Eingabe"; AnzeigeIstZahl = false; }
                    if (zahlAnzeige == 180 || zahlAnzeige == 360) { txtEingabeText = "0"; zahlAnzeige = 0; }
                }
                else
                {
                    double winkel = Math.PI * zahlAnzeige / 180.0;
                    zahlAnzeige = Math.Tan(winkel);
                    txtEingabeText = zahlAnzeige.ToString();
                }
            }
            NormierungEingabe(false);
        }

        private void BtnKlammerAuf_Klick()
        {
            if (potenzAbfolge > 0) { potenzRZ = "("; BtnPotenz_Klick(); return; }
            if (sperreKLauf)
            {
                MessageBox.Show("BtnKlammerAuf\r\nKlammer \"Auf\" ist zurzeit gesperrt.");
                return;
            }
            if (prioIndex == 0) { txtFormelText = ""; }
            ArrEintragZeichen(klammerNR + 50);
            klammerNR += 1;
            txtFormelText += "(";
            txtEingabeText = "0";
            sperreZahl = false;
            sperreRA1 = true;
            sperreRA2 = true;
            sperreKLauf = false;
            sperreKLzu = false;
            sperreBerechnung = true;
            sperreSofortBerechnung = false;
        }

        private void BtnKlammerZu_Klick()
        {
            if (potenzAbfolge > 0)
            {
                potenzRZ = ")"; BtnPotenz_Klick();
                //if (potenzAbfolge != 0) { return; }
                return;
            }
            if (sperreKLzu)
            {
                MessageBox.Show("BtnKlammerZu\r\nKlammer \"Zu\" ist zurzeit gesperrt.");
                return;
            }
            if (!sperreZahl)
            {
                if (!AnzeigeIstZahl) { if (!Umwandlung("BtnKlammerZu")) { return; } }
                txtFormelText += zahlAnzeige.ToString("#,##0.#####");
                ArrEintragZahl(zahlAnzeige);
            }
            txtFormelText += ")";
            if (klammerNR > 0) { klammerNR = klammerNR - 1; }
            ArrEintragZeichen(klammerNR + 50);
            NormierungEingabe(true);
            txtEingabeText = "0";
            sperreKLzu = false;
            if (klammerNR < 1) { sperreKLzu = true; }
            sperreZahl = true;
            sperreRA1 = false;
            sperreRA2 = false;
            sperreKLauf = true;
            sperreBerechnung = false;
            sperreSofortBerechnung = true;
        }

        private void BtnGleich_Klick()
        {
            if (sperreBerechnung) { MessageBox.Show("BtnGleich\r\nDieseRechenart ist zurzeit gesperrt."); return; }
            if (potenzAbfolge > 0) { potenzRZ = "Abschluss"; BtnPotenz_Klick(); }
            if (prioIndex < 2)
            {
                MessageBox.Show("Zu wenig Parameter. Berechnung nicht möglich.");
                return;
            }
            if (klammerNR != 0)
            { MessageBox.Show("BtnGleich\r\nKlammern nicht regelkonform"); return; }
            if (!sperreZahl)
            {
                if (!AnzeigeIstZahl)
                { if (!Umwandlung("Berechnung")) { return; } }
                ArrEintragZahl(zahlAnzeige);
                txtFormelText += zahlAnzeige.ToString("#,##0.#####");
            }
            if (prozentFolgeAktion) { BtnProzent_Klick(); }
            txtFormelText += "=";
            ArrEintragZeichen(0);
            resultat = rechne.Berechnungen(prioIndex, zahlenIndex, prio, zahlen);
            txtFormelText += "\r\n" + resultat.ToString("#,##0.#####");
            txtEingabeText = resultat.ToString("#,##0.###############################");
            zahlAnzeige = resultat; AnzeigeIstZahl = true; // Mit dem Ergebnis kann weitergerechnet werden.
            prio[0] = 0; prio[1] = 0; prioIndex = 0;
            zahlen[0] = 0; zahlen[1] = 0; zahlenIndex = 0;
            NormierungEingabe(false); // true setzt txtErgebnis.Text = "" und AnzeigeIstZahl = false .
            sperreZahl = false;
            sperreRA1 = false;
            sperreRA2 = false;
            sperreKLauf = false;
            sperreKLzu = true;
            sperreBerechnung = true;
            sperreSofortBerechnung = false;
            folgeAktion = true;
        }

        private void BtnMS_Klick()
        {
            if (!AnzeigeIstZahl)
            { if (!Umwandlung("BtnMS_Click")) { return; } }
            memory = zahlAnzeige;
            lblMemoryText = memory.ToString("#,##0.#####");

        }

        private void BtnMplus_Klick()
        {
            if (!AnzeigeIstZahl)
            { if (!Umwandlung("BtnMS_Click")) { return; } }
            memory = memory + zahlAnzeige;
            lblMemoryText = memory.ToString("#,##0.#####");
        }
        private void BtnMminus_Klick()
        {
            if (!AnzeigeIstZahl)
            { if (!Umwandlung("BtnMS_Click")) { return; } }
            memory = memory - zahlAnzeige;
            lblMemoryText = memory.ToString("#,##0.#####");
        }

        private void BtnMR_Klick()
        {
            zahlAnzeige = memory;
            AnzeigeIstZahl = true;
            txtEingabeText = memory.ToString("#,##0.#####");
        }

        private void BtnMC_Klick()
        { memory = 0; lblMemoryText = "0"; }

        private bool Umwandlung(string ursprung)
        {
            if ((Double.TryParse(txtEingabeText, out zahlAnzeige)))
            { AnzeigeIstZahl = true; return true; }  // kein Fehler
            else
            {
                MessageBox.Show("Methode = " + ursprung + "\n" + "Es wurde keine korrekte Zahl eingegeben!\nBitte wiederholen sie die Eingabe");
                txtEingabeText = "0";
                return false;   // Fehler
            }
        }

        private void TrageWertInTxtErgebnisEin(double wert)
        {
            txtEingabeText = wert.ToString("#,##0.#####");
            AnzeigeIstZahl = true;
        }

        private void ArrEintragZahl(double wert)
        {
            if (prioIndex > 49 || zahlenIndex > 24)
            {
                MessageBox.Show("Abbruch: Arraygröße wurde überschritten");
                Environment.Exit(0);
            }
            zahlen[zahlenIndex] = wert;
            prio[prioIndex] = zahlenIndex + 100;
            prioIndex += 1;
            zahlenIndex += 1;
        }

        private void ArrEintragZeichen(int i_rArt)
        {
            if (prioIndex > 49)
            {
                MessageBox.Show("Abbruch: Arraygröße wurde überschritten");
                Environment.Exit(0);
            }
            prio[prioIndex] = i_rArt;
            prioIndex += 1;
        }

        private void NormierungEingabe(bool zusatzLoeschung)
        {
            firstChar = true;
            kommaSperre = false;
            exponentSperre = false;
            if (zusatzLoeschung) { txtEingabeText = "0"; AnzeigeIstZahl = false; }
        }

        private void NormiereBerechnung(bool zusatzLoeschung)
        {
            txtFormelText = "";
            resultat = 0;
            folgeAktion = false;
            zahlenIndex = 0;
            potenzAbfolge = 0;
            if (zusatzLoeschung) { txtEingabeText = "0"; }
        }

        private void BtnDel_Klick()
        {
            if (txtEingabeText != "")
            {
                if (AnzeigeIstZahl)
                {
                    txtEingabeText = zahlAnzeige.ToString("#,##0.#####");
                    AnzeigeIstZahl = false;
                }
                txtEingabeText = txtEingabeText.Substring(0, txtEingabeText.Length - 1);
            }
            else { BtnCE_Klick(); }
        }

        private void BtnCE_Klick()
        {
            NormierungEingabe(true);
            potenzAbfolge = 0;
            AnzeigeIstZahl = false;
            sperreZahl = false;
            sperreRA1 = true;
            sperreRA2 = true;
            sperreKLauf = false;
            sperreKLzu = true;
            sperreBerechnung = true;
            sperreSofortBerechnung = false;
        }

        private void BtnC_Klick()
        {
            NormierungEingabe(true);
            NormiereBerechnung(true);
            klammerNR = 0;
            AnzeigeIstZahl = false;
            sperreZahl = false;
            sperreRA1 = true;
            sperreRA2 = true;
            sperreKLauf = false;
            sperreKLzu = true;
            sperreBerechnung = true;
            sperreSofortBerechnung = false;
            prioIndex = 0;
            zahlenIndex = 0;
        }
    }
}
