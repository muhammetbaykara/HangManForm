using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HangManForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_(object sender, MouseEventArgs e)
        {
        }

        int Move;
        int Mouse_X;
        int Mouse_Y;

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        int hataSayac = 0;
        string[] kelimeler = { "Şehirler", "İsimler", "Ülkeler","Meyveler", "Nesneler"};
        Random rnd = new Random();
        List<string> secilenKelimeler = new List<string>();
        string kelime = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            oyunOlustur();
        }

        private void oyunOlustur()
        {
            pictureBox1.Load("Resimler/" + hataSayac + ".png");
            label3.Text = kelimeler[rnd.Next(kelimeler.Length)];
            FileStream fs = new FileStream("Kelimeler/" + label3.Text + ".txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string yazi = sw.ReadLine();
            while (yazi != null)
            {
                secilenKelimeler.Add(yazi.ToUpper());
                yazi = sw.ReadLine();
            }
            sw.Close();
            fs.Close();
            kelime = secilenKelimeler[rnd.Next(secilenKelimeler.Count)];

            lblTahmin.Text = "";
            for (int j = 0; j < kelime.Length; j++) 
                lblTahmin.Text += "_ ";
        }

        private void Oyun(object sender, EventArgs e)
        {
            Button seciliBtn = sender as Button;
            seciliBtn.Enabled = false;
            if (!kelime.Contains(seciliBtn.Text))
            {
                hataSayac++;
                pictureBox1.Load("Resimler/" + hataSayac + ".png");
                label5.Text = (11 - hataSayac).ToString();
            }
            else
            {
                string text = lblTahmin.Text.Replace(" ", "");
                for (int i = 0; i < kelime.Length; i++)
                {
                    if (kelime[i].ToString() == seciliBtn.Text)
                        text = ReplaceAt(text, i, 1, seciliBtn.Text);
                }

                string sonuc = "";
                for (int j = 0; j < text.Length; j++) 
                    sonuc += text[j].ToString() + " ";
                lblTahmin.Text = sonuc;
            }

            if (label5.Text == "0")
            {
                DialogResult dr = MessageBox.Show("MAALESEF KAYBETTİNİZ! ", "Kelime:" + kelime + " Tekrar oynamak ister misiniz?", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    oyunBitir();
                else
                    Application.Exit();
            }

            if (lblTahmin.Text.Replace(" ", "") == kelime)
            {
                DialogResult dr = MessageBox.Show("TEBRİKLER KAZANDINIZ! ", "Kelime: " + kelime + "- Tekrar oynamak ister misiniz?", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    oyunBitir();
                else
                    Application.Exit();
            }
        }

        private void oyunBitir()
        {
            foreach (Control btns in this.Controls)
                if (btns is Button)
                    ((Button)btns).Enabled = true;

            lblTahmin.Text = "";
            label5.Text = "11";
            hataSayac = 0;
            oyunOlustur();
        }

        public string ReplaceAt(string str, int index, int lenght, string replace)
        {
            return str.Remove(index, Math.Min(lenght, str.Length - index)).Insert(index, replace);
        }
    }
}
