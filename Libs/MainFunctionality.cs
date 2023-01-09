using Projekt.Libs.TrieClass;
using System.Text;

namespace Projekt.Libs
{
    internal static class MainFunctionality
    {
        /// <summary>
        /// Funkcja odpowiedzialna za wczytanie słownika wskazanego przez użytkownika
        /// </summary>
        /// <param name="trie">wskaźnik na korzeń drzewa do któego będą załadowane </param>
        public static async void OpenDict(Trie.Trie trie, Form1 form)
        {
            var fileContent = string.Empty;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    using var reader = new StreamReader(fileStream);
                    form.Enabled = false;
                    form.Text = @"No dictionary loaded.";
                    var dialogThread = new Thread(() =>
                    {
                        MessageBox.Show(@"Czytam plik. Jezeli jest on bardzo dlugi (>20000 linii) moze to chwile potrwac.", "Otwieram slownik.", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    });
                    dialogThread.Start();
                    fileContent = await reader.ReadToEndAsync();
                    form.Enabled = true;
                    form.Text = $@"{Path.GetFileName(filePath)} dictionary loaded.";
                }
            }
            List<string> dict = fileContent.Split('\n').ToList();
            trie.AddCollection(dict);
        }


        /// <summary>
        /// funkcja odpowiedzialna za obsługę wpisanego tekstu
        /// </summary>
        /// <param name="lastWord">ostatnie wpisane słowo</param>
        /// <param name="update">flaga mówiąca czy aktualizować stan textboxa</param>
        /// <param name="selectedItem">element wybrany z listy podpowiedzi</param>
        /// <param name="mappedKeys">słownik z klawiszami</param>
        /// <param name="textBox1">pole tekstowe</param>
        /// <param name="listBox1">lista podpowiedzi</param>
        /// <param name="trie">wskaźnik do korzenia drzewa</param>
        public static void TextChanged(ref string lastWord, bool update, ref int selectedItem, Dictionary<int, string> mappedKeys,
            TextBox textBox1, ListBox listBox1, Trie.Trie trie)
        {
            //sprawdzam czy trzeba aktualizować wejście
            //na przykład w momencie gdy wybieramy inne słowo z listy podpowiedzi
            //nie chcemy od nowa przeliczać wszystkiego
            if (!update)
                return;

            List<string> words = new();
            //wybieramy sobie ostatnio wpisane słowo
            var i = GetLastWord(textBox1);
            //walidujemy wczytane słowo - sprawdzamy czy zawiera tylko cyfry
            var containsOnlyNumbers = System.Text.RegularExpressions.Regex.IsMatch(i, @"^[0-9]+$");

            //jeśli nie, przyjmujemy że zawiera tylko litery, więc zamieniamy je na cyfry
            if (!containsOnlyNumbers)
                i = ConvertWordToNumbers(i, mappedKeys);

            //jeżeli wczytane słowo nie jest puste
            //oraz jeżeli słowo nie jest spacją
            if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text[textBox1.TextLength - 1] != ' ')
                //generujemy wszystkie możliwe kombinacje klawiszy
                words = GetCombinations(mappedKeys, i).ToList();

            lastWord = i;

            //sprawdzamy czy lista wygenerowanych kombinacji nie jest pusta
            if (words is null || (words is not null && words.Count == 0))
            {
                //jeśli tak, to czyścimy listę odpowiedzi
                listBox1.Items.Clear();
                selectedItem = 0;
                return;
            }

            List<string> output = new();
            
            //dla każdej kombinacji szukamy podpowiedzi w słowniku
            foreach (var item in words)
                output.AddRange(trie.GetSuggestions(item));

            //czyscimy listę podpowiedzi i dodajemy do niej nowe, posortowane po długości
            listBox1.Items.Clear();
           var temp = output.OrderBy(x => x.ToString().Length);
            foreach (var item in temp)
                listBox1.Items.Add(item);
            //zaznaczny pierwszy element na liście jako domyślny
            selectedItem = 0;
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = selectedItem;
        }

        /// <summary>
        /// Funkcja zamienia słowa na liczby
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mappedKeys"></param>
        /// <returns>słowo zamienione na jego licznbowy odpowiednik klawiatury T9</returns>
        private static string ConvertWordToNumbers(string input, Dictionary<int, string> mappedKeys)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
            {
                foreach (var key in mappedKeys.Where(key => key.Value.Contains(c)))
                {
                    sb.Append(key.Key);
                    break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// funkcja odpowiedzialna za wygenerowanie wszystkich możliwych kombinacji słów z wciśniętych klawiszy
        /// </summary>
        /// <param name="mappedKeys">słownik klawiszy</param>
        /// <param name="input">dane wejściowe (liczby)</param>
        /// <returns>wszystkie możliwe kombinacje słów</returns>
        private static IEnumerable<string> GetCombinations(IReadOnlyDictionary<int, string> mappedKeys, string input)
        {
            if (input.Length == 0)
            {
                yield return "";
            }
            else
            {
                foreach (var c in mappedKeys[int.Parse(input[0].ToString())])
                {
                    foreach (var combination in GetCombinations(mappedKeys, input.Substring(1)))
                    {
                        yield return c + combination;
                    }
                }
            }
        }

        /// <summary>
        /// funkcja odpowiedzialna za zamienianie ostatnio wpisanego do pola tekstowego słowa
        /// </summary>
        /// <param name="val">wartość na jaką ma być zamienione słowo</param>
        /// <param name="textBox1">referencja do pola tekstowego</param>
        public static void ReplaceLastWord(string val, TextBox textBox1)
        {
            var temp = textBox1.Text.TrimEnd();
            //więcej niż jedno słowo w polu tekstowym
            if (temp.Contains(' '))
            {
                var startIndex = textBox1.Text.LastIndexOf(' ');
                var s = textBox1.Text.Remove(startIndex);
                textBox1.Text = s + ' ' + val;
            }
            //tylko jedno słowo w polu
            else
            {
                textBox1.Text = val;
            }
        }

        /// <summary>
        /// funkcja zwracająca ostatnie wpisane do pola tekstowego słowo 
        /// </summary>
        /// <param name="textBox1">refrencja do pola tekstowego</param>
        /// <returns>ostatnio wpisane słowo</returns>
        private static string GetLastWord(TextBox textBox1)
        {
            var temp = textBox1.Text.TrimEnd();
            //więcej niż jedno słowo w polu tekstowym
            if (temp.Contains(' ')) 
            {
                var startIndex = textBox1.Text.LastIndexOf(' ');
                var endIndex = textBox1.TextLength;
                var s = textBox1.Text.Substring(startIndex, endIndex - startIndex);
                return s.Trim();
            }
            //tylko jedno słowo
            else
            {
                return textBox1.Text.Trim();
            }

        }
    }
}
