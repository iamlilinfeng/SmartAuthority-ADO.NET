/*技术支持QQ群：226090960*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SmartAuthority.Factory;

namespace SmartAuthority.UI.Controllers
{
    public class ControllerBase : Controller
    {
        Application.Authority authorityApplication = ApplicationFactory.GetAuthorityApplication();

        DTO.Authority.Account account = null;

        public DTO.Authority.Account Account
        {
            get
            {
                if (account != null)
                    return account;
                if (!User.Identity.IsAuthenticated)
                    return null;
                return account = authorityApplication.GetBaseAccount(Convert.ToInt32(User.Identity.Name));
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string Action = ControllerContext.RequestContext.RouteData.Values["action"].ToString().ToLower();
            string Controller = ControllerContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            if (Account != null)
            {
                var Operate = Account.Operates.SingleOrDefault(m => m.Control.ToLower() == Controller && m.Action.ToLower() == Action);
                if (Operate != null)
                {
                    ViewBag.Title = Operate.Name;
                    if (Operate.Level == 2)
                    {
                        ViewBag.SelectMenu = Operate.AuthorityOperateId;
                        ViewBag.ParentSelectMenu = Operate.ParentId;
                    }
                    if (Operate.Level == 3)
                    {
                        ViewBag.SelectMenu = Operate.ParentId;
                        ViewBag.ParentSelectMenu = Account.Operates.Single(m => m.AuthorityOperateId == Operate.ParentId).ParentId;
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                }

                //姓名
                ViewBag.UserName = account.Name;
                //菜单
                ViewBag.Menu = account.Operates;

                List<string> PageOperate = new List<string>();
                foreach (var item in account.Operates)
                {
                    PageOperate.Add(item.Control.ToLower() + "-" + item.Action.ToLower());
                }
                //功能
                ViewBag.PageOperate = string.Join(",", PageOperate);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}