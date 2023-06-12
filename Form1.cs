using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizier2019
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string filetext = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы txt (*.txt)|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string file = dialog.FileName;
                filetext = System.IO.File.ReadAllText(file);
                textBox1.Text = filetext;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            filetext = textBox1.Text;
            List<Token> tokens = new List<Token>();
            LexicalAnalizier lexical = new LexicalAnalizier();
            tokens = lexical.GetTokens(filetext);
            foreach (Token token in tokens)
            {
                textBox2.Text += token.ToString() + Environment.NewLine;
            }
            LR lexR = new LR(tokens);
            lexR.Start();
            MessageBox.Show("Разбор завершён");
        }

       
    }
}
