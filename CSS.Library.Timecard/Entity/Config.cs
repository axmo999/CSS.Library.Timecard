namespace CSS.Library.Timecard.Entity
{
    public class Config
    {

        public Config()
        {

        }

        public class Properties
        {
            /// <summary>
            /// SMB接続用ユーザー名です
            /// </summary>
            public string User { get; set; }

            ///// <summary>
            ///// SMB接続用パスワードです
            ///// </summary>
            public string Password { get; set; }

            ///// <summary>
            ///// SMBサーバーのIPアドレスです（テスト用）
            ///// </summary>
            public string UriAdress { get; set; }

            ///// <summary>
            ///// SMBサーバー接続先フォルダ名です（テスト用）
            ///// </summary>
            public string FolderName { get; set; }
        }
    }
}
