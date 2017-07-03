using SharpCifs.Smb;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace CSS.Library.Timecard.DAO
{
    /// <summary>
    /// Samba接続用クラスです
    /// </summary>
    public class SharedFile
	{
		///
		/// クラス変数群
		///

		/// <summary>
		/// ローカルホストネームです
		/// </summary>
		//private string _hostName = "xamarin.mobile";

		/// <summary>
		/// ローカルIPです
		/// </summary>
		//private string _localAddress = "192.168.250.41";

		/// <summary>
		/// SMB接続用ドメイン名です
		/// </summary>
		//private string _domain = "chubu-ishikai.local";

		/// <summary>
		/// SMB接続用ユーザー名です
		/// </summary>
		private string _user = "localadmin";

        /// <summary>
        /// SMB接続用パスワードです
        /// </summary>
        private string _password = "chubu#82OO";

        /// <summary>
        /// SMBサーバーのIPアドレスです（テスト用）
        /// </summary>
        private string _uriAdress = "192.168.250.200";

        /// <summary>
        /// SMBサーバー接続先フォルダ名です（テスト用）
        /// </summary>
        private string _folderName = "/share/test/";

		/// <summary>
		/// SMBサーバー接続先ファイル名です（テスト用）
		/// </summary>
        //private string _fileName = "201706_0043108.txt";

        /// <summary>
        /// ファイルエンコーディングです
        /// </summary>
        System.Text.Encoding _encoding;

        /// <summary>
        /// コンストラクタです
        /// </summary>
        public SharedFile(Entity.Config.Properties props)
		{
            // 設定クラスからクラス変数へ書き込み
            _user = props.User;
            _password = props.Password;
            _uriAdress = props.UriAdress;
            _folderName = props.FolderName;

            // 文字化け対策です。
            var provider = System.Text.CodePagesEncodingProvider.Instance;
            System.Text.Encoding.RegisterProvider(provider);
            _encoding = System.Text.Encoding.GetEncoding("shift-jis");

            // ホスト名取得
            string localhostname = Dns.GetHostName();

            // Samba接続プロパティ
            SharpCifs.Util.Sharpen.Properties properties = new SharpCifs.Util.Sharpen.Properties();

#if DEBUG
            //ローカルIP取得
            List<IPAddress> localadr = this.GetLocalIPAddress();
            string localIP = localadr[0].ToString();
            properties.SetProperty("jcifs.netbios.laddr", localIP);
            properties.SetProperty("jcifs.smb.client.laddr", localIP);
#endif

            properties.SetProperty("jcifs.smb.client.useExtendedSecurity", "true");
			properties.SetProperty("jcifs.smb.lmCompatibility", "3");
			properties.SetProperty("jcifs.netbios.cachePolicy", "180");
			// cache timeout: cache names
			properties.SetProperty("jcifs.netbios.hostname", localhostname);
			properties.SetProperty("jcifs.netbios.retryCount", "3");
			properties.SetProperty("jcifs.netbios.retryTimeout", "5000");
			// Name query timeout
			properties.SetProperty("jcifs.smb.client.responseTimeout", "10000");
			// increased for NAS where HDD is off!

			properties.SetProperty("jcifs.netbios.baddr", "255.255.255.255");
			properties.SetProperty("jcifs.resolveOrder", "LMHOSTS,BCAST,DNS");

			//You can store authentication information in SharpCifs.Std.
			SharpCifs.Config.SetProperty("jcifs.smb.client.username", _user);
			SharpCifs.Config.SetProperty("jcifs.smb.client.password", _password);

			SharpCifs.Config.SetProperties(properties);
		}

        private List<IPAddress> GetLocalIPAddress()
        {
            var ipaddress = new List<IPAddress>();

            // 物理インターフェース情報をすべて取得
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // 各インターフェースごとの情報を調べる
            foreach (var adapter in interfaces)
            {
                // 有効なインターフェースのみを対象とする
                if (adapter.OperationalStatus != OperationalStatus.Up)
                {
                    continue;
                }

                // インターフェースに設定されたIPアドレス情報を取得
                var properties = adapter.GetIPProperties();

                // 設定されているすべてのユニキャストアドレスについて
                foreach (var unicast in properties.UnicastAddresses)
                {
                    if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        // IPv4アドレス
                        ipaddress.Add(unicast.Address);
                    }
                    //else if (unicast.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    //{
                    //    // IPv6アドレス
                    //    ipaddress.Add(unicast.Address);
                    //}
                }
            }

            return ipaddress;
        }


        /// <summary>
        /// SMBコネクション
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>SmbFile</returns>
        public SmbFile Connect(string fileName)
		{
			string uri = "smb://" + _uriAdress + "/" + _folderName + "/" + fileName;
            return new SmbFile(uri);
		}

        /// <summary>
        /// SMBファイルをTextReaderへ読み込みます
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>TextReader</returns>
        public TextReader SmbReader(string fileName)
        {
            SmbFile smbFile = this.Connect(fileName);
            TextReader txtRead = new StreamReader(smbFile.GetInputStream(), _encoding);
            smbFile.GetInputStream().Close();
            smbFile.GetInputStream().Dispose();
            return txtRead;
        }

        public TextWriter SmbWriter(string fileName)
        {
            SmbFile smbFile = this.Connect(fileName);
            TextWriter txtWrite = new StreamWriter(smbFile.GetOutputStream(), _encoding);
            smbFile.GetOutputStream().Close();
            smbFile.GetOutputStream().Dispose();
            return txtWrite;
        }

        public bool SmbExists(string fileName)
        {
            SmbFile smbFile = this.Connect(fileName);
            return smbFile.Exists();
        }


	}

}
