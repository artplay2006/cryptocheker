using HtmlAgilityPack;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;
using Newtonsoft.Json;

// Установка культуры вывода чисел
CultureInfo culture = new CultureInfo("en-US");
culture.NumberFormat.NumberDecimalSeparator = ".";

// Применение установленной культуры
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

//string url = "https://ru.tradingview.com/symbols/BTCUSD/?exchange=CRYPTO";
//string ?htmlCode;
//using (WebClient client = new WebClient())
//{
//    htmlCode = client.DownloadString(url);
//}

//// Создание объекта HtmlDocument и загрузка HTML-кода
////Он анализирует только полученный статический HTML-код!!!!
//var doc = new HtmlDocument();
//doc.LoadHtml(htmlCode);

//// Извлечение информации из doc
//HtmlNode spanElement = doc.DocumentNode.SelectSingleNode("//span[@class='tv-footer-links-column__title']");
//string text = spanElement.InnerText;

//if (spanElement != null)
//{
//    Console.WriteLine(text);
//}
//if (text.Length == 0 || text == "")
//{
//    Console.WriteLine("Строка пустая");
//}

// Create a new instance of the ChromeDriver
//IWebDriver driver = new ChromeDriver();

//// Navigate to the URL
//driver.Navigate().GoToUrl("https://ru.tradingview.com/symbols/BTCUSD/?exchange=CRYPTO");
//double p = 43046;
//for (; ; )
//{
//    // Find the span element using its class name
//    IWebElement Element = driver.FindElement(By.CssSelector("span.last-JWoJqCpY.js-symbol-last"));

//    // Print the extracted text
//    Console.WriteLine(Element.Text);
//    if (p <= double.Parse(Element.Text))
//    {
//        Console.WriteLine("Вот тебе нужная цена");
//        break;
//    }
//}

// Close the browser
//driver.Quit();

//это работает как надо
using (HttpClient client = new HttpClient())
{
    try
    {
        for (; ; )
        {
            Console.Clear();
            HttpResponseMessage response = await client.GetAsync("https://api.binance.com/api/v3/ticker/price?symbol=LUNCUSDT");
            response.EnsureSuccessStatusCode();

            string? json = await response.Content.ReadAsStringAsync();
            //json=json.Substring(1, json.Length-2);
            // Преобразование JSON-строки в словарь
            Dictionary<string, object>? data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            // Использование словаря
            //foreach (KeyValuePair<string, object> kvp in data)
            //{
            //    Console.WriteLine("Key: " + kvp.Key + ", Value: " + kvp.Value);
            //}
            Console.WriteLine(data["price"]);
            Thread.Sleep(500);
        }
        //Console.WriteLine("Bitcoin price: " + ExtractBitcoinPrice(json));
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine("Error fetching Bitcoin price: " + ex.Message);
    }
}