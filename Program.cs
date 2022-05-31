using Newtonsoft.Json;
namespace ConversorDeMoedas
{
    class program
    {
        static void Main()
        {
            Console.Clear();
            menu();
        }
        static void menu()
        {
            Console.WriteLine("-------------Conversor de moedas-------------");
            Console.WriteLine("Selecione a moeda que deseja converter");
            Console.WriteLine("1 - Real");
            Console.WriteLine("2 - Dólar");
            Console.WriteLine("3 - Euro");
            Console.WriteLine("0 - Sair");

            var first_currency = int.Parse(Console.ReadLine());
            if (first_currency >= 1 && first_currency <= 3)
            {
                selectSecondCurrency(first_currency);
            };
        }

        static void selectSecondCurrency(int first_currency)
        {
            Console.WriteLine("Para qual moeda?");
            var second_currency = int.Parse(Console.ReadLine());
            if (second_currency >= 1 && second_currency <= 3)
            {
                if (first_currency != second_currency)
                {
                    PrintResult(getCurrencyName(first_currency), getCurrencyName(second_currency));
                }
                else
                {
                    Console.WriteLine("Selecione outra!");
                    selectSecondCurrency(first_currency);
                }
            }
        }

        static string getCurrencyName(int currency)
        {
            var currency_name = "";
            switch (currency)
            {
                case 1: currency_name = "BRL"; break;
                case 2: currency_name = "USD"; break;
                case 3: currency_name = "EUR"; break;
            }
            return currency_name;
        }

        static double getPrice(string first_currency, string second_currency)
        {
            HttpClient client = new HttpClient();
            var url = $"https://economia.awesomeapi.com.br/last/{first_currency}-{second_currency}";
            var req = client.GetStreamAsync(url).Result;
            var contentJSON = new StreamReader(req).ReadToEnd();

            // o nome variava conforme a moeda que era convertida, dificultando o acesso pela classe. Dessa forma, substitui o nome que era por exemplo USDBRL, EURBRL etc para DATA
            var contentJSON_geral = contentJSON.Replace(first_currency + second_currency, "DATA");
            var result = JsonConvert.DeserializeObject<root>(contentJSON_geral);
            var price = double.Parse(result.DATA.bid.Replace(".", ","));

            return price;
        }

        static void PrintResult(string first_currency, string second_currency)
        {
            Console.WriteLine("Qual valor?");
            var value = double.Parse(Console.ReadLine());
            var price = getPrice(first_currency, second_currency);
            var result = value * price;

            Console.WriteLine($"O resultado é: {result.ToString("N2")}");
        }

    }
}