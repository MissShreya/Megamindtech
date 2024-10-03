using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MEGAMINDTECH.Controllers
{
    public class MegaTaskController : Controller
    {
        // GET: MegaTask
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddDetail(FormCollection frm)
        {
            string _error = "";
            string _mess = "";

            try
            {

                string strName = Convert.ToString(frm["strName"]);
                string strEmail = Convert.ToString(frm["strEmail"]).Trim();
                string numNumber = Convert.ToString(frm["numNumber"]);
                string strAddress = Convert.ToString(frm["strAddress"]);
                string ddlState = Convert.ToString(frm["ddlState"]);
                string ddlCity = Convert.ToString(frm["ddlCity"]);

                if (string.IsNullOrEmpty(strName))
                {
                    _error = "ERROR";
                    _mess = "Please mention your name.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                if (string.IsNullOrEmpty(strEmail))
                {
                    _error = "ERROR";
                    _mess = "Please mention your email.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                try
                {
                    var email = new MailAddress(strEmail);
                }
                catch (FormatException)
                {
                    _error = "ERROR";
                    _mess = "Invalid email format.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                if (string.IsNullOrEmpty(numNumber))
                {
                    _error = "ERROR";
                    _mess = "Please mention your phone number.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                if (numNumber.Length != 10)
                {
                    _error = "ERROR";
                    _mess = "Invalid phone number.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                if (!Regex.IsMatch(numNumber, @"^\d+$"))
                {
                    _error = "ERROR";
                    _mess = "Phone number must be numeric.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                if (string.IsNullOrEmpty(ddlState))
                {
                    _error = "ERROR";
                    _mess = "Please mention your state.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }

                if (string.IsNullOrEmpty(ddlCity))
                {
                    _error = "ERROR";
                    _mess = "Please mention your City.";
                    return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);
                }
                string[] pName = { "@Mode", "@RegID", "@Name", "@Email", "@Phone", "@Address", "@StateID", "@CityID" };
                string[] pValue = { "INSERT", "", strName, strEmail, numNumber, strAddress, ddlState, ddlCity };
                DataTable DT = new DALBase().ExecuteProcedure("sp_CRUDRegistration", pName, pValue).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    if (Convert.ToString(DT.Rows[0]["Code"]) == "0")
                    {
                        _error = "OK";
                        _mess = Convert.ToString(DT.Rows[0]["Remark"]);
                    }
                    else
                    {
                        _error = "ERROR";
                        _mess = Convert.ToString(DT.Rows[0]["Remark"]);
                    }
                }
                else
                {
                    _error = "ERROR";
                    _mess = "Some error occurred please try again later.";
                }

                return Json(new { ERROR = _error, MESSAGE = _mess }, JsonRequestBehavior.DenyGet);

            }
            catch (Exception ex)
            {
                return Json(new { ERROR = _error, MESSAGE = ex.Message.Length > 25 ? ex.Message.Substring(0, 25) + "..." : ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public ActionResult GetList()
        {
            string _error = "";
            string _mess = "";

            try
            {
                string[] pName = { "@Mode", "@RegID", "@Name", "@Email", "@Phone", "@Address", "@StateID", "@CityID" };
                string[] pValue = { "SELECT", "", "", "", "", "", "", "" };
                DataTable tmpDT = new DALBase().ExecuteProcedure("sp_CRUDRegistration", pName, pValue).Tables[0];
                List<Dictionary<string, object>> lstRows = new List<Dictionary<string, object>>();


                if (tmpDT.Rows.Count > 0)
                {
                    lstRows = B_clsUtility.GetJsonFromTable(tmpDT);

                    _error = "OK";
                    return Json(new
                    {
                        ERROR = _error,
                        MESSAGE = _mess,
                        COUNT = tmpDT.Rows.Count,
                        RECORD = lstRows
                    }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    _error = "OK";
                    _mess = "No Record Found.";
                    return Json(new
                    {
                        ERROR = _error,
                        MESSAGE = _mess,
                        COUNT = "0",
                        RECORD = ""
                    }, JsonRequestBehavior.DenyGet);
                }
            }

            catch (Exception ex)
            {

                return Json(new
                {
                    ERROR = "ERROR",
                    MESSAGE = ex.Message,
                    COUNT = "0",
                    RECORD = ""
                }, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpPost]
        public ActionResult GetStateDetail()
        {
            string _error = "";
            string _mess = "";

            try
            {
                string[] pName = { };
                string[] pValue = { };
                DataTable tmpDT = new DALBase().ExecuteProcedure("sp_StateData", pName, pValue).Tables[0];
                List<Dictionary<string, object>> lstRows = new List<Dictionary<string, object>>();


                if (tmpDT.Rows.Count > 0)
                {
                    lstRows = B_clsUtility.GetJsonFromTable(tmpDT);

                    _error = "OK";
                    return Json(new
                    {
                        ERROR = _error,
                        MESSAGE = _mess,
                        COUNT = tmpDT.Rows.Count,
                        RECORD = lstRows
                    }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    _error = "ERROR";
                    _mess = "No Record Found.";
                    return Json(new
                    {
                        ERROR = _error,
                        MESSAGE = _mess,
                        COUNT = "0",
                        RECORD = ""
                    }, JsonRequestBehavior.DenyGet);
                }
            }

            catch (Exception ex)
            {

                return Json(new
                {
                    ERROR = "ERROR",
                    MESSAGE = ex.Message,
                    COUNT = "0",
                    RECORD = ""
                }, JsonRequestBehavior.DenyGet);
            }

        }
        [HttpPost]
        public ActionResult GetCityDetail()
        {
            string _error = "";
            string _mess = "";

            try
            {
                string[] pName = { };
                string[] pValue = { };
                DataTable tmpDT = new DALBase().ExecuteProcedure("sp_CityData", pName, pValue).Tables[0];
                List<Dictionary<string, object>> lstRows = new List<Dictionary<string, object>>();


                if (tmpDT.Rows.Count > 0)
                {
                    lstRows = B_clsUtility.GetJsonFromTable(tmpDT);

                    _error = "OK";
                    return Json(new
                    {
                        ERROR = _error,
                        MESSAGE = _mess,
                        COUNT = tmpDT.Rows.Count,
                        RECORD = lstRows
                    }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    _error = "ERROR";
                    _mess = "No Record Found.";
                    return Json(new
                    {
                        ERROR = _error,
                        MESSAGE = _mess,
                        COUNT = "0",
                        RECORD = ""
                    }, JsonRequestBehavior.DenyGet);
                }
            }

            catch (Exception ex)
            {

                return Json(new
                {
                    ERROR = "ERROR",
                    MESSAGE = ex.Message,
                    COUNT = "0",
                    RECORD = ""
                }, JsonRequestBehavior.DenyGet);
            }

        }

    }
}