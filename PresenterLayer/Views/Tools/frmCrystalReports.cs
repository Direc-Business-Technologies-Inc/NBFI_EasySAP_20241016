using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer;
using DirecLayer;
using DomainLayer.Helper;
using MetroFramework;
using MetroFramework.Forms;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirecLayer
{
    public partial class frmCrystalReports : MetroForm
    {
        private string DocKey;

        public string oDocKey { get { return DocKey; } set => DocKey = value; }

        public string rptName = "", view = "", type = "", userCode = "";
        public bool print = false;

        public frmCrystalReports()
        {
            InitializeComponent();
        }

        private void frmCrystalReports_Load(object sender, EventArgs e)
        {
            txtDocEntry.Text = DocKey;
        }

        private void pbSelectForms_Click(object sender, EventArgs e)
        {
            var sboCred = new SboCredentials();
            using (var ofd = new OpenFileDialog())
            {
                var path = type == "PO" || type == "ITR" || type == "AR" || type == "IT" & type == "ARI" ? AppDomain.CurrentDomain.BaseDirectory + $"Crystals\\Forms\\{type}" : $"{sboCred.Reports}\\Cartons";

                ofd.InitialDirectory = path;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                        {
                            txtPath.Text = ofd.FileName;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            //if(type == "AR")
            //{
            //    var res = MetroMessageBox.Show(StaticHelper._MainForm, "Do you want to print pick list?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (res == DialogResult.Yes)
            //    {
            //        var cr = new frmCrystalReports();
            //        cr.oDocKey = PublicStatic.DtRunID;
            //        cr.type = "AR";
            //        FormHelper.ShowForm(cr);
            //    }
            //}
        }

        private void frmCrystalReports_FormClosing(object sender, FormClosingEventArgs e)
        {
            print = true;
        }

        private void pbFindDocument_Click(object sender, EventArgs e)
        {
            frmSearch2 fS = new frmSearch2();

            switch (type)
            {
                case "Outright":
                    fS.oSearchMode = "SearchDocPackinglist";
                    fS.oFormTitle = "List of Packing List";
                    break;

                case "Concession":
                    fS.oSearchMode = "SearchDocAsnList";
                    fS.oFormTitle = "List of Advance Shipping Notice";
                    break;

                case "PO":
                    fS.oSearchMode = "OPOR";
                    fS.oFormTitle = "List of Purchase Orders";
                    break;

                case "ITR":
                    fS.oSearchMode = "OWTQ_FIND";
                    fS.oFormTitle = "List of Inventory Transfer Requests";
                    break;

                case "IT":
                    fS.oSearchMode = "OWTR";
                    fS.oFormTitle = "List of Inventory Transfer";
                    break;

                case "ARI":
                    fS.oSearchMode = "OINV";
                    fS.oFormTitle = "List of AR Invoice";
                    break;
            }

            fS.ShowDialog();

            if (fS.oCode != null)
            {
                txtDocEntry.Text = type == "PO" ? fS.oName : fS.oCode;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            //Default path: \\HANASERVERNBFI\b1_shf\AttachmentsPath\Extensions
            try
            {
                if (txtPath.Text != string.Empty)
                {
                    var cryRpt = new ReportDocument();
                    var crtableLogoninfos = new TableLogOnInfos();
                    var crtableLogoninfo = new TableLogOnInfo();
                    var crConnectionInfo = new ConnectionInfo();

                    cryRpt.Load(txtPath.Text);

                    if(txtPath.Text.Contains("Report Type"))
                    {
                        cryRpt.SetParameterValue("UploadID", txtDocEntry.Text);
                    }
                    else
                    {
                        cryRpt.SetParameterValue("DocKey@", txtDocEntry.Text);

                        if (userCode != string.Empty && txtPath.Text.Contains("Stock Transfer Request - CR") == false
                            && (txtPath.Text.Contains("OWTR - Delivery Receipts Summary Sheet") == false
                            && txtPath.Text.Contains("OWTR - Item Information(DR Summary)") == false
                            && txtPath.Text.Contains("OWTQ - (SM) Pull-Out Letter") == false))
                        //&& txtPath.Text.Contains("@OPKL") == false))
                        {
                            cryRpt.SetParameterValue("UserCode@", userCode);
                        }
                    }
                    

                    //#############################################################################
                    // DbConnectionAttributes contains some, but not all, consts.
                    var logonProperties = new DbConnectionAttributes();
                    var sboCred = new SboCredentials();

                    logonProperties.Collection.Set("Connection String", @"DRIVER={HDBODBC32};SERVERNODE=" + sboCred.DbServer + "; UID=" + sboCred.DbUserId + ";PWD=" + sboCred.DbPassword + ";CS=" + sboCred.Database + "; ");
                    //logonProperties.Collection.Set("Connection String", @"DRIVER ={B1CRHPROXY32}; SERVERNODE = " + sboCred.DbServer + "; DATABASE = " + sboCred.Database + "");
                    logonProperties.Collection.Set("UseDSNProperties", false);

                    var connectionAttributes = new DbConnectionAttributes();
                    connectionAttributes.Collection.Set("Database DLL", "crdb_odbc.dll");
                    connectionAttributes.Collection.Set("QE_DatabaseName", String.Empty);
                    connectionAttributes.Collection.Set("QE_DatabaseType", "ODBC (RDO)");
                    connectionAttributes.Collection.Set("QE_LogonProperties", logonProperties);
                    connectionAttributes.Collection.Set("QE_ServerDescription", sboCred.DbServer);
                    connectionAttributes.Collection.Set("QE_SQLDB", false);
                    connectionAttributes.Collection.Set("SSO Enabled", false);
                    //#############################################################################

                    //crConnectionInfo.ServerName = sboCred.DbServer;
                    //crConnectionInfo.DatabaseName = sboCred.Database;
                    //crConnectionInfo.UserID = sboCred.DbUserId;
                    //crConnectionInfo.Password = sboCred.DbPassword;

                    crConnectionInfo.Attributes = connectionAttributes;
                    crConnectionInfo.Type = ConnectionInfoType.CRQE;
                    crConnectionInfo.IntegratedSecurity = false;

                    foreach (Table CrTable in cryRpt.Database.Tables)
                    {
                        crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo);
                    }

                    crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();
                }
                else
                {
                    StaticHelper._MainForm.ShowMessage("Error: No forms selected", true);
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
            }
        }

        private string GetDBFolder()
        {
            var sboCred = new SboCredentials();
            string result;
            string strFolderName = "";

            if (sboCred.Database.Contains("NBFI") || sboCred.Database.Contains("EPC"))
            {
                strFolderName = sboCred.Database.Contains("NBFI") ? "NBFI" : "EPC";
                strFolderName += sboCred.Database.Contains("SOFTLIVE") ? " SOFTLIVE" : " LIVE";
            }

            result = strFolderName;

            return result;
        }
    }
}
