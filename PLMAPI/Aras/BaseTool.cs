using Aras.IOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace PLMAPI.Aras
{
    public class BaseTool
    {
        public Innovator innovator;
        public Item item;
        public Item logItem;

        private const string fileName = @"D:\Aras\aten.ini";
        private StringBuilder url = new StringBuilder(255);
        private StringBuilder db = new StringBuilder(255);
        private StringBuilder user = new StringBuilder(255);
        private StringBuilder password = new StringBuilder(255);
        private string pass;
        private HttpServerConnection conn;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);


        public BaseTool(Innovator innerInnovator)
        {
            if (innerInnovator == null)
            {
                init();
            }
            else
            {
                innovator = innerInnovator;
            }
        }

        public BaseTool()
        {
            init();
        }

        public void init()
        {
            GetPrivateProfileString("innovator", "url", "", url, 255, fileName);
            GetPrivateProfileString("innovator", "db", "", db, 255, fileName);
            GetPrivateProfileString("innovator", "user", "", user, 255, fileName);
            GetPrivateProfileString("innovator", "password", "", password, 255, fileName);

            pass = Innovator.ScalcMD5(password.ToString());
            conn = IomFactory.CreateHttpServerConnection(url.ToString(), db.ToString(), user.ToString(), pass);
            item = conn.Login();

            if (item.isError())
            {
                throw new Exception("innovator initial error");
            }
            innovator = IomFactory.CreateInnovator(conn);
        }
    }
}