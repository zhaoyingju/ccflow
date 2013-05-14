using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.WF
{
    /// <summary>
    /// 计算的数据来源
    /// </summary>
    public enum ConnDataFrom
    {
        /// <summary>
        /// 表单数据
        /// </summary>
        Form,
        /// <summary>
        /// 岗位数据
        /// </summary>
        Stas,
        /// <summary>
        /// Depts
        /// </summary>
        Depts,
        /// <summary>
        /// 按sql计算.
        /// </summary>
        SQL,
        /// <summary>
        /// 按参数
        /// </summary>
        Paras
    }
  
    /// <summary>
    /// 条件属性
    /// </summary>
    public class CondAttr
    {
        /// <summary>
        /// 数据来源
        /// </summary>
        public const string DataFrom = "DataFrom";
        /// <summary>
        /// 属性Key
        /// </summary>
        public const string NodeID = "NodeID";
        /// <summary>
        /// 属性Key
        /// </summary>
        public const string AttrKey = "AttrKey";
        /// <summary>
        /// 名称
        /// </summary>
        public const string AttrName = "AttrName";
        /// <summary>
        /// 属性
        /// </summary>
        public const string FK_Attr = "FK_Attr";
        /// <summary>
        /// 运算符号
        /// </summary>
        public const string FK_Operator = "FK_Operator";
        /// <summary>
        /// 运算的值
        /// </summary>
        public const string OperatorValue = "OperatorValue";
        /// <summary>
        /// 操作值
        /// </summary>
        public const string OperatorValueT = "OperatorValueT";
        /// <summary>
        /// Node
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        /// 条件类型
        /// </summary>
        public const string CondType = "CondType";
        /// <summary>
        /// 流程
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// 对方向条件有效
        /// </summary>
        public const string ToNodeID = "ToNodeID";
        /// <summary>
        /// 判断方式
        /// </summary>
        public const string ConnJudgeWay = "ConnJudgeWay";
        /// <summary>
        /// MyPOID
        /// </summary>
        public const string MyPOID = "MyPOID";
        /// <summary>
        /// PRI
        /// </summary>
        public const string PRI = "PRI";
    }
    /// <summary>
    /// 条件类型
    /// </summary>
    public enum CondType
    {
        /// <summary>
        /// 节点完成条件
        /// </summary>
        Node,
        /// <summary>
        /// 流程条件
        /// </summary>
        Flow,
        /// <summary>
        /// 方向条件
        /// </summary>
        Dir
    }
    /// <summary>
    /// 条件
    /// </summary>
    public class Cond : EntityMyPK
    {
        public Work en = null;
        /// <summary>
        /// 数据来源
        /// </summary>
        public ConnDataFrom HisDataFrom
        {
            get
            {
                return (ConnDataFrom)this.GetValIntByKey(CondAttr.DataFrom);
            }
            set
            {
                this.SetValByKey(CondAttr.DataFrom, (int)value);
            }
        }
        /// <summary>
        /// 流程编号
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(CondAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(CondAttr.FK_Flow, value);
            }
        }
        /// <summary>
        /// 条件类型(表单条件，岗位条件，部门条件，开发者参数)
        /// </summary>
        public CondType HisCondType
        {
            get
            {
                return (CondType)this.GetValIntByKey(CondAttr.CondType);
            }
            set
            {
                this.SetValByKey(CondAttr.CondType, (int)value);
            }
        }
        /// <summary>
        /// 要运算的节点
        /// </summary>
        public Node HisNode
        {
            get
            {
                return new Node(this.NodeID);
            }
        }
        /// <summary>
        /// 优先级
        /// </summary>
        public int PRI
        {
            get
            {
                return this.GetValIntByKey(CondAttr.PRI);
            }
            set
            {
                this.SetValByKey(CondAttr.PRI, value);
            }
        }
        /// <summary>
        /// MyPOID
        /// </summary>
        public int MyPOID
        {
            get
            {
                return this.GetValIntByKey(CondAttr.MyPOID);
            }
            set
            {
                this.SetValByKey(CondAttr.MyPOID, value);
            }
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(CondAttr.NodeID);
            }
            set
            {
                this.SetValByKey(CondAttr.NodeID, value);
            }
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                int i = this.GetValIntByKey(CondAttr.FK_Node);
                if (i == 0)
                    return this.NodeID;
                return i;
            }
            set
            {
                this.SetValByKey(CondAttr.FK_Node, value);
            }
        }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string FK_NodeT
        {
            get
            {
                Node nd = new Node(this.FK_Node);
                return nd.Name;
            }
        }
        /// <summary>
        /// 对方向条件有效
        /// </summary>
        public int ToNodeID
        {
            get
            {
                return this.GetValIntByKey(CondAttr.ToNodeID);
            }
            set
            {
                this.SetValByKey(CondAttr.ToNodeID, value);
            }
        }

        /// <summary>
        /// 在更新与插入之前要做得操作。
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdateInsertAction()
        {
            this.RunSQL("UPDATE WF_Node SET IsCCNode=0,IsCCFlow=0");
            this.RunSQL("UPDATE WF_Node SET IsCCNode=1 WHERE NodeID IN (SELECT NodeID FROM WF_Cond WHERE CondType=" + (int)CondType.Node + ")");
            this.RunSQL("UPDATE WF_Node SET IsCCFlow=1 WHERE NodeID IN (SELECT NodeID FROM WF_Cond WHERE CondType=" + (int)CondType.Flow + ")");

            this.MyPOID = BP.DA.DBAccess.GenerOID();
            return base.beforeUpdateInsertAction();
        }

        #region 实现基本的方方法
        /// <summary>
        /// 属性
        /// </summary>
        public string FK_Attr
        {
            get
            {
                return this.GetValStringByKey(CondAttr.FK_Attr);
            }
            set
            {
                if (value == null)
                    throw new Exception("FK_Attr不能设置为null");

                value = value.Trim();

                this.SetValByKey(CondAttr.FK_Attr, value);

                BP.Sys.MapAttr attr = new BP.Sys.MapAttr(value);
                this.SetValByKey(CondAttr.AttrKey, attr.KeyOfEn);
                this.SetValByKey(CondAttr.AttrName, attr.Name);
            }
        }
        /// <summary>
        /// 要运算的实体属性
        /// </summary>
        public string AttrKey
        {
            get
            {
                return this.GetValStringByKey(CondAttr.AttrKey);
            }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttrName
        {
            get
            {
                return this.GetValStringByKey(CondAttr.AttrName);
            }
        }
        /// <summary>
        /// 操作的值
        /// </summary>
        public string OperatorValueT
        {
            get
            {
                return this.GetValStringByKey(CondAttr.OperatorValueT);
            }
            set
            {
                this.SetValByKey(CondAttr.OperatorValueT, value);
            }
        }
        /// <summary>
        /// 运算符号
        /// </summary>
        public string FK_Operator
        {
            get
            {
                string s = this.GetValStringByKey(CondAttr.FK_Operator);
                if (s == null || s == "")
                    return "=";
                return s;
            }
            set
            {
                this.SetValByKey(CondAttr.FK_Operator, value);
            }
        }
        /// <summary>
        /// 运算值
        /// </summary>
        public object OperatorValue
        {
            get
            {
                return this.GetValStringByKey(CondAttr.OperatorValue);
            }
            set
            {
                this.SetValByKey(CondAttr.OperatorValue, value as string);
            }
        }
        /// <summary>
        /// 操作值Str
        /// </summary>
        public string OperatorValueStr
        {
            get
            {
                string sql= this.GetValStringByKey(CondAttr.OperatorValue);
                sql = sql.Replace("~", "'");
                return sql;
            }
        }
        /// <summary>
        /// 操作值int
        /// </summary>
        public int OperatorValueInt
        {
            get
            {
                return this.GetValIntByKey(CondAttr.OperatorValue);
            }
        }
        /// <summary>
        /// 操作值boolen
        /// </summary>
        public bool OperatorValueBool
        {
            get
            {
                return this.GetValBooleanByKey(CondAttr.OperatorValue);
            }
        }
        /// <summary>
        /// workid
        /// </summary>
        private Int64 _WorkID = 0;
        public Int64 WorkID
        {
            get
            {
                return _WorkID;
            }
            set
            {
                _WorkID = value;
            }
        }
        /// <summary>
        /// 条件消息
        /// </summary>
        public string MsgOfCond = "";
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="fk_node">节点ID</param>
        public void DoUp(int fk_node)
        {
            int condtypeInt = (int)this.HisCondType;
            this.DoOrderUp(CondAttr.FK_Node, fk_node.ToString(),CondAttr.CondType, condtypeInt.ToString(), CondAttr.PRI);
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="fk_node">节点ID</param>
        public void DoDown(int fk_node)
        {
            int condtypeInt = (int)this.HisCondType;
            this.DoOrderDown(CondAttr.FK_Node, fk_node.ToString(), CondAttr.CondType, condtypeInt.ToString(), CondAttr.PRI);
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 条件
        /// </summary>
        public Cond() { }
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="mypk"></param>
        public Cond(string mypk)
        {
            this.MyPK = mypk;
            this.Retrieve();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 这个条件能不能通过
        /// </summary>
        public virtual bool IsPassed
        {
            get
            {
                Node nd = new Node(this.FK_Node);
                if (this.en == null)
                {
                    Work en = nd.HisWork;
                    try
                    {
                        en.SetValByKey("OID", this.WorkID);
                        en.Retrieve();
                        this.en = en;
                    }
                    catch (Exception ex)
                    {
                        //this.Delete();
                        return false;
                        //throw new Exception("@在取得判断条件实体[" + nd.EnDesc + "], 出现错误:" + ex.Message + "@错误原因是定义流程的判断条件出现错误,可能是你选择的判断条件工作类是当前工作节点的下一步工作造成,取不到该实体的实例.");
                    }
                }

                if (this.HisDataFrom == ConnDataFrom.Stas)
                {
                    string strs = this.OperatorValue.ToString();
                    BP.Port.EmpStations sts = new BP.Port.EmpStations();
                    sts.Retrieve("FK_Emp",en.Rec);
                    string strs1 = "";
                    foreach (BP.Port.EmpStation st in sts)
                    {
                        if (strs.Contains("@" + st.FK_Station+"@"))
                        {
                            this.MsgOfCond = "@以岗位判断方向，条件为true：岗位集合" + strs + "，操作员(" + en.Rec + ")岗位:" + st.FK_Station + st.FK_StationT;
                            return true;
                        }
                        strs1 += st.FK_Station + "-" + st.FK_StationT;
                    }

                    this.MsgOfCond = "@以岗位判断方向，条件为false：岗位集合" + strs + "，操作员(" + en.Rec + ")岗位:" + strs1;
                    return false;
                }

                if (this.HisDataFrom == ConnDataFrom.Depts)
                {
                    string strs = this.OperatorValue.ToString();
                    BP.Port.EmpDepts sts = new BP.Port.EmpDepts();
                    sts.Retrieve("FK_Emp", en.Rec);
                    string strs1 = "";
                    foreach (BP.Port.EmpDept st in sts)
                    {
                        if (strs.Contains("@" + st.FK_Dept + "@"))
                        {
                            this.MsgOfCond = "@以岗位判断方向，条件为true：部门集合" + strs + "，操作员(" + en.Rec + ")部门:" + st.FK_Dept + st.FK_DeptT;
                            return true;
                        }
                        strs1 += st.FK_Dept + "-" + st.FK_DeptT;
                    }

                    this.MsgOfCond = "@以部门判断方向，条件为false：部门集合" + strs + "，操作员(" +  en.Rec+ ")部门:" + strs1;
                    return false;
                }

                if (this.HisDataFrom == ConnDataFrom.SQL)
                {
                    //this.MsgOfCond = "@以表单值判断方向，值 " + en.EnDesc + "." + this.AttrKey + " (" + en.GetValStringByKey(this.AttrKey) + ") 操作符:(" + this.FK_Operator + ") 判断值:(" + this.OperatorValue.ToString() + ")";
                    string sql = this.OperatorValueStr;
                    sql = sql.Replace("~", "'");
                    sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                    sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                    sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                    if (sql.Contains("@") == true)
                    {
                        /* 如果包含 @ */
                        foreach (Attr attr in this.en.EnMap.Attrs)
                        {
                            sql = sql.Replace("@" + attr.Key, en.GetValStrByKey(attr.Key));
                        }
                    }

                    int result = DBAccess.RunSQLReturnValInt(sql, -1);
                    if (result == 0)
                        return false;

                    if (result >= 1)
                        return true;

                    throw new Exception("@您设置的sql返回值，不符合ccflow的要求，必须是0或大于等于1。");
                }

                if (this.HisDataFrom == ConnDataFrom.Paras)
                {
                    //this.MsgOfCond = "@以表单值判断方向，值 " + en.EnDesc + "." + this.AttrKey + " (" + en.GetValStringByKey(this.AttrKey) + ") 操作符:(" + this.FK_Operator + ") 判断值:(" + this.OperatorValue.ToString() + ")";
                    string exp = this.OperatorValueStr;
                    string[] strs = exp.Trim().Split(' ');

                    string key = strs[0].Trim();
                    string oper = strs[1].Trim();
                    string val = strs[2].Trim();
                    val = val.Replace("'", "");
                    val = val.Replace("%", "");
                    val = val.Replace("~", "");

                    BP.En.Row row = this.en.Row;
                    foreach (string item in row.Keys)
                    {
                        if (key != item.Trim())
                            continue;

                        string valPara = row[key].ToString();
                        if (oper == "=")
                        {
                            if (valPara == val)
                                return true;
                            else
                                return false;
                        }

                        if (oper.ToUpper() == "LIKE")
                        {
                            if (valPara.Contains(val))
                                return true;
                            else
                                return false;
                        }

                        if (oper == ">")
                        {
                            if (float.Parse(valPara) > float.Parse(val))
                                return true;
                            else
                                return false;
                        }
                        if (oper == ">=")
                        {
                            if (float.Parse(valPara) >= float.Parse(val))
                                return true;
                            else
                                return false;
                        }
                        if (oper == "<")
                        {
                            if (float.Parse(valPara) < float.Parse(val))
                                return true;
                            else
                                return false;
                        }
                        if (oper == "<=")
                        {
                            if (float.Parse(valPara) <= float.Parse(val))
                                return true;
                            else
                                return false;
                        }

                        if (oper == "!=")
                        {
                            if (float.Parse(valPara) != float.Parse(val))
                                return true;
                            else
                                return false;
                        }
                        throw new Exception("@参数格式错误:" + exp + " Key=" + key + " oper=" + oper + " Val=" + val);
                    }

                    throw new Exception("@判断条件时错误,没有找到对应的表达式:" + exp + " Key=" + key + " oper=" + oper + " Val=" + val);
                }

                try
                {
                    if (en.EnMap.Attrs.Contains(this.AttrKey) == false)
                        throw new Exception("判断条件方向出现错误：实体：" + nd.EnDesc + " 属性" + this.AttrKey + "已经不存在.");

                    this.MsgOfCond = "@以表单值判断方向，值 " + en.EnDesc + "." + this.AttrKey + " (" + en.GetValStringByKey(this.AttrKey) + ") 操作符:(" + this.FK_Operator + ") 判断值:(" + this.OperatorValue.ToString() + ")";

                    switch (this.FK_Operator.Trim().ToLower())
                    {
                        case "<>":
                            if (en.GetValStringByKey(this.AttrKey) != this.OperatorValue.ToString())
                                return true;
                            else
                                return false;
                        case "=":  // 如果是 = 
                            if (en.GetValStringByKey(this.AttrKey) == this.OperatorValue.ToString())
                                return true;
                            else
                                return false;
                        case ">":
                            if (en.GetValDoubleByKey(this.AttrKey) > Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case ">=":
                            if (en.GetValDoubleByKey(this.AttrKey) >= Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "<":
                            if (en.GetValDoubleByKey(this.AttrKey) < Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "<=":
                            if (en.GetValDoubleByKey(this.AttrKey) <= Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "!=":
                            if (en.GetValDoubleByKey(this.AttrKey) != Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "like":
                            if (en.GetValStringByKey(this.AttrKey).IndexOf(this.OperatorValue.ToString()) == -1)
                                return false;
                            else
                                return true;
                        default:
                            throw new Exception("@没有找到操作符号(" + this.FK_Operator.Trim().ToLower() + ").");
                    }
                }
                catch (Exception ex)
                {
                    Node nd23 = new Node(this.NodeID);
                    throw new Exception("@判断条件:Node=[" + this.NodeID + "," + nd23.EnDesc + "], 出现错误。@" + ex.Message + "。有可能您设置了非法的条件判断方式。");
                }
            }
        }
        /// <summary>
        /// 属性
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("WF_Cond");
                map.EnDesc = "流程条件";

                map.AddMyPK();
                map.AddTBInt(CondAttr.CondType, 0, "条件类型", true, true);
                //map.AddDDLSysEnum(CondAttr.CondType, 0, "条件类型", true, false, CondAttr.CondType,"@0=节点完成条件@1=流程完成条件@2=方向条件");

                map.AddTBInt(CondAttr.DataFrom, 0, "条件数据来源0表单,1岗位(对方向条件有效)", true, true);
                map.AddTBString(CondAttr.FK_Flow, null, "流程", true, true, 0, 60, 20);
                
                map.AddTBInt(CondAttr.NodeID, 0, "发生的事件MainNode", true, true);

                map.AddTBInt(CondAttr.FK_Node, 0, "节点ID", true, true);
                map.AddTBString(CondAttr.FK_Attr, null, "属性", true, true, 0, 80, 20);
                map.AddTBString(CondAttr.AttrKey, null, "属性键", true, true, 0, 60, 20);
                map.AddTBString(CondAttr.AttrName, null, "中文名称", true, true, 0, 500, 20);
                map.AddTBString(CondAttr.FK_Operator, "=", "运算符号", true, true, 0, 60, 20);
                map.AddTBString(CondAttr.OperatorValue, "", "要运算的值", true, true, 0, 4000, 20);
                map.AddTBString(CondAttr.OperatorValueT, "", "要运算的值T", true, true, 0, 4000, 20);
                map.AddTBInt(CondAttr.ToNodeID, 0, "ToNodeID（对方向条件有效）", true, true);
                map.AddDDLSysEnum(CondAttr.ConnJudgeWay, 0, "条件关系", true, false, CondAttr.ConnJudgeWay, "@0=or@1=and");
                map.AddTBInt(CondAttr.MyPOID, 0, "MyPOID", true, true);
                map.AddTBInt(CondAttr.PRI, 0, "计算优先级", true, true);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    /// 条件s
    /// </summary>
    public class Conds : Entities
    {
        #region 属性
        /// <summary>
        /// 获得Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get { return new Cond(); }
        }
        /// <summary>
        /// 在这里面的所有条件是不是都符合.
        /// </summary>
        public bool IsAllPassed
        {
            get
            {
                if (this.Count == 0)
                    throw new Exception("@没有要判断的集合.");

                foreach (Cond en in this)
                {
                    if (en.IsPassed == false)
                        return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 是否通过 (统一口径都是按and方式判断没有or的关系。)
        /// </summary>
        public bool IsPass
        {
            get
            {
                // 判断  and. 的关系。
                foreach (Cond en in this)
                {
                    if (en.IsPassed == false)
                        return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string MsgOfDesc
        {
            get
            {
                string msg = "";
                foreach (Cond c in this)
                {
                    msg += "@" + c.MsgOfCond;
                }
                return msg;
            }
        }
        /// <summary>
        /// 是不是其中的一个passed. 
        /// </summary>
        public bool IsOneOfCondPassed
        {
            get
            {
                foreach (Cond en in this)
                {
                    if (en.IsPassed == true)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 取出其中一个的完成条件。. 
        /// </summary>
        public Cond GetOneOfCondPassed
        {
            get
            {
                foreach (Cond en in this)
                {
                    if (en.IsPassed == true)
                        return en;
                }
                throw new Exception("@没有完成条件。");
            }
        }
        public int NodeID = 0;
        #endregion

        #region 构造
        /// <summary>
        /// 条件
        /// </summary>
        public Conds()
        {
        }
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="fk_flow">流程编号</param>
        public Conds(string fk_flow)
        {
            this.Retrieve(CondAttr.FK_Flow, fk_flow);
        }
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="ct">类型</param>
        /// <param name="nodeID">节点</param>
        public Conds(CondType ct, int nodeID, Int64 workid,Work enData)
        {
            this.NodeID = nodeID;
            this.Retrieve(CondAttr.NodeID, nodeID, CondAttr.CondType, (int)ct, CondAttr.PRI);
            foreach (Cond en in this)
            {
                en.WorkID = workid;
                en.en = enData;
            }
        }

        public string ConditionDesc
        {
            get
            {
                return "";
            }
        }
        #endregion
    }
}
