using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEV03
{
    public class hardQuery
    {
        goClass.dbConn db = new goClass.dbConn();
        goClass.ctool ct = new goClass.ctool();
        SqlConnection mainConn = new goClass.dbConn().MDS();

        /* -------------------------------------------------------- First Page -------------------------------------------------------- */
        public void get_sl_smplNo(SearchLookUpEdit sl)
        {
            string sql = "Select OIDSMPL, SMPLNo From SMPLRequest where Status = 2"; /*เอามาแค่ smpl ที่ Approved แล้วเท่านั้น*/
            db.getSl(sql,mainConn,sl, "OIDSMPL", "SMPLNo");
        }

        public void get_gl_Season(GridLookUpEdit gl)
        {
            string sql = "Select distinct s.Season as Season From( Select SUBSTRING( cast(Year(GETDATE())-1 as nvarchar(4)) , 3 , 2)+SeasonNo as Season From Season union Select SUBSTRING( cast(Year(GETDATE()) as nvarchar(4)) , 3 , 2) +SeasonNo as Season From Season union Select SUBSTRING( cast(Year(GETDATE())+1 as nvarchar(4)) , 3 , 2)+SeasonNo as Season From Season) as s left join SMPLRequest as smpl on s.Season = smpl.Season";
            db.getGl(sql,mainConn,gl, "Season", "Season");
        }

        public void get_sl_Customer(SearchLookUpEdit sl)
        {
            string sql = "Select OIDCUST,Name From Customer";
            db.getSl(sql, mainConn, sl, "OIDCUST", "Name");
        }

        public void get_gcListof_Bom(GridControl gc)
        {
            /* รอตาราง Bom ตัวจริงให้ฟาสร้างก่อน */
            //string sql = "Select ROW_NUMBER() over(order by OIDBOM) as No , OIDBOM , smpl.Status as SmplStatus_Enum ,(case smpl.Status when 0 then 'New' when 1 then 'WaitApprove' when 2 then 'Approved' end) as BomStatus , BOMNo , bom.RevisionNo , SMPLItemNo, SMPLNo, smpl.Season , c.Name as Customer , i.Code as ItemCode , cat.CategoryName , pStyle.StyleName as Style , smpl.SMPLPatternNo , bom.Status as BomStatus_Enum , (case bom.Status when 0 then 'Non-Active' when 1 then 'Active' end) as BomStatus From BOM bom inner join SMPLRequest smpl on smpl.OIDSMPL = bom.OIDSMPL inner join ProductStyle pStyle on pStyle.OIDSTYLE = bom.OIDSTYLE inner join GarmentCategory cat on cat.OIDGCATEGORY = bom.OIDCATEGORY inner join Items i on i.OIDITEM = bom.OIDITEM inner join Customer c on c.OIDCUST = bom.OIDCUST";
            db.getDgv("EXEC sp_getBOM", gc,mainConn);
        }
        /* -------------------------------------------------------- End First Page -------------------------------------------------------- */




        /* -------------------------------------------------------- Tab Entry ----------------------------------------------------------- */
        public void get_gl_Branch(GridLookUpEdit gl)
        {
            string sql = "Select OIDBranch,Name From Branchs";
            db.getGl(sql,mainConn,gl, "OIDBranch", "Name");
        }

        public void get_gcListof_SMPL(GridControl gc)
        {
            string sql = "Select smpl.OIDSMPL,SMPLItem,c.ColorName,s.SizeName From SMPLRequest smpl inner join SMPLQuantityRequired q on q.OIDSMPL = smpl.OIDSMPL inner join ProductColor c on c.OIDCOLOR = q.OIDCOLOR inner join ProductSize s on s.OIDSIZE = q.OIDSIZE Where Status = 2";
            db.getDgv(sql,gc,mainConn);
        }
        /* -------------------------------------------------------- End Tab Entry -------------------------------------------------------- */



        /* -------------------------------------------------------- Tab Header ----------------------------------------------------------- */
        public string getvalByField(string getColName, string setcolName, string conditionWhere)
        {
            string sql = "Select "+ setcolName + " From SMPLRequest smpl inner join SMPLQuantityRequired q on q.OIDSMPL = smpl.OIDSMPL inner join SMPLRequestFabric fb on fb.OIDSMPLDT = q.OIDSMPLDT inner join Items i on i.OIDITEM = fb.OIDITEM";
            sql += conditionWhere;
            return db.get_oneParameter(sql, mainConn, getColName);
        }
        public string get_running_BomNo()
        {
            /* รอแก้ Query ดึงจากตาราง Bom */
            string sql = "SELECT CASE WHEN ISNULL(MAX(OIDSMPLMT), '') = '' THEN 1 ELSE MAX(OIDSMPLMT)+1 END AS newOIDMat FROM SMPLRequestMaterial";
            return db.get_oneParameter(sql,mainConn, "newOIDMat");
        }
        public void get_sl_StyleNmae(SearchLookUpEdit sl)
        {
            string sql = "Select OIDSTYLE,StyleName From ProductStyle";
            db.getSl(sql,mainConn,sl, "OIDSTYLE", "StyleName");
        }

        public void get_gl_Category(GridLookUpEdit gl)
        {
            string sql = "Select OIDGCATEGORY,CategoryName FRom GarmentCategory";
            db.getGl(sql,mainConn,gl, "OIDGCATEGORY", "CategoryName");
        }

        public void get_sl_Color(SearchLookUpEdit sl)
        {
            string sql = "Select OIDCOLOR,ColorName From ProductColor";
            db.getSl(sql,mainConn,sl, "OIDCOLOR", "ColorName");
        }

        public void get_sl_Size(SearchLookUpEdit sl)
        {
            string sql = "Select OIDSIZE,SizeName From ProductSize";
            db.getSl(sql,mainConn,sl, "OIDSIZE", "SizeName");
        }

        public void get_gl_Unit(GridLookUpEdit gl)
        {
            string sql = "Select OIDUNIT,UnitName From Unit";
            db.getGl(sql,mainConn,gl, "OIDUNIT", "UnitName");
        }
        /* -------------------------------------------------------- End Tab Header -------------------------------------------------------- */



        /* -------------------------------------------------------- Tab Details ----------------------------------------------------------- */
        public void get_gl_matType(GridLookUpEdit gl)
        {
            string sql = "Select OIDITEM,(case MaterialType When 0 then 'FinishGood' when 1 then 'Fabric' when 2 then 'Accessory' when 3 then 'Packaging' when 4 then 'Sample' when 9 then 'Other' end) as MaterialType From Items";
            db.getGl(sql, mainConn, gl, "OIDITEM", "MaterialType");
        }

        public void get_sl_NAVCode(SearchLookUpEdit sl)
        {
            string sql = "Select OIDITEM,Code as NAVCode From Items";
            db.getSl(sql,mainConn,sl, "OIDITEM", "NAVCode");
        }

        public void get_gl_Currency(GridLookUpEdit gl)
        {
            string sql = "Select OIDCURR,Currency From Currency";
            db.getGl(sql, mainConn, gl, "OIDCURR", "Currency");
        }

        public void get_sl_Vendor(SearchLookUpEdit sl)
        {
            string sql = "Select OIDVEND,Name as Vendor From Vendor";
            db.getSl(sql,mainConn,sl, "OIDVEND", "Vendor");
        }

        public void get_gl_WorkStation(GridLookUpEdit gl)
        {
            string sql = "Select OIDDEPT,brn.Name as BranName,dep.Name as Department From Departments dep inner join Branchs brn on brn.OIDBranch = dep.OIDBRANCH Where DepartmentType in(1,4,5) order by OIDDEPT";
            db.getGl(sql, mainConn, gl, "OIDDEPT", "Department");
        }
        /* -------------------------------------------------------- End Tab Details -------------------------------------------------------- */

        public class csMaterial
        {
            public csMaterial() { }
            public csMaterial(string type, string itemno, string composition, string color, string size, string unit, string consumption, string price, string cost)
            {
                /*type,itemno,composition,color,size,unit,consumption,price,cost*/
                Type = type;
                Itemno = itemno;
                Composition = composition;
                Color = color;
                Size = size;
                Unit = unit;
                Consumption = consumption;
                Price = price;
                Cost = cost;
            }
            public string Type { get; set; }
            public string Itemno { get; set; }
            public string Composition { get; set; }
            public string Color { get; set; }
            public string Size { get; set; }
            public string Unit { get; set; }
            public string Consumption { get; set; }
            public string Price { get; set; }
            public string Cost { get; set; }
        }

        public BindingList<csMaterial> dsMaterial()
        {
            BindingList<csMaterial> ds = new BindingList<csMaterial>();
            return ds;
        }

    }//end-class hardQuery
}
