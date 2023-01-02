using Projekt.Libs.TrieClass;
using System.Text;
using MF = Projekt.Libs.MainFunctionality;

namespace Projekt
{
    public partial class Form1 : Form
    {
        private readonly Trie? _trie;
        private List<string> _dict;
        private int _selectedItem = 0;
        private string lastWord = "";
        private bool _update = true;
        private Dictionary<int, string> mappedKeys = new()
        {
            {2, "abc"},
            {3, "def"},
            {4, "ghi"},
            {5, "jkl"},
            {6, "mno"},
            {7, "pqrs"},
            {8, "tuv"},
            {9, "wxyz"}
        };

        public Form1()
        {
            InitializeComponent();
            _trie = new();
            _dict = new();
            MF.OpenDict(_dict, _trie);
            _trie.Limit = 10;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MF.TextChanged(ref lastWord, _update, ref _selectedItem, mappedKeys, textBox1,listBox1, _trie);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0)
                textBox1.Text = textBox1.Text.Substring(0, textBox1.TextLength - 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += 5;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += 6;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += 7;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += 8;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += 9;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                var word = listBox1.SelectedItem.ToString();
                MF.ReplaceLastWord(word, textBox1);
            }

            textBox1.Text += " ";
            _selectedItem = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _update = false;
            if (_selectedItem > 0)
            {
                _selectedItem--;
                listBox1.SelectedIndex = _selectedItem;
                var word = listBox1.SelectedItem.ToString();
                MF.ReplaceLastWord(word, textBox1);
            }
            else
            {
                _selectedItem--;
                MF.ReplaceLastWord(lastWord, textBox1);
            }
            _update = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            _update = false;
            if (_selectedItem < listBox1.Items.Count - 1)
            {
                _selectedItem++;
                if (_selectedItem < 0)
                    _selectedItem = 0;
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = _selectedItem;
                    var word = listBox1.SelectedItem.ToString();
                    MF.ReplaceLastWord(word, textBox1);
                }

            }
            _update = true;
        }

    }
}