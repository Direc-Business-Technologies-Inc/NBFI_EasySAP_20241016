using System;
using System.Data;
using System.Linq;
using MetroFramework.Forms;
using System.Windows.Forms;
using System.Collections.Generic;
using DirecLayer;
using DomainLayer;
using MetroFramework;
using PresenterLayer.Helper;
using System.Text;
using System.IO;


namespace PresenterLayer.Views
{
    public partial class frmShowMessage : MetroForm
    {
        public string uploadType { get; set; }

        private string isUploaded = "No";
        internal string fieldheader;
        internal string fieldrow;
        DataContextList context = new DataContextList();

        public frmShowMessage()
        {
            InitializeComponent();
        }

        private void frmShowMessage_Load(object sender, EventArgs e)
        {
            LoadMessages();
        }

        private void CmbSelectExcel_SelectedIndexChanged(object sender, EventArgs e)
        {
            isUploaded = CmbSelectExcel.Text == "Error" ? "No" : "Yes";

            LoadMessages();
        }

        public void LoadMessages()
        {
            var headers = MarketingDocument.DocHeader;
            var GetErrMsgs = DataContextList.GetErrorIds;
            var sql = new SAPMsSqlAccess();

            DgvMessages.DataSource = null;

            if (uploadType == "Inventory Transfer Request")
            {
               // string query = $"SELECT DocDate, CardName, {fieldheader}, ErrorMessage FROM MarketingDocumentHeaders as Header " +
               //$"WHERE ISNULL(Header.CardCode,'') <> '' AND Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' AND Uploaded = '{isUploaded}' " +
               //$"GROUP BY DocDate, CardName, {fieldheader}, ErrorMessage";

                string query = $"SELECT DocDate, CardName, {fieldheader}, ErrorMessage FROM MarketingDocumentHeaders as Header " +
                 $"WHERE Header.Session = '{PublicStatic.DtRunID}' AND Header.[User] = '{Environment.MachineName}' AND Uploaded = '{isUploaded}' " +
                 $"GROUP BY DocDate, CardName, {fieldheader}, ErrorMessage";

                DgvMessages.DataSource = sql.Get(query);

                //DgvMessages.DataSource = headers.Where(x => x.Uploaded == isUploaded).Select(s => new { s.CardCode, s.CardName, s.DocDate, s.ErrorMessage }).ToList();
            }
            else if (uploadType == "Carton Packing List")
            {
                if (isUploaded == "Yes")
                {
                    DgvMessages.DataSource = GetErrMsgs.Where(x => x.Uploaded == isUploaded).Select(s => new { s.UploadType, s.RowCount, s.CardCode, s.CardName, s.DocEntry, s.DocRef, s.Ref1, s.Ref2, s.CartonNo, s.Remarks, s.URemarks, s.ErrMsg }).ToList();
                }
                else
                {
                    DgvMessages.DataSource = GetErrMsgs.Where(x => x.Uploaded == isUploaded).Select(s => new { s.UploadType, s.RowCount, s.CardCode, s.CardName, s.DocEntry, s.DocRef, s.Ref1, s.Ref2, s.CartonNo, s.ItemCode, s.Remarks, s.URemarks, s.ErrMsg }).ToList();
                }
            }
            else
            {
                string query = $"SELECT DocDate, CardName, {fieldheader}, ErrorMessage FROM MarketingDocumentHeaders as Header " +
                $"WHERE ISNULL(Header.CardCode,'') <> '' AND Header.Session = '{PublicStatic.DtRunID}' " +
                //$"AND Header.DocDate != 'CreditDate' AND Header.DocDate not like '%ARCM%'" +
                $"AND Header.[User] = '{Environment.MachineName}' AND Uploaded = '{isUploaded}' " +
                $"GROUP BY DocDate, CardName, {fieldheader}, ErrorMessage";

                DgvMessages.DataSource = sql.Get(query);

                //MessageBox.Show(sql.Get(query).ToString());
            }

            if (DgvMessages.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DgvMessages.Rows)
                {
                    row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
                }
                DgvMessages.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            }

        }

        private void BtnGenerateExcel_Click(object sender, EventArgs e)
        {
            if (TxtFile.Text != string.Empty)
            {
                try
                {
                    //if (uploadType == "Inventory Transfer Request")
                    //{
                    //    GenerateItrReport();
                    //}
                    //else
                    if (uploadType == "Carton Packing List")
                    {
                        GenerateCartonUploadStatus();
                    }
                    else
                    {
                        SqlList();
                    }
                }
                catch (Exception ex)
                { }
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("There is no path selected",true);
            }
        }

        void GenerateCartonUploadStatus()
        {
            if (DgvMessages.Rows.Count > 0)
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                foreach (DataGridViewColumn col in DgvMessages.Columns)
                {
                    dt.Columns.Add(col.Name);
                }

                foreach (DataGridViewRow row in DgvMessages.Rows)
                {
                    DataRow dRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    dt.Rows.Add(dRow);
                }

                GenerateCSVUploadStatus(dt);
            }

        }

        void SqlList()
        {
            var sql = new SAPMsSqlAccess();

            var h = $" Header.{fieldheader}";
            var l = $" Lines.{fieldrow}";

            string query = $"SELECT DISTINCT Header.DocDate, Header.ErrorMessage, Header.CardName, {h}, {l} " +

            "FROM MarketingDocumentHeaders as Header " +

            "INNER JOIN MarketingDocumentLines as Lines ON Header.DocEntry = Lines.DocEntry and Header.Session = Lines.Session " +

            $"WHERE Header.Session = '{PublicStatic.DtRunID}' AND ISNULL(Header.CardCode,'') <> '' AND ISNULL(Lines.ItemCode,'') <> ''  AND Header.[User] = '{Environment.MachineName}' AND Header.Uploaded = '{isUploaded}' " +
            
            $"GROUP BY Header.DocDate, CardName, ErrorMessage, {h}, {l}";

            var dt = sql.Get(query);

            //GenerateExcelUploadStatus(dt);
            GenerateCSVUploadStatus(dt);
        }

        public void GenerateCSVUploadStatus(System.Data.DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            int ColumnCount = 1;
            int MaxColumnCount = dt.Columns.Count;

            // Generating Excel Column value name 
            foreach (DataColumn Col in dt.Columns)
            {
                sb.Append($"{Col.ColumnName},");
                if (ColumnCount == MaxColumnCount)
                {
                    sb.Append($"\r\n");
                }
                ColumnCount++;
            }

            // Excel Cell Value
            var cnt = 1;
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; dt.Columns.Count > 0; i++)
                {
                    try
                    {
                        string value = ValidateInput.String(row[i]);

                        if (dt.Columns[i].ColumnName.ToString() == "ItemCode")
                        {
                            sb.Append($"'{value},");
                        }
                        else
                        {
                            sb.Append($"{value.Replace("] ,", "] ")},");
                        }
                    }
                    catch { 
                        break; 
                    }
                }
                sb.Append($"\r\n");

                StaticHelper._MainForm.Invoke(new System.Action(() =>
                           StaticHelper._MainForm.Progress($"Saving data to excel please wait. {cnt++} out of {dt.Rows.Count}", cnt, dt.Rows.Count)
                       ));
            }

            using (TextWriter sw = new StreamWriter($@"{TxtFile.Text.Replace("/","").Replace(".xlsx","")}.csv"))
            {
                sw.WriteLine(sb.ToString());
            }

            StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
            StaticHelper._MainForm.ProgressClear();
        }

        public void GenerateExcelUploadStatus(System.Data.DataTable dt)
        {
            //Microsoft.Office.Interop.Excel.Application excel;
            //Workbook _workbook;
            //Worksheet _worksheet;

            //excel = new Microsoft.Office.Interop.Excel.Application()
            //{
            //    Visible = false,
            //    DisplayAlerts = false
            //};

            //_workbook = excel.Workbooks.Add(Type.Missing);
            //_worksheet = (Worksheet)_workbook.ActiveSheet;
            //_worksheet.Name = CmbSelectExcel.Text == "Error" ? "Error logs" : "Uploaded";
            //_worksheet.Cells.NumberFormat = "@";

            //int ColumnCount = 1;
            //int RowCount = 2;

            //// Generating Excel Column value name 
            //foreach (DataColumn Col in dt.Columns)
            //{
            //    _worksheet.Cells[1, ColumnCount] = Col.ColumnName;
            //    ColumnCount++;
            //}

            //// Excel Cell Value
            //var cnt = 1;
            //foreach (DataRow row in dt.Rows)
            //{
            //    for (int i = 0; dt.Columns.Count > 0; i++)
            //    {
            //        try
            //        {
            //            string value = ValidateInput.String(row[i]);
            //            var col = i + 1;
            //            _worksheet.Cells[RowCount, col] = value;
            //        }
            //        catch { break; }


            //    }
            //    StaticHelper._MainForm.Invoke(new System.Action(() =>
            //               StaticHelper._MainForm.Progress($"Saving data to excel please wait. {cnt++} out of {dt.Rows.Count}", cnt, dt.Rows.Count)
            //           ));
            //    RowCount++;
            //}

            //_workbook.SaveAs(TxtFile.Text);
            //_workbook.Close();
            //excel.Quit();

            //_workbook = null;
            //_worksheet = null;
            //StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
            //StaticHelper._MainForm.ProgressClear();
        }

        void GenerateItrReport()
        {
            //System.Data.DataTable dt = new System.Data.DataTable();

            //try
            //{
            //    int headerCountList = MarketingDocument.DocHeader.Where(w => w.Uploaded == isUploaded).ToList().Count;

            //    for (int i = 0; headerCountList > i; i++)
            //    {
            //        int ids = 0;

            //        List<string> colValue = new List<string>();

            //        List<string> colValue2 = new List<string>();

            //        if (colValue.Count > 0)
            //        {
            //            colValue.Clear();
            //        }

            //        var a = typeof(MarketingDocumentHeaders)
            //            .GetProperties()
            //            .Select(x => new
            //            {
            //                property = x.Name,
            //                value = x.GetValue(MarketingDocument.DocHeader.Where(w => w.Uploaded == isUploaded).ToList()[i])
            //            })
            //            .Where(x => x.value != null)
            //            .ToList();

            //        int headerColumnCount = a.Count();

            //        foreach (var b in a)
            //        {
            //            if (i == 0)
            //            {
            //                var columnHeader = b.property;
            //                dt.Columns.Add(columnHeader);
            //            }

            //            if (b.property == "DocEntry")
            //            {
            //                ids = Convert.ToInt32(b.value);
            //            }

            //            var columnHeaderValue = b.value.ToString();

            //            colValue.Add(columnHeaderValue);
            //        }

            //        int itemCount = MarketingDocument.DocLines.Where(w => w.DocEntry == ids.ToString()).ToList().Count;

            //        for (int item = 0; itemCount > item; item++)
            //        {
            //            if (itemCount != item)
            //            {
            //                var list = MarketingDocument.DocLines.Where(w => w.DocEntry == ids.ToString()).ToList()[item];
            //                var c = typeof(MarketingDocumentLines)
            //                            .GetProperties()
            //                            .Select(x => new { property = x.Name, value = x.GetValue(list) })
            //                            .Where(x => x.value != null)
            //                            .ToList();

            //                foreach (var d in c)
            //                {
            //                    if (i == 0)
            //                    {
            //                        string ColumnName = d.property;

            //                        if (d.property != "DocEntry")
            //                        {
            //                            dt.Columns.Add(ColumnName);
            //                        }
            //                    }

            //                    if (d.property != "DocEntry")
            //                    {
            //                        string ColumnValue = d.value.ToString();
            //                        colValue2.Add(ColumnValue);
            //                    }
            //                }

            //                var columnCount = headerColumnCount + c.Count;

            //                var sad = new object[columnCount - 1];

            //                int sadCount = 0;
            //                foreach (var column in colValue)
            //                {
            //                    sad[sadCount] = column;
            //                    sadCount++;
            //                }

            //                foreach (var column in colValue2)
            //                {
            //                    sad[sadCount] = column;
            //                    sadCount++;
            //                }

            //                dt.Rows.Add(sad);
            //                colValue2.Clear();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message);
            //}

            //Microsoft.Office.Interop.Excel.Application excel;
            //Workbook _workbook;
            //Worksheet _worksheet;

            //excel = new Microsoft.Office.Interop.Excel.Application()
            //{
            //    Visible = false,
            //    DisplayAlerts = false
            //};

            //_workbook = excel.Workbooks.Add(Type.Missing);
            //_worksheet = (Worksheet)_workbook.ActiveSheet;
            //_worksheet.Name = CmbSelectExcel.Text == "Error" ? "Error logs" : "Uploaded";

            //int colCount = 1;
            //foreach (DataColumn Col in dt.Columns)
            //{
            //    _worksheet.Cells[1, colCount] = Col.ColumnName;
            //    colCount++;
            //}

            //int rowValue = 2;

            //var columnCOunt = dt.Columns.Count;
            //foreach (DataRow row in dt.Rows)
            //{
            //    for (int i = 1; columnCOunt > 1; i++)
            //    {
            //        try
            //        {
            //            string value = row[i - 1].ToString();
            //            _worksheet.Cells[rowValue, i] = value;
            //        }
            //        catch (Exception exex)
            //        {
            //            break;
            //        }
            //    }

            //    rowValue++;
            //}

            //var qwe = "";

            //_workbook.SaveAs(TxtFile.Text);
            //_workbook.Close();
            //excel.Quit();

            //_workbook = null;
            //_worksheet = null;

            //MessageBox.Show("Success");
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            var browser = new FolderBrowserDialog();

            string tempPath = "";

            if (browser.ShowDialog() == DialogResult.OK)
            {
                tempPath = browser.SelectedPath + $"\\{CmbSelectExcel.Text}{uploadType.ToUpper()} ({DateTime.Now.ToShortDateString().Replace("/", "-")} {DateTime.Now.ToShortTimeString().Replace(":", "-")}).xlsx"; // prints path

                TxtFile.Text = tempPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
