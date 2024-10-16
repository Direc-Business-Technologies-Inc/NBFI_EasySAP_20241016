using System.Text;
using DirecLayer;
using System;
using MSXML2;
using Translator;
using Newtonsoft.Json.Linq;
using PresenterLayer.Helper;
using ServiceLayer.Services;
using DomainLayer.Helper;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using DomainLayer.Models;
using System.Collections.Generic;
using System.Threading;
using System.Web.Script.Serialization;

namespace PresenterLayer
{
    public class SAPHana
    {
        public static XMLHTTP60 ServiceLayer { get; set; }
        public static TranslatorTool tool = new TranslatorTool();
        public static SP _SP = new SP();
        public static void ServiceURL_Update()
        {
            var sboCred = new SboCredentials();
            SboCred.strCurrentServiceURL = $"{SboCred.HttpsUrl}{SboCred.SAPHanaTag}";
            const string httpStr = "http://";
            const string httpsStr = "https://";
            if (!SboCred.strCurrentServiceURL.StartsWith(httpStr, true, null) &&
                !SboCred.strCurrentServiceURL.StartsWith(httpsStr, true, null))
            {
                SboCred.strCurrentServiceURL = httpStr + sboCred.ServiceLayer;
            }

            if (ServiceLayer == null)
            { ServiceLayer = new XMLHTTP60(); }
        }

        public static string GetJsonString(string ret, string tag)
        {
            var startTag = "{";
            int startIndex = ret.IndexOf(startTag) + startTag.Length;
            int endIndex = ret.IndexOf("}", startIndex);
            return ret.Substring(startIndex, endIndex - startIndex);
        }

        public static string GetJsonError(string json)
        {
            JObject err = JObject.Parse(json);
            return (string)err["error"]["message"]["value"];
        }

        public static string GetJsonValue(string json, string value)
        {
            try
            {
                if (json != null)
                {
                    JObject err = JObject.Parse(json);
                    if (err.ToString().Contains("error"))
                    {
                        return $"error : {GetJsonError(err.ToString())}";
                    }
                    else
                    {
                        return (string)err[value];
                    }
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                if (json.Contains("error"))
                {
                    string retJson = GetJsonString(json, "");
                    var sbJson = new StringBuilder();
                    sbJson.Append("{" + retJson + "}}}");
                    return GetJsonError(sbJson.ToString());
                }
                else { return "Operation completed successfully"; }
            }

        }

        public static string GetJson(string request, string method, string json, string getvalue, bool isCancel = false)
        {
            string result = "";
            try
            {
                var url = $@"{SboCred.strCurrentServiceURL}{request}";

                if (isCancel)
                {
                    url += @"/Cancel";
                }

                ServiceLayer.open(method.ToUpper(), url, false, null, null);

                //if (method == "PATCH")
                //{
                //    ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
                //}

                ServiceLayer.send(json);

                string ret = ServiceLayer.responseText;

                result = GetJsonValue(ret, getvalue);
                if (string.IsNullOrEmpty(result))
                { result = $"error : {GetJsonError(ret)}"; }
            }
            catch (Exception ex)
            {
                result = $"error : {ex.Message}";
            }

            return result;
        }

        public static bool Login(out string err)
        {
            bool result = LoginAction(out err);
            return result;
        }


        public static bool LoginAction(out string err)
        {
            ServiceURL_Update();

            bool result = true;
            var json = new StringBuilder();
            var sboCred = new SboCredentials();
            json.AppendLine("{");
            json.AppendLine($@" ""CompanyDB"" : ""{sboCred.Database}"",");
            json.AppendLine($@" ""UserName"" : ""{sboCred.UserId}"",");
            json.AppendLine($@" ""Password"" : ""{sboCred.Password}""");
            json.AppendLine("}");
            //ServiceLayer.open("POST", $@"{SboCred.strCurrentServiceURL}Login");
            ServiceLayer.open("POST", $@"{SboCred.strCurrentServiceURL}Login");

            try
            {
                ServiceLayer.send(json.ToString());
            }
            catch (Exception ex)
            {
                err = ex.Message;
                result = false;
                return result;
            }

            string ret = GetJsonValue(ServiceLayer.responseText, "SessionId");

            if (string.IsNullOrEmpty(ret))
            {
                err = GetJsonError(ServiceLayer.responseText);
                result = false;
            }
            else
            {
                result = ret.Contains("-");
                err = ret;
                SboCred.SessionId = ret;
            }
            return result;
        }


        public enum SL_Mode
        {
            PATCH = 1,
            POST = 2,
            DELETE = 3
        }

        #region SingePosting

        public static string PatchITR_AllocationWizard(StringBuilder json, int DocEntry)
        {
            var serviceLayerAccess = new ServiceLayerAccess();
            serviceLayerAccess.ServiceLayer_Posting(json, "PATCH", $"InventoryTransferRequests({DocEntry})", "DocEntry", out string output, out string val);
            return output;
        }

        public static string PostITR_AllocationWizard(StringBuilder json)
        {
            var serviceLayerAccess = new ServiceLayerAccess();
            serviceLayerAccess.ServiceLayer_Posting(json, "POST", "InventoryTransferRequests", "CardCode", out string output, out string val);
            return output;
        }

        public static string PostPatch_ItemMasterData(StringBuilder json)
        {
            var serviceLayerAccess = new ServiceLayerAccess();
            serviceLayerAccess.ServiceLayer_Posting(json, "POST", "Items", "ItemCode", out string output, out string val);
            return output;
        }

        public static string PostPatch_ItemMasterData(StringBuilder json, string id)
        {
            var serviceLayerAccess = new ServiceLayerAccess();
            serviceLayerAccess.ServiceLayer_Posting(json, "PATCH", $"Items('{id}')", "ItemCode", out string output, out string val);
            return output;
        }

        public static string Delete_ItemMasterData(string id)
        {
            var serviceLayerAccess = new ServiceLayerAccess();
            serviceLayerAccess.ServiceLayer_Posting(null, "DELETE", $"Items('{id}')", "ItemCode", out string output, out string val);
            return output;
        }

        public static bool SL_Cancel()
        {
            bool output = true;


            return output;
        }

        public static bool SL_AJAX(Func<string, string> value, string url,
            string request, string json, string returnValue)
        {
            var sMessage = "";
            bool isSuccess = true;
            string result = "";
            var sboCred = new SboCredentials();
            var serviceLayer = new ServiceLayerAccess();
            try
            {
                if (serviceLayer.Login(out sMessage))
                {
                    // 1
                    var mainurl = serviceLayer.ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);
                    url = $@"{mainurl}{url}";
                    ServiceLayer.open(request.ToUpper(), "PurchaseOrders");
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {1} out of {8} process", 1, 8);

                    // 2
                    if (request == "PATCH")
                    {
                        ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
                    }
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {2} out of {8} process", 2, 8);

                    // 3

                    if (request != "DELETE")
                    {
                        ServiceLayer.send(json);
                    }
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {3} out of {8} process", 3, 8);

                    // 4
                    string ret = ServiceLayer.responseText;
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {4} out of {8} process", 4, 8);

                    // 5
                    result = GetJsonValue(ret, returnValue);
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {5} out of {8} process", 5, 8);

                    // 6
                    var docnum = GetJsonValue(ret, "DocNum");
                    StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {6} out of {8} process", 6, 8);

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (ret.Contains("error"))
                        {
                            if (ret.Contains("No Matching Record"))
                            {
                                //PublicStatic.frmMain.NotiMsg($"Document has been successfully Added",
                                //System.Drawing.Color.Green);
                            }
                            else
                            {
                                //PublicStatic.frmMain.NotiMsg($"error : {GetJsonError(ret)}", System.Drawing.Color.Red);
                                //isSuccess = false;
                            }

                            return isSuccess;
                        }
                        else
                        {
                            string requestType = request == "PATCH" ? "updated" : "added";
                            //PublicStatic.frmMain.NotiMsg($"{docnum} Document has been successfully {requestType}",
                            //    System.Drawing.Color.Green);
                        }

                        StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {7} out of {8} process", 7, 8);
                    }
                }
            }
            catch (Exception ex)
            {
                if (url.ToUpper().Contains("CANCEL"))
                {
                    // 8
                    StaticHelper._MainForm.ShowMessage($"Document has been successfully cancelled");
                    isSuccess = true;
                }
                else
                {
                    // 8
                    StaticHelper._MainForm.ShowMessage($"error : {ex.Message}", true);
                    isSuccess = false;
                }

                StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {7} out of {8} process", 7, 8);
                return isSuccess;
            }

            // 9
            value(result);
            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded. {8} out of {8} process", 8, 8);

            // 10
            StaticHelper._MainForm.ProgressClear();
            return isSuccess;
        }


        static string GetMethod(SL_Mode slMode)
        {
            return slMode == SL_Mode.POST ? "POST" : (slMode == SL_Mode.PATCH ? "PATCH" : "DELETE");
        }
        public static bool SL_Posting(string sModule, SL_Mode slMode, StringBuilder sbJson, string sRetValue, out string sErr, bool isCancel = false)
        {
            bool output = true;
            try
            {
                if (LoginAction(out string err))
                {
                    string module, retvalue;
                    module = slMode == SL_Mode.POST ? sModule : $"{sModule}('{sRetValue}')";
                    retvalue = slMode == SL_Mode.POST ? sRetValue : "value";

                    sErr = GetJson(module, slMode == SL_Mode.POST ? "POST" : (slMode == SL_Mode.PATCH ? "PATCH" : "DELETE"), sbJson.ToString(), retvalue, isCancel);
                    output = sErr.Contains("error") ? false : true;
                    if (output)
                    {
                        StaticHelper._MainForm.ShowMessage($"Transaction Complete");
                    }
                    else
                    {
                        StaticHelper._MainForm.ShowMessage($"{sErr}", true);
                    }
                }
                else
                {
                    sErr = err;
                    output = false;
                }
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
                output = false;
            }
            return output;
        }

        public static bool SL_Posting(string sModule, SL_Mode slMode, StringBuilder sbJson, int sRetValue, out string sErr, bool isCancel = false)
        {
            bool output = true;
            try
            {
                if (LoginAction(out string err))
                {
                    string module, retvalue;

                    //module = slMode == SL_Mode.POST ? sModule : $"{sModule}({sRetValue})";
                    module = sRetValue > 0 ? $"{sModule}({sRetValue})" : sModule;
                    retvalue = slMode == SL_Mode.POST ? sRetValue.ToString() : "value";

                    sErr = GetJson(module, slMode == SL_Mode.POST ? "POST" : (slMode == SL_Mode.PATCH ? "PATCH" : "DELETE"), sbJson.ToString(), retvalue, isCancel);
                    output = sErr.Contains("error") ? false : true;
                }
                else
                {
                    sErr = err;
                    output = false;
                }
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
                output = false;
            }
            return output;
        }

        #endregion

        #region Batch Uploading

        public static bool SL_BatchPosting(StringBuilder sbJson, string sModule, out string sError)
        {
            bool result = true;
            try
            {
                if (Login(out sError))
                {
                    ServiceLayer.open("POST", $@"{SboCred.strCurrentServiceURL}$batch");
                    ServiceLayer.setRequestHeader("Content-type", $"multipart/mixed;boundary={sModule}");
                    try
                    {
                        ServiceLayer.send(sbJson.ToString());
                        sError = GetJsonValue(ServiceLayer.responseText, "error");
                    }
                    catch (Exception ex)
                    {
                        sError = ex.Message;
                    }

                    result = sError == "Operation completed successfully" ? true : false;
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                result = false;

            }
            return result;
        }

        public static StringBuilder SL_BatchUpload(string sBatchCode, string sBody)
        {
            var json = new StringBuilder();
            json.AppendLine(sBody);
            json.AppendLine($"--{sBatchCode}--");
            return json;
        }


        public static string SL_BatchBody(string sBatchCode, SL_Mode sPostingMode, string sModule, string sBody)
        {
            string mode = sPostingMode == SL_Mode.POST ? "POST" : "PATCH";

            var json = new StringBuilder();
            json.AppendLine("");
            json.AppendLine($"--{sBatchCode}");
            json.AppendLine("Content-Type: application/http");
            json.AppendLine("Content-Transfer-Encoding:binary");
            json.AppendLine("");
            json.AppendLine($"{mode} /b1s/v1/{sModule}");
            json.AppendLine("Content-Type: application/json");
            json.AppendLine("");
            json.AppendLine(sBody);
            return json.ToString();
        }

        #endregion


       

        public static bool SL_PostingWithApproval(string sModule, StringBuilder sbJson, out string errMsg, string DraftDocEntry, bool isApproved = false)
        {
           
            var sMessage = "";
            var bRet = true;
            var slMode = "POST";
            var CurrentModule = "Drafts";
            try
            {
                
              
                var serviceLayer = new ServiceLayerAccess();
              
                var sboCred = new SboCredentials();
                if (serviceLayer.Login(out sMessage))
                {
                    if (!isApproved)
                    {
                        
                        var url = serviceLayer.ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);
                        ServiceLayerAccess.ServiceLayer.open(slMode, $@"{url}{CurrentModule}");

                        try
                        {
                            ServiceLayerAccess.ServiceLayer.send(sbJson.ToString());
                        }
                        catch (Exception ex)
                        {
                            sMessage = $"1st - Posting Error : {ex.Message}";
                            bRet = false;
                        }
                        var sDocEntry = GetJsonValue(ServiceLayerAccess.ServiceLayer.responseText, "DocEntry");

                        if (!sDocEntry.Contains("error"))
                        {
                            var sAppUserName = JsonHelper.GetJsonValue(ServiceLayerAccess.ServiceLayer.responseText, "U_ApproverUserCode");

                            ServiceLayerAccess.ServiceLayer.open("DELETE", $@"{url}{CurrentModule}({sDocEntry})");

                            try
                            {
                                ServiceLayerAccess.ServiceLayer.send("");
                            }
                            catch (Exception ex)
                            {
                                sMessage = $"2nd - Posted in SAP but has an error: {ex.Message}";
                                bRet = false;
                            }

                            var val = JsonHelper.GetJsonValue(ServiceLayerAccess.ServiceLayer.responseText, "DocEntry");

                            if (val.Equals("Operation completed successfully"))
                            {
                                if (!string.IsNullOrEmpty(sAppUserName))
                                {
                                    var json = new StringBuilder();

                                    json.AppendLine("{");
                                    json.AppendLine($@" ""CompanyDB"" : ""{sboCred.Database}"",");
                                    json.AppendLine($@" ""UserName"" : ""{sAppUserName}"",");
                                    json.AppendLine($@" ""Password"" : ""1234"""); // DAPAT MY MAINTENANCE TO DARREL
                                    json.AppendLine("}");

                                    ServiceLayerAccess.ServiceLayer.open("POST", $@"{url}Login");

                                    try
                                    {
                                        ServiceLayerAccess.ServiceLayer.send(json.ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        sMessage = $"3rd - Posted in SAP w/approver but has an error: {ex.Message}";
                                        bRet = false;
                                    }

                                    var ret = JsonHelper.GetJsonValue(ServiceLayerAccess.ServiceLayer.responseText, "SessionId");

                                    if (string.IsNullOrEmpty(ret))
                                    {
                                        sMessage = $"4th - Posted in SAP w/approver but has an error: {JsonHelper.GetJsonError(ServiceLayerAccess.ServiceLayer.responseText)}";
                                        bRet = false;
                                    }
                                    else
                                    {
                                        ServiceLayerAccess.ServiceLayer.open(slMode, $@"{url}{sModule}");
                                        try
                                        {
                                            ServiceLayerAccess.ServiceLayer.send(sbJson.ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                            sMessage = $"5th - Posted in SAP w/approver but has an error: {ex.Message}";
                                            bRet = false;
                                        }
                                        if (ServiceLayerAccess.ServiceLayer.responseText.Contains("2028"))
                                        {
                                            sMessage = "Operation completed successfully";
                                            bRet = true;
                                        }
                                    }
                                }
                                else
                                {
                                    bRet = serviceLayer.ServiceLayer_Posting(sbJson, "POST", sModule, "DocEntry", out sMessage, out string value);

                                    if (ServiceLayerAccess.ServiceLayer.responseText.Contains("2028"))
                                    {
                                        sMessage = "Operation completed successfully";
                                        bRet = true;
                                    }
                                    //sMessage = bRet == true ? sMessage : $"6th - Posted in SAP w/o approver but has an error: {sMessage}";
                                }
                            }
                        }
                        else
                        {
                            sMessage = sDocEntry;
                            bRet = false;
                        }
                    }
                    else
                    {
                        //bRet = serviceLayer.ServiceLayer_Posting(sbJson, "POST", sModule, "DocEntry", out sMessage, out string value);
                     
                        var nsbJson = new StringBuilder("{");

                        nsbJson.AppendLine(@"   ""Document"": {");
                        nsbJson.AppendLine($@"       ""DocEntry"": ""{DraftDocEntry}""");
                        nsbJson.AppendLine(@"   }   }");

                        bRet = serviceLayer.ServiceLayer_Posting(nsbJson, "POST", "DraftsService_SaveDraftToDocument", "DocEntry", out sMessage, out string value);

                        if (ServiceLayerAccess.ServiceLayer.responseText != null)
                        {
                            if (ServiceLayerAccess.ServiceLayer.responseText.Contains("2028"))
                            {
                                sMessage = "Operation completed successfully";
                                bRet = true;
                            }
                        }
                        else
                        {
                            if (sMessage.Contains("Operation aborted"))
                            {
                                sMessage = "Operation completed successfully";
                                bRet = true;
                            }
                        }
                    }

                }
                else
                {
                    bRet = false;
                }
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
                bRet = false;
            }
            errMsg = sMessage;
            return bRet;
        }
    }
    //public class ServiceLayerAccess
    //{

    //    public static ServerXMLHTTP60 ServiceLayer { get; set; }

    //    public string ServiceURL_Update(string sUrl, string sTag)
    //    {
    //        var output = string.Empty;
    //        try
    //        {
    //            output = $"{sUrl}{sTag}";
    //            const string httpStr = "http://";
    //            const string httpsStr = "https://";
    //            if (!output.StartsWith(httpStr, true, null) &&
    //                !output.StartsWith(httpsStr, true, null))
    //            {
    //                output = httpsStr + output;
    //            }

    //            if (ServiceLayer == null)
    //            { ServiceLayer = new ServerXMLHTTP60(); }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception($"Error : Service Layer Access Return Service URL {ex.Message}");
    //        }
    //        return output;
    //    }

    //    public bool Login(out string sMessage)
    //    {
    //        var output = false;
           
    //        try
    //        {
    //            var sboCred = new SboCredentials();
    //            var model = new List<LoginModel>();

    //            model.Add(new LoginModel
    //            {
    //                CompanyDB = sboCred.Database,
    //                UserName = sboCred.UserId,
    //                Password = sboCred.Password
    //            });

    //            var url = ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);

               
    //            ServiceLayer.open("POST", $@"{url}Login");
    //            ServiceLayer.setOption(SERVERXMLHTTP_OPTION.SXH_OPTION_IGNORE_SERVER_SSL_CERT_ERROR_FLAGS, 13056);
    //            var json = new JavaScriptSerializer().Serialize(model).ToString();

    //            if (json.Length > 2)
    //            {
    //                json = json.Substring(1, json.Length - 2);
    //            }
    //            MessageBox.Show("Mag Execute na");
    //            ServiceLayer.send(json);
    //            MessageBox.Show("Done na");
    //            sMessage = JsonHelper.GetJsonValue(ServiceLayer.responseText, "SessionId");

    //            output = IsLoginSuccess(sMessage);
    //        }
    //        catch (Exception ex)
    //        {
    //            sMessage = $"Error : Service Layer Access Return Login {ex.Message}";
    //            throw new Exception(sMessage);
    //        }

    //        return output;
    //    }

    //    bool IsLoginSuccess(string sResponse)
    //    {
    //        var output = false;
    //        try
    //        {
    //            if (string.IsNullOrEmpty(sResponse))
    //            { output = false; }
    //            else
    //            { output = sResponse.Contains("-"); }
    //        }
    //        catch (Exception ex)
    //        { throw new Exception($"Error : Service Layer Access Return IsLoginSuccess {ex.Message}"); }

    //        return output;
    //    }

    //    public bool ServiceLayer_Posting(StringBuilder sbJson, string sTypeOfRequest, string sModule, string sGetValueIfError, out string sError, out string sValue)
    //    {

    //        var sboCred = new SboCredentials();
    //        bool result = true;
    //        try
    //        {
    //            if (Login(out sError))
    //            {
    //                var url = ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);

    //                ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);
    //                if (sTypeOfRequest == "PATCH" && (sModule.Contains("CartonList") || sModule.Contains("CartonMngt") || sModule.Contains("OPKL") || sModule.Contains("Orders") || sModule.Contains("InventoryTransferRequests")))
    //                {
    //                    ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
    //                }

    //                try
    //                {

    //                    var json = sbJson == null ? "" : sbJson.ToString();
    //                    if (!string.IsNullOrEmpty(json))
    //                    {
    //                        var jObject = JObject.Parse(json);
    //                    }

    //                    ServiceLayer.send(json);

    //                    sError = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);

    //                    for (int x = 0; sError.Contains("Unknown error"); x++)
    //                    {
    //                        Thread.Sleep(3000);
    //                        //ServiceLayer.abort(sTypeOfRequest, $@"{url}{sModule}", false, null, null);

    //                        ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);

    //                        if (sTypeOfRequest == "PATCH" && (sModule.Contains("CartonList") || sModule.Contains("CartonMngt") || sModule.Contains("OPKL") || sModule.Contains("Orders") || sModule.Contains("InventoryTransferRequests")))
    //                        {
    //                            ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
    //                        }

    //                        ServiceLayer.send(json);
    //                        sError = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
    //                        //sError = jObject.ToString();

    //                        //MessageBox.Show($"{sGetValueIfError} {ServiceLayer.responseText} {jObject}");

    //                        if (!sError.Contains("Unknown error"))
    //                        {
    //                            break;
    //                        }
    //                    }

    //                    //MessageBox.Show($"{sGetValueIfError} {ServiceLayer.responseText} {jObject}");

    //                    sValue = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
    //                    sError = sError != "" && sError.Contains("error") == false ? "Operation completed successfully" : sError;
    //                    //sError = jObject.ToString();

    //                }
    //                catch (Exception ex)
    //                {
    //                    if (ex.Message.StartsWith("Operation aborted"))
    //                    {
    //                        sError = "Operation completed successfully";
    //                        sValue = "0";
    //                    }
    //                    else
    //                    {
    //                        sError = ex.Message;
    //                        sValue = "0";
    //                    }
    //                }

    //                result = sError == "Operation completed successfully" ? true : false;
    //            }
    //            else
    //            {
    //                sValue = "0";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            sError = ex.Message;
    //            sValue = "0";
    //            result = false;
    //        }
    //        return result;
    //    }

    //    public bool ServiceLayer_DataTransfer(StringBuilder sbJson, string sTypeOfRequest, string sModule, string sGetValueIfError, out string sError, out string sValue)
    //    {
    //        var sboCred = new SboCredentials();
    //        bool result = true;
    //        try
    //        {
    //            XMLHTTP60 serviceLayer = new XMLHTTP60();

    //            var model = new List<LoginModel>();

    //            model.Add(new LoginModel
    //            {
    //                CompanyDB = sboCred.Database,
    //                UserName = sboCred.UserId,
    //                Password = sboCred.Password
    //            });

    //            var url = $@"http://{sboCred.ServiceLayer}{sboCred.SLTag}Login";

    //            serviceLayer.open("POST", url);
    //            var json = new JavaScriptSerializer().Serialize(model).ToString();

    //            if (json.Length > 2)
    //            {
    //                json = json.Substring(1, json.Length - 2);
    //            }

    //            serviceLayer.send(json);
    //            sError = JsonHelper.GetJsonValue(serviceLayer.responseText, "SessionId");

    //            var output = IsLoginSuccess(sError);
    //            if (output)
    //            {
    //                url = $@"http://{sboCred.ServiceLayer}{sboCred.SLTag}{sModule}";
    //                serviceLayer.open(sTypeOfRequest, url);

    //                try
    //                {
    //                    json = sbJson.ToString();
    //                    var jObject = JObject.Parse(json);

    //                    serviceLayer.send(json);
    //                    sError = JsonHelper.GetJsonValue(serviceLayer.responseText, sGetValueIfError);

    //                    for (int x = 0; sError.Contains("Unknown error"); x++)
    //                    {
    //                        //Thread.Sleep(3000);

    //                        // added 12062021 - to resolve Exception from HRESULT: 0xC00C0240
    //                        serviceLayer.open(sTypeOfRequest, url);
    //                        serviceLayer.send(json);
    //                        sError = JsonHelper.GetJsonValue(serviceLayer.responseText, sGetValueIfError);
    //                        if (!sError.Contains("Unknown error"))
    //                        {
    //                            break;
    //                        }
    //                    }

    //                    sValue = JsonHelper.GetJsonValue(serviceLayer.responseText, sGetValueIfError);
    //                    sError = sError != "" && sError.Contains("error") == false ? "Operation completed successfully" : sError;

    //                }
    //                catch (Exception ex)
    //                {
    //                    sError = ex.Message;
    //                    sValue = "0";

    //                }

    //                result = sError == "Operation completed successfully" ? true : false;
    //            }
    //            else
    //            {
    //                sValue = "0";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            sError = ex.Message;
    //            sValue = "0";
    //            result = false;
    //        }
    //        return result;
    //    }

    //    public bool ServiceLayer_PostingUDO(StringBuilder sbJson, string sTypeOfRequest, string sModule, string sGetValueIfError, out string sError, out string sValue)
    //    {
    //        var sboCred = new SboCredentials();
    //        bool result = true;
    //        try
    //        {
    //            if (Login(out sError))
    //            {
    //                var url = ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);

    //                ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);
    //                if (sTypeOfRequest == "PATCH")
    //                {
    //                    ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
    //                }

    //                try
    //                {
    //                    var json = sbJson.ToString();
    //                    ServiceLayer.send(@"{ PKL1Collection : []}");
    //                    //var jObject = JObject.Parse(json);
    //                    ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);
    //                    ServiceLayer.send(json);
    //                    sError = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
    //                    sValue = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
    //                    sError = sError != "" && sError.Contains("error") == false ? "Operation completed successfully" : sError;

    //                }
    //                catch (Exception ex)
    //                {
    //                    sError = ex.Message;
    //                    sValue = "0";
    //                }

    //                result = sError == "Operation completed successfully" ? true : false;
    //            }
    //            else
    //            {
    //                sValue = "0";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            sError = ex.Message;
    //            sValue = "0";
    //            result = false;
    //        }
    //        return result;
    //    }

    //}
}
