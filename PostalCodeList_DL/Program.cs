using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SeleniumTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(">>処理開始");
            //ダウンロード
            Download();

            //Zipファイル解凍
            ZipDecompress();

            //Csvファイル読み込み
            List<PostalCodeDto> pdto = ReadCsv();

            //Csvファイル加工
            List<PostalCodeDto> edto = EditCsv(pdto);

            //加工済Csvファイル出力
            OutPutCsv(edto);
        }

        //CSVファイルを読込し、Dto型のListに値をセットする
        public static List<PostalCodeDto> ReadCsv()
        {
            List<PostalCodeDto> lists = new List<PostalCodeDto>();
            string readFile = "/Users/noname/Downloads/decompress/add_2202.CSV";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StreamReader sr = new StreamReader(readFile, System.Text.Encoding.GetEncoding("shift_jis"));
            {
                var dto = new PostalCodeDto();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    var ret = dto.SetCsv(line);
                    lists.Add(ret);
                }
            }
            return lists;
        }

        //Dto型のListを受取、不要なデータを部分的に削除する
        public static List<PostalCodeDto> EditCsv(List<PostalCodeDto> lists)
        {
            List<PostalCodeDto> editDtoList = new List<PostalCodeDto>();
            for (int i = 0; i < lists.Count; i++)
            {
                var list = lists[i];
                if (list.townAreaName.Contains("以下に掲載がない場合"))
                {
                    list.townAreaName = "";
                }
                if (list.townAreaNameKana.Contains("ｲｶﾆｹｲｻｲｶﾞﾅｲﾊﾞｱｲ"))
                {
                    list.townAreaNameKana = "";
                }
                if (list.prefecturesName.Contains("("))
                {
                    int length = list.prefecturesName.Length;
                    int index = list.prefecturesName.IndexOf("(");
                    string str = list.prefecturesName.Substring(0, index);
                    string strx = String.Concat(str, "\"");
                    list.prefecturesName = strx;
                }
                if (list.municipalitiesNameKana.Contains("("))
                {
                    int length = list.municipalitiesNameKana.Length;
                    int index = list.municipalitiesNameKana.IndexOf("(");
                    string str = list.municipalitiesNameKana.Substring(0, index);
                    string strx = String.Concat(str, "\"");
                    list.municipalitiesNameKana = strx;
                }
                editDtoList.Add(list);
            }
            return editDtoList;
        }

        //出力用のListに格納してDLフォルダにCsv出力
        private static void OutPutCsv(List<PostalCodeDto> editDtoList)
        {
            string filePath = @"/Users/noname/Downloads/outputCsv/edit.csv";
            var sb = new StringBuilder();
            foreach (var editDto in editDtoList)
            {
                var row = $"{editDto.postalCode},{editDto.prefecturesName},{editDto.municipalitiesName},{editDto.townAreaName},";
                sb.Append(row);
            }
            StreamWriter file = new StreamWriter(filePath, false, Encoding.GetEncoding("Shift_JIS"));
            file.Write(sb.ToString());
            file.Close();
        }

        //Zipファイル解凍
        public static void ZipDecompress()
        {
            try
            {
                string zipPath = "/Users/noname/Downloads/add_2202.zip";
                string extractPath = "/Users/noname/Downloads/decompress";
                var fileCount = Directory.GetFiles(extractPath, "*.csv");

                if (fileCount.Length > 0)
                {
                    Console.WriteLine("既に解凍済ファイルが存在します。");
                }
                else
                {
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                    Console.WriteLine(">>解凍完了");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }

        //CSVファイルをDL
        public static void Download()
        {
            // Webドライバーのインスタンス化
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddUserProfilePreference("download.default_directory", "/Users/noname/Downloads");
            options.AddUserProfilePreference("download.prompt_for_download", "false");
            options.AddUserProfilePreference("download.directory_upgrade", "true");
            IWebDriver driver = new ChromeDriver(options);

            // URLに移動
            driver.Navigate().GoToUrl(@"https://www.post.japanpost.jp/zipcode/dl/kogaki-zip.html");
            Console.WriteLine(">>ブラウザ起動");

            //Zipファイルをダウンロードする(DL先はデフォルトでDownlodフォルダ)
            driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div[1]/div/div/table[2]/tbody/tr[2]/td[2]/a")).Click();
            Console.WriteLine(">>DL完了");
            Thread.Sleep(1000);

            // なにかコンソールに文字を入力したらクロームを閉じる
            Console.WriteLine(">>処理を完了させるにはなにかキーを入力してください。");
            Console.ReadKey();
            driver.Quit();
        }
    }
}