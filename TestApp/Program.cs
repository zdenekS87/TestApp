using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputText = File.ReadAllText("test.in"); 
            string outputText = string.Empty;

            StringReader strReader = new StringReader(inputText);
            var countTestCase = Int16.Parse(strReader.ReadLine());
            var synonymDictionary = new Dictionary<Tuple<int, string>, string> { };

            for (int i = 1; i <= countTestCase; i++)
            {
                Console.WriteLine("CASES : " + i);
                var countSynonymDictionary = Int16.Parse(strReader.ReadLine());
                synonymDictionary.Clear();

                for (int y = 1; y <= countSynonymDictionary; y++)
                {
                    Console.WriteLine("DICTIONARY : " + y);
                    string[] synonymLine = strReader.ReadLine().Split(' ');
                    synonymLine[0] = synonymLine[0].ToLower();
                    synonymLine[1] = synonymLine[1].ToLower();
                    synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, synonymLine[0]), synonymLine[1]);

                    AddSynonym(synonymLine[0], synonymLine[1], ref synonymDictionary);
                    AddSynonym(synonymLine[1], synonymLine[0], ref synonymDictionary);

                    /*
                    indirectlySynonym = synonymDictionary.Where(x => x.Key.Item2 == synonymLine[0] || x.Value == synonymLine[0]).ToList();
                    if(indirectlySynonym.Any())
                    { 
                        //chybír rekurence
                        foreach (var dictionary in indirectlySynonym)
                        {
                            if(!synonymDictionary.Any(x => x.Key.Item2 == synonymLine[1] && x.Value == dictionary.Value))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, synonymLine[1]), dictionary.Value);
                            if (!synonymDictionary.Any(x => x.Key.Item2 == synonymLine[1] && x.Value == dictionary.Key.Item2))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, synonymLine[1]), dictionary.Key.Item2);
                            if (!synonymDictionary.Any(x => x.Key.Item2 == dictionary.Value && x.Value == synonymLine[1]))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, dictionary.Value), synonymLine[1]);
                            if (!synonymDictionary.Any(x => x.Key.Item2 == dictionary.Key.Item2 && x.Value == synonymLine[1]))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, dictionary.Key.Item2), synonymLine[1]);
                        }
                    }

                    indirectlySynonym = synonymDictionary.Where(x => x.Key.Item2 == synonymLine[1] || x.Value == synonymLine[1]).ToList();
                    if (indirectlySynonym.Any())
                    {
                        foreach (var dictionary in indirectlySynonym)
                        {
                            if (!synonymDictionary.Any(x => x.Key.Item2 == synonymLine[0] && x.Value == dictionary.Value))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, synonymLine[0]), dictionary.Value);
                            if (!synonymDictionary.Any(x => x.Key.Item2 == synonymLine[0] && x.Value == dictionary.Key.Item2))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, synonymLine[0]), dictionary.Key.Item2);
                            if (!synonymDictionary.Any(x => x.Key.Item2 == dictionary.Value && x.Value == synonymLine[0]))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, dictionary.Value), synonymLine[0]);
                            if (!synonymDictionary.Any(x => x.Key.Item2 == dictionary.Key.Item2 && x.Value == synonymLine[0]))
                                synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, dictionary.Key.Item2), synonymLine[0]);
                        }
                    }*/
                }

                var countQuery = Int16.Parse(strReader.ReadLine());

                for (int z = 1; z <= countQuery; z++)
                {
                    Console.WriteLine("QUERY : " + z);
                    string[] synonymLine = strReader.ReadLine().Split(' ');
                    synonymLine[0] = synonymLine[0].ToLower();
                    synonymLine[1] = synonymLine[1].ToLower();
                    if (synonymLine[0] == synonymLine[1])
                    {
                        outputText += "synonyms" + Environment.NewLine;
                    }
                    else 
                    {
                        var listOfSynonym = synonymDictionary.Where(x => x.Key.Item2 == synonymLine[0] || x.Value == synonymLine[0]);
                        listOfSynonym = listOfSynonym.Where(x => x.Key.Item2 == synonymLine[1] || x.Value == synonymLine[1]);
                        outputText += (listOfSynonym.Any() ? "synonyms" : "different") + Environment.NewLine;
                    }
                }
            }

            Encoding utf8WithoutBom = new UTF8Encoding(false);
            File.WriteAllText("test.out", outputText, utf8WithoutBom);
        }

        private static void AddSynonym(string forSynonym, string toSynonym, ref Dictionary<Tuple<int, string>, string> synonymDictionary)
        {
            var indirectlySynonym = synonymDictionary.Where(x => x.Key.Item2 == forSynonym || x.Value == forSynonym).ToList();
            if (indirectlySynonym.Any())
            {
                //chybír rekurence
                foreach (var dictionary in indirectlySynonym)
                {
                    if (!synonymDictionary.Any(x => x.Key.Item2 == toSynonym && x.Value == dictionary.Value))
                    { 
                        synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, toSynonym), dictionary.Value);
                        AddSynonym(dictionary.Value, forSynonym, ref synonymDictionary);
                    }

                    if (!synonymDictionary.Any(x => x.Key.Item2 == toSynonym && x.Value == dictionary.Key.Item2))
                    { 
                        synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, toSynonym), dictionary.Key.Item2);
                        AddSynonym(dictionary.Key.Item2, forSynonym, ref synonymDictionary);
                    }

                    if (!synonymDictionary.Any(x => x.Key.Item2 == dictionary.Value && x.Value == toSynonym))
                    { 
                        synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, dictionary.Value), toSynonym);
                    }

                    if (!synonymDictionary.Any(x => x.Key.Item2 == dictionary.Key.Item2 && x.Value == toSynonym))
                    { 
                        synonymDictionary.TryAdd(new Tuple<int, string>(synonymDictionary.Count, dictionary.Key.Item2), toSynonym);
                    }
                }
            }
        }

        public struct Tuple<T1, T2>
        {
            public readonly T1 Item1;
            public readonly T2 Item2;

            public Tuple(T1 item1, T2 item2)
            {
                Item1 = item1;
                Item2 = item2;
            }
        }
    }
}
