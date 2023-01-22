using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            if (args.Length == 0)
            {
                throw new ArgumentNullException("Err: Brak argumentu (URL)");
            }
            String url = args[0];
            Regex urlRegex = new Regex(@"^(http|ftp|https|www)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$", RegexOptions.IgnoreCase);//
            Match urlMatch = urlRegex.Match(url);
            if (!urlMatch.Success)
            {
                throw new ArgumentException("Err: Niepoprawny format URL.");
            }

            //Crawler

            using (var hc = new HttpClient())
            {

                HttpResponseMessage response = await hc.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    string[] words = content.Split(' ');//split string with spaces

                    Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);//define regex expr (ignore case)

                    List<string> addressList = new List<string>();

                    foreach (var word in words)
                    {
                        Match match = regex.Match(word);
                        if (match.Success)
                        {
                            if (!addressList.Contains($"{match.Value}"))//dont add duplicates
                            {
                                addressList.Add($"{match.Value}");
                            }
                        }
                    }

                    if (addressList.Count == 0) { System.Console.WriteLine("Nie znaleziono adresów email"); }

                    System.Console.WriteLine(String.Join("\n", addressList));//preint addresses

                }
                else
                {
                    throw new Exception("Błąd w czasie pobierania strony");
                }
            }

            //hc.Dispose();
            //1
            //Console.WriteLine("Hello World!");



        }
    }
}
