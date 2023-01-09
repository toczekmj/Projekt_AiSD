using Projekt.Libs.TrieClass;

namespace Projekt.Libs.Trie;



public class Trie
{
    //korzeń
    private readonly Node _root;
    //limit jak głęboko w drzewie mamy szukać
    public int Limit { get; set; }

    public Trie()
    {
        _root = new Node(' ');
    }

    /// <summary>
    /// Dodajemy listę słów
    /// </summary>
    /// <param name="coll"></param>
    public void AddCollection(IEnumerable<string> coll)
    {
        foreach (var item in coll)
        {
            AddWord(item);
        }
    }
    
    //dodajemy słowo do drzewa
    private void AddWord(string word)
    {
        var current = _root;

        //dla każdej litery w podanym słowie
        foreach (var c in word)
        {
            //jeżeli dzieci obecnego node'a nie zawieraja danej literki
            //tworzymy nowe dziecko z podaną wartością
            if (!current.Children.ContainsKey(c))
            {
                current.Children[c] = new Node(c);
            }
            //jeżeli zawierają, przechodzimy do niego
            current = current.Children[c];
        }

        current.IsWord = true;
    }
    
    /// <summary>
    /// funkcja zwraca podpowiedzi dla danego słowa wejściowego
    /// </summary>
    /// <param name="input">słowo wejściowe, dla któego bęą szukane podpowiedzi</param>
    /// <returns>lista podpowiedzi</returns>
    public IEnumerable<string> GetSuggestions(string input)
    {
        var current = _root;
        var suggestions = new List<string>();

        //dla każdej literki w słowie wejściowym 
        foreach (var c in input)
        {
            //jeśli dziecko zawiera kolejną literę z wejścia
            //przechodzimy do tego dziecka
            if (current.Children.ContainsKey(c))
            {
                current = current.Children[c];
            }
            //jeżeli nie zawiera, to zwracamy pustą liste
            //bo znaczy to, że w danym branchu nie będzie więcej słów / podpowiedzi
            //bazujących na wczytanych do tej pory literach
            else
            {
                return suggestions;
            }
        }
        
        //dla znalezionego możliwie najgłębszego node'a wywołujemy funkcję szukającą podpowiedzi
        FindWords(current, suggestions, Limit, input, true);

        return suggestions;
    }

    private  static void FindWords(Node node, ICollection<string> words, int limit, string word = "", bool first = false)
    {
        //sprawdzamy czy badany node jest kompletym słowem, czy tylko kolejną literką w słowie
        //jeżeli jest słowem
        if (node.IsWord)
        {
            //jeżeli nie jest pierwszym znalezionym znakiem
            //dodajemy do istniejącego słowa kolejne znaki
            if(!first)
                word += node.Value;
            //dodajemy słowo do listy wyjściowej
            words.Add(word);
            
            //jeśli został osiągnięty ustalony limit słów, funkcja kończy się 
            if(words.Count == limit)
                return;
        }
        //jeżeli nie jest słowem
        else
        {
            //jeżeli nie jest pierwszym znakime dodajmy kolejne znaki do istniejącego słowa
            if(!first)
                word += node.Value;
        }

        //dla każdego kolejnego znaku który zawiera się w dziecku obecnego node'a 
        //szukamy kolejnych podpowiedzi
        foreach (var child in node.Children.Values)
        {
            FindWords(child, words, limit, word);
            if(words.Count == limit)
                return;
        }

        
    }
}