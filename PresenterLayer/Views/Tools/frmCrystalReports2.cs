using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DirecLayer;
using DomainLayer.Helper;
using DomainLayer.Models;
using MetroFramework.Forms;
using PresenterLayer;
using PresenterLayer.Helper;
using System;
using System.Data;
using System.Drawing;

namespace DirecLayer
{
    public partial class frmCrystalReports2 : MetroForm
    {
        public string moduleType;
        private string DocKey, UserCode;
        public string xcode, xcardcode;
        public Boolean fromReportList = false;
        public string zPath, zReportName;
        public string zDocKey, zLayoutName, zPrinterName, zUserType, zDocCode;
        public int count, zQty;
        public string oDocKey { get { return DocKey; } set { DocKey = value; } }
        public string oUserCode { get { return UserCode; } set { UserCode = value; } }
        ReportDocument cryRpt = new ReportDocument();
        TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
        TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
        ConnectionInfo crConnectionInfo = new ConnectionInfo();
        SboCredentials sboCredsss = new SboCredentials();

        public frmCrystalReports2()
        {
            InitializeComponent();
        }

        private void frmCrystalReports_Load(object sender, EventArgs e)
        {
            // crystalReportViewer1.DisplayGroupTree = false;

            switch (moduleType)
            {
                case "PO":
                    POPprint("PO");
                    break;
                case "PL":
                    POPprint("PL");
                    break;
                case "OutRight":
                    POPprint("OutRight");
                    break;
                case "AR":
                    POPprint("ARI");
                    break;
            }

        }

        private void POPprint(string strPrintName)
        {
            Tables CrTables;
            var sboCred = new SboCredentials();

            if (strPrintName == "PO")
            {

                if (fromReportList == false)
                {
                    string xgetpathandname, xpath, xname;
                    //NEW QUERY as of 9.13.16
                    xgetpathandname = "SELECT B.U_Path [xpath], B.U_LayoutName [xlayout] FROM [@ODPL] A INNER JOIN [@ADPL] B ON A.Code = B.Code" +
                                       $" WHERE B.U_UserCode = '{EasySAPCredentialsModel.GetEmployeeCode()}'" +
                                       $" AND A.U_CardCode = '{xcardcode}'" +
                                       $" AND B.U_Object = '{  xcode }'";

                    var dt = new DataTable();
                    dt = DataAccess.Select(DataAccess.conStr("HANA"), xgetpathandname/*, StaticHelper._MainForm*/);

                    if (DataAccess.Exist(dt/*, StaticHelper._MainForm*/) == true)
                    {
                        xpath = DataAccess.DataTableReplace(dt, 0, "xpath", "");
                        xname = DataAccess.DataTableReplace(dt, 0, "xlayout", "");

                        cryRpt.Load($"{xpath}\\{xname}.rpt");
                        cryRpt.SetParameterValue("DocKey@", oDocKey);
                        cryRpt.SetParameterValue("UserCode@", SboCred.UserID);

                        crConnectionInfo.ServerName = SboCred.Server;
                        crConnectionInfo.DatabaseName = SboCred.Database;
                        crConnectionInfo.UserID = SboCred.DBUserid;
                        crConnectionInfo.Password = SboCred.DBPassword;
                        CrTables = cryRpt.Database.Tables;
                        foreach (Table CrTable in CrTables)
                        { crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo); }
                        crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();

                    }
                    else
                    { /*PublicStatic.frmMain.NotiMsg("No report found!", Color.Red);*/ }
                }
                else
                {
                    cryRpt.Load($"{zPath}\\{zReportName}.rpt");
                    cryRpt.SetParameterValue("dockey@", oDocKey);
                    cryRpt.SetParameterValue("UserCode@", SboCred.UserID);

                    crConnectionInfo.ServerName = SboCred.Server;

                    crConnectionInfo.DatabaseName = SboCred.Database;
                    crConnectionInfo.UserID = SboCred.DBUserid;
                    crConnectionInfo.Password = SboCred.DBPassword;
                    CrTables = cryRpt.Database.Tables;
                    foreach (Table CrTable in CrTables)
                    { crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo); }
                    crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();
                }
            }
            else if (strPrintName == "PL")
            {
                ReportDocument cryRpt = new ReportDocument();
                string path = AppDomain.CurrentDomain.BaseDirectory + $"\\Crystals\\Forms\\Inv_PackList\\{zLayoutName}.rpt";

                if (System.IO.File.Exists(path) == true)
                {
                    crConnectionInfo.ServerName = SboCred.Server;
                    crConnectionInfo.DatabaseName = SboCred.Database;
                    crConnectionInfo.UserID = SboCred.DBUserid;
                    crConnectionInfo.Password = SboCred.DBPassword;
                    CrTables = cryRpt.Database.Tables;
                    foreach (Table CrTable in CrTables)
                    { crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo); }
                    crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();
                }
                else
                {
                    string xgetpathandname = "SELECT T0.U_Path [xpath] FROM [@ADPL] T0 " +
                                             "LEFT JOIN RDOC T1 ON T1.DocCode = T0.U_LayoutName " +
                                             $"WHERE (T0.U_UserCode = '{zUserType}' and T0.U_LayoutName = '{zDocCode}')";

                    var dt = new DataTable();
                    dt = DataAccess.Select(DataAccess.conStr("HANA"), xgetpathandname/*, PublicStatic.frmMain*/);

                    string xpath = DataAccess.DataTableReplace(dt, 0, "xpath", "");

                    cryRpt.Load($"{xpath}\\{zLayoutName}.rpt");

                    cryRpt.SetParameterValue("dockey@", zDocKey);

                    crConnectionInfo.ServerName = SboCred.Server;
                    crConnectionInfo.DatabaseName = SboCred.Database;
                    crConnectionInfo.UserID = SboCred.DBUserid;
                    crConnectionInfo.Password = SboCred.DBPassword;
                    CrTables = cryRpt.Database.Tables;
                    foreach (Table CrTable in CrTables)
                    { crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo); }
                    crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();
                }

            }
            else if (strPrintName == "OutRight")
            {
                ReportDocument cryRpt = new ReportDocument();
                string path = AppDomain.CurrentDomain.BaseDirectory + $"\\Crystals\\Forms\\Inv_PackList\\@OPKL - CMC Carton Label.rpt";

                if (System.IO.File.Exists(path) == true)
                {
                    crConnectionInfo.ServerName = SboCred.Server;
                    crConnectionInfo.DatabaseName = SboCred.Database;
                    crConnectionInfo.UserID = SboCred.DBUserid;
                    crConnectionInfo.Password = SboCred.DBPassword;
                    CrTables = cryRpt.Database.Tables;
                    foreach (Table CrTable in CrTables)
                    { crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo); }
                    crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();
                }
            }
            else if (strPrintName == "ARI")
            {
                ReportDocument cryRpt = new ReportDocument();
                string path = AppDomain.CurrentDomain.BaseDirectory + $"\\Crystals\\Forms\\AR\\OINV - Sales Invoice Online - EPC (Report Type) v2.rpt";


                if (System.IO.File.Exists(path) == true)
                {
                    //crConnectionInfo.ServerName = SboCred.Server;
                    //crConnectionInfo.DatabaseName = SboCred.Database;
                    //crConnectionInfo.UserID = SboCred.DBUserid;
                    //crConnectionInfo.Password = SboCred.DBPassword;

                    crConnectionInfo.ServerName = sboCred.DbServer;
                    crConnectionInfo.DatabaseName = sboCred.Database;
                    crConnectionInfo.UserID = sboCred.DbUserId;
                    crConnectionInfo.Password = sboCred.DbPassword;

                    cryRpt.Load(path);
                    CrTables = cryRpt.Database.Tables;

                    foreach (Table CrTable in CrTables)
                    { crtableLogoninfo = CrTable.LogOnInfo; crtableLogoninfo.ConnectionInfo = crConnectionInfo; CrTable.ApplyLogOnInfo(crtableLogoninfo); }
                    crystalReportViewer1.ReportSource = cryRpt; crystalReportViewer1.Refresh();
                }
            }
        }
    }
}
