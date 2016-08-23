using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SPA_Angular_CRUD.Controllers
{
    [Route("api/upload")]
    public class UploadController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported Media Type");
            }
            const string filePrefix = "Content/Images/";
            string targetFolder = HttpContext.Current.Server.MapPath("~/" + filePrefix);
            var provider = new MultipartFormDataStreamProvider(targetFolder);
            var fileNames = new List<string>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (MultipartFileData file in provider.FileData)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    if (File.Exists(Path.Combine(targetFolder, fileName)))
                    {
                        File.Delete(Path.Combine(targetFolder, fileName));
                    }
                    File.Move(file.LocalFileName, Path.Combine(targetFolder, fileName));
                    fileNames.Add(filePrefix + fileName);
                }
                // return List of uploaded files
                return Ok(fileNames);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}