using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rechner
{
    public partial class Form1 : Form
    {
        EingabeVerarbeiten verarbeitung = new EingabeVerarbeiten();
        bool winkelUmkehr = false;       // wird zum Ändern der Tastenfunktionen sin, cos und tan benötigt..

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true; //Wenn diese Eigenschaft auf "true"festgelegt wird, 
                                    // erhält das Formular alle KeyPress, KeyDown, und KeyUp Ereignisse.
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress); // aktiviert beim Ereignis KeyPress die Methode "Form1_KeyPress".
            this.TxtFormel.Multiline = true;
            this.TxtFormel.Size = new Size(274, 73);
            TxtFormel.Text = "";
            //this.TxtEingabe.Multiline = false;// 
            //this.txtEingabe.MinimumSize = new Size(274, 50);
            TxtEingabe.Text = "0";
        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        { // Tastatureingabe verarbeiten
            verarbeitung.TastaturVerarbeiten(e.KeyChar);
            e.Handled = true; // verhindert das das Zeichen noch einmal ausgegeben wird. genaue Arbeitsweise muss noch untersucht werden.
            UebernahmeVariablen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Btn0.Click += new EventHandler(WeitergabeZiffer);
            BtnKomma.Click += new EventHandler(WeitergabeZiffer);
            Btn1.Click += new EventHandler(WeitergabeZiffer);
            Btn2.Click += new EventHandler(WeitergabeZiffer);
            Btn3.Click += new EventHandler(WeitergabeZiffer);
            Btn4.Click += new EventHandler(WeitergabeZiffer);
            Btn5.Click += new EventHandler(WeitergabeZiffer);
            Btn6.Click += new EventHandler(WeitergabeZiffer);
            Btn7.Click += new EventHandler(WeitergabeZiffer);
            Btn8.Click += new EventHandler(WeitergabeZiffer);
            Btn9.Click += new EventHandler(WeitergabeZiffer);
            //---------------------------------------------------
            BtnPI.Click += new EventHandler(Weitergabe);
            BtnFE.Click += new EventHandler(Weitergabe);
            BtnExp.Click += new EventHandler(Weitergabe);
            BtnPlusMinus.Click += new EventHandler(Weitergabe);
            BtnAddieren.Click += new EventHandler(Weitergabe);
            BtnSubtrahieren.Click += new EventHandler(Weitergabe);
            BtnMultiplizieren.Click += new EventHandler(Weitergabe);
            BtnDividieren.Click += new EventHandler(Weitergabe);
            BtnQuadrat.Click += new EventHandler(Weitergabe);
            BtnWurzel.Click += new EventHandler(Weitergabe);
            BtnPotenz.Click += new EventHandler(Weitergabe);
            BtnKehrwert.Click += new EventHandler(Weitergabe);
            BtnProzent.Click += new EventHandler(Weitergabe);
            BtnKlammerAuf.Click += new EventHandler(Weitergabe);
            BtnKlammerZu.Click += new EventHandler(Weitergabe);
            BtnSin.Click += new EventHandler(Weitergabe);
            BtnCos.Click += new EventHandler(Weitergabe);
            BtnTan.Click += new EventHandler(Weitergabe);
            BtnDel.Click += new EventHandler(Weitergabe);
            BtnCE.Click += new EventHandler(Weitergabe);
            BtnC.Click += new EventHandler(Weitergabe);
            BtnGleich.Click += new EventHandler(Weitergabe);
            BtnMC.Click += new EventHandler(Weitergabe);
            BtnMR.Click += new EventHandler(Weitergabe);
            BtnMminus.Click += new EventHandler(Weitergabe);
            BtnMplus.Click += new EventHandler(Weitergabe);
            BtnMS.Click += new EventHandler(Weitergabe);
        }

        private void WeitergabeZiffer(object sender, EventArgs e)
        {
            // https://msdn.microsoft.com/de-de/library/system.windows.forms.button(v=vs.110).aspx
            Button ziffer = sender as Button;
            verarbeitung.ButtonVerarbeitungZiffern(ziffer.Name);
            UebernahmeVariablen();
        }

        private void Weitergabe(object sender, EventArgs e)
        {
            // https://msdn.microsoft.com/de-de/library/system.windows.forms.button(v=vs.110).aspx
            Button ziffer = sender as Button;
            verarbeitung.ButtonVerarbeitung(ziffer.Name, winkelUmkehr);
            UebernahmeVariablen();
        }

        private void UebernahmeVariablen()
        {
            TxtEingabe.Text = verarbeitung.txtEingabeText;
            TxtFormel.Text = verarbeitung.txtFormelText;
            LblMemory.Text = verarbeitung.lblMemoryText;
            TxtEingabe.Focus();
            TxtEingabe.Select(TxtEingabe.Text.Length, 0);
        }

        private void BtnFunktionsaenderung_Click(object sender, EventArgs e)
        {
            if (winkelUmkehr)
            {
                BtnSin.Text = "sin";
                BtnCos.Text = "cos";
                BtnTan.Text = "tan";
                winkelUmkehr = false;
            }
            else
            {
                BtnSin.Text = "sin-1";
                BtnCos.Text = "cos-1";
                BtnTan.Text = "tan-1";
                winkelUmkehr = true;
            }
        }
    }
}
