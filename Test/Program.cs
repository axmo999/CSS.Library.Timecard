using System;
using CSS.Library.Timecard.DAO;
using CSS.Library.Timecard.Entity;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Properties config = new Config.Properties();
            config.User = "administrator";
            config.Password = "chubu#201";
            config.UriAdress = "192.168.250.200";
            config.FolderName = "/share/test/";

            SharedFile smbFile = new SharedFile(config);

            CsvToList csv = new CsvToList();
            TextReader txtRead = smbFile.SmbReader("201706_0043108.txt");
            List<CsvTimeRecord.Record> records = csv.ReadCsv(txtRead);

            string today = DateTime.Now.ToString("yyyy/MM/dd");

            string nowtime = DateTime.Now.ToString("HH:mm");

            var recordToday = records.FirstOrDefault(record => record.Date.ToString() == today);

            int index = records.IndexOf(recordToday);

            recordToday.ArrivalTime = nowtime;

            records[index] = recordToday;

            TextWriter txtWrite = smbFile.SmbWriter("201706_0043108.txt");

            csv.WriteCsv(txtWrite, records);

            Console.WriteLine("Hello World!");
        }
    }
}