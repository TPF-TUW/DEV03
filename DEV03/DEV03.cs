using System;
using System.Text;
using DBConnection;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Extensions;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using TheepClass;
using System.Data.SqlClient;
using DevExpress.XtraGrid;
using System.Collections;
using System.Data;
using System.ComponentModel;

namespace DEV03
{
    public partial class DEV03 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        goClass.dbConn db       = new goClass.dbConn();
        goClass.ctool ct        = new goClass.ctool();
        goClass.enums en        = new goClass.enums();
        hardQuery q             = new hardQuery();
        SqlConnection mainConn  = new goClass.dbConn().MDS();

        private Functionality.Function FUNC = new Functionality.Function();
        public DEV03()
        {
            InitializeComponent();
            UserLookAndFeel.Default.StyleChanged += MyStyleChanged;
        }

        private void MyStyleChanged(object sender, EventArgs e)
        {
            UserLookAndFeel userLookAndFeel = (UserLookAndFeel)sender;
            cUtility.SaveRegistry(@"Software\MDS", "SkinName", userLookAndFeel.SkinName);
            cUtility.SaveRegistry(@"Software\MDS", "SkinPalette", userLookAndFeel.ActiveSvgPaletteName);
        }

        private void XtraForm1_Load(object sender, EventArgs ex)
        {
            //bbiNew.PerformClick(); 

            // Set Tabbed
            tabbed_Master.SelectedTabPageIndex  = 0;
            tabbedBom.SelectedTabPageIndex      = 0;
            ct.show_bbi(bbiRefresh);

            q.get_sl_smplNo(sl_smplNo);
            q.get_sl_Customer(sl_Customer);
            q.get_gl_Season(gl_Season);
            q.get_gcListof_Bom(gcListof_Bom); //get tbl Bom

            //get dsMaterial

            db.get_repGl("Select OIDDEPT,brn.Name as BranName,dep.Name as Department From Departments dep inner join Branchs brn on brn.OIDBranch = dep.OIDBRANCH Where DepartmentType in(1,4,5) order by OIDDEPT ", mainConn,rep_glWorkStation, "OIDDEPT", "Department");
            db.get_repSl("Select OIDITEM,Code as NAVCode From Items",mainConn,rep_slItemCode, "OIDITEM", "NAVCode");

            //gvListof_BomDetail.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gcListof_BomDetail.DataSource = q.dsMaterial();

            gcListof_BomDetail.ProcessGridKey += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.Control)
                {
                    if (XtraMessageBox.Show("Delete row(s)?", "Delete rows dialog", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    GridControl grid = s as GridControl;
                    GridView view = grid.FocusedView as GridView;
                    view.DeleteSelectedRows();
                }
            };
        }

        public void clearForm()
        {
            txtBomNo.EditValue = db.getMaxID_v2(mainConn,"BOM","OIDBOM");
            txtCostsheetNo.EditValue = null;
            txtSmplNo_Header.EditValue = null;
            txtPatternNo.EditValue = null;
            txtPatternSizeZone.EditValue = null;
            txtItemNo.EditValue = null;
            txtModelName.EditValue = null;
            sl_StyleName.EditValue = null;
            gl_Category.EditValue = null;
            gl_Season_Header.EditValue = null;
            sl_Customer_Header.EditValue = null;
            txtFGProductCode.EditValue = null;
            sl_Color.EditValue = null;
            sl_Size.EditValue = null;
            gl_Unit.EditValue = null;
            txtUnitCost.EditValue = null;
            rdoStatus.SelectedIndex = 0;
            
            // ---------------------- End Header -------------------------
            
            txtListNo.EditValue = null;
            gl_MaterialType.EditValue = null;
            sl_ItemNo.EditValue = null;
            sl_MatColor.EditValue = null;
            sl_matSize.EditValue = null;
            txtComposition.EditValue = null;
            gl_Currency.EditValue = null;
            txtPrice.EditValue = null;
            txtConsumption.EditValue = null;
            txtCost.EditValue = null;
            sl_Vendor.EditValue = null;
            txtVendMatCode.EditValue = null;
            txtSmplLotNo.EditValue = null;
            gl_WorkStation.EditValue = null;
            txtMatLoss.EditValue = null;
            gl_MatUnit.EditValue = null;

            gcListof_SMPLDetail.DataSource = null;
            gcListof_BomDetail.DataSource = q.dsMaterial();
        }

        public bool chkCondition()
        {
            bool chk = false;

            //ChkRequired : Customer , Season , CostSheet , Model , CustomerItemCode , CustomerItemName , SKUCode , Size , Color
            if (dtLastDate.Text.ToString() == "") { ct.showWarningMessage("Please Input LastDate !"); tabbedBom.SelectedTabPageIndex = 0; dtLastDate.Focus(); return false; }
            else if (txtSmplNo_Header.Text.ToString() == "") { ct.showWarningMessage("Please Key SMPLNo !"); tabbedBom.SelectedTabPageIndex = 0; txtSmplNo_Header.Focus(); return false; }
            else if (txtPatternNo.Text.ToString() == "") { ct.showWarningMessage("Please Key PatternNo !"); tabbedBom.SelectedTabPageIndex = 0; txtPatternNo.Focus(); return false; }
            else if (txtPatternSizeZone.Text.ToString() == "") { ct.showWarningMessage("Please Key PatternSizeZone !"); tabbedBom.SelectedTabPageIndex = 0; txtPatternSizeZone.Focus(); return false; }
            else if (txtItemNo.Text.ToString() == "") { ct.showWarningMessage("Please Key ItemNo !"); tabbedBom.SelectedTabPageIndex = 0; txtItemNo.Focus(); return false; }
            else if (txtModelName.Text.ToString() == "") { ct.showWarningMessage("Please Key ModelName !"); tabbedBom.SelectedTabPageIndex = 0; txtModelName.Focus(); return false; }
            else if (sl_StyleName.Text.ToString() == "") { ct.showWarningMessage("Please Select StyleName !"); tabbedBom.SelectedTabPageIndex = 0; sl_StyleName.Focus(); return false; }
            else if (gl_Category.Text.ToString() == "") { ct.showWarningMessage("Please Select Category !"); tabbedBom.SelectedTabPageIndex = 0; gl_Category.Focus(); return false; }
            else if (gl_Season_Header.Text.ToString() == "") { ct.showWarningMessage("Please Select Season !"); tabbedBom.SelectedTabPageIndex = 0; gl_Season_Header.Focus(); return false; }
            else if (sl_Customer_Header.Text.ToString() == "") { ct.showWarningMessage("Please Select Customer !"); tabbedBom.SelectedTabPageIndex = 0; sl_Customer_Header.Focus(); return false; }
            else if (txtFGProductCode.Text.ToString() == "") { ct.showWarningMessage("Please Key ProductCode !"); tabbedBom.SelectedTabPageIndex = 0; txtFGProductCode.Focus(); return false; }
            else if (sl_Color.Text.ToString() == "") { ct.showWarningMessage("Please Select Color !"); tabbedBom.SelectedTabPageIndex = 0; sl_Color.Focus(); return false; }
            else if (sl_Size.Text.ToString() == "") { ct.showWarningMessage("Please Select Size !"); tabbedBom.SelectedTabPageIndex = 0; sl_Size.Focus(); return false; }
            else if (gl_Unit.Text.ToString() == "") { ct.showWarningMessage("Please Select Unit !"); tabbedBom.SelectedTabPageIndex = 0; gl_Unit.Focus(); return false; }
            else if (txtUnitCost.Text.ToString() == "") { ct.showWarningMessage("Please Key UnitCost !"); tabbedBom.SelectedTabPageIndex = 0; txtUnitCost.Focus(); return false; }
            /* ------------------------------------------------------------------------ End Header Tab ------------------------------------------------------------------- */
            else if (gl_Currency.Text.ToString() == "") { ct.showWarningMessage("Please Select Currency !"); tabbedBom.SelectedTabPageIndex = 1; gl_Currency.Focus(); return false; }
            else if (sl_Vendor.Text.ToString() == "") { ct.showWarningMessage("Please Select Vendor !"); tabbedBom.SelectedTabPageIndex = 1; sl_Vendor.Focus(); return false; }
            else if (txtVendMatCode.Text.ToString() == "") { ct.showWarningMessage("Please Key VendorMatCode !"); tabbedBom.SelectedTabPageIndex = 1; txtVendMatCode.Focus(); return false; }
            else if (txtSmplLotNo.Text.ToString() == "") { ct.showWarningMessage("Please Key SampleLotno !"); tabbedBom.SelectedTabPageIndex = 1; txtSmplLotNo.Focus(); return false; }
            else if (gl_WorkStation.Text.ToString() == "") { ct.showWarningMessage("Please Selected WorkStation !"); tabbedBom.SelectedTabPageIndex = 1; gl_WorkStation.Focus(); return false; }
            else if (txtMatLoss.Text.ToString() == "") { ct.showWarningMessage("Please Key MatLoss !"); tabbedBom.SelectedTabPageIndex = 1; txtMatLoss.Focus(); return false; }
            else if (gl_MatUnit.Text.ToString() == "") { ct.showWarningMessage("Please Select MaterialUnit !"); tabbedBom.SelectedTabPageIndex = 1; gl_MatUnit.Focus(); return false; }
            {
                chk = true;
            }
            return chk;
        }

        public bool chkConditionDetail()
        {
            bool chkDT = false;

            for (int i = 0; i < gvListof_BomDetail.RowCount; i++)
            {
                GridView gv = gvListof_BomDetail;
                string Type = gv.GetRowCellValue(i, "Type").ToString();
                string Itemno = gv.GetRowCellValue(i, "Itemno").ToString();
                string Composition = gv.GetRowCellValue(i, "Composition").ToString();
                string Color = gv.GetRowCellValue(i, "Color").ToString();
                string Size = gv.GetRowCellValue(i, "Size").ToString();
                string Unit = gv.GetRowCellValue(i, "Unit").ToString();
                string Consumption = gv.GetRowCellValue(i, "Consumption").ToString();
                string Price = gv.GetRowCellValue(i, "Price").ToString();
                string Cost = gv.GetRowCellValue(i, "Cost").ToString();

                if (Composition == "") { ct.focusColumns(gv, i, 3, "Please Key Composition Value"); return false; }
                else if (Consumption == "") { ct.focusColumns(gv, i, 7, "Please Key Consumption Value"); return false; }
                else if (Price == "") { ct.focusColumns(gv, i, 8, "Please Key Price Value"); return false;  }
                else if (Cost == "") { ct.focusColumns(gv, i, 9, "Please Key Cost Value"); return false; }
                else
                {
                    chkDT = true;
                }

                Console.WriteLine($"{Type} {Itemno} {Composition} {Color} {Size} {Unit} {Consumption} {Price} {Cost}");
            }

            return chkDT;
        }

        public double sumCost()
        {
            double totalCost = 0.00;
            for (int i = 0; i < gvListof_BomDetail.RowCount; i++)
            {
                totalCost += Convert.ToDouble(gvListof_BomDetail.GetRowCellValue(i, "Cost").ToString());
            }
            return totalCost;
            Console.WriteLine(totalCost);
        }

        private void LoadData()
        {
            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.Append("SELECT OIDPayment AS No, Name, Description, DuedateCalculation, Status, CreatedBy, CreatedDate ");
            //sbSQL.Append("FROM PaymentTerm ");
            //sbSQL.Append("ORDER BY OIDPayment ");
            //new ObjDevEx.setGridControl(gcPTerm, gvPTerm, sbSQL).getData(false, false, false, true);

        }

        private void NewData()
        {
            //txeName.Text = "";
            //lblStatus.Text = "* Add Payment Term";
            //lblStatus.ForeColor = Color.Green;

            //txeID.Text = new DBQuery("SELECT CASE WHEN ISNULL(MAX(OIDPayment), '') = '' THEN 1 ELSE MAX(OIDPayment) + 1 END AS NewNo FROM PaymentTerm").getString();
            //txeDescription.Text = "";
            //txeDueDate.Text = "";
            //rgStatus.SelectedIndex = -1;

            //txeCREATE.Text = "0";
            //txeDATE.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            //////txeID.Focus();
        }

        private void bbiNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tabbed_Master.SelectedTabPageIndex == 0) //List Tab หน้า List
            {
                tabbed_Master.SelectedTabPageIndex = 1;
                ct.hide_bbi(bbiEdit);
                ct.hide_bbi(bbiClone);
                ct.show_bbi(bbiSave);
                tabbedBom.SelectedTabPageIndex = 0;
            }
            else //Entry tab หน้า Entry
            {
                ct.show_bbi(bbiSave);
                ct.hide_bbi(bbiEdit);
                ct.hide_bbi(bbiClone);
                tabbedBom.SelectedTabPageIndex = 0;
            }
        }

        private void gvGarment_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            
        }

        private void selectStatus(int value)
        {
            //switch (value)
            //{
            //    case 0:
            //        rgStatus.SelectedIndex = 0;
            //        break;
            //    case 1:
            //        rgStatus.SelectedIndex = 1;
            //        break;
            //    default:
            //        rgStatus.SelectedIndex = -1;
            //        break;
            //}
        }

        private bool chkDuplicate()
        {
            bool chkDup = true;
            //if (txeName.Text != "")
            //{
            //    txeName.Text = txeName.Text.Trim();
            //    if (txeName.Text.Trim() != "" && lblStatus.Text == "* Add Payment Term")
            //    {
            //        StringBuilder sbSQL = new StringBuilder();
            //        sbSQL.Append("SELECT TOP(1) Name FROM PaymentTerm WHERE (Name = N'" + txeName.Text.Trim() + "') ");
            //        if (new DBQuery(sbSQL).getString() != "")
            //        {
            //            FUNC.msgWarning("Duplicate payment term. !! Please Change.");
            //            txeName.Text = "";
            //            chkDup = false;
            //        }
            //    }
            //    else if (txeName.Text.Trim() != "" && lblStatus.Text == "* Edit Payment Term")
            //    {
            //        StringBuilder sbSQL = new StringBuilder();
            //        sbSQL.Append("SELECT TOP(1) OIDPayment ");
            //        sbSQL.Append("FROM PaymentTerm ");
            //        sbSQL.Append("WHERE (Name = N'" + txeName.Text.Trim() + "') ");
            //        string strCHK = new DBQuery(sbSQL).getString();
            //        if (strCHK != "" && strCHK != txeID.Text.Trim())
            //        {
            //            FUNC.msgWarning("Duplicate payment term. !! Please Change.");
            //            txeName.Text = "";
            //            chkDup = false;
            //        }
            //    }
            //}
            return chkDup;
        }

        private void txeName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    txeDescription.Focus();
            //}
        }

        private void txeName_LostFocus(object sender, EventArgs e)
        {
            //txeName.Text = txeName.Text.ToUpper().Trim();
            //bool chkDup = chkDuplicate();
            //if (chkDup == false)
            //{
            //    txeName.Text = "";
            //    txeName.Focus();
            //}
            //else
            //{
            //    txeDescription.Focus();
            //}
        }

        private void txeDescription_KeyDown(object sender, KeyEventArgs e)
        {
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        txeDueDate.Focus();
        //    }
        }

        private void txeDueDate_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    rgStatus.Focus();
            //}
        }

        private void gvPTerm_RowStyle(object sender, RowStyleEventArgs e)
        {
            
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //chkCondition
            //1. ทุก Field ในหน้า Header ต้องไม่ว่าง บังคับให้กรอกข้อมูลทั้งหมด
            //2. ในตาราง Detail บังคับว่าต้องใส่ Cost ถ้าใส่ครบทุกช่อง ให้ Sum Cost ไปใส่ในช่อง Cost ของ Header

            //if (chkCondition() == true)
            //{ 
            //    //
            //}

            // Loop chk tblDetail

            gvListof_BomDetail.PostEditor();
            gvListof_BomDetail.UpdateCurrentRow();
            gcListof_BomDetail.FocusedView.UpdateCurrentRow();

            

            if (chkConditionDetail() == true)
            {
                txtUnitCost.EditValue = sumCost();

                if (chkCondition() == true)
                {
                    if (ct.doConfirm("Save Bom ?") == true)
                    {
                        bool chkSave = false;

                        /*OIDBOM, OIDSMPL, OIDITEM, OIDSIZE, OIDColor, OIDCUST, OIDSTYLE, OIDCATEGORY, OIDUNIT, BOMNo, RevisionNo, IssueDate, Season, PatternZone, SMPLItemNo, Model, Cost, Status, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate*/

                        string oidsmpl = db.get_oneParameter("Select OIDSMPL From SMPLRequest Where SMPLNo = " + ct.chkNull_txt(txtSmplNo_Header) + " ", mainConn, "OIDSMPL");
                        string oiditem = db.get_oneParameter("Select OIDITEM From Items Where Code = " + ct.chkNull_txt(txtFGProductCode) + " ", mainConn, "OIDITEM");

                        string sql = "Insert Into BOM(OIDSMPL, OIDITEM, OIDSIZE, OIDColor, OIDCUST, OIDSTYLE, OIDCATEGORY, OIDUNIT, BOMNo, RevisionNo, IssueDate, Season, PatternZone, SMPLItemNo, ModelName, Cost, Status, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)";
                        sql += " Values(" + oidsmpl + ", " + oiditem + " , " + ct.chkNull_slInt(sl_Size) + ", " + ct.chkNull_slInt(sl_Color) + ", " + ct.chkNull_slInt(sl_Customer_Header) + ", " + ct.chkNull_slInt(sl_StyleName) + ", " + ct.chkNull_glInt(gl_Category) + ", " + ct.chkNull_glInt(gl_Unit) + ", " + ct.chkNull_txtInt(txtBomNo) + ", " + ct.chkNull_txtInt(txtReviseNo) + ", " + ct.chkNull_dt(dtLastDate) + ", " + ct.chkNull_gl(gl_Season_Header) + ", " + en.getID_PatternSizeZone(txtPatternSizeZone.EditValue.ToString()) + ", " + ct.chkNull_txt(txtItemNo) + ", " + ct.chkNull_txt(txtModelName) + ", " + ct.chkNull_txtInt(txtUnitCost) + ", " + rdoStatus.SelectedIndex + " , 1 , " + ct.getCurrentDate() + ", 1, " + ct.getCurrentDate() + " )";
                        Console.WriteLine(sql);
                        int qi = db.Query(sql, mainConn); //Query Insert

                        int maxoidBom = Convert.ToInt32(db.getMaxID_v2(mainConn, "BOM", "OIDBOM")) - 1; //ct.showInfoMessage($"maxoidBom : {maxoidBom}");

                        // Loop ListDetail
                        for (int i = 0; i < gvListof_BomDetail.RowCount; i++)
                        {
                            GridView gv = gvListof_BomDetail;
                            string Type = gv.GetRowCellValue(i, "Type").ToString();             /**/
                            string Itemno = gv.GetRowCellValue(i, "Itemno").ToString();
                            string Composition = gv.GetRowCellValue(i, "Composition").ToString();
                            string Color = gv.GetRowCellValue(i, "Color").ToString();            /**/
                            string Size = gv.GetRowCellValue(i, "Size").ToString();             /**/
                            string Unit = gv.GetRowCellValue(i, "Unit").ToString();             /**/
                            string Consumption = gv.GetRowCellValue(i, "Consumption").ToString();
                            string Price = gv.GetRowCellValue(i, "Price").ToString();
                            string Cost = gv.GetRowCellValue(i, "Cost").ToString();

                            // string oidUnit = db.getValue_inTable(mainConn,"Items", "DefaultUnit", " OIDITEM = "+ Itemno + " ");
                            string oidUnit = ct.chkNull_glInt(gl_MatUnit);

                            /*OIDBOMDT, OIDBOM, OIDITEM, OIDUNIT, OIDVEND, OIDDEPT, MatNo, MatDetail, Currency, Price, Consumption, Cost, PercentLoss, SMPLColor, SMPLLotNo*/
                            string sql2 = "Insert Into BOMDetail(OIDBOM, OIDITEM, OIDUNIT, OIDVEND, OIDDEPT, MatNo, MatDetail, Currency, Price, Consumption, Cost, PercentLoss, SMPLColor, SMPLLotNo)";
                            sql2 += "Values(" + maxoidBom + ", " + Itemno + ", " + oidUnit + ", " + ct.chkNull_slInt(sl_Vendor) + ", " + ct.chkNull_glInt(gl_WorkStation) + ", " + ct.chkNull_txt(txtVendMatCode) + ", '" + Composition + "', " + ct.chkNull_glInt(gl_Currency) + ", " + Price + ", " + Consumption + ", " + Cost + ", " + ct.chkNull_txtInt(txtMatLoss) + ", " + ct.chkNull_slInt(sl_MatColor) + ", " + ct.chkNull_txt(txtSmplLotNo) + ")";
                            Console.WriteLine(sql2);
                            int qii = db.Query(sql2, mainConn); chkSave = (qii > 0) ? true : false; // Query InsertDetail

                            // Console.WriteLine($"{Type} {Itemno} {Composition} {Color} {Size} {Unit} {Consumption} {Price} {Cost}");
                        }

                        if (chkSave == true)
                        {
                            ct.showInfoMessage("Save Success.");
                            clearForm();
                        }
                        else
                        {
                            ct.showErrorMessage("Can't Save. Please Contact Administrator.");
                        }
                    }
                }
            }    
        }

        private void gvListof_BomDetail_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            ct.validate_Numeric(sender, e, "Consumption");
            ct.validate_Numeric(sender, e, "Price");
            ct.validate_Numeric(sender, e, "Cost");
        }

        private void bbiExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string pathFile = new ObjSet.Folder(@"C:\MDS\Export\").GetPath() + "PaymentTermList_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            //gvPTerm.ExportToXlsx(pathFile);
            //System.Diagnostics.Process.Start(pathFile);
        }

        private void gvPTerm_RowClick(object sender, RowClickEventArgs e)
        {
            //lblStatus.Text = "* Edit Payment Term";
            //lblStatus.ForeColor = Color.Red;

            //txeID.Text = gvPTerm.GetFocusedRowCellValue("No").ToString();
            //txeName.Text = gvPTerm.GetFocusedRowCellValue("Name").ToString();
            //txeDescription.Text = gvPTerm.GetFocusedRowCellValue("Description").ToString();
            //txeDueDate.Text = gvPTerm.GetFocusedRowCellValue("DuedateCalculation").ToString();

            //int status = -1;
            //if (gvPTerm.GetFocusedRowCellValue("Status").ToString() != "")
            //{
            //    status = Convert.ToInt32(gvPTerm.GetFocusedRowCellValue("Status").ToString());
            //}

            //selectStatus(status);

            //txeCREATE.Text = gvPTerm.GetFocusedRowCellValue("CreatedBy").ToString();
            //txeDATE.Text = gvPTerm.GetFocusedRowCellValue("CreatedDate").ToString();
        }

        private void bbiPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gcPTerm.ShowPrintPreview();
        }

        private void bbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gcPTerm.Print();
        }

        private void tabbed_Master_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            if (tabbed_Master.SelectedTabPageIndex == 1) // Bom Entry Tab
            {
                q.get_sl_smplNo(sl_smplNo_Entry);
                q.get_gl_Branch(gl_branch_entry);
                q.get_gl_Season(gl_Season_Entry);
                q.get_sl_Customer(sl_Customer_Entry);
                //q.get_gcListof_SMPL(gcListof_SMPL); gvListof_SMPL.OptionsBehavior.Editable = false;
                txtCreateBy.EditValue = 0;
                txtCreateDate.EditValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                txtUpdateBy.EditValue = 0;
                txtUpdateDate.EditValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Header
                txtBomNo.EditValue = q.get_running_BomNo(); txtBomNo.ReadOnly = true;
                dtLastDate.EditValue = DateTime.Now;
                q.get_sl_StyleNmae(sl_StyleName);
                q.get_gl_Category(gl_Category);
                q.get_gl_Season(gl_Season_Header);
                q.get_sl_Customer(sl_Customer_Header);
                q.get_sl_Color(sl_Color);
                q.get_sl_Size(sl_Size);
                q.get_gl_Unit(gl_Unit);
                rdoStatus.SelectedIndex = 0;

                // Detail
                q.get_gl_matType(gl_MaterialType);
                q.get_sl_NAVCode(sl_ItemNo);
                q.get_sl_Color(sl_MatColor);
                q.get_sl_Size(sl_matSize);
                q.get_gl_Currency(gl_Currency);
                q.get_sl_Vendor(sl_Vendor);
                q.get_gl_WorkStation(gl_WorkStation);
                q.get_gl_Unit(gl_MatUnit);

                ct.hide_bbi(bbiRefresh);
            }
            else //Bom List Tab
            {
                ct.show_bbi(bbiNew);
                ct.hide_bbi(bbiEdit);
                ct.hide_bbi(bbiClone);
                ct.show_bbi(bbiRefresh);
            }
        }

        private void gvListof_Bom_DoubleClick(object sender, EventArgs e)
        {
            ct.hide_bbi(bbiSave);
            ct.show_bbi(bbiEdit);
            ct.show_bbi(bbiClone);
            tabbed_Master.SelectedTabPageIndex = 1;
        }

        private void gvListof_SMPL_DoubleClick(object sender, EventArgs e)
        {
            // ดึงข้อมูลจากตารางด้านซ้ายมาแสดงที่ ListDetail
            
            // Input
            /* smpl.OIDSMPL = 24 and q.OIDSIZE = 1596 and q.OIDCOLOR = 20228 */
            string oidSmpl  = ct.getCellVal(sender, "OIDSMPL").ToString();
            string oidSize  = ct.getCellVal(sender, "OIDSIZE").ToString();
            string oidColor = ct.getCellVal(sender, "OIDCOLOR").ToString();
            //ct.showInfoMessage(oidSmpl+","+oidSize+","+oidColor);

            /* ----------------------------------------------------------[ Process ]--------------------------------------------------------- */

            // Process
            // -- Process (1)
            string sql = "Select ROW_NUMBER() over(order by q.OIDSIZE) as No,q.OIDSMPLDT,d.OIDDEPT,d.Name as Type,i.OIDITEM,i.Code as ItemNo,fb.Composition,c.ColorName as Color,s.SizeName as Size , u.UnitName as Unit,mat.Consumption,mat.Price,null as Cost From SMPLRequest smpl inner join SMPLQuantityRequired q on q.OIDSMPL = smpl.OIDSMPL inner join ProductSize s on s.OIDSIZE = q.OIDSIZE inner join ProductColor c on c.OIDCOLOR = q.OIDCOLOR inner join Unit u on u.OIDUNIT = q.OIDUnit inner join SMPLRequestFabric fb on fb.OIDSMPLDT = q.OIDSMPLDT inner join SMPLRequestMaterial mat on mat.OIDSMPLDT = q.OIDSMPLDT left join Items i on i.OIDITEM = mat.OIDITEM inner join Departments d on d.OIDDEPT = mat.OIDDEPT";
            string condition = $" Where smpl.OIDSMPL = {oidSmpl} and q.OIDSIZE = {oidSize} and q.OIDCOLOR = {oidColor} ";
            sql += condition;

            string sqlTest = $"EXEC sp_getSMPLMaterial @oidSmpl = {oidSmpl}, @oidSize = {oidSize}, @oidColor = {oidColor}";

            //Console.WriteLine(sql);

            // -- Process (2)
            //string sql2 = "Select SMPLItem,SMPLNo,SMPLPatternNo,(case PatternSizeZone when 0 then 'Japan' when 1 then 'Europe' when 2 then 'US' end) as PatternSizeZone,smpl.ModelName ,smpl.OIDSTYLE,smpl.OIDCATEGORY,smpl.Season,smpl.OIDCUST,i.OIDITEM,q.OIDCOLOR,q.OIDSIZE,q.OIDUnit From SMPLRequest smpl inner join SMPLQuantityRequired q on q.OIDSMPL = smpl.OIDSMPL inner join SMPLRequestFabric fb on fb.OIDSMPLDT = q.OIDSMPLDT inner join Items i on i.OIDITEM = fb.OIDITEM";

            /* ----------------------------------------------------------[ Output ]--------------------------------------------------------- */

            // Output
            // -- Out Process (1)
            db.getDgv(sqlTest, gcListof_SMPLDetail,mainConn);
            gvListof_SMPLDetail.Columns["No"].Width = 30;
            // Hide 1 Colummns : OIDSMPLDT
            gvListof_SMPLDetail.Columns["OIDSMPLDT"].Visible    = false;
            gvListof_SMPLDetail.Columns["OIDITEM"].Visible      = false;
            gvListof_SMPLDetail.Columns["OIDDEPT"].Visible      = false;
            
            // -- Out Process (2)
            txtBomNo.EditValue              = db.getMaxID_v2(mainConn, "BOM", "OIDBOM");
            txtReviseNo.EditValue           = 0;
            txtCostsheetNo.EditValue        = null;//ct.getCellVal(sender, "SMPLItem");
            txtSmplNo_Header.EditValue      = sl_smplNo_Entry.Text.ToString();
            txtPatternNo.EditValue          = q.getvalByField("SMPLPatternNo", "SMPLPatternNo", condition);
            txtPatternSizeZone.EditValue    = en.getName_PatternSizeZone( q.getvalByField("PatternSizeZone", "PatternSizeZone", condition) );
            txtItemNo.EditValue             = ct.getCellVal(sender, "SMPLItem");
            txtModelName.EditValue          = q.getvalByField("ModelName", "smpl.ModelName", condition);
            sl_StyleName.EditValue          = q.getvalByField("OIDSTYLE", "smpl.OIDSTYLE", condition);
            gl_Category.EditValue           = q.getvalByField("OIDCATEGORY", "smpl.OIDCATEGORY", condition);
            gl_Season_Header.EditValue      = q.getvalByField("Season", "smpl.Season", condition);
            sl_Customer_Header.EditValue    = q.getvalByField("OIDCUST", "smpl.OIDCUST", condition);
            txtFGProductCode.EditValue      = (q.getvalByField("OIDITEM", "i.OIDITEM", condition) == "") ? "" : db.get_oneParameter("Select Code From Items Where OIDITEM = " + q.getvalByField("OIDITEM", "i.OIDITEM", condition) + " ", mainConn, "Code");
            sl_Color.EditValue              = q.getvalByField("OIDCOLOR", "q.OIDCOLOR", condition);
            sl_Size.EditValue               = q.getvalByField("OIDSIZE", "q.OIDSIZE", condition);
            gl_Unit.EditValue               = q.getvalByField("OIDUnit", "q.OIDUnit", condition);
            txtUnitCost.EditValue           = null;

            // -- Out Process (Other)
            tabbedBom.SelectedTabPageIndex = 0;
        }

        private void sl_smplNo_Entry_EditValueChanged(object sender, EventArgs e)
        {
            gcListof_SMPLDetail.DataSource = null;

            if (sl_smplNo_Entry.Text.ToString() != "")
            {
                string oidsmpl = sl_smplNo_Entry.EditValue.ToString();

                gl_branch_entry.EditValue       = db.get_oneParameter("Select OIDBranch From SMPLRequest Where OIDSMPL = " + oidsmpl + " ", mainConn, "OIDBranch");
                gl_Season_Entry.EditValue       = db.get_oneParameter("Select Season From SMPLRequest Where OIDSMPL =  " + oidsmpl + " ", mainConn, "Season");
                sl_Customer_Entry.EditValue     = db.get_oneParameter("Select OIDCUST From SMPLRequest Where OIDSMPL = " + oidsmpl + " ", mainConn, "OIDCUST");
                txtSmplItemNo_Entry.EditValue   = db.get_oneParameter("Select SMPLItem From SMPLRequest Where OIDSMPL = " + oidsmpl + " ", mainConn, "SMPLItem");

                string sql = "Select smpl.OIDSMPL,q.OIDSIZE,q.OIDCOLOR,q.OIDUnit,SMPLItem,s.SizeName,c.ColorName,u.UnitName From SMPLRequest smpl inner join SMPLQuantityRequired q on q.OIDSMPL = smpl.OIDSMPL inner join ProductColor c on c.OIDCOLOR = q.OIDCOLOR inner join ProductSize s on s.OIDSIZE = q.OIDSIZE inner join Unit u on u.OIDUNIT = q.OIDUnit";
                sql += " Where Status = 2 And smpl.OIDSMPL = " + oidsmpl + " Order By q.OIDSIZE DESC";
                db.getDgv(sql, gcListof_SMPL,mainConn);
                // Hide 3 Columns
                gvListof_SMPL.Columns["SMPLItem"].Visible = false;
                gvListof_SMPL.Columns["OIDSMPL"].Visible = false;
                gvListof_SMPL.Columns["OIDSIZE"].Visible = false;
                gvListof_SMPL.Columns["OIDCOLOR"].Visible = false;
                gvListof_SMPL.Columns["OIDUnit"].Visible = false;
            }
            else
            {
                gl_branch_entry.EditValue       = null;
                gl_Season_Entry.EditValue       = null;
                sl_Customer_Entry.EditValue     = null;
                txtSmplItemNo_Entry.EditValue   = null;
                gcListof_SMPL.DataSource        = null;
            }
        }

        private void gvListof_SMPLDetail_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            /* ดูว่า chk อันไหน */
            tabbedBom.SelectedTabPageIndex = 1;

            GridView view = sender as GridView;
            int selectedRowHandle = e.ControllerRow;
            if (e.Action == CollectionChangeAction.Refresh)
            {
                selectedRowHandle = view.FocusedRowHandle;
            }
            if (e.Action == CollectionChangeAction.Add)
            {
                // เพิ่มเข้าไปในตาราง Detail byid
                //ct.showInfoMessage($"check {selectedRowHandle}");
                GridView gvDetail = gvListof_BomDetail;
                gvDetail.AddNewRow();
                ct.addColumns(gvDetail, "Type",         ct.getCellVal(sender, "OIDDEPT"));
                ct.addColumns(gvDetail, "Itemno",       ct.getCellVal(sender, "OIDITEM"));
                ct.addColumns(gvDetail, "Composition",  ct.getCellVal(sender, "Composition"));
                ct.addColumns(gvDetail, "Color",        ct.getCellVal(sender, "Color"));
                ct.addColumns(gvDetail, "Size",         ct.getCellVal(sender, "Size"));
                ct.addColumns(gvDetail, "Unit",         ct.getCellVal(sender, "Unit"));
                ct.addColumns(gvDetail, "Consumption",  ct.getCellVal(sender, "Consumption"));
                ct.addColumns(gvDetail, "Price",        ct.getCellVal(sender, "Price"));
                ct.addColumns(gvDetail, "Cost",         ct.getCellVal(sender, "Cost"));
                gcListof_BomDetail.FocusedView.UpdateCurrentRow();
            }
            else if (e.Action == CollectionChangeAction.Remove)
            {
                // ลบออกจากตาราง Detail byid
                //ct.showInfoMessage($"uncheck {selectedRowHandle}");
                //GridControl grid = s as GridControl;
                //GridView view = grid.FocusedView as GridView;
                //view.DeleteSelectedRows();

                // loop row chk matid and remove row
                GridView gvDetail = gvListof_BomDetail;
                //gvDetail.DeleteRow(gvDetail.FocusedRowHandle);
                //gvDetail.DeleteRow(0);
            }

            /* --------------------------------------------------------------------------------------- */

            //ArrayList rows = ct.getList_isChecked(gvListof_SMPLDetail);

            //if (rows.Count > 0)
            //{
            //    tabbedBom.SelectedTabPageIndex = 1;
            //    try
            //    {
            //        // Create DataTable
            //        DataTable dt = new DataTable();

            //        //dt.Columns.Add("No", typeof(string));
            //        dt.Columns.Add("Type", typeof(string));
            //        dt.Columns.Add("Itemno", typeof(string));
            //        dt.Columns.Add("Composition", typeof(string));
            //        dt.Columns.Add("Color", typeof(string));
            //        dt.Columns.Add("Size", typeof(string));
            //        dt.Columns.Add("Unit", typeof(string));
            //        dt.Columns.Add("Consumption", typeof(string));
            //        dt.Columns.Add("Price", typeof(string));
            //        dt.Columns.Add("Cost", typeof(string));

            //        int listfbNo = 1;
            //        for (int i = 0; i < rows.Count; i++)
            //        {
            //            DataRow row = rows[i] as DataRow;

            //            /* Add to dt */
            //            /* type,itemno,composition,color,size,unit,consumption,price,cost */
            //            // Type,ItemNo,Composition,Color,Size,Unit,Consumption,Price,Cost

            //            dt.Rows.Add(new object[] {
            //                //listfbNo++
            //                row["Type"].ToString()
            //                //,row["ItemNo"].ToString()
            //                , db.get_oneParameter("Select OIDITEM From Items Where Code = '"+row["ItemNo"].ToString()+"' ",mainConn,"OIDITEM")
            //                ,row["Composition"].ToString()
            //                ,row["Color"].ToString()
            //                ,row["Size"].ToString()
            //                ,row["Unit"].ToString()
            //                ,row["Consumption"].ToString()
            //                ,row["Price"].ToString()
            //                ,row["Cost"].ToString()
            //            });
            //        }
            //        gcListof_BomDetail.DataSource = dt;
            //    }
            //    catch { }
            //}
        }

        private void gvListof_SMPLDetail_DoubleClick(object sender, EventArgs e)
        {
            tabbedBom.SelectedTabPageIndex = 1;

            GridView gv = gvListof_BomDetail;
            gv.AddNewRow();
            ct.addColumns(gv, "Type", ct.getCellVal(sender, "OIDDEPT"));
            ct.addColumns(gv, "Itemno", ct.getCellVal(sender, "OIDITEM"));
            ct.addColumns(gv, "Composition", ct.getCellVal(sender, "Composition"));
            ct.addColumns(gv, "Color", ct.getCellVal(sender, "Color"));
            ct.addColumns(gv, "Size", ct.getCellVal(sender, "Size"));
            ct.addColumns(gv, "Unit", ct.getCellVal(sender, "Unit"));
            ct.addColumns(gv, "Consumption", ct.getCellVal(sender, "Consumption"));
            ct.addColumns(gv, "Price", ct.getCellVal(sender, "Price"));
            ct.addColumns(gv, "Cost", ct.getCellVal(sender, "Cost"));
            gcListof_BomDetail.FocusedView.UpdateCurrentRow();
        }

        private void sl_ItemNo_EditValueChanged(object sender, EventArgs e)
        {
            if (sl_ItemNo.Text.ToString() != "")
            {
                gl_MaterialType.EditValue = db.get_oneParameter("Select OIDITEM From Items Where OIDITEM = " + sl_ItemNo.EditValue.ToString() + " ", mainConn, "OIDITEM");
            }
            else
            {
                gl_MaterialType.EditValue = null;
            }
        }

        private void rep_slItemCode_EditValueChanged(object sender, EventArgs e)
        {
            // ดึง OID ของ rep_slItemCode
            SearchLookUpEdit sl = sender as SearchLookUpEdit;
            string repid        = sl.EditValue.ToString();
            //ct.showInfoMessage(repid);

            GridView gv = gvListof_BomDetail;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Itemno"], db.get_oneParameter("Select OIDITEM From Items Where OIDITEM = " + repid+" ", mainConn, "OIDITEM"));
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Color"], db.get_oneParameter("Select c.ColorName as Color From Items i inner join ProductColor c on c.OIDCOLOR = i.OIDCOLOR Where OIDITEM = "+ repid + " ", mainConn, "Color"));
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Size"], db.get_oneParameter("Select s.SizeName as Size From Items i inner join ProductSize s on s.OIDSIZE = i.OIDSIZE Where OIDITEM = "+ repid + " ", mainConn, "Size"));
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Unit"], db.get_oneParameter("Select u.UnitName as Unit From Items i inner join Unit u on u.OIDUNIT = i.DefaultUnit Where OIDITEM = "+repid+" ", mainConn, "Unit"));
            gcListof_BomDetail.FocusedView.UpdateCurrentRow();
        }

        private void gcListof_BomDetail_ProcessGridKey(object sender, KeyEventArgs e)
        {
            ct.nextColumns(e, gvListof_BomDetail);
        }

        private void bbiRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            q.get_gcListof_Bom(gcListof_Bom);
        }
    }
}