using Backend;
using System.IO;
using System.Reflection;
using System.Text;

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
            OpenDict();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void OpenDict()
        {
            var filePath = string.Empty;
            var fileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    using StreamReader reader = new StreamReader(fileStream);
                    fileContent = reader.ReadToEnd();
                }
            }
            _dict = fileContent.Split('\n').ToList();
            _trie.AddCollection(_dict);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!_update)
                return;

            List<string> words = new();
            var i = GetLastWord();
            bool containsOnlyNumbers = System.Text.RegularExpressions.Regex.IsMatch(i, @"^[0-9]+$");

            if(!containsOnlyNumbers)
                i = ConvertWordToNumbers(i);

            if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text[textBox1.TextLength-1] != ' ')
                words = GetAllCombinations(i);

            lastWord = i;

            if (words is null || (words is not null && words.Count == 0))
            {
                listBox1.Items.Clear();
                _selectedItem = 0;
                return;
            }

            List<string> output = new();

            foreach (var item in words)
            {
                output.AddRange(_trie.GetSuggestions(item));
            }

            listBox1.Items.Clear();
            var temp = output.OrderBy(x => x.ToString().Length);

            foreach (var item in temp)
            {
                listBox1.Items.Add(item);
            }

            

            _selectedItem = 0;
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = _selectedItem;
        }

        private string ConvertWordToNumbers(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                foreach (var key in mappedKeys)
                {
                    if (key.Value.Contains(c))
                    {
                        sb.Append(key.Key);
                        break;
                    }
                }
            }
            return sb.ToString();
        }

        private List<string> GetAllCombinations(string str)
        {
            var outList = new List<string>();
            foreach (string combination in GetCombinations(mappedKeys, str))
            {
                outList.Add(combination);
            }
            return outList;
        }

        private static IEnumerable<string> GetCombinations(Dictionary<int, string> mappedKeys, string input)
        {
            if (input.Length == 0)
            {
                yield return "";
            }
            else
            {
                foreach (char c in mappedKeys[int.Parse(input[0].ToString())])
                {
                    foreach (string combination in GetCombinations(mappedKeys, input.Substring(1)))
                    {
                        yield return c + combination;
                    }
                }
            }
        }

        private string GetLastWord()
        {
            var temp = textBox1.Text.TrimEnd();
            if (temp.Contains(' ')) //wiêcej ni¿ jedno s³owo w polu tekstowym
            {
                var startIndex = textBox1.Text.LastIndexOf(' ');
                var endIndex = textBox1.TextLength;
                var s = textBox1.Text.Substring(startIndex, endIndex - startIndex);
                return s.Trim();
            }
            else  //tylko jedno s³owo
            {
                return textBox1.Text.Trim();
            }
        }

        private void ReplaceLastWord(string val)
        {
            var temp = textBox1.Text.TrimEnd();
            if (temp.Contains(' '))
            {
                var startIndex = textBox1.Text.LastIndexOf(' ');
                var s = textBox1.Text.Remove(startIndex);
                textBox1.Text = s + ' ' + val;
            }
            else
            {
                textBox1.Text = val;
            }
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
            if(listBox1.Items.Count > 0)
            {
                var word = listBox1.SelectedItem.ToString();
                ReplaceLastWord(word);
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
                ReplaceLastWord(word);
            }
            else
            {
                _selectedItem--;
                ReplaceLastWord(lastWord);
            }
            _update = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            _update = false;
            if (_selectedItem < listBox1.Items.Count - 1)
            {
                _selectedItem++;
                listBox1.SelectedIndex = _selectedItem;
                var word = listBox1.SelectedItem.ToString();
                ReplaceLastWord(word);
            }
            _update = true;
        }

    }
}