namespace CSS.Library.Timecard.Entity
{
	/// <summary>
	/// CSVの行とリンクする構造体クラスです
	/// </summary>
	public class CsvTimeRecord
	{
		/// <summary>
		/// コンストラクタです
		/// </summary>
		public CsvTimeRecord()
		{

		}

		/// <summary>
		/// CSVのデータを格納するクラス
		/// </summary>
		public class Record
		{
			public string Date { get; set; }
			public string ArrivalTime { get; set; }
			public string LeaveTime { get; set; }
			public string Note { get; set; }
		}

		/// <summary>
		/// 格納するルール
		/// </summary>
		public class CsvMapper : CsvHelper.Configuration.CsvClassMap<Record>
		{
			public CsvMapper()
			{
				Map(x => x.Date).Index(0);
				Map(x => x.ArrivalTime).Index(1);
				Map(x => x.LeaveTime).Index(2);
				Map(x => x.Note).Index(3);
			}
		}
	}

}