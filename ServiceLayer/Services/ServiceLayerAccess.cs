using DirecLayer;
using DomainLayer.Helper;
using DomainLayer.Models;
using MSXML2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Threading;
using DomainLayer;

namespace ServiceLayer.Services
{
    public class ServiceLayerAccess
    {
        public static XMLHTTP60 ServiceLayer { get; set; }
        //public static ServerXMLHTTP60 ServiceLayer { get; set; } //09212024 - andric & Joses/ Commented Error (Method Not Found get_ServiceLayer)
        public string ServiceURL_Update(string sUrl, string sTag)
        {
            var output = string.Empty;
            try
            {
                output = $"{sUrl}{sTag}";
                const string httpStr = "http://";
                const string httpsStr = "https://";
                if (!output.StartsWith(httpStr, true, null) &&
                    !output.StartsWith(httpsStr, true, null))
                {
                    output = httpsStr + output;
                }

                if (ServiceLayer == null)
                {
                    //ServiceLayer = new ServerXMLHTTP60();
                    ServiceLayer = new XMLHTTP60();
                }
            }
            catch (Exception ex)
            {
            throw new Exception($"Error : Service Layer Access Return Service URL {ex.Message}"); }
            return output;
        }

        public bool Login(out string sMessage)
        {
            var output = false;
            try
            {
                var sboCred = new SboCredentials();
                var model = new List<LoginModel>();

                model.Add(new LoginModel
                {
                    CompanyDB = sboCred.Database,
                    UserName = sboCred.UserId,
                    Password = sboCred.Password
                });

                var url = ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);

                ServiceLayer.open("POST", $@"{url}Login");
                //ServiceLayer.setOption(SERVERXMLHTTP_OPTION.SXH_OPTION_IGNORE_SERVER_SSL_CERT_ERROR_FLAGS, 13056); //09212024 - andric & Joses/ Commented Error (Method Not Found get_ServiceLayer)
                var json = new JavaScriptSerializer().Serialize(model).ToString();

                if (json.Length > 2)
                {
                    json = json.Substring(1, json.Length - 2);
                }

                ServiceLayer.send(json);
                sMessage = JsonHelper.GetJsonValue(ServiceLayer.responseText, "SessionId");

                output = IsLoginSuccess(sMessage);
            }
            catch (Exception ex)
            {
                sMessage = $"Error : Service Layer Access Return Login {ex.Message}";
                throw new Exception(sMessage);
            }

            return output;
        }

        bool IsLoginSuccess(string sResponse)
        {
            var output = false;
            try
            {
                if (string.IsNullOrEmpty(sResponse))
                { output = false; }
                else
                { output = sResponse.Contains("-"); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Service Layer Access Return IsLoginSuccess {ex.Message}"); }

            return output;
        }

        public bool ServiceLayer_Posting(StringBuilder sbJson, string sTypeOfRequest, string sModule, string sGetValueIfError, out string sError, out string sValue)
        {
           
            var sboCred = new SboCredentials();
            bool result = true;
            try
            {
                if (Login(out sError))
                {
                    var url = ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);

                    ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);
                    if (sTypeOfRequest == "PATCH" && (sModule.Contains("CartonList") || sModule.Contains("CartonMngt") || sModule.Contains("OPKL") || sModule.Contains("Orders") || sModule.Contains("InventoryTransferRequests")))
                    {
                        ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
                    }

                    try
                    {

                        var json = sbJson == null ? "" : sbJson.ToString();
                        if (!string.IsNullOrEmpty(json))
                        {
                            var jObject = JObject.Parse(json);
                        }

                        ServiceLayer.send(json);

                        sError = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);

                        for (int x = 0; sError.Contains("Unknown error"); x++)
                        {
                            Thread.Sleep(3000);
                            //ServiceLayer.abort(sTypeOfRequest, $@"{url}{sModule}", false, null, null);

                            ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);

                            if (sTypeOfRequest == "PATCH" && (sModule.Contains("CartonList") || sModule.Contains("CartonMngt") || sModule.Contains("OPKL") || sModule.Contains("Orders") || sModule.Contains("InventoryTransferRequests")))
                            {
                                ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
                            }

                            ServiceLayer.send(json);
                            sError = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
                            //sError = jObject.ToString();

                            //MessageBox.Show($"{sGetValueIfError} {ServiceLayer.responseText} {jObject}");

                            if (!sError.Contains("Unknown error"))
                            {
                                break;
                            }
                        }

                        //MessageBox.Show($"{sGetValueIfError} {ServiceLayer.responseText} {jObject}");

                        sValue = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
                        sError = sError != "" && sError.Contains("error") == false ? "Operation completed successfully" : sError;
                        //sError = jObject.ToString();

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.StartsWith("Operation aborted"))
                        {
                            sError = "Operation completed successfully";
                            sValue = "0";
                        }
                        else
                        {
                            sError = ex.Message;
                            sValue = "0";
                        }
                    }

                    result = sError == "Operation completed successfully" ? true : false;
                }
                else
                {
                    sValue = "0";
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                sValue = "0";
                result = false;
            }
            return result;
        }

        public bool ServiceLayer_DataTransfer(StringBuilder sbJson, string sTypeOfRequest, string sModule, string sGetValueIfError, out string sError, out string sValue)
        {
            var sboCred = new SboCredentials();
            bool result = true;
            try
            {
                XMLHTTP60 serviceLayer = new XMLHTTP60();

                var model = new List<LoginModel>();

                model.Add(new LoginModel
                {
                    CompanyDB = sboCred.Database,
                    UserName = sboCred.UserId,
                    Password = sboCred.Password
                });

                var url = $@"http://{sboCred.ServiceLayer}{sboCred.SLTag}Login";

                serviceLayer.open("POST", url);
                var json = new JavaScriptSerializer().Serialize(model).ToString();

                if (json.Length > 2)
                {
                    json = json.Substring(1, json.Length - 2);
                }

                serviceLayer.send(json);
                sError = JsonHelper.GetJsonValue(serviceLayer.responseText, "SessionId");

                var output = IsLoginSuccess(sError);
                if (output)
                {
                    url = $@"http://{sboCred.ServiceLayer}{sboCred.SLTag}{sModule}";
                    serviceLayer.open(sTypeOfRequest, url);

                    try
                    {
                        json = sbJson.ToString();
                        var jObject = JObject.Parse(json);

                        serviceLayer.send(json);
                        sError = JsonHelper.GetJsonValue(serviceLayer.responseText, sGetValueIfError);

                        for (int x = 0; sError.Contains("Unknown error"); x++)
                        {
                            //Thread.Sleep(3000);

                            // added 12062021 - to resolve Exception from HRESULT: 0xC00C0240
                            serviceLayer.open(sTypeOfRequest, url);
                            serviceLayer.send(json);
                            sError = JsonHelper.GetJsonValue(serviceLayer.responseText, sGetValueIfError);
                            if (!sError.Contains("Unknown error"))
                            {
                                break;
                            }
                        }

                        sValue = JsonHelper.GetJsonValue(serviceLayer.responseText, sGetValueIfError);
                        sError = sError != "" && sError.Contains("error") == false ? "Operation completed successfully" : sError;

                    }
                    catch (Exception ex)
                    {
                        sError = ex.Message;
                        sValue = "0";

                    }

                    result = sError == "Operation completed successfully" ? true : false;
                }
                else
                {
                    sValue = "0";
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                sValue = "0";
                result = false;
            }
            return result;
        }

        public bool ServiceLayer_PostingUDO(StringBuilder sbJson, string sTypeOfRequest, string sModule, string sGetValueIfError, out string sError, out string sValue)
        {
            var sboCred = new SboCredentials();
            bool result = true;
            try
            {
                if (Login(out sError))
                {
                    var url = ServiceURL_Update(sboCred.ServiceLayer, sboCred.SLTag);

                    ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);
                    if (sTypeOfRequest == "PATCH")
                    {
                        ServiceLayer.setRequestHeader("B1S-ReplaceCollectionsOnPatch", "true");
                    }

                    try
                    {
                        var json = sbJson.ToString();
                        ServiceLayer.send(@"{ PKL1Collection : []}");
                        //var jObject = JObject.Parse(json);
                        ServiceLayer.open(sTypeOfRequest, $@"{url}{sModule}", false, null, null);
                        ServiceLayer.send(json);
                        sError = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
                        sValue = JsonHelper.GetJsonValue(ServiceLayer.responseText, sGetValueIfError);
                        sError = sError != "" && sError.Contains("error") == false ? "Operation completed successfully" : sError;

                    }
                    catch (Exception ex)
                    {
                        sError = ex.Message;
                        sValue = "0";
                    }

                    result = sError == "Operation completed successfully" ? true : false;
                }
                else
                {
                    sValue = "0";
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                sValue = "0";
                result = false;
            }
            return result;
        }

    }
}
