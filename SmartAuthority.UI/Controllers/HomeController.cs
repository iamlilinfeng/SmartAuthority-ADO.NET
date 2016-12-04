/*技术支持QQ群：226090960*/
using System;
using System.Drawing;
using System.Web.Mvc;
using System.Web.Security;
using SmartAuthority.Factory;
using SmartAuthority.Util;

namespace SmartAuthority.UI.Controllers
{
    public class HomeController : Controller
    {
        Application.Authority authorityApplication = ApplicationFactory.GetAuthorityApplication();

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public JsonResult Login(DTO.Authority.Login Login)
        {
            string ReturnUrl = Request.Params["ReturnUrl"];
            var result = new DTO.AjaxResult();
            if (Session["ImageCode"] == null || Session["ImageCode"].ToString() != Login.CheckCode)
            {
                result.Status = false;
                result.Data = "验证码不正确!";
                return Json(result);
            }
            int AccountId = 0;
            DTO.Enum.LoginStatus loginResult = authorityApplication.Login(Login.AccountName, Login.Password, out AccountId);
            if (loginResult == DTO.Enum.LoginStatus.AccountNotExist)
            {
                result.Status = false;
                result.Data = "用户不存在!";
                return Json(result);
            }
            if (loginResult == DTO.Enum.LoginStatus.PasswordWrong)
            {
                result.Status = false;
                result.Data = "登录密码不正确!";
                return Json(result);
            }
            result.Status = true;
            FormsAuthentication.SetAuthCookie(AccountId.ToString(), true);
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                result.Data = ReturnUrl;
            else
                result.Data = "/Authority/Index";
            return Json(result);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public void CheckCode()
        {
            string CheckCode = SecurityHelper.GenerateRandom(4);
            Session["ImageCode"] = CheckCode;
            CreateCheckCodeImage(CheckCode);
        }

        private void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            Bitmap image = new Bitmap(88, 34);
            Graphics g = Graphics.FromImage(image);

            try
            {
                Random random = new Random();

                g.Clear(Color.White);

                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }

                Font font = new Font("Arial", 24, (FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}