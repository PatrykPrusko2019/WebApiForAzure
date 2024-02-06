using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Formats.Asn1;
using System.Formats.Tar;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.DownloadFileCSV
{
    public static class Download
    {
        public static void DownloadFile(File products)
        {
            string path = @$"FilesCSV\{products.Name}.csv";
            if (!Directory.Exists("FilesCSV")) Directory.CreateDirectory("FilesCSV");

            if (!System.IO.File.Exists(path)) 
            {
                using (FileStream fs = System.IO.File.Create(path))
                {
                    fs.Close();

                    WebClient client = new WebClient();
                    var buffer = client.DownloadData(products.UrlAddress);

                    Stream stream = new FileStream(path, FileMode.Create);
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(buffer);
                    stream.Close();
                }

            }

        }

    }
}
