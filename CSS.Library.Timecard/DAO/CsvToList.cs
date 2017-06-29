using CsvHelper;
using SharpCifs.Smb;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;


namespace CSS.Library.Timecard.DAO
{
    public class CsvToList
	{
        ///
        /// クラス変数群
        ///

        /// <summary>
        /// ファイルエンコーディングです
        /// </summary>
        System.Text.Encoding _encoding;

		/// <summary>
		/// コンストラクタです
		/// </summary>
		public CsvToList()
		{
            var provider = System.Text.CodePagesEncodingProvider.Instance;
            System.Text.Encoding.RegisterProvider(provider);

            _encoding = System.Text.Encoding.GetEncoding("shift-jis");
        }

        /// <summary>
        /// SMB上のCSVファイルをList構造体に変換します
        /// </summary>
        /// <param name="smbFile">SMBファイル</param>
        /// <returns>List<Record>List構造体</returns>
        public List<Timecard.Entity.CsvTimeRecord.Record> ReadCsv(SmbFile smbFile)
		{
			// Recordリストを作成します
			List<Timecard.Entity.CsvTimeRecord.Record> records = new List<Timecard.Entity.CsvTimeRecord.Record>();

			// SMBファイルから読み込みストリーム作成、文字コード指定
			using (TextReader txtRead = new StreamReader(smbFile.GetInputStream(), _encoding))
			{
				// CSVとして読み込み開始
				var csvRead = new CsvReader(txtRead);

				// CSVファイル設定
				// ヘッダーなし
				csvRead.Configuration.HasHeaderRecord = false;
				// CsvMapper通りにマッピングする
				csvRead.Configuration.RegisterClassMap<Timecard.Entity.CsvTimeRecord.CsvMapper>();
				// 文字コード設定
				csvRead.Configuration.Encoding = _encoding;

				// Recordリストに流し込み
				records = csvRead.GetRecords<Timecard.Entity.CsvTimeRecord.Record>().ToList();

				// コネクション破棄
				smbFile.GetInputStream().Dispose();
				txtRead.Dispose();
				csvRead.Dispose();

				// List形式で返す
				return records;
			}
		}

        /// <summary>
        /// streamのCSVファイルをList構造体に変換します
        /// </summary>
        /// <param name="txtRead">TextReader</param>
        /// <returns>List<Record>List構造体</returns>
        public List<Timecard.Entity.CsvTimeRecord.Record> ReadCsv(TextReader txtRead)
        {
            // Recordリストを作成します
            List<Timecard.Entity.CsvTimeRecord.Record> records = new List<Timecard.Entity.CsvTimeRecord.Record>();

            // CSVとして読み込み開始
            var csvRead = new CsvReader(txtRead);

            // CSVファイル設定
            // ヘッダーなし
            csvRead.Configuration.HasHeaderRecord = false;
            // CsvMapper通りにマッピングする
            csvRead.Configuration.RegisterClassMap<Timecard.Entity.CsvTimeRecord.CsvMapper>();
            // 文字コード設定
            csvRead.Configuration.Encoding = _encoding;

            // Recordリストに流し込み
            records = csvRead.GetRecords<Timecard.Entity.CsvTimeRecord.Record>().ToList();

            // コネクション破棄
            txtRead.Dispose();
            csvRead.Dispose();

            // List形式で返す
            return records;
        }

        /// <summary>
        /// ListからCSVへ書き込みます。
        /// </summary>
        /// <param name="smbFile"></param>
        /// <param name="records">List構造体</param>
		public void WriteCsv(SmbFile smbFile, List<Timecard.Entity.CsvTimeRecord.Record> records)
		{
            //// SMBファイルから書き込みストリーム作成、文字コード指定
            using (TextWriter txtWrite = new StreamWriter(smbFile.GetOutputStream(), _encoding))
            {
                // CSVとして書き込み開始
                var csvWrite = new CsvWriter(txtWrite);

                // CSVファイル設定
                // ヘッダーなし
                csvWrite.Configuration.HasHeaderRecord = false;
                // CsvMapper通りにマッピングする
                csvWrite.Configuration.RegisterClassMap<Timecard.Entity.CsvTimeRecord.CsvMapper>();
                // 文字コード設定
                csvWrite.Configuration.Encoding = _encoding;

                // リストを書き込み
                csvWrite.WriteRecords(records);

                // コネクション破棄
                smbFile.GetOutputStream().Dispose();
                txtWrite.Dispose();
                csvWrite.Dispose();
            }
		}

        /// <summary>
        /// Listからstreamへ書き込みます。
        /// </summary>
        /// <param name="txtWrite">TextWriter</param>
        /// <param name="records">List構造体</param>
		public void WriteCsv(TextWriter txtWrite, List<Timecard.Entity.CsvTimeRecord.Record> records)
        {
            // CSVとして書き込み開始
            var csvWrite = new CsvWriter(txtWrite);

            // CSVファイル設定
            // ヘッダーなし
            csvWrite.Configuration.HasHeaderRecord = false;
            // CsvMapper通りにマッピングする
            csvWrite.Configuration.RegisterClassMap<Timecard.Entity.CsvTimeRecord.CsvMapper>();
            // 文字コード設定
            csvWrite.Configuration.Encoding = _encoding;

            // リストを書き込み
            csvWrite.WriteRecords(records);

            // コネクション破棄
            txtWrite.Dispose();
            csvWrite.Dispose();
        }

        /// <summary>
        /// streamのユーザーリストをList構造体に変換します
        /// </summary>
        /// <param name="txtRead"></param>
        /// <returns></returns>
        public List<Entity.UserList.User> Users(TextReader txtRead)
        {
            // Recordリストを作成します
            List<Entity.UserList.User> records = new List<Entity.UserList.User>();

            // CSVとして読み込み開始
            var csvRead = new CsvReader(txtRead);

            // CSVファイル設定
            // ヘッダーなし
            csvRead.Configuration.HasHeaderRecord = true;
            // CsvMapper通りにマッピングする
            csvRead.Configuration.RegisterClassMap<Entity.UserList.CsvMapper>();
            // 文字コード設定
            csvRead.Configuration.Encoding = _encoding;

            // Recordリストに流し込み
            records = csvRead.GetRecords<Entity.UserList.User>().ToList();

            // コネクション破棄
            txtRead.Dispose();
            csvRead.Dispose();

            // List形式で返す
            return records;
        }
    }

}
