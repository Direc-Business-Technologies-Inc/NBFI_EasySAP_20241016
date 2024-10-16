using DirecLayer;
using DomainLayer;
using DomainLayer.Models;
using MetroFramework;
using PresenterLayer.Helper;
using PresenterLayer.Services;
using PresenterLayer.Views.Main;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PresenterLayer.Views
{
    public partial class UcGeneric : UserControl
    {
        Excel ex = new Excel();
        DataContextList contextList = new DataContextList();
        DtController dtController = new DtController();
        DtDgvController dgvSetup = new DtDgvController();
        SAOContext model = new SAOContext();
        UploadController controller = new UploadController();

        public string MapCode { get; set; }
        public MainForm frmMain { get; set; }
        public frmDT frmDt { get; set; }
        public string objType { get; set; }
        private DataSet DataSetResult { get; set; }

        int id;

        public UcGeneric()
        {
            InitializeComponent();
            DtController.objType = objType;
        }

        private void UcGeneric_Load(object sender, EventArgs e)
        {
            id = Convert.ToInt32(MapCode);

            FetchMapping(id);

            TxtMap.Text = model.dtheader.ToList().Find(x => x.MapID == id).MapCode.ToString();
            CmbUploadType.Text = model.dtheader.ToList().Find(x => x.MapID == id).UploadType;
        }

        private void BtnOpenTemplate_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog odf = new OpenFileDialog())
            {
                if (odf.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(odf.FileName, FileMode.Open, FileAccess.Read))
                        {
                            TxtTemplate.Text = odf.FileName;

                            if (ex.ReadExcel((ds) => DataSetResult = ds, fs, Path.GetExtension(odf.FileName)))
                            {
                                ex.CloseExcel();
                            }

                            CmbWorkSheet.Items.Clear();
                            CmbWorkSheet.Text = string.Empty;
                            DeleteSession();

                            foreach (DataTable dt in DataSetResult.Tables)
                            {
                                CmbWorkSheet.Items.Add(dt.TableName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // frmMain.NotiMsg(ex.Message, Color.Red);
                    }
                }
            }
        }

        private void CmbWorkSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dt = DataSetResult.Tables[CmbWorkSheet.SelectedIndex];
            DgvExcel.DataSource = dt;

            CmbFirstColumn.Items.Clear();
            CmbSecondColumn.Items.Clear();

            int colcount = 0;
            foreach (DataColumn col in dt.Columns)
            {

                CmbFirstColumn.Items.Add(col.ColumnName);
                CmbSecondColumn.Items.Add(col.ColumnName);

                if (colcount++ == 0)
                {
                    CmbFirstColumn.Text = col.ColumnName;
                }
                else if (colcount == dt.Columns.Count)
                {
                    CmbSecondColumn.Text = col.ColumnName;
                }
            }

            TxtDocumentCount.Text = dt.Rows.Count.ToString();
            TxtLineCount.Text = dt.Columns.Count.ToString();


            if (CmbUploadType.Text == "Carton Packing List")
            {
                for (int i = 0; DgvExcel.RowCount > i; i++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(DgvExcel.Rows[i + 1].Cells["CTN NUMBER"].Value.ToString()))
                        {
                            DgvExcel.Rows[i + 1].Cells["CTN NUMBER"].Value = DgvExcel.Rows[i].Cells["CTN NUMBER"].Value;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            var result = MetroMessageBox.Show(StaticHelper._MainForm, "Do you want to proceed to uploading?", SystemSettings.Info.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {                
                try
                {
                    FetchMapping(id);
                    BtnUpload.Enabled = false;

                    MarketingDocument.DocHeader.Clear();
                    MarketingDocument.DocLines.Clear();
                    DataContextList.GetErrorIds.Clear();
                    StaticHelper._MainForm.Progress("Please wait...", 1, 100);

                    if (CmbUploadType.Text == "Carton Packing List")
                    {
                        MarketingDocumentsAlgoUpload(out bool ret);
                    }
                    else if (CmbUploadType.Text == "Inventory Counting")
                    {
                        UploadingInventoryCounting();
                    }
                    else if (CmbUploadType.Text == "Sales Order" && TxtMap.Text.ToUpper().Contains("SMPO"))
                    {
                        DataArrangementSMPO();
                    }
                    else
                    {
                        Uploading();
                    }


                    if (CmbUploadType.Text.ToLower().Contains("a/r invoice"))
                    {
                        var sapHana = new SAPHanaAccess();
                        var sapSql = new SAPMsSqlAccess();
                        var strHeader = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());

                        var uploadedTxns = sapHana.Exec($@"Select Count (DocEntry) FROM OINV WHERE U_UploadID = '{PublicStatic.DtRunID}' and CANCELED = 'N'");
                        var uploadedErrors = sapSql.Get($@"Select {strHeader} FROM [MarketingDocumentHeaders] WHERE Session = '{PublicStatic.DtRunID}' and Uploaded = 'No' GROUP BY {strHeader}");
                        var printedSI = sapHana.Exec($@"Select Count (DocEntry) FROM OINV WHERE U_UploadID = '{PublicStatic.DtRunID}' and U_InvoicePrinted = 'Y' and CANCELED = 'N'");
                        var printedPKL = sapHana.Exec($@" Select Count (DocEntry) FROM OINV WHERE U_UploadID = '{PublicStatic.DtRunID}' and U_PKLPrinted = 'Y' and CANCELED = 'N'");

                        var ttt = $"Transaction Complete!@@Total Number of Uploaded Transactions: {uploadedTxns}@Total Number of Upload Errors: {uploadedErrors.Rows.Count}@Total Document Printed SI: {printedSI}@Total Document Printed Picklist: {printedPKL}";

                        ttt = ttt.Replace("@", "" + System.Environment.NewLine);

                        MessageBox.Show($"{ttt}", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Transaction Complete!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    StaticHelper._MainForm.ProgressClear();
                    BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = true));

                }

                catch (Exception ex)
                {
                    StaticHelper._MainForm.ShowMessage(ex.Message, true);
                    if (ex.Message.Contains("OutOfMemoryException"))
                    {
                        GC.Collect();
                    }
                }

            }

        }

        void InventoryCountingAlgo()
        {
            InventoryCounting invCounting = new InventoryCounting();
            InventoryCountingRow invCountingRow = new InventoryCountingRow();

            int headerCount = TxtDocumentCount.Text == string.Empty ? 1 : Convert.ToInt32(TxtDocumentCount.Text);
            int lineCount = TxtLineCount.Text == string.Empty ? 0 : Convert.ToInt32(TxtLineCount.Text);
            int currentCount = 0;
            int ids = 0;

            while (headerCount > currentCount)
            {
                for (int y = 0; DgvExcel.RowCount > y; y++)
                {
                    ids++;
                    StaticHelper._MainForm.Invoke(new Action(() =>
                           StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {currentCount} out of {headerCount}", currentCount, headerCount)
                       ));
                    foreach (var mapping in MappingList.fields.ToList())
                    {
                        try
                        {
                            string SAP = mapping.SapField;
                            string type = mapping.Type;
                            int RowStart = Convert.ToInt32(mapping.RowStart) - 1 + y;
                            int ColStart = Convert.ToInt32(mapping.ColumnStart) - 1;
                            string Flow = mapping.Flow;
                            int RowInterval = Convert.ToInt32(mapping.RowInterval);
                            int ColInterval = Convert.ToInt32(mapping.ColumnInterval);

                            if (type == "Header")
                            {
                                switch (SAP)
                                {
                                    case "CountDate":
                                        invCounting.CountDate = DgvExcel[ColStart, RowStart].Value.ToString();
                                        break;

                                    case "WhsCode":
                                        invCounting.WhsCode = DgvExcel[ColStart, RowStart].Value.ToString();
                                        break;
                                }
                            }
                            else
                            {
                                switch (SAP)
                                {
                                    case "Quantity":
                                        invCountingRow.Quantity = Convert.ToInt32(DgvExcel[ColStart, RowStart].Value);
                                        break;

                                    case "ItemCode":
                                        invCountingRow.ItemCode = DgvExcel[ColStart, RowStart].Value.ToString();
                                        break;
                                }
                            }

                            int dgvExcelCount = DgvExcel.RowCount - 2;
                            if (dgvExcelCount == y && Flow == "Horizontal" && RowInterval <= 0 && ColInterval > 0)
                            {
                                int newColumnInterval = ColStart + ColInterval + 1;
                                mapping.ColumnStart = newColumnInterval;
                            }
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }

                    if (DgvExcel.RowCount - 1 != y)
                    {
                        invCounting.PreparedBy = EasySAPCredentialsModel.EmployeeCompleteName;
                        //model.InventoryCounting.Add(invCounting);
                        //model.InventoryCountingRow.Add(invCountingRow);
                        model.SaveChanges();
                    }
                }

                currentCount++;
            }
        }

        void InventoryTransferRequestAlgoUpload()
        {
            int documentCount = Convert.ToInt32(TxtDocumentCount.Text);
            int lineCount = Convert.ToInt32(TxtLineCount.Text);

            int docEntry = 0;

            contextList.headers.Clear();
            contextList.rows.Clear();

            for (int i = 0; documentCount > i; i++)
            {
                foreach (var mapRow in MappingList.fields.ToList())
                {
                    try
                    {
                        int rw = Convert.ToInt32(mapRow.RowStart) - 1;
                        int cl = Convert.ToInt32(mapRow.ColumnStart) - 1;
                        string Sap = mapRow.SapField;
                        string type = mapRow.Type;
                        string flow = mapRow.Flow;

                        if (type == "Header")
                        {
                            contextList.headers.Add(new Header
                            {
                                Id = i,
                                Name = Sap,
                                Value = GetValue(cl, rw)
                            });
                        }
                        else
                        {
                            if (flow == "Vertical")
                            {
                                int newRw = rw;

                                for (int a = 0; lineCount > a; a++)
                                {
                                    contextList.rows.Add(new Row
                                    {
                                        Id = newRw,
                                        Name = Sap,
                                        Value = GetValue(cl, newRw)
                                    });

                                    newRw++;
                                }
                            }
                            else
                            {
                                int newCl = cl;

                                for (int a = 0; lineCount > a; a++)
                                {
                                    contextList.rows.Add(new Row
                                    {
                                        Id = newCl,
                                        Name = Sap,
                                        Value = GetValue(newCl, rw)
                                    });

                                    newCl++;
                                }
                            }
                        }

                        if (flow == "Vertical" && type != "Row")
                        {
                            int qwe = rw + 2;
                            mapRow.RowStart = qwe;
                        }

                        if (flow == "Vertical" && Sap == "Quantity")
                        {
                            int qwe = cl + 2;
                            mapRow.ColumnStart = qwe;
                        }

                        if (flow == "Horizontal" && Sap == "Quantity")
                        {
                            int qwe = rw + 2;
                            mapRow.RowStart = qwe;
                        }

                        if (flow == "Horizontal" && Sap == "CardCode")
                        {
                            int qwe = cl + 2;
                            mapRow.ColumnStart = qwe;
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }

                //try
                //{
                //    if (SAPAccess.ConnectToSAPDI())
                //    {
                //        foreach (var head in contextList.headers.ToList()) // header upload
                //        {
                //            SAPAccess.oITR = (StockTransfer)SAPAccess.oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);

                //            string cardCode = contextList.headers.Find(x => x.Id == i && x.Name == "CardCode").Value.ToString();

                //            SAPAccess.oITR.CardCode = cardCode;
                //            SAPAccess.oITR.UserFields.Fields.Item("U_AddID").Value = controller.AddId(cardCode);
                //            SAPAccess.oITR.DocDate = Convert.ToDateTime(frmDT_UDF.DocDate);
                //            SAPAccess.oITR.DueDate = Convert.ToDateTime(frmDT_UDF.DocDueDate);
                //            SAPAccess.oITR.TaxDate = Convert.ToDateTime(frmDT_UDF.TaxDate);
                //            SAPAccess.oITR.Series = Convert.ToInt32(frmDT_UDF.oSeries);
                //            SAPAccess.oITR.ToWarehouse = controller.DeliveryWhsCode(cardCode);
                //            SAPAccess.oITR.FromWarehouse = frmDT_UDF.frmWhs;

                //            SAPAccess.oITR.Comments = "Upload From Easy SAP";

                //            MarketingDocument.DocHeader.Add(new MarketingDocumentHeader
                //            {
                //                CardCode = cardCode,
                //                DocDate = frmDT_UDF.DocDate,
                //                DocDueDate = frmDT_UDF.DocDueDate,
                //                TaxDate = frmDT_UDF.TaxDate,
                //                Series = frmDT_UDF.oSeries,
                //                ToWarehouse = controller.DeliveryWhsCode(cardCode),
                //                FromWarehouse = frmDT_UDF.frmWhs,
                //                DocEntry = docEntry.ToString(),
                //            });

                //            if (DECLARE.udf.Where(x => x.ObjCode == objType).ToList().Count > 0)
                //            {
                //                foreach (var x in DECLARE.udf.Where(x => x.ObjCode == objType))
                //                {
                //                    try
                //                    {
                //                        if (x.FieldValue != null)
                //                        {
                //                            SAPAccess.oITR.UserFields.Fields.Item(x.FieldCode).Value = x.FieldValue;
                //                        }
                //                    }
                //                    catch (Exception exp)
                //                    { }
                //                }
                //            }

                //            int count = 0;

                //            foreach (var row in contextList.rows.ToList()) // rows or line upload
                //            {
                //                if (count != lineCount)
                //                {

                //                    string itemcode = contextList.rows.Find(x => x.Id == row.Id && x.Name == "ItemCode").Value.ToString();

                //                    string itemcode2 = controller.DeliveryItem(cardCode, itemcode);

                //                    SAPAccess.oITR.Lines.ItemCode = itemcode2;
                //                    SAPAccess.oITR.Lines.Quantity = Convert.ToDouble(contextList.rows.Find(x => x.Id == row.Id && x.Name == "Quantity").Value);
                //                    SAPAccess.oITR.Lines.ProjectCode = controller.GetProjectCode(cardCode);
                //                    SAPAccess.oITR.Lines.WarehouseCode = controller.DeliveryWhsCode(cardCode);
                //                    SAPAccess.oITR.Lines.FromWarehouseCode = frmDT_UDF.frmWhs;

                //                    SAPAccess.oITR.Lines.Add();

                //                    MarketingDocument.DocLines.Add(new MarketingDocumentLines
                //                    {
                //                        ItemCode = itemcode2,
                //                        Quantity = contextList.rows.Find(x => x.Id == row.Id && x.Name == "Quantity").Value,
                //                        ProjectCode = controller.GetProjectCode(cardCode),
                //                        WarehouseCode = controller.DeliveryWhsCode(cardCode),
                //                        FromWarehouseCode = controller.DeliveryWhsCode(cardCode),
                //                        DocEntry = docEntry.ToString(),
                //                    });
                //                }
                //                else
                //                {
                //                    break;
                //                }

                //                count++;
                //            }

                //            SAPAccess.lRetCode = SAPAccess.oITR.Add();

                //            if (SAPAccess.lRetCode != 0)
                //            {
                //                if (SAPAccess.oCompany.InTransaction)
                //                {
                //                    SAPAccess.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                //                }

                //                SAPAccess.oCompany.GetLastError(out int lErrCode, out string sErrMsg);
                //                SAPAccess.lErrCode = lErrCode;
                //                SAPAccess.sErrMsg = sErrMsg;
                //                frmMain.NotiMsg($"Error Code: {SAPAccess.lErrCode}, Error Message: {SAPAccess.sErrMsg}", Color.Red);

                //                MarketingDocument.DocHeader.Find(x => x.CardCode == cardCode).Uploaded = "No";
                //                MarketingDocument.DocHeader.Find(x => x.CardCode == cardCode).ErrorMessage = $"Error Code: {SAPAccess.lErrCode}, Error Message: {SAPAccess.sErrMsg}";
                //            }
                //            else
                //            {
                //                frmMain.NotiMsg("Document Successfully uploaded", Color.Green);
                //            }

                //            contextList.headers.Clear();
                //            contextList.rows.Clear();

                //            docEntry++;
                //            break;
                //        }
                //    }
                //}
                //catch (Exception exp)
                //{
                //    frmMain.NotiMsg(exp.Message, Color.Red);
                //}
            }
        }

        private string GetValue(int col, int row)
        {
            return DgvExcel[col, row].Value.ToString();
        }

        void GenericUploading()
        {
            //PublicStatic.DtRunID = DateTime.Now.ToString("yyMMddHHmmss");

            //string cmb = CmbUploadType.Text;

            //var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
            //var context = new SAOContext();

            //int ids = 1;
            //int rowEnd = Convert.ToInt32(TxtDocumentCount.Text) - MappingList.fields.Min(x => x.RowStart) + 1;
            //int colEnd = Convert.ToInt32(TxtLineCount.Text);
            //int maxMoveColumn = MappingList.fields.Max(x => x.ColumnInterval);
            //int lowMoveColumn = MappingList.fields.Where(x => x.Flow == "Horizontal").Count() <= 0 ? 1 : MappingList.fields.Where(x => x.Flow == "Horizontal").Min(x => x.ColumnStart);

            //int lowestColumnStart = 0;

            //try
            //{
            //    lowestColumnStart = MappingList.fields.FirstOrDefault(x => x.ColumnInterval == maxMoveColumn && x.ColumnStart == lowMoveColumn).ColumnStart;
            //}
            //catch
            //{
            //    lowestColumnStart = 0;
            //}

            //int numerator = (colEnd - lowestColumnStart);

            //colEnd = maxMoveColumn <= 0 ? 1 : numerator / maxMoveColumn;
            //colEnd = colEnd > 1 ? colEnd + 1 : 1;

            //
            //for (int row = 0; rowEnd > row; row++)
            //{
            //    StaticHelper._MainForm.Invoke(new Action(() =>
            //             StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {row} out of {rowEnd}", row, rowEnd)
            //         ));

            //    for (int col = 0; col < colEnd; col++)
            //    {
            //        var header = new MarketingDocumentHeader();
            //        var lines = new MarketingDocumentLines();

            //        for (var mapIndex = 0; sortMapping.Count > mapIndex; mapIndex++)
            //        {
            //            int colIndex = sortMapping[mapIndex].ColumnInterval >= 1 ?
            //                            sortMapping[mapIndex].ColumnStart + (maxMoveColumn * col) - 1 :
            //                            sortMapping[mapIndex].ColumnStart - 1;

            //            int rowIndex = sortMapping[mapIndex].RowStart - 1;

            //            string SAP = sortMapping[mapIndex].SapField;

            //            var oData = getGridViewData(DgvExcel, colIndex, rowIndex);

            //            if (string.IsNullOrEmpty(oData) == false)
            //            {
            //                if (sortMapping[mapIndex].Type == "Header")
            //                {
            //                    dtController.Header(header, DgvExcel, SAP, colIndex, rowIndex);
            //                }
            //                else
            //                {
            //                    dtController.Row(lines, DgvExcel, SAP, colIndex, rowIndex);
            //                }
            //            }

            //            var currentCol = colEnd - 1;
            //            if (currentCol == col)
            //            {
            //                if (sortMapping[mapIndex].RowInterval > 0)
            //                {
            //                    sortMapping[mapIndex].RowStart += sortMapping[mapIndex].RowInterval;
            //                }
            //            }
            //        }

            //        header.DocEntry = ids.ToString();
            //        header.User = Environment.MachineName;
            //        header.Session = PublicStatic.DtRunID;
            //        context.DocumentHeaders.Add(header);

            //        lines.DocEntry = ids.ToString();
            //        lines.Session = PublicStatic.DtRunID;
            //        lines.User = Environment.MachineName;
            //        context.DocumentLines.Add(lines);

            //        ids++;
            //    }
            //}
            #region MyRegion
            PublicStatic.DtRunID = DateTime.Now.ToString("yyMMddHHmmss");

            string cmb = CmbUploadType.Text;

            var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
            var context = new SAOContext();

            int ids = 1;
            int rowEnd = Convert.ToInt32(TxtDocumentCount.Text) - MappingList.fields.Min(x => x.RowStart) + 1;
            int colEnd = Convert.ToInt32(TxtLineCount.Text);
            int maxMoveColumn = MappingList.fields.Max(x => x.ColumnInterval);
            int lowMoveColumn = MappingList.fields.Where(x => x.Flow == "Horizontal").Count() <= 0 ? 1 : MappingList.fields.Where(x => x.Flow == "Horizontal").Min(x => x.ColumnStart);
            int lowestColumnStart = MappingList.fields.SingleOrDefault(x => x.ColumnInterval == maxMoveColumn && x.ColumnStart == lowMoveColumn).ColumnStart;
            int numerator = (colEnd - lowestColumnStart);

            colEnd = maxMoveColumn <= 0 ? 1 : numerator / maxMoveColumn;
            colEnd = colEnd > 1 ? colEnd + 1 : 1;

            for (int row = 0; rowEnd > row; row++)
            {
                StaticHelper._MainForm.Invoke(new Action(() =>
                         StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {row} out of {rowEnd}", row, rowEnd)
                     ));

                for (int col = 0; col < colEnd; col++)
                {
                    var header = new MarketingDocumentHeaders();
                    var lines = new MarketingDocumentLines();

                    for (var mapIndex = 0; sortMapping.Count > mapIndex; mapIndex++)
                    {
                        int colIndex = sortMapping[mapIndex].ColumnStart > 1 ? sortMapping[mapIndex].ColumnStart + (maxMoveColumn * col) - 1 : sortMapping[mapIndex].ColumnStart - 1;
                        int rowIndex = sortMapping[mapIndex].RowStart - 1;

                        string SAP = MappingList.fields[mapIndex].SapField;

                        if (MappingList.fields[mapIndex].Type == "Header")
                        {
                            dtController.Header(header, DgvExcel, SAP, colIndex, rowIndex);
                        }
                        else
                        {
                            dtController.Row(lines, DgvExcel, SAP, colIndex, rowIndex);
                        }

                        var currentCol = colEnd - 1;
                        if (currentCol == col)
                        {
                            if (sortMapping[mapIndex].RowInterval > 0)
                            {
                                sortMapping[mapIndex].RowStart += sortMapping[mapIndex].RowInterval;
                            }
                        }
                    }

                    header.DocEntry = ids.ToString();
                    header.User = Environment.MachineName;
                    header.Session = PublicStatic.DtRunID;
                    header.DocDate = frmDT_UDF.PostingDate;
                    context.DocumentHeaders.Add(header);

                    lines.DocEntry = ids.ToString();
                    lines.Session = PublicStatic.DtRunID;
                    lines.User = Environment.MachineName;
                    lines.DocDate = frmDT_UDF.PostingDate;
                    context.DocumentLines.Add(lines);

                    ids++;
                }
            }

            context.SaveChanges();

            var strHeader = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());
            var strRow = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Row").Select(x => x.SapField).ToArray());

            //dtController.UploadSQLmarketingDocument(strHeader, strRow, cmb);

            StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
            BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = true));
            //PublicStatic.frmMain.Invoke(new Action(() => PublicStatic.frmMain.ProgressClear()));
            #endregion

            #region NewThread
            //new Thread(() =>
            //{
            //    for (int row = 0; rowEnd > row; row++)
            //    {
            //        //PublicStatic.frmMain.Invoke(new Action(() =>
            //        //        PublicStatic.frmMain.Progress2($"Please wait until all data are uploaded to Add-on database. {row} out of {rowEnd}", row, rowEnd)
            //        //    ));

            //        for (int col = 0; col < colEnd; col++)
            //        {
            //            var header = new MarketingDocumentHeader();
            //            var lines = new MarketingDocumentLines();

            //            for (var mapIndex = 0; sortMapping.Count > mapIndex; mapIndex++)
            //            {
            //                int colIndex = sortMapping[mapIndex].ColumnInterval >= 1 ?
            //                                sortMapping[mapIndex].ColumnStart + (maxMoveColumn * col) - 1 :
            //                                sortMapping[mapIndex].ColumnStart - 1;

            //                int rowIndex = sortMapping[mapIndex].RowStart - 1;

            //                string SAP = sortMapping[mapIndex].SapField;


            //                if (sortMapping[mapIndex].Type == "Header")
            //                {
            //                    dtController.Header(header, DgvExcel, SAP, colIndex, rowIndex);
            //                }
            //                else
            //                {
            //                    dtController.Row(lines, DgvExcel, SAP, colIndex, rowIndex);
            //                }

            //                var currentCol = colEnd - 1;
            //                if (currentCol == col)
            //                {
            //                    if (sortMapping[mapIndex].RowInterval > 0)
            //                    {
            //                        sortMapping[mapIndex].RowStart += sortMapping[mapIndex].RowInterval;
            //                    }
            //                }
            //            }

            //            header.DocEntry = ids.ToString();
            //            header.User = Environment.MachineName;
            //            header.Session = PublicStatic.DtRunID;
            //            context.DocumentHeaders.Add(header);

            //            lines.DocEntry = ids.ToString();
            //            lines.Session = PublicStatic.DtRunID;
            //            lines.User = Environment.MachineName;
            //            context.DocumentLines.Add(lines);

            //            ids++;
            //        }
            //    }

            //    context.SaveChanges();

            //    var strHeader = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());
            //    var strRow = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Row").Select(x => x.SapField).ToArray());

            //    dtController.UploadSQLmarketingDocument(strHeader, strRow, cmb);

            //    StaticHelper._MainForm.ShowMessage(StaticHelper.OperationMessage);
            //    BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = true));
            //    //PublicStatic.frmMain.Invoke(new Action(() => PublicStatic.frmMain.ProgressClear()));

            //}).Start();
            #endregion
        }

        string getGridViewData(DataGridView dgv, int iCol, int iRow)
        {
            try
            {
                var output = "";
                if (dgv[iCol, iRow] == null)
                {
                    output = "";
                }
                else if (dgv[iCol, iRow].Value == null)
                {
                    output = "";
                }
                else if (string.IsNullOrEmpty(dgv[iCol, iRow].Value.ToString()))
                {
                    output = "";
                }
                else
                {
                    output = dgv[iCol, iRow].Value.ToString();
                }

                return output;
            }
            catch
            {
                return "";
            }
        }

        void Uploading()
        {
            var errorCode = "";

            try
            {
                PublicStatic.DtRunID = DateTime.Now.ToString("yyMMddHHmmss");
                var context = new SAOContext();
                var uploadingType = CmbUploadType.Text;
                errorCode = "sortMapping";
                var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
                var cardCode = sortMapping.Where(x => x.Type == "Header" && x.SapField == "CardCode").ToList().FirstOrDefault();

                int ids = 1;
                var rowStart = MappingList.fields.Min(x => x.RowStart) - 1;
                var rowEnd = Convert.ToInt32(TxtDocumentCount.Text);

                var colStart = MappingList.fields.Min(x => x.ColumnStart) - 1;
                var colEnd = Convert.ToInt32(TxtLineCount.Text);

                string strActiveFlow = "";
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sortMapping);
                DataTable ConvertedSortMapping = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json);


                if (cardCode == null && uploadingType.Contains("A/R Credit Memo"))
                {
                    if (string.IsNullOrEmpty(frmDT_UDF.CardCode))
                    {
                        StaticHelper._MainForm.ShowMessage("Please enter cardcode of the transaction to be uploaded.", true);
                        return;
                    }
                }

                if (cardCode == null && !uploadingType.Contains("A/R Credit Memo"))
                {
                    StaticHelper._MainForm.ShowMessage("Please do not use a mapping without cardcode.", true);
                    return;
                }
                else if (CheckFlowDifference(ConvertedSortMapping, out strActiveFlow))
                {
                    DataArrangementSMPO();
                    return;
                }
                else
                {
                    for (int iRow = rowStart; iRow < rowEnd; iRow++)
                    {
                        StaticHelper._MainForm.Invoke(new Action(() =>
                                StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                            ));


                        int iColSta = 0;
                        if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                        {
                            iColSta = 1;
                        }
                        else
                        {
                            iColSta = cardCode.ColumnStart - 1;
                        }

                        int cardCodeColInterval = 0;
                        if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                        {
                        }
                        else
                        {
                            dtController.cardcode = true;
                            cardCodeColInterval = cardCode.ColumnInterval;
                        }

                        //var columnStart = cardCode.ColumnStart - 1;
                        for (int iCol = iColSta; iCol < colEnd; iCol++)
                        {
                            //var oCardCode = sortMapping.Where(x => x.SapField == "CardCode").FirstOrDefault().ColumnInterval;

                            var diff = iCol - iColSta;
                            if (cardCodeColInterval == 0 || (diff % cardCodeColInterval == 0))
                            {
                                var header = new MarketingDocumentHeaders();
                                var lines = new MarketingDocumentLines();
                                foreach (var item in sortMapping)
                                {
                                    var sapField = item.SapField;
                                    var oData = string.Empty;
                                    var col = 0;
                                    var row = 0;

                                    col = item.ColumnInterval > 0 ? iCol : item.ColumnStart - 1;
                                    row = item.RowInterval > 0 ? iRow : item.RowStart - 1;

                                    oData = getGridViewData(DgvExcel, col, row);

                                    if (string.IsNullOrEmpty(oData) == false)
                                    {
                                        if (item.Type == "Header")
                                        {
                                            if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "U_OrderNo")
                                            {
                                                //dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));

                                                if (row > 4 && col == 1)
                                                {
                                                    dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                                }

                                                //if (row == 0 && col > 5)
                                                //{
                                                //    dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                                //}
                                            }
                                            else if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "DocDate")
                                            {
                                                if (row > 4 && col == 26)
                                                {
                                                    dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                                }
                                                //if (row == 25 && col > 5)
                                                //{
                                                //    dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                                //}
                                            }
                                            else if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "U_SINo")
                                            {
                                                if (row > 4 && col == 2)
                                                {
                                                    dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                                }
                                                //if (row == 1 && col > 5)
                                                //{
                                                //    dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                                //}
                                            }
                                            else
                                            {
                                                dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                            }
                                        }
                                        else
                                        {
                                            if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "ItemCode")
                                            {
                                                if (col > 5 && col < 19 || col == 21 || col == 23)
                                                {
                                                    dtController.Row(lines, DgvExcel, sapField, col, row);
                                                }

                                                //if (row > 4 && row < 18 || row == 20 || row == 22)
                                                //{
                                                //    dtController.Row(lines, DgvExcel, sapField, col, row);
                                                //}
                                            }
                                            //else if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "PriceAfVAT")
                                            //{
                                            //    if (col > 5 && col < 19 || col == 21 || col == 23)
                                            //    {
                                            //        dtController.Row(lines, DgvExcel, sapField, col, row);
                                            //    }
                                            //}
                                            else
                                            {
                                                dtController.Row(lines, DgvExcel, sapField, col, row);
                                            }
                                        }
                                    }
                                }

                                if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                                {
                                    //dtController.Header(header, DgvExcel, "CardCode", 0, iRow, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));

                                    dtController.HeaderCardCode(header, ValidateInput.String(frmDT_UDF.CardCode), "CardCode", (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                    //var info = controller.BpCardProjectCode(ValidateInput.String(frmDT_UDF.CardCode), (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));

                                    //header.CardCode = info[0]; //ValidateInput.String(DgvExcel[col, row].Value);
                                    //header.Project = info[1];
                                    //header.CardName = controller.CardName(header.CardCode);

                                }

                                header.DocEntry = ids.ToString();
                                header.User = Environment.MachineName;
                                header.Session = PublicStatic.DtRunID;
                                context.DocumentHeaders.Add(header);

                                lines.DocEntry = ids.ToString();
                                lines.Session = PublicStatic.DtRunID;
                                lines.User = Environment.MachineName;
                                context.DocumentLines.Add(lines);

                                if ((iRow % 50) == 0)
                                {
                                    context.SaveChanges();
                                    context = new SAOContext();
                                }

                                ids++;
                            }

                        }
                    }
                }

                context.SaveChanges();
                var strHeader = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());
                if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                {
                    strHeader += $",CardCode";
                }
                //MessageBox.Show("Before Saving to Database");
                var strRow = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Row").Select(x => x.SapField).ToArray());
                dtController.UploadSQLmarketingDocument(strHeader, strRow, uploadingType, Path.GetFileName(TxtTemplate.Text));
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                if (ex.Message.Contains("OutOfMemoryException"))
                {
                    GC.Collect();
                }
            }

        }

        private string NextKey()
        {
            try
            {
                var msSql = new SAPMsSqlAccess();
                var helper = new DataHelper();

                var dt = msSql.Get("SBO_GetNextAutoKey '3','Y'");
                var autokey = helper.ReadDataRow(dt, "AutoKey", "", 0);

                return autokey;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        void UploadingInventoryCounting()
        {
            try
            {
                PublicStatic.DtRunID = DateTime.Now.ToString("yyMMddHHmmss");

                var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
                var context = new SAOContext();


                var rowStart = MappingList.fields.Min(x => x.RowStart) - 1;
                var rowEnd = Convert.ToInt32(TxtDocumentCount.Text);
                var colStart = MappingList.fields.Min(x => x.ColumnStart) - 1;
                var colEnd = Convert.ToInt32(TxtLineCount.Text);

                for (int iRow = rowStart; iRow < rowEnd; iRow++)
                {
                    StaticHelper._MainForm.Invoke(new Action(() =>
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                        ));

                    var header = new DTInventoryCounting();
                    var lines = new DTInventoryCountingRow();
                    foreach (var item in sortMapping)
                    {
                        var sapField = item.SapField;
                        var oData = string.Empty;
                        var col = 0;
                        var row = 0;

                        col = item.ColumnStart - 1;
                        row = item.RowInterval > 0 ? iRow : item.RowStart - 1;

                        oData = getGridViewData(DgvExcel, col, row);

                        if (string.IsNullOrEmpty(oData) == false)
                        {

                            if (item.Type == "Header")
                            {
                                dtController.HeaderInventoryCount(header, DgvExcel, sapField, col, row);
                                if (sapField.Equals("WhsCode"))
                                {
                                    lines.WhsCode = oData;
                                }
                            }
                            else
                            {
                                dtController.RowInventoryCount(lines, DgvExcel, sapField, col, row);
                            }
                        }
                    }

                    int ids = int.Parse(NextKey());
                    header.DocEntry = ids;
                    //header.DocStatus = "O";
                    //header.Canceled = "N";
                    header.RefNo = PublicStatic.DtRunID;//DateTime.Now.ToString("yyMMddHHmmssfff");//.ToString("yyMMddHHmmss.SSS");
                                                        //header.SapUsername = EasySAPCredentialsModel.EmployeeName;
                    header.DocDate = DateTime.Now.ToShortDateString();
                    header.SapCode = EasySAPCredentialsModel.GetEmployeeCode();
                    //header.Remarks = $"Created by EasySAP | Data Transfer : {DateTime.Now} | Powered By : DIREC";
                    //header.PreparedBy = EasySAPCredentialsModel.EmployeeCompleteName;
                    context.DTInventoryCounting.Add(header);
                    lines.HeaderId = ids;
                    context.DTInventoryCountingRow.Add(lines);

                    if ((iRow % 50) == 0)
                    {
                        context.SaveChanges();
                        context = new SAOContext();
                    }
                }

                context.SaveChanges();
                dtController.UploadToMain();
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                if (ex.Message.Contains("OutOfMemoryException"))
                {
                    GC.Collect();
                }
            }


        }



        void UploadingGeneric()
        {
            PublicStatic.DtRunID = DateTime.Now.ToString("yyMMddHHmmss");

            var uploadingType = CmbUploadType.Text;

            var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
            var context = new SAOContext();

            int ids = 1;
            var rowStart = MappingList.fields.Min(x => x.RowStart) - 1;
            var rowEnd = Convert.ToInt32(TxtDocumentCount.Text);
            var colStart = MappingList.fields.Min(x => x.ColumnStart) - 1;
            var colEnd = Convert.ToInt32(TxtLineCount.Text);
            var cardCode = sortMapping.Where(x => x.Type == "Header" && x.SapField == "CardCode").ToList().FirstOrDefault();
            var itemCode = sortMapping.Where(x => x.Type == "Row" && x.SapField == "ItemCode").ToList().FirstOrDefault();

            if (cardCode == null)
            {
                StaticHelper._MainForm.ShowMessage("Please do not use a mapping without cardcode.");
                return;
            }
            if (cardCode.Flow == "Horizontal")
            {
                var isColumn = sortMapping.Where(x => x.SapField == "CardCode").ToList().FirstOrDefault();
                for (int iRow = rowStart; iRow < rowEnd; iRow++)
                {
                    StaticHelper._MainForm.Invoke(new Action(() =>
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                        ));

                    var columnStart = isColumn.ColumnStart - 1;
                    for (int iCol = columnStart; iCol < colEnd; iCol++)
                    {
                        //var oCardCode = sortMapping.Where(x => x.SapField == "CardCode").FirstOrDefault().ColumnInterval;

                        var diff = iCol - columnStart;
                        if (diff % isColumn.ColumnInterval == 0)
                        {
                            var header = new MarketingDocumentHeaders();
                            var lines = new MarketingDocumentLines();
                            foreach (var item in sortMapping)
                            {
                                var sapField = item.SapField;
                                var oData = string.Empty;
                                var col = 0;
                                var row = 0;
                                //col = sapField == "ItemCode" ? item.ColumnStart - 1 : iCol;

                                col = item.ColumnInterval > 1 ? iCol : item.ColumnStart - 1;
                                row = sapField == "CardCode" ? item.RowStart - 1 : iRow;
                                //if (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO"))
                                //{
                                //    oData = getGridViewData(DgvExcel, item.ColumnStart - 1, iRow);
                                //}
                                //else
                                //{
                                //    oData = getGridViewData(DgvExcel, col, row);
                                //}
                                oData = getGridViewData(DgvExcel, col, row);

                                if (string.IsNullOrEmpty(oData) == false)
                                {


                                    //if (item.Type == "Header")
                                    //{
                                    //    dtController.Header(header, DgvExcel, sapField, item.ColumnStart - 1, iRow);
                                    //}
                                    //else
                                    //{
                                    //    dtController.Row(lines, DgvExcel, sapField, item.ColumnStart - 1, iRow);
                                    //}

                                    if (item.Type == "Header")
                                    {
                                        //if (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO"))
                                        //{
                                        //    dtController.Header(header, DgvExcel, sapField, item.ColumnStart - 1, iRow, true);
                                        //}
                                        //else
                                        //{
                                        //    dtController.Header(header, DgvExcel, sapField, col, row);
                                        //}

                                        dtController.Header(header, DgvExcel, sapField, col, row, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                    }
                                    else
                                    {
                                        dtController.Row(lines, DgvExcel, sapField, col, row);
                                    }
                                }
                            }
                            header.DocEntry = ids.ToString();
                            header.User = Environment.MachineName;
                            header.Session = PublicStatic.DtRunID;
                            context.DocumentHeaders.Add(header);

                            lines.DocEntry = ids.ToString();
                            lines.Session = PublicStatic.DtRunID;
                            lines.User = Environment.MachineName;
                            context.DocumentLines.Add(lines);

                            if ((iRow % 50) == 0)
                            {
                                context.SaveChanges();
                                context = new SAOContext();
                            }

                            ids++;
                        }

                    }
                }
            }
            else if (itemCode.Flow == "Horizontal")
            {
                var isColumn = sortMapping.Where(x => x.SapField == "CardCode").ToList().FirstOrDefault();
                for (int iRow = isColumn.RowStart - 1; iRow < rowEnd; iRow++)
                {
                    StaticHelper._MainForm.Invoke(new Action(() =>
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                        ));

                    for (int iCol = 0; iCol < colEnd; iCol++)
                    {
                        var oItemCode = sortMapping.Where(x => x.SapField == "ItemCode").FirstOrDefault();
                        if ((iCol >= oItemCode.ColumnStart - 1) && ((iCol % oItemCode.ColumnInterval) == 0))
                        {
                            var header = new MarketingDocumentHeaders();
                            var lines = new MarketingDocumentLines();
                            foreach (var item in sortMapping)
                            {
                                var sapField = item.SapField;
                                var oData = string.Empty;
                                var col = 0;
                                var row = 0;
                                col = sapField == "CardCode" ? item.ColumnStart - 1 : iCol;
                                row = sapField == "ItemCode" ? item.RowStart - 1 : iRow;
                                oData = getGridViewData(DgvExcel, col, row);
                                if (string.IsNullOrEmpty(oData) == false)
                                {
                                    if (item.Type == "Header")
                                    {
                                        dtController.Header(header, DgvExcel, sapField, col, row);
                                    }
                                    else
                                    {
                                        dtController.Row(lines, DgvExcel, sapField, col, row);
                                    }
                                }
                            }
                            header.DocEntry = ids.ToString();
                            header.User = Environment.MachineName;
                            header.Session = PublicStatic.DtRunID;
                            context.DocumentHeaders.Add(header);

                            lines.DocEntry = ids.ToString();
                            lines.Session = PublicStatic.DtRunID;
                            lines.User = Environment.MachineName;
                            context.DocumentLines.Add(lines);
                            if ((iRow % 50) == 0)
                            {
                                context.SaveChanges();
                                context = new SAOContext();
                            }

                            ids++;
                        }

                    }
                }
            }
            else
            {
                for (int iRow = rowStart; iRow < rowEnd; iRow++)
                {
                    StaticHelper._MainForm.Invoke(new Action(() =>
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                        ));
                    var header = new MarketingDocumentHeaders();
                    var lines = new MarketingDocumentLines();

                    foreach (var item in sortMapping)
                    {
                        var sapField = item.SapField;

                        for (int iCol = item.ColumnStart - 1; iCol < colEnd; iCol++)
                        {
                            var oData = string.Empty;
                            oData = getGridViewData(DgvExcel, iCol, iRow);

                            if (string.IsNullOrEmpty(oData) == false)
                            {
                                if (item.Type == "Header")
                                {
                                    dtController.Header(header, DgvExcel, sapField, iCol, iRow);
                                }
                                else
                                {
                                    dtController.Row(lines, DgvExcel, sapField, iCol, iRow);
                                }

                                if (item.ColumnInterval > 0)
                                {
                                    iCol = iCol + item.ColumnInterval - 1;
                                }
                                else
                                { break; }
                            }
                        }
                    }

                    header.DocEntry = ids.ToString();
                    header.User = Environment.MachineName;
                    header.Session = PublicStatic.DtRunID;
                    context.DocumentHeaders.Add(header);

                    lines.DocEntry = ids.ToString();
                    lines.Session = PublicStatic.DtRunID;
                    lines.User = Environment.MachineName;
                    context.DocumentLines.Add(lines);
                    if ((iRow % 50) == 0)
                    {
                        context.SaveChanges();
                        context = new SAOContext();
                    }

                    ids++;
                }
            }

            context.SaveChanges();
            var strHeader = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());
            var strRow = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Row").Select(x => x.SapField).ToArray());

            //dtController.UploadSQLmarketingDocument(strHeader, strRow, uploadingType);
        }

        void MarketingDocumentsAlgoUpload(out bool ret)
        {
            ret = true;

            //PreviousDataArrangement();
            NewDataArrangement();

            /////////////////////////////////////////////// Upload data ///////////////////////////////////////////////
            dtController.uploadCarton(MarketingDocument.DocHeader, MarketingDocument.DocLines, frmMain, out ret);
        }

        void NewDataArrangement()
        {
            try
            {
                var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
                int ids = 1;
                var rowStart = MappingList.fields.Min(x => x.RowStart) - 1;
                var rowEnd = Convert.ToInt32(TxtDocumentCount.Text);

                for (int iRow = rowStart; iRow < rowEnd; iRow++)
                {
                    StaticHelper._MainForm.Invoke(new Action(() =>
                            StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                        ));
                    var header = new MarketingDocumentHeaders();
                    var lines = new MarketingDocumentLines();

                    foreach (var item in sortMapping)
                    {
                        var sapField = item.SapField;
                        int iCol = item.ColumnStart - 1;

                        var oData = string.Empty;
                        oData = getGridViewData(DgvExcel, iCol, iRow);

                        if (string.IsNullOrEmpty(oData) == false)
                        {
                            if (item.Type == "Header")
                            {
                                dtController.Header(header, DgvExcel, sapField, iCol, iRow);
                            }
                            else
                            {
                                dtController.Row(lines, DgvExcel, sapField, iCol, iRow);
                            }
                        }
                    }

                    header.DocEntry = ids.ToString();
                    MarketingDocument.DocHeader.Add(header);
                    lines.DocEntry = ids.ToString();
                    MarketingDocument.DocLines.Add(lines);

                    ids++;
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                if (ex.Message.Contains("OutOfMemoryException"))
                {
                    GC.Collect();
                }
            }

        }

        //Created for SMPO 090319
        void DataArrangementSMPO()
        {
            //Req on SMPO start
            try
            {
                PublicStatic.DtRunID = DateTime.Now.ToString("yyMMddHHmmss");
                var context = new SAOContext();
                var uploadingType = CmbUploadType.Text;

                var sortMapping = MappingList.fields.OrderBy(x => x.ColumnStart).ToList();
                var cardCode = sortMapping.Where(x => x.Type == "Header" && x.SapField == "CardCode").ToList().FirstOrDefault();
                //Req on SMPO end

                int ids = 1;
                int linenums = 1;
                var rowStart = MappingList.fields.Min(x => x.RowStart) - 1;
                var rowEnd = Convert.ToInt32(TxtDocumentCount.Text);

                ////List for itemcodes 03/07/20 remove once done checking
                //string sItemList = "";
                //string sRowCnt = "";

                var colEnd = Convert.ToInt32(TxtLineCount.Text);

                if (cardCode == null && uploadingType.Contains("A/R Credit Memo"))
                {
                    if (string.IsNullOrEmpty(frmDT_UDF.CardCode))
                    {
                        StaticHelper._MainForm.ShowMessage("Please enter cardcode of the transaction to be uploaded.", true);
                        return;
                    }
                }

                if (cardCode == null && !uploadingType.Contains("A/R Credit Memo"))
                {
                    StaticHelper._MainForm.ShowMessage("Please do not use a mapping without cardcode.", true);
                    return;
                }
                else
                {
                    for (int iRow = rowStart; iRow <= rowEnd; iRow++)
                    {
                        StaticHelper._MainForm.Invoke(new Action(() =>
                                StaticHelper._MainForm.Progress($"Please wait until all data are uploaded to Add-on database. {iRow} out of {rowEnd}", iRow, rowEnd)
                            ));

                        //On Comment due to conflict in mapping with EPC 091319
                        //if (cardCode.ColumnInterval > 0)
                        //{
                        string iOrderNo = "";
                        int iColSta = 0;
                        if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                        {
                            iColSta = 1;
                        }
                        else
                        {
                            iColSta = cardCode.ColumnStart;
                        }

                        int iAddCnt = 0;
                        int cardCodeColInterval = 0;
                        if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                        {
                        }
                        else
                        {
                            dtController.cardcode = true;
                            cardCodeColInterval = cardCode.ColumnInterval;// added for WTR
                        }

                        for (int GetCol = iColSta; GetCol < colEnd; GetCol += cardCodeColInterval)
                        {
                            var header = new MarketingDocumentHeaders();
                            var lines = new MarketingDocumentLines();

                            foreach (var item in sortMapping)
                            {
                                var sapField = item.SapField;
                                if (sapField.Contains("U_OrRecDate"))
                                {
                                    var test = "candzzz";
                                }

                                int iCol = item.ColumnStart;

                                int iGetLastCol = item.ColumnInterval > 0 ? (iAddCnt > 0 ? ((iCol + (item.ColumnInterval * iAddCnt)) - 1) : iCol - 1) : iCol - 1;

                                var oData = string.Empty;
                                oData = getGridViewData(DgvExcel, iGetLastCol, iRow);

                                if (string.IsNullOrEmpty(oData) == false || sapField == "CardCode")
                                {
                                    if (item.Type == "Header")
                                    {
                                        dtController.Header(header, DgvExcel, sapField, iGetLastCol, iRow, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                    }
                                    else
                                    {
                                        ////List for itemcodes 03/07/20 remove once done checking
                                        //if (DgvExcel[iGetLastCol, iRow].Value.ToString() == "2039981098980001")
                                        //{         
                                        //    StaticHelper._MainForm.ShowMessage("Got it 2039981098980001.", true);
                                        //}
                                        //if (item.SapField.ToString() == "ItemCode")
                                        //{
                                        //    sItemList = sItemList == "" ? DgvExcel[iGetLastCol, iRow].Value.ToString() : sItemList + " , " + DgvExcel[iGetLastCol, iRow].Value.ToString();
                                        //    sRowCnt = sRowCnt == "" ? iRow.ToString() : sRowCnt + " , " + iRow.ToString();
                                        //}

                                        //if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "ItemCode")
                                        //{
                                        //    if (iGetLastCol == 20 && iRow == 2 || iGetLastCol == 24 && iRow == 2 || iGetLastCol == 25 && iRow == 2 || iGetLastCol == 26 && iRow == 2 || iGetLastCol == 28 && iRow == 2 ||
                                        //        iGetLastCol == 29 && iRow == 2 || iGetLastCol == 30 && iRow == 2 || iGetLastCol == 31 && iRow == 2 || iGetLastCol == 33 && iRow == 2 || iGetLastCol == 34 && iRow == 2)
                                        //    {
                                        //        dtController.Row(lines, DgvExcel, sapField, iGetLastCol, iRow);
                                        //    }
                                        //}
                                        //else if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "Price")
                                        //{
                                        //    if (iGetLastCol == 15 && iRow == 2)
                                        //    {
                                        //        dtController.Row(lines, DgvExcel, sapField, iGetLastCol, iRow);

                                        //    }
                                        //}
                                        //else if (uploadingType.Contains("A/R Credit Memo") && frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && sapField == "Quantity")
                                        //{
                                        //    if (iGetLastCol == 20 && iRow == 2 || iGetLastCol == 24 && iRow == 2 || iGetLastCol == 25 && iRow == 2 || iGetLastCol == 26 && iRow == 2 || iGetLastCol == 28 && iRow == 2 ||
                                        //        iGetLastCol == 29 && iRow == 2 || iGetLastCol == 30 && iRow == 2 || iGetLastCol == 31 && iRow == 2 || iGetLastCol == 33 && iRow == 2 || iGetLastCol == 34 && iRow == 2)
                                        //    {
                                        //        dtController.Row(lines, DgvExcel, sapField, iGetLastCol, iRow);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        dtController.Row(lines, DgvExcel, sapField, iGetLastCol, iRow);
                                        //}
                                    }
                                }
                            }
                            if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                            {
                                //dtController.Header(header, DgvExcel, "CardCode", 0, iRow, (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                if (frmDT_UDF.CardCode.ToUpper().Contains("SHOPEE") && header.U_OrderNo != "")
                                {
                                    dtController.HeaderCardCode(header, ValidateInput.String(frmDT_UDF.CardCode), "CardCode", (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                }
                                else
                                {
                                    dtController.HeaderCardCode(header, ValidateInput.String(frmDT_UDF.CardCode), "CardCode", (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));
                                }
                                //var info = controller.BpCardProjectCode(ValidateInput.String(frmDT_UDF.CardCode), (TxtMap.Text.ToUpper().Contains("SMPO") || TxtMap.Text.ToUpper().Contains("SM PO")));

                                //header.CardCode = info[0]; //ValidateInput.String(DgvExcel[col, row].Value);
                                //header.Project = info[1];
                                //header.CardName = controller.CardName(header.CardCode);

                            }
                            header.DocEntry = ids.ToString();
                            header.User = Environment.MachineName;
                            header.Session = PublicStatic.DtRunID;

                            if (header.CardCode == "00139")
                            {
                                var adna = "addada";
                            }
                            context.DocumentHeaders.Add(header);

                            if (uploadingType.Contains("A/R invoice (Group By Name)"))
                            {

                                lines.DocEntry = ids.ToString();
                                lines.Session = PublicStatic.DtRunID;
                                lines.User = Environment.MachineName;
                                context.DocumentLines.Add(lines);
                            }
                            else
                            {
                                lines.DocEntry = ids.ToString();
                                lines.Session = PublicStatic.DtRunID;
                                lines.User = Environment.MachineName;
                                context.DocumentLines.Add(lines);
                            }

                            if ((iRow % 50) == 0)
                            {
                                context.SaveChanges();
                                context = new SAOContext();
                            }

                            ids++;
                            iAddCnt++;

                            if (cardCodeColInterval == 0)//&& !string.IsNullOrEmpty(header.CardCode)
                            {
                                break;
                            }
                        }

                        //}
                    }

                    context.SaveChanges();
                    var strHeader = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());

                    if (uploadingType.Contains("A/R Credit Memo") && !string.IsNullOrEmpty(frmDT_UDF.CardCode) && cardCode == null)
                    {
                        strHeader += $",CardCode";
                    }

                    var strRow = string.Join(",", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Row").Select(x => x.SapField).ToArray());
                    dtController.UploadSQLmarketingDocument(strHeader, strRow, uploadingType, Path.GetFileName(TxtTemplate.Text));
                }
            }
            catch (Exception ex)
            {
                StaticHelper._MainForm.ShowMessage(ex.Message, true);
                if (ex.Message.Contains("OutOfMemoryException"))
                {
                    GC.Collect();
                }
            }


        }
        //Created 090319

        private void BtnStatus_Click(object sender, EventArgs e)
        {
            var strHeader = string.Join(", Header.", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Header").Select(x => x.SapField).ToArray());
            var strRow = string.Join(", Lines.", model.dtrows.Where(x => x.HeaderID == id && x.Type == "Row").Select(x => x.SapField).ToArray());

            if (CmbUploadType.Text.Equals("Inventory Counting") == false)
            {
                frmShowMessage msg = new frmShowMessage();
                msg.uploadType = CmbUploadType.Text;
                msg.fieldheader = strHeader;
                msg.fieldrow = strRow;
                msg.ShowDialog();
            }
            else
            {
                StaticHelper._MainForm.ShowMessage("Please check it in inventory counting module in EasySAP.", true);
            }

        }

        private void CmbUploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string obj = "";

            switch (CmbUploadType.Text)
            {
                case "Delivery":
                    obj = "ODLN";
                    break;

                case "Sales Order":
                    obj = "ORDR";
                    break;

                case "Inventory Transfer Request":
                    obj = "OWTQ";

                    break;

                case "Sales Quotation":
                    obj = "OQUT";
                    break;

                case "A/R invoice":
                    obj = "OINV";
                    break;

                case "A/R invoice (Group By Name)":
                    obj = "OINV";
                    break;

                case "A/R Credit Memo":
                    obj = "ORIN";
                    break;


                case "Carton Packing List":

                    obj = "OCTN";

                    for (int i = 0; DgvExcel.RowCount > i; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(DgvExcel.Rows[i + 1].Cells["CTN NUMBER"].Value.ToString()))
                            {
                                DgvExcel.Rows[i + 1].Cells["CTN NUMBER"].Value = DgvExcel.Rows[i].Cells["CTN NUMBER"].Value;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    break;
            }

            if (obj != string.Empty)
            {
                DtController.objType = obj;

                frmDt.DisplayUdf(obj);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            DeleteSession();
            frmDt.closeUDF();

            DataContextList.GetErrorIds.Clear();
            frmDt.pnlContainer.Controls.Clear();
            frmDt.pnlContainer.Controls.Add(new UcMainMenu(frmDt, frmMain));
        }

        private void DgvExcel_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(DgvExcel.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void DeleteSession()
        {
            if (PublicStatic.DtRunID != null)
            {
                var context = new SAOContext();

                var header = context.DocumentHeaders.Where(x => x.Session == PublicStatic.DtRunID && x.User == Environment.MachineName);
                context.DocumentHeaders.RemoveRange(header);

                var row = context.DocumentLines.Where(x => x.Session == PublicStatic.DtRunID && x.User == Environment.MachineName);
                context.DocumentLines.RemoveRange(row);

                context.SaveChanges();
            }
        }

        #region FUNCTION
        void FetchMapping(int id)
        {
            MappingList.fields.Clear();

            model.dtrows.Where(x => x.HeaderID == id).ToList().ForEach(x =>
            {
                MappingList.fields.Add(new MappingList.MapField
                {
                    SapField = x.SapField,
                    Type = x.Type,
                    RowStart = x.RowStart,
                    ColumnStart = x.ColumnStart,
                    Flow = x.Flow,
                    ColumnInterval = x.ColumnInterval,
                    RowInterval = x.RowInterval
                });
            });
        }
        #endregion

        private bool CheckFlowDifference(DataTable SortMapping, out string ActiveFlow)
        {
            bool stat = false;
            ActiveFlow = "none";
            int iVerCnt = 0;
            int iHorCnt = 0;

            if (SortMapping.Rows.Count > 0)
            {
                foreach (DataRow map in SortMapping.Rows)
                {
                    string Flow = map["Flow"].ToString();

                    if (Flow == "Horizontal") { iHorCnt += 1; }
                    else { iVerCnt += 1; }
                }

                stat = (iHorCnt == 0) || (iVerCnt == 0) ? true : false;
                ActiveFlow = iHorCnt == 0 ? "Vertical" : (iVerCnt == 0) ? "Horizontal" : "none";
            }

            return stat;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
