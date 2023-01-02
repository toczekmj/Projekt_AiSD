using Projekt.Libs.TrieClass;
using System.Text;

namespace Projekt.Libs
{
    internal static class MainFunctionality
    {

        public static void OpenDict(IList<string> _dict, Trie _trie)
        {
            var fileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string? filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    using StreamReader reader = new StreamReader(fileStream);
                    fileContent = reader.ReadToEnd();
                }
            }
            _dict = fileContent.Split('\n').ToList();
            _trie.AddCollection(_dict);
        }

        public static void TextChanged(ref string lastWord, bool _update, ref int _selectedItem, Dictionary<int, string> mappedKeys,
            TextBox textBox1, ListBox listBox1, Trie _trie)
        {
            if (!_update)
                return;

            List<string> words = new();
            var i = GetLastWord(textBox1);
            bool containsOnlyNumbers = System.Text.RegularExpressions.Regex.IsMatch(i, @"^[0-9]+$");

            if (!containsOnlyNumbers)
                i = ConvertWordToNumbers(i, mappedKeys);

            if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text[textBox1.TextLength - 1] != ' ')
                words = GetAllCombinations(i, mappedKeys);

            lastWord = i;

            if (words is null || (words is not null && words.Count == 0))
            {
                listBox1.Items.Clear();
                _selectedItem = 0;
                return;
            }

            List<string> output = new();

            foreach (var item in words)
                output.AddRange(_trie.GetSuggestions(item));

            listBox1.Items.Clear();
            var temp = output.OrderBy(x => x.ToString().Length);

            foreach (var item in temp)
                listBox1.Items.Add(item);
            



            _selectedItem = 0;
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = _selectedItem;
        }

        public static string ConvertWordToNumbers(string input, Dictionary<int, string> mappedKeys)
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

        public static List<string> GetAllCombinations(string str, Dictionary<int, string> mappedKeys)
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

        public static void ReplaceLastWord(string val, TextBox textBox1)
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

        private static string GetLastWord(TextBox textBox1)
        {
            var temp = textBox1.Text.TrimEnd();
            if (temp.Contains(' ')) //więcej niż jedno słowo w polu tekstowym
            {
                var startIndex = textBox1.Text.LastIndexOf(' ');
                var endIndex = textBox1.TextLength;
                var s = textBox1.Text.Substring(startIndex, endIndex - startIndex);
                return s.Trim();
            }
            else  //tylko jedno słowo
            {
                return textBox1.Text.Trim();
            }

        }
    }
}
