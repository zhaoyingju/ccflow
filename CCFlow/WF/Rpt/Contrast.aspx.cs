﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP;
using BP.WF;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys;
using BP.Rpt;
using BP.Sys.Xml;
namespace CCFlow.WF.Rpt
{
    public partial class WF_Rpt_Contrast : System.Web.UI.Page
    {
        #region 属性
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                {
                    // throw new Exception("丢失FK_Flow参数.");
                    s = "001";
                }
                s = s.Replace("ND", "");
                s = s.Replace("Rpt", "");
                return s;
            }
        }
        public new string EnsName
        {
            get
            {
                return "ND" + int.Parse(this.FK_Flow) + "Rpt";
            }
        }
        public Entities _HisEns = null;
        public new Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                {
                    if (this.EnsName != null)
                    {
                        if (this._HisEns == null)
                            _HisEns = BP.DA.ClassFactory.GetEns(this.EnsName);
                    }
                }
                return _HisEns;
            }
        }
        /// <summary>
        /// key
        /// </summary>
        public  string Key
        {
            get
            {
                try
                {
                    return this.ToolBar1.GetTBByID("TB_Key").Text;
                }
                catch
                {
                    return null;
                }
            }
        }
        public UserRegedit ur = null;
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsFY
        {
            get
            {
                string str = this.Request.QueryString["IsFY"];
                if (str == null || str == "0")
                    return false;
                return true;
            }
        }
        public string NumKey
        {
            get
            {
                string str = this.Request.QueryString["NumKey"];
                if (str == null)
                    return ViewState["NumKey"] as string;
                else
                    return str;
            }
            set
            {
                ViewState["NumKey"] = value;
            }
        }
        public string OrderBy
        {
            get
            {
                string str = this.Request.QueryString["OrderBy"];
                if (str == null)
                    return ViewState["OrderBy"] as string;
                else
                    return str;
            }
            set
            {
                ViewState["OrderBy"] = value;
            }
        }
        public string DoType
        {
            get
            {
                string s = this.Request.QueryString["DoType"];
                if (s == null)
                    s = "My";
                return s;
            }
        }
        public string OrderWay
        {
            get
            {
                string str = this.Request.QueryString["OrderWay"];
                if (str == null)
                    return ViewState["OrderWay"] as string;
                else
                    return str;
            }
            set
            {
                ViewState["OrderWay"] = value;
            }
        }
        public bool IsReadonly
        {
            get
            {
                string i = this.Request.QueryString["IsReadonly"];
                if (i == "1")
                    return true;
                else
                    return false;
            }
        }
        public bool IsShowSum
        {
            get
            {
                string i = this.Request.QueryString["IsShowSum"];
                if (i == "1")
                    return true;
                else
                    return false;
            }
        }
        public bool IsContainsNDYF
        {
            get
            {
                if (this.ViewState["IsContinueNDYF"].ToString().ToUpper() == "TRUE")
                    return true;
                else
                    return false;
            }
        }
        public string CfgVal
        {
            get
            {
                return "";
                //return this.ViewState["CfgVal"].ToString();
            }
            set
            {
                this.ViewState["CfgVal"] = value;
            }
        }
        public MapData HisMD = null;
        public MapAttrs HisMAs = null;
        #endregion

        #region 控件
        public DDL DDL_Num
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Num");
            }
        }
        public DDL DDL_D
        {
            get
            {
                return this.Left.GetDDLByID("DDL_D");
            }
        }
        public DDL DDL_FXWay
        {
            get
            {
                return this.Left.GetDDLByID("DDL_FXWay");
            }
        }
        public DDL DDL_V1
        {
            get
            {
                return this.Left.GetDDLByID("DDL_V1");
            }
        }
        public DDL DDL_Group
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Group");
            }
        }
        public DDL DDL_Order
        {
            get
            {
                return this.Left.GetDDLByID("DDL_Order");
            }
        }
        public DDL DDL_V2
        {
            get
            {
                return this.Left.GetDDLByID("DDL_V2");
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.HisMD = new MapData(this.EnsName);
            this.HisMAs = new MapAttrs(this.EnsName);
            this.ur = new UserRegedit(WebUser.No, this.EnsName + "_Con");

            this.InitToolbar();
            this.InitLeft();
            if (this.IsPostBack == false)
                this.BindDG();
        }
        public void InitToolbar()
        {
            AttrSearchs searchs = null;

            #region 处理查询设的默认.
            if (this.DoType == "My")
            {
                #region 处理查询权限
                Entity en = this.HisEns.GetNewEntity;
                Map map = en.EnMap;
                this.ToolBar1.InitByMapV2(map, 1, this.EnsName);
                searchs = map.SearchAttrs;
                string defVal = "";
                System.Data.DataTable dt = null;
                foreach (AttrSearch attr in searchs)
                {
                    DDL mydll = this.ToolBar1.GetDDLByKey("DDL_" + attr.Key);
                    if (mydll == null)
                        continue;
                    defVal = mydll.SelectedItemStringVal;
                    mydll.Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + this.EnsName + "','" + attr.Key + "')";
                    switch (attr.Key)
                    {
                        case "FK_NY":
                            dt = DBAccess.RunSQLReturnTable("SELECT DISTINCT FK_NY FROM " + this.EnsName + " WHERE FK_NY!='' ORDER BY FK_NY");
                            mydll.Items.Clear();
                            mydll.Items.Add(new ListItem("=>月份", "all"));
                            foreach (DataRow dr in dt.Rows)
                            {
                                mydll.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
                            }
                            mydll.SetSelectItem(defVal);
                            break;
                        case "FlowStarter":
                            dt = DBAccess.RunSQLReturnTable("SELECT No,Name FROM WF_Emp WHERE  FK_Dept IN (SELECT FK_Dept FROM  WF_DeptFlowSearch WHERE FK_Emp='" + WebUser.No + "'  AND FK_Flow='" + this.FK_Flow + "') AND No IN (SELECT DISTINCT FlowStarter FROM " + this.EnsName + " WHERE FlowStarter!='')");
                            mydll.Items.Clear();
                            mydll.Items.Add(new ListItem("=>发起人", "all"));
                            foreach (DataRow dr in dt.Rows)
                            {
                                mydll.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                            }
                            mydll.SetSelectItem(defVal);
                            mydll.Attributes["onchange"] = "DDL_mvals_OnChange(this,'ND" + int.Parse(this.FK_Flow) + "Rpt','" + attr.Key + "')";
                            break;
                        case "FK_Dept":
                            if (WebUser.No != "admin")
                            {
                                dt = DBAccess.RunSQLReturnTable("SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM  WF_DeptFlowSearch WHERE FK_Emp='" + WebUser.No + "'  AND FK_Flow='" + this.FK_Flow + "')");
                                if (dt.Rows.Count == 0)
                                {
                                    this.Right.AddMsgOfWarning("提示", "<h2>系统管理员没有给您设置查询权限。</h2>");
                                    this.ToolBar1.Controls.Clear();
                                    return;
                                }
                                mydll.Items.Clear();
                                foreach (DataRow dr in dt.Rows)
                                    mydll.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                            }
                            if (mydll.Items.Count >= 2)
                            {
                                ListItem liMvals = new ListItem("*多项组合..", "mvals");
                                liMvals.Attributes.CssStyle.Add("style", "color:green");
                                liMvals.Attributes.Add("color", "green");
                                liMvals.Attributes.Add("style", "color:green");
                            }
                            mydll.SetSelectItem(defVal);
                            break;
                        default:
                            break;
                    }
                }
                #endregion 处理查询权限
            }
            else
            {
                #region 处理查询权限
                Entity en = this.HisEns.GetNewEntity;
                Map map = en.EnMap;
                this.ToolBar1.InitByMapV2(map, 1, this.EnsName);
                searchs = map.SearchAttrs;
                string defVal = "";
                System.Data.DataTable dt = null;
                foreach (AttrSearch attr in searchs)
                {
                    DDL mydll = this.ToolBar1.GetDDLByKey("DDL_" + attr.Key);
                    if (mydll == null)
                        continue;
                    defVal = mydll.SelectedItemStringVal;
                    mydll.Attributes["onchange"] = "DDL_mvals_OnChange(this,'" + this.EnsName + "','" + attr.Key + "')";
                    switch (attr.Key)
                    {
                        case "FK_NY":
                            dt = DBAccess.RunSQLReturnTable("SELECT DISTINCT FK_NY FROM " + this.EnsName + " WHERE FK_NY!='' ORDER BY FK_NY");
                            mydll.Items.Clear();
                            mydll.Items.Add(new ListItem("=>月份", "all"));
                            foreach (DataRow dr in dt.Rows)
                            {
                                mydll.Items.Add(new ListItem(dr[0].ToString(), dr[0].ToString()));
                            }
                            mydll.SetSelectItem(defVal);
                            break;
                        case "FlowStarter":
                            dt = DBAccess.RunSQLReturnTable("SELECT No,Name FROM WF_Emp WHERE  FK_Dept IN (SELECT FK_Dept FROM  WF_DeptFlowSearch WHERE FK_Emp='" + WebUser.No + "'  AND FK_Flow='" + this.FK_Flow + "') AND No IN (SELECT DISTINCT FlowStarter FROM " + this.EnsName + " WHERE FlowStarter!='')");
                            mydll.Items.Clear();
                            mydll.Items.Add(new ListItem("=>发起人", "all"));
                            foreach (DataRow dr in dt.Rows)
                            {
                                mydll.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                            }
                            mydll.SetSelectItem(defVal);
                            mydll.Attributes["onchange"] = "DDL_mvals_OnChange(this,'ND" + int.Parse(this.FK_Flow) + "Rpt','" + attr.Key + "')";
                            break;
                        case "FK_Dept":
                            if (WebUser.No != "admin")
                            {
                                dt = DBAccess.RunSQLReturnTable("SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM  WF_DeptFlowSearch WHERE FK_Emp='" + WebUser.No + "'  AND FK_Flow='" + this.FK_Flow + "')");
                                if (dt.Rows.Count == 0)
                                {
                                    this.Right.AddMsgOfWarning("提示", "<h2>系统管理员没有给您设置查询权限。</h2>");
                                    this.ToolBar1.Controls.Clear();
                                    return;
                                }
                                mydll.Items.Clear();
                                foreach (DataRow dr in dt.Rows)
                                    mydll.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                            }

                            if (mydll.Items.Count >= 2)
                            {
                                ListItem liMvals = new ListItem("*多项组合..", "mvals");
                                liMvals.Attributes.CssStyle.Add("style", "color:green");
                                liMvals.Attributes.Add("color", "green");
                                liMvals.Attributes.Add("style", "color:green");
                            }
                            mydll.SetSelectItem(defVal);
                            break;
                        default:
                            break;
                    }
                }
                #endregion 处理查询权限

                this.ToolBar1.GetBtnByID("Btn_Search").Click += new System.EventHandler(this.ToolBar1_ButtonClick);
                //  this.ToolBar1.GetBtnByID(BP.Web.Controls.NamesOfBtn.Export).Click += new System.EventHandler(this.ToolBar1_ButtonClick);
            }
            #endregion 处理查询设的默认。
        }

        public void InitLeft()
        {
            this.Left.AddTable();
            string paras = this.ur.Paras;

            #region 选项
            this.Left.AddTR();
            this.Left.AddTDTitle("colspan=2", "分析数据");
            this.Left.AddTREnd();

            DDL ddl = new DDL();
            ddl.ID = "DDL_Num";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);

            ListItem li = null;
            this.Left.AddTR();
            this.Left.AddTD("数据项");
            Attrs attrs = this.HisMD.AttrsInTableEns;
            attrs.AddTBInt("MyNum", 1, "流程数量", true, true);
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType != UIContralType.TB)
                    continue;

                if (attr.UIVisible == false)
                    continue;

                if (attr.IsNum == false)
                    continue;

                if (attr.Key == "OID" || attr.Key == "WorkID" || attr.Key == "FID" || attr.Key == "MID")
                    continue;

                li = new ListItem(attr.Desc, attr.Key);
                if (paras.Contains("@Num=" + attr.Key))
                    li.Selected = true;
                ddl.Items.Add(li);
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            this.Left.AddTR();
            this.Left.AddTD("分析方式");
            ddl = new DDL();
            ddl.ID = "DDL_FXWay";
            ddl.AutoPostBack = true;

            li = new ListItem("求和", "SUM");
            if (paras.Contains("@FXWay=SUM"))
                li.Selected = true;
            ddl.Items.Add(li);

            li = new ListItem("求平均", "AVG");
            if (paras.Contains("@FXWay=AVG"))
                li.Selected = true;
            ddl.Items.Add(li);
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            ddl = new DDL();
            ddl.ID = "DDL_Order";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);

            li = new ListItem("升序", "UP");
            if (paras.Contains("@Order=UP"))
                li.Selected = true;
            ddl.Items.Add(li);

            li = new ListItem("降序", "DESC");
            if (paras.Contains("@Order=DESC"))
                li.Selected = true;
            ddl.Items.Add(li);

            this.Left.AddTR();
            this.Left.AddTD("排序方式");
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            #endregion 选项

            #region 比较对象
            this.Left.AddTR();
            this.Left.AddTDTitle("colspan=2", "比较内容");
            this.Left.AddTREnd();

            ddl = new DDL();
            ddl.ID = "DDL_D";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTR();
            this.Left.AddTD("比较选项");
            string dAttr = null;
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType == UIContralType.DDL)
                {
                    li = new ListItem(attr.Desc, attr.Key);

                    if (paras.Contains("@D=" + attr.Key))
                    {
                        dAttr = attr.Key;
                        li.Selected = true;
                    }
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            /*获取分析项目的数据。*/
            SysEnums sesD = null;
            Entities ensD = null;
            Map map = this.HisMD.HisEn.EnMap;
            Attr attrD1 = attrs.GetAttrByKey(this.DDL_D.SelectedItemStringVal);
            if (attrD1.IsEnum)
                sesD = new SysEnums(attrD1.UIBindKey);
            else
                ensD = attrD1.HisFKEns;

            this.Left.AddTR();
            this.Left.AddTD("基准项目");
            ddl = new DDL();
            ddl.ID = "DDL_V1";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            if (sesD != null)
            {
                if (sesD.Count == 0)
                    throw new Exception(attrD1.UIBindKey);

                foreach (SysEnum se in sesD)
                {
                    li = new ListItem(se.Lab, se.IntKey.ToString());
                    if (paras.Contains("@V1=" + se.IntKey))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            if (ensD != null)
            {
                ensD.RetrieveAll();
                if (ensD.Count == 0)
                    ensD.RetrieveAll();
                // throw new Exception(attrD1.UIBindKey);

                foreach (Entity en in ensD)
                {
                    li = new ListItem(en.GetValStrByKey("Name"), en.GetValStrByKey("No"));
                    if (paras.Contains("@V1=" + li.Value))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();

            this.Left.AddTR();
            this.Left.AddTD("对比项目");
            ddl = new DDL();
            ddl.ID = "DDL_V2";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            if (sesD != null)
            {
                foreach (SysEnum se in sesD)
                {
                    li = new ListItem(se.Lab, se.IntKey.ToString());
                    if (paras.Contains("@V2=" + se.IntKey))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }

            if (ensD != null)
            {
                foreach (Entity en in ensD)
                {
                    li = new ListItem(en.GetValStrByKey("Name"), en.GetValStrByKey("No"));
                    if (paras.Contains("@V2=" + li.Value))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            #endregion 横纬度

            ddl = new DDL();
            ddl.ID = "DDL_Group";
            ddl.AutoPostBack = true;
            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.Left.AddTR();
            this.Left.AddTD("分组项目");
            foreach (Attr attr in attrs)
            {
                if (attr.UIContralType == UIContralType.DDL)
                {
                    li = new ListItem(attr.Desc, attr.Key);
                    if (paras.Contains("@Group=" + attr.Key))
                        li.Selected = true;
                    ddl.Items.Add(li);
                }
            }
            this.Left.AddTD(ddl);
            this.Left.AddTREnd();
            this.Left.AddTableEnd();
        }
        public void SaveState()
        {
            #region 保存状态
            string paras = "";
            paras += "@Num=" + this.DDL_Num.SelectedItemStringVal;
            paras += "@FXWay=" + this.DDL_FXWay.SelectedItemStringVal;
            try
            {
                paras += "@V1=" + this.DDL_V1.SelectedItemStringVal;
                paras += "@V2=" + this.DDL_V2.SelectedItemStringVal;
            }
            catch
            {
            }

            paras += "@D=" + this.DDL_D.SelectedItemStringVal;
            paras += "@Order=" + this.DDL_Order.SelectedItemStringVal;
            paras += "@Group=" + this.DDL_Group.SelectedItemStringVal;

            ur.CfgKey = this.EnsName + "_Con";
            ur.MyPK = WebUser.No + "_" + ur.CfgKey;
            ur.FK_Emp = WebUser.NoOfSessionID;
            ur.Paras = paras;
            ur.Save();
            #endregion
        }
        public DataTable BindDG()
        {
            #region 校验选择项目。
            if (this.DDL_V1.SelectedIndex == this.DDL_V2.SelectedIndex)
            {
                if (this.DDL_V1.SelectedIndex == 0)
                    this.DDL_V2.SelectedIndex = 1;
                else
                    this.DDL_V2.SelectedIndex = 0;
            }

            if (this.DDL_Group.SelectedIndex == this.DDL_D.SelectedIndex)
            {
                if (this.DDL_Group.SelectedIndex == 0)
                    this.DDL_D.SelectedIndex = 1;
                else
                    this.DDL_D.SelectedIndex = 0;
            }
            this.SaveState();
            #endregion 校验选择项目。


            Attrs attrs = this.HisMD.AttrsInTableEns;
            Attr d1 = attrs.GetAttrByKey(this.DDL_D.SelectedItemStringVal);
            Attr dNum = attrs.GetAttrByKey(this.DDL_Num.SelectedItemStringVal);


            string v1 = this.DDL_V1.SelectedItemStringVal;
            string v2 = this.DDL_V2.SelectedItemStringVal;
            string groupAttr = this.DDL_Group.SelectedItemStringVal;
            string fxField = this.DDL_FXWay.SelectedItemStringVal + "(" + dNum.Key + ")";

            string sql1 = "SELECT " + groupAttr + "," + d1.Key + "," + fxField + " FROM " + this.HisMD.No + " WHERE " + d1.Field + "='" + this.DDL_V1.SelectedItemStringVal + "'  GROUP BY " + groupAttr + "," + d1.Key;
            string sql2 = "SELECT " + groupAttr + "," + d1.Key + "," + fxField + " FROM " + this.HisMD.No + " WHERE " + d1.Field + "='" + this.DDL_V2.SelectedItemStringVal + "' GROUP BY " + groupAttr + "," + d1.Key;

            //   throw new Exception(sql1);

            DataTable dt1 = DBAccess.RunSQLReturnTable(sql1);
            DataTable dt2 = DBAccess.RunSQLReturnTable(sql2);
            this.Bind(dt1, dt2, "sss");
            return null;

            SysEnums sesD1 = null;
            Entities ensD1 = null;
            SysEnums sesD2 = null;
            Entities ensD2 = null;
            Map map = this.HisMD.HisEn.EnMap;

            #region 生成两个纬度报表
            Attr attrD1 = attrs.GetAttrByKey(this.DDL_V1.SelectedItemStringVal);
            if (attrD1.IsEnum)
                sesD1 = new SysEnums(attrD1.UIBindKey);
            else
                ensD1 = attrD1.HisFKEns;

            Attr attrD2 = attrs.GetAttrByKey(this.DDL_V2.SelectedItemStringVal);
            if (attrD2.IsEnum)
                sesD2 = new SysEnums(attrD2.UIBindKey);
            else
                ensD2 = attrD2.HisFKEns;
            #endregion


            #region 生成执行的原始sql
            string Condition = ""; //处理特殊字段的条件问题。
            Paras myps = new BP.DA.Paras();
            string sql = "SELECT " + attrD1.Key + "," + attrD2.Key + ", " + this.DDL_FXWay.SelectedItemStringVal + "(" + this.DDL_Num.SelectedItemStringVal + ") FROM " + map.PhysicsTable;
            // 找到 WHERE 数据。
            string where = " WHERE ";
            string whereOfLJ = " WHERE "; // 累计的where.
            string url = "";
            foreach (Control item in this.ToolBar1.Controls)
            {
                if (item.ID == null)
                    continue;
                if (item.ID.IndexOf("DDL_") == -1)
                    continue;
                if (item.ID.IndexOf("DDL_Form_") == 0 || item.ID.IndexOf("DDL_To_") == 0)
                    continue;

                string key = item.ID.Substring("DDL_".Length);
                DDL ddl = (DDL)item;
                if (ddl.SelectedItemStringVal == "all")
                    continue;

                string val = ddl.SelectedItemStringVal;
                if (val == null)
                    continue;

                if (val == "mvals")
                {
                    UserRegedit sUr = new UserRegedit();
                    sUr.MyPK = WebUser.No + this.EnsName + "_SearchAttrs";
                    sUr.RetrieveFromDBSources();

                    /* 如果是多选值 */
                    string cfgVal = sUr.MVals;
                    AtPara ap = new AtPara(cfgVal);
                    string instr = ap.GetValStrByKey(key);
                    if (instr == null || instr == "")
                    {
                        if (key == "FK_Dept" || key == "FK_Unit")
                        {
                            if (key == "FK_Dept")
                            {
                                val = WebUser.FK_Dept;
                                ddl.SelectedIndex = 0;
                            }

                            if (key == "FK_Unit")
                            {
                                //   val = WebUser.FK_Unit;
                                ddl.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        instr = instr.Replace("..", ".");
                        instr = instr.Replace(".", "','");
                        instr = instr.Substring(2);
                        instr = instr.Substring(0, instr.Length - 2);
                        where += " " + key + " IN (" + instr + ")  AND ";
                        continue;
                    }
                }

                if (key == "FK_Dept")
                {
                    if (val.Length == 8)
                    {
                        where += " FK_Dept =" + SystemConfig.AppCenterDBVarStr + "V_Dept    AND ";
                    }
                    else
                    {
                        switch (SystemConfig.AppCenterDBType)
                        {
                            case DBType.Oracle:
                            case DBType.Informix:
                                where += " FK_Dept LIKE '%'||:V_Dept||'%'   AND ";
                                break;
                            case DBType.MSSQL:
                            default:
                                where += " FK_Dept LIKE  " + SystemConfig.AppCenterDBVarStr + "V_Dept+'%'   AND ";
                                break;
                        }
                    }
                    myps.Add("V_Dept", val);
                }
                else
                {
                    where += " " + key + " =" + SystemConfig.AppCenterDBVarStr + key + "   AND ";
                    if (key != "FK_NY")
                        whereOfLJ += " " + key + " =" + SystemConfig.AppCenterDBVarStr + key + "   AND ";

                    myps.Add(key, val);
                }
            }
            #endregion

            #region 加上 where like 条件
            try
            {
                string key = this.ToolBar1.GetTBByID("TB_Key").Text.Trim();
                if (key.Length > 1)
                {
                    string whereLike = "";

                    bool isAddAnd = false;
                    foreach (Attr likeKey in attrs)
                    {
                        if (likeKey.IsNum)
                            continue;
                        if (likeKey.IsRefAttr)
                            continue;

                        switch (likeKey.Field)
                        {
                            case "MyFileExt":
                            case "MyFilePath":
                            case "WebPath":
                                continue;
                            default:
                                break;
                        }


                        if (isAddAnd == false)
                        {
                            isAddAnd = true;
                            whereLike += "      " + likeKey.Field + " LIKE '%" + key + "%' ";
                        }
                        else
                        {
                            whereLike += "   AND   " + likeKey.Field + " LIKE '%" + key + "%'";
                        }
                    }
                    whereLike += "          ";
                    where += whereLike;
                }
            }
            catch
            {
            }
            #endregion

            #region 加上日期时间段.
            if (map.DTSearchWay != DTSearchWay.None)
            {
                string dtFrom = this.ToolBar1.GetTBByID("TB_S_From").Text.Trim();
                string dtTo = this.ToolBar1.GetTBByID("TB_S_To").Text.Trim();
                string field = map.DTSearchKey;
                if (map.DTSearchWay == DTSearchWay.ByDate)
                {
                    where += "( " + field + ">='" + dtFrom + " 01:01' AND " + field + "<='" + dtTo + " 23:59')     ";
                }
                else
                {
                    where += "(";
                    where += field + " >='" + dtFrom + "' AND " + field + "<='" + dtTo + "'";
                    where += ")";
                }
            }
            if (where == " WHERE ")
            {
                where = "" + Condition.Replace("and", "");
                whereOfLJ = "" + Condition.Replace("and", "");
            }
            else
            {
                where = where.Substring(0, where.Length - " AND ".Length) + Condition;
                whereOfLJ = whereOfLJ.Substring(0, whereOfLJ.Length - " AND ".Length) + Condition;
            }
            #endregion

            sql += where + " GROUP BY  " + attrD1.Key + "," + attrD2.Key;
            myps.SQL = sql;
            DataTable dt = DBAccess.RunSQLReturnTable(myps);


            return null;
        }
        public void Bind(DataTable dt1, DataTable dt2, string url)
        {
            string key = this.DDL_Group.SelectedItemStringVal;
            Attr attr = this.HisMD.HisEn.EnMap.GetAttrByKey(key);
            Entities ensOfGroup = attr.HisFKEns;
            ensOfGroup.RetrieveAll();

            string str = "";
            str += "<Table style='border-collapse: collapse' bordercolor='#111111'  >";
            str += "<TR>";
            str += "  <TD warp=false class='GroupTitle' nowrap >序</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >" + this.DDL_D.SelectedItem.Text + "</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >" + this.DDL_V1.SelectedItem.Text + "</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >" + this.DDL_V2.SelectedItem.Text + "</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >降低值</TD>";
            str += "  <TD warp=false class='GroupTitle' nowrap >降低比例%</TD>";
            str += "</TR>";

            int idx = 0;
            foreach (Entity en in ensOfGroup)
            {
                bool isHave = false;
                foreach (DataRow dr in dt1.Rows)
                {
                    string kv = dr[0].ToString();
                    if (en.GetValStringByKey(attr.UIRefKeyValue) == kv)
                    {
                        isHave = true;
                        break;
                    }
                }
                if (isHave == false)
                    continue;  // 不存在这个值 就continue;

                idx++;
                str += "<TR >";
                str += "  <TD warp=false class='Idx' nowrap >" + idx.ToString() + "</TD>";
                str += "  <TD warp=false class='TD' nowrap >" + en.GetValStringByKey(attr.UIRefKeyText) + "</TD>";
                decimal num1 = 0;
                decimal num2 = 0;
                foreach (DataRow dr1 in dt1.Rows)
                {
                    string kv = dr1[0].ToString();  //循环到值 。
                    if (en.GetValStringByKey(attr.UIRefKeyValue) != kv)
                        continue;

                    num1 = decimal.Parse(dr1[2].ToString());
                    num2 = 0;
                    str += "  <TD warp=false class='TDNum' nowrap ><a href=\"javascript:WinOpen('" + url + "&" + this.DDL_D.SelectedItemStringVal + "=" + dr1[0].ToString() + "&" + this.DDL_Num.SelectedItemStringVal + "=" + this.DDL_V1.SelectedItemStringVal + "')\"  >" + dr1[2].ToString() + "</a></TD>"; // 时间段1值.
                    isHave = false;
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        num2 = decimal.Parse(dr2[2].ToString());
                        if (dr2[0].ToString() == en.GetValStringByKey(attr.UIRefKeyValue))
                        {
                            isHave = true;
                            decimal cz = num1 - num2;
                            str += "  <TD warp=false class='TDNum' nowrap ><a href=\"javascript:WinOpen('" + url + "&" + this.DDL_D.SelectedItemStringVal + "=" + dr2[0].ToString() + "&" + this.DDL_Num.SelectedItemStringVal + "=" + this.DDL_V2.SelectedItemStringVal + "')\"  >" + num2.ToString() + "</a></TD>"; // 时间段1值.
                            str += "  <TD warp=false class='TDNum' nowrap >" + cz + "</TD>"; // 降低数.
                            break;
                        }
                    }
                    if (isHave == false)
                    {
                        num2 = 0;
                        str += "  <TD warp=false class='TDNum' nowrap >0</TD>"; // 时间段1值.
                        str += "  <TD warp=false class='TDNum' nowrap >" + num1 + "</TD>"; // 降低数.
                    }
                }
                if (num1 == 0)
                {
                    str += "  <TD warp=false class='TDNum' nowrap >0.00</TD>";
                }
                else
                {
                    decimal fz = decimal.Parse(num1.ToString()) - decimal.Parse(num2.ToString());
                    decimal fm = decimal.Parse(num1.ToString());
                    decimal rate = fz / fm * 100;
                    str += "  <TD warp=false class='TDNum' nowrap >" + rate.ToString("0.00") + "</TD>";
                }
                str += "</TR>";
            }
            str += "</Table>";
            this.Right.Add(str);
        }

        void cb_CheckedChanged(object sender, EventArgs e)
        {
            this.BindDG();
        }
        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL ddl = sender as DDL;
            if (ddl != null && ddl.ID == "DDL_D")
            {
                this.SaveState();
                this.Response.Redirect("Contrast.aspx?FK_Flow=" + this.FK_Flow + "&DoType=" + this.DoType, true);
                return;
            }
            this.BindDG();
        }
        void ToolBar1_ButtonClick(object sender, EventArgs e)
        {
            Btn btn = (Btn)sender;
            switch (btn.ID)
            {
                case NamesOfBtn.Help:
                    break;
                case NamesOfBtn.Excel:
                    DataTable dt = this.BindDG();
                    // this.ExportDGToExcel(this.DealTable(dt), this.HisEns.GetNewEntity.EnDesc);
                    return;
                default:
                    this.ToolBar1.SaveSearchState(this.EnsName, this.Key);
                    this.BindDG();
                    return;
            }
        }
    }
}