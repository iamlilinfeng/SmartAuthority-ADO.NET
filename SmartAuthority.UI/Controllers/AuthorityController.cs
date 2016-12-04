/*技术支持QQ群：226090960*/
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Webdiyer.WebControls.Mvc;
using SmartAuthority.Factory;
using SmartAuthority.DTO;

namespace SmartAuthority.UI.Controllers
{
    [Authorize]
    public class AuthorityController : ControllerBase
    {
        Application.Authority authorityApplication = ApplicationFactory.GetAuthorityApplication();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccountEdit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AccountEdit(DTO.Authority.AuthorityAccount authorityAccount)
        {
            var result = new AjaxResult();
            authorityAccount.Enable = true;
            authorityAccount.CreateTime = DateTime.Now;
            DTO.Enum.AddAccountStatus status = authorityApplication.AddAccount(authorityAccount);

            if (status == DTO.Enum.AddAccountStatus.AccountNameExist)
            {
                result.Status = false;
                result.Data = "登录名已存在!";
                return Json(result);
            }

            result.Status = true;
            result.Data = "添加成功!";
            return Json(result);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(string OldPassword, string NewPassword, string ReNewPassword)
        {
            var result = new AjaxResult();
            if (NewPassword != ReNewPassword)
            {
                result.Status = false;
                result.Data = "新密码与重复新密码不同!";
                return Json(result);
            }

            DTO.Enum.ChangePasswordStatus changeStatus = authorityApplication.ChangePassword(Account.AccouontId, OldPassword.Trim(), NewPassword.Trim());

            if (changeStatus == DTO.Enum.ChangePasswordStatus.OldPasswordWrong)
            {
                result.Status = false;
                result.Data = "原密码不正确!";
                return Json(result);
            }

            result.Status = true;
            result.Data = "修改成功!";
            return Json(result);
        }

        [HttpGet]
        public ActionResult AccountList(string Name, int? PagerId = 1)
        {
            PagerParm PagerParm = new PagerParm();
            PagerParm.PageIndex = PagerId ?? 1;
            int totalCount = 0;
            PagedList<DTO.Authority.AuthorityAccount> InfoPager = authorityApplication.GetAccountList(Name, PagerParm, out totalCount).ToPagedList(PagerParm.PageIndex, PagerParm.PageSize);
            InfoPager.TotalItemCount = totalCount;
            InfoPager.CurrentPageIndex = (int)(PagerId ?? 1);
            ViewBag.AccountName = Name;
            return View(InfoPager);
        }

        [HttpGet]
        public JsonResult AccountDisable(int AccountId)
        {
            var result = new AjaxResult();
            if (AccountId == 1)
            {
                result.Status = true;
                result.Data = "管理员无法禁用!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            authorityApplication.AccountDisable(AccountId);
            result.Status = true;
            result.Data = "禁用成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AccountEnable(int AccountId)
        {
            var result = new AjaxResult();
            authorityApplication.AccountEnable(AccountId);
            result.Status = true;
            result.Data = "启用成功!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult RoleEdit(int RoleId = 0)
        {
            ViewBag.Operate = authorityApplication.GetAllOperate();

            if (RoleId != 0)
            {
                ViewBag.Role = authorityApplication.GetRoleById(RoleId);

                var SelectItem = authorityApplication.GetOperateListByRole(RoleId);
                List<int> Item = new List<int>();
                foreach (var menu in SelectItem)
                {
                    Item.Add(menu.AuthorityOperateId);
                }

                ViewBag.Select = string.Join(",", Item);
                ViewBag.RoleId = RoleId;
            }
            else
            {
                ViewBag.Role = new DTO.Authority.AuthorityRole();
                ViewBag.Select = "";
            }
            return View();
        }

        [HttpPost]
        public JsonResult RoleEdit()
        {
            string strList = Request.Params["idList"];
            string[] strArray = strList.Split(',');
            List<int> OperateIds = new List<int>();
            for (int i = 0; i < strArray.Length; i++)
            {
                OperateIds.Add(Convert.ToInt32(strArray[i]));
            }

            var result = new AjaxResult();
            result.Status = true;

            if (!string.IsNullOrEmpty(Request.Params["hidroleid"]))
            {
                var Role = authorityApplication.GetRoleById(Convert.ToInt32(Request.Params["hidroleid"]));
                Role.Name = Request.Params["Name"];
                Role.Describe = Request.Params["Describe"];
                authorityApplication.SaveRole(Role, OperateIds);
                result.Data = "修改成功!";
            }
            else
            {
                var Role = new DTO.Authority.AuthorityRole();
                Role.Name = Request.Params["Name"];
                Role.Describe = Request.Params["Describe"];
                authorityApplication.AddRole(Role, OperateIds);
                result.Data = "添加成功!";
            }
            return Json(result);
        }

        [HttpGet]
        public ActionResult RoleList(string Name, int? PagerId = 1)
        {
            PagerParm PagerParm = new PagerParm();
            PagerParm.PageIndex = PagerId ?? 1;
            int totalCount = 0;
            PagedList<DTO.Authority.AuthorityRole> InfoPager = authorityApplication.GetRoleList(Name, PagerParm, out totalCount).ToPagedList(PagerParm.PageIndex, PagerParm.PageSize);
            InfoPager.TotalItemCount = totalCount;
            InfoPager.CurrentPageIndex = (int)(PagerId ?? 1);
            ViewBag.AccountName = Name;
            return View(InfoPager);
        }

        [HttpGet]
        public ActionResult RoleAccount(int RoleId)
        {
            ViewBag.RoleId = RoleId;
            var InRole = authorityApplication.GetInRoleAccount(RoleId);
            var OutRole = authorityApplication.GetOutRoleAccount(RoleId);

            List<string> InRoleItem = new List<string>();
            foreach (DTO.Authority.AuthorityAccount account in InRole)
            {
                InRoleItem.Add(account.AuthorityAccountId.ToString());
            }

            List<string> OutRoleItem = new List<string>();
            foreach (DTO.Authority.AuthorityAccount account in OutRole)
            {
                OutRoleItem.Add(account.AuthorityAccountId.ToString());
            }
            ViewBag.InRole = InRole;
            ViewBag.OutRole = OutRole;
            ViewBag.InRoleValue = string.Join(",", InRoleItem);
            ViewBag.OutRoleValue = string.Join(",", OutRoleItem);
            return View();
        }

        [HttpPost]
        public JsonResult RoleAccount()
        {
            var RoleId = Convert.ToInt32(Request.Params["role_id"]);
            string InRoleAccount = Request.Form["in_role_account"];

            List<int> AccountIds = new List<int>();
            if (!string.IsNullOrEmpty(InRoleAccount))
            {
                string[] strArray = InRoleAccount.TrimEnd(',').Split(',');
                for (int i = 0; i < strArray.Length; i++)
                {
                    AccountIds.Add(Convert.ToInt32(strArray[i]));
                }
            }
            authorityApplication.SaveRoleAccount(RoleId, AccountIds);

            var result = new AjaxResult();
            result.Status = true;
            result.Data = "编辑成功!";
            return Json(result);
        }

        [HttpGet]
        public ActionResult OperateList(string Name, int PagerId = 1)
        {
            PagerParm PagerParm = new PagerParm();
            PagerParm.PageIndex = Convert.ToInt32(PagerId);
            int totalCount = 0;
            PagedList<DTO.Authority.AuthorityOperate> InfoPager = authorityApplication.GetOperateList(Name, PagerParm, out totalCount).ToPagedList(PagerParm.PageIndex, PagerParm.PageSize);
            InfoPager.TotalItemCount = totalCount;
            InfoPager.CurrentPageIndex = PagerId;
            ViewBag.AccountName = Name;
            return View(InfoPager);
        }

        [HttpGet]
        public ActionResult OperateEdit(int OperateId)
        {
            var operate = authorityApplication.GetOperateById(OperateId);
            return View(operate);
        }

        [HttpPost]
        public JsonResult OperateEdit(DTO.Authority.AuthorityOperate operate)
        {
            var result = new AjaxResult();
            authorityApplication.SaveOperate(operate);
            result.Status = false;
            result.Data = "修改成功!";
            return Json(result);
        }
    }
}