using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataModel.DataTypeConverters;

namespace DataModel
{
    public class DocumentHelper
    {
        public static void ParseDocument(ref Document document, FormCollection formData, HttpRequestBase request)
        {
            //Отследить изменение документа
            document.Id = DataTypeParser.Int(formData["document_Id"]);
            document.Name = DataTypeParser.String(formData["Document.Name"]);
            document.Id_DocumentFormat = DataTypeParser.Int(formData["Document.Id_DocumentFormat_VI"]);

            document.Description = DataTypeParser.String(formData["Document.Description"]);

            document.Filename = null;

            if (request.Files.Count > 0)
            {
                var fileContent = request.Files[0];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    document.Filename = fileContent.FileName;
                    var filename = Guid.NewGuid().ToString();
                    var filePath = FileSystemHelper.MapFileName(filename);//Path.Combine(Server.MapPath(PATH), System.IO.Path.GetFileName(filename));


                    using (FileStream fs = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] bytes = new byte[3000];
                        int bytesRead;
                        while ((bytesRead = fileContent.InputStream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            fs.Write(bytes, 0, bytesRead);
                        }


                    }
                    document.PathToFile = filename;
                }
            }

        }
    }
}
