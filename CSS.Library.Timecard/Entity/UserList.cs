namespace CSS.Library.Timecard.Entity
{
    public class UserList
    {
        public UserList()
        {

        }

        public class User
        {
            public string EmployeeId { get; set; }
            public string Name { get; set; }
            public string FelicaId { get; set; }
        }

        /// <summary>
        /// 格納するルール
        /// </summary>
        public class CsvMapper : CsvHelper.Configuration.CsvClassMap<User>
        {
            public CsvMapper()
            {
                Map(x => x.EmployeeId).Index(0);
                Map(x => x.Name).Index(1);
                Map(x => x.FelicaId).Index(2);
            }
        }

    }
}
