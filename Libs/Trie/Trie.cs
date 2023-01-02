namespace Projekt.Libs.TrieClass;



public class Trie
{
    private readonly Node root;
    public int Limit { get; set; }

    public Trie()
    {
        root = new Node(' ');
    }

    public void AddCollection(ICollection<string> coll)
    {
        foreach (var item in coll)
        {
            AddWord(item);
        }
    }

    public void AddWord(string word)
    {
        var current = root;

        foreach (char c in word)
        {
            if (!current.Children.ContainsKey(c))
            {
                current.Children[c] = new Node(c);
            }

            current = current.Children[c];
        }

        current.IsWord = true;
    }

    public List<string> GetSuggestions(string input)
    {
        var current = root;
        var suggestions = new List<string>();
        //var flag = false;



        foreach (var c in input)
        {
            if (current.Children.ContainsKey(c))
            {
                current = current.Children[c];
            }
            else
            {
                return suggestions;
            }
        }

        //Parallel.ForEach(input, (c, state) =>
        //{
        //    if (current.Children.ContainsKey(c))
        //    {
        //        current = current.Children[c];
        //    }
        //    else
        //    {
        //        flag = true;
        //        state.Break();   
        //    }
        //});

        //if (flag)
        //    return suggestions;
        
        FindWords(current, suggestions, Limit, input, true);

        return suggestions;
    }

    private  static void FindWords(Node node, ICollection<string> words, int limit, string word = "", bool first = false)
    {
        if (node.IsWord)
        {
            if(!first)
                word += node.Value;
            words.Add(word);
            
            if(words.Count == limit)
                return;
        }
        else
        {
            if(!first)
                word += node.Value;
        }

        foreach (var child in node.Children.Values)
        {
            FindWords(child, words, limit, word);
            if(words.Count == limit)
                return;
        }

        
    }
}