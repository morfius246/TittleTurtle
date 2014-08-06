using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TitleTurtle.Models
{
    public class Captcha : ActionResult
    {
        public string MyCaptcha(int length)
        {
            int Zero = '0';
            int Nine = '9';
            int BigA = 'A';
            int SmallZ = 'z';
            int Count = 0;
            int RandomNumber = 0;
            string CaptchaString = "";

            Random random = new Random(System.DateTime.Now.Millisecond);

            while (Count < length)
            {
                RandomNumber = random.Next(Zero, SmallZ);
                if (((RandomNumber >= Zero) && (RandomNumber <= Nine) || (RandomNumber >= BigA) && (RandomNumber <= SmallZ)))
                {
                    CaptchaString = CaptchaString + (char)RandomNumber;
                    Count = Count + 1;
                }
            }
            return CaptchaString;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            Bitmap bmp = new Bitmap(130, 40);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Blue);
            string randomString = MyCaptcha(5);
            context.HttpContext.Session["captchastring"] = randomString;
            g.DrawString(randomString, new Font("Courier", 16), new SolidBrush(Color.WhiteSmoke), 2, 2);
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            bmp.Save(response.OutputStream, ImageFormat.Jpeg);
            bmp.Dispose();
        }
    }
}
