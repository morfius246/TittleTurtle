using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace TitleTurtle.Controllers
{
    public class ImageReturner : ActionResult
    {
        byte[] img;

        public ImageReturner(byte[] media)
        {
            img = media;
        }

        public ImageReturner(string str)
        {
            using (var ms = new MemoryStream())
            {
                Image tmp = Image.FromFile(str);
                tmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img = ms.ToArray();
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/plain;";               
            response.OutputStream.Write(img, 0, img.Count());
        }
    }
}