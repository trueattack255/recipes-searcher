using System;
using System.IO;
using Mobile.Interfaces;
using Mobile.Droid.Native;
using Xamarin.Forms;

[assembly: Dependency(typeof(ISQLiteAndroid))]
namespace Mobile.Droid.Native
{   
    class ISQLiteAndroid : ISQLite
    {
        public string GetDatabasePath(string fileName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);
            return path;
        }
    }
}
