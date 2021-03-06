using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BP.En;
using BP.DA;
using BP.Sys;

namespace BP.WF
{
    /// <summary>
    ///  属性
    /// </summary>
    public class TrackAttr:EntityMyPKAttr
    {
        /// <summary>
        /// 记录日期
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        /// 完成日期
        /// </summary>
        public const string CDT = "CDT";
        /// <summary>
        /// FID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        /// WorkID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        /// 活动类型
        /// </summary>
        public const string ActionType = "ActionType";
        /// <summary>
        /// 活动类型名称
        /// </summary>
        public const string ActionTypeText = "ActionTypeText";
        /// <summary>
        /// 时间跨度
        /// </summary>
        public const string WorkTimeSpan = "WorkTimeSpan";
        /// <summary>
        /// 节点数据
        /// </summary>
        public const string NodeData = "NodeData";
        /// <summary>
        /// 轨迹字段
        /// </summary>
        public const string TrackFields = "TrackFields";
        /// <summary>
        /// 备注
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        /// 从节点
        /// </summary>
        public const string NDFrom = "NDFrom";
        /// <summary>
        /// 到节点
        /// </summary>
        public const string NDTo = "NDTo";
        /// <summary>
        /// 从人员
        /// </summary>
        public const string EmpFrom = "EmpFrom";
        /// <summary>
        /// 到人员
        /// </summary>
        public const string EmpTo = "EmpTo";
        /// <summary>
        /// 审核
        /// </summary>
        public const string Msg = "Msg";
        /// <summary>
        /// EmpFromT
        /// </summary>
        public const string EmpFromT = "EmpFromT";
        /// <summary>
        /// NDFromT
        /// </summary>
        public const string NDFromT = "NDFromT";
        /// <summary>
        /// NDToT
        /// </summary>
        public const string NDToT = "NDToT";
        /// <summary>
        /// EmpToT
        /// </summary>
        public const string EmpToT = "EmpToT";
        /// <summary>
        /// 实际执行人员
        /// </summary>
        public const string Exer = "Exer";
    }
    /// <summary>
    /// 轨迹
    /// </summary>
    public class Track : BP.En.Entity
    {
        public override string PK
        {
            get
            {
                return "MyPK";
            }
        }

        public override string PKField
        {
            get
            {
                return "MyPK";
            }
        }

        #region attrs
        /// <summary>
        /// 节点从
        /// </summary>
        public int NDFrom
        {
            get
            {
                return this.GetValIntByKey(TrackAttr.NDFrom);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDFrom, value);
            }
        }
        /// <summary>
        /// 节点到
        /// </summary>
        public int NDTo
        {
            get
            {
                return this.GetValIntByKey(TrackAttr.NDTo);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDTo, value);
            }
        }
        /// <summary>
        /// 从人员
        /// </summary>
        public string EmpFrom
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpFrom);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpFrom, value);
            }
        }
        /// <summary>
        /// 到人员
        /// </summary>
        public string EmpTo
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpTo);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpTo, value);
            }
        }
        /// <summary>
        /// 记录日期
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.RDT);
            }
            set
            {
                this.SetValByKey(TrackAttr.RDT, value);
            }
        }
        /// <summary>
        /// fid
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(TrackAttr.FID);
            }
            set
            {
                this.SetValByKey(TrackAttr.FID, value);
            }
        }
        /// <summary>
        /// Workid
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(TrackAttr.WorkID);
            }
            set
            {
                this.SetValByKey(TrackAttr.WorkID, value);
            }
        }
        /// <summary>
        /// 活动类型
        /// </summary>
        public ActionType HisActionType
        {
            get
            {
                return (ActionType)this.GetValIntByKey(TrackAttr.ActionType);
            }
            set
            {
                this.SetValByKey(TrackAttr.ActionType, (int)value);
            }
        }
        public static string GetActionTypeT(ActionType at)
        {
            switch (at)
            {
                case ActionType.Forward:
                    return "前进";
                case ActionType.Return:
                    return "退回";
                case ActionType.Shift:
                    return "移交";
                case ActionType.UnShift:
                    return "撤消移交";
                case ActionType.Start:
                    return "发起";
                case ActionType.UnSend:
                    return "撤消发起";
                case ActionType.ForwardFL:
                    return " -前进(分流点)";
                case ActionType.ForwardHL:
                    return " -向合流点发送";
                case ActionType.FlowOver:
                    return "流程结束";
                case ActionType.CallSubFlow:
                    return "调用起子流程";
                case ActionType.StartSubFlow:
                    return "子流程发起";
                case ActionType.SubFlowForward:
                    return "子流程前进";
                case ActionType.RebackOverFlow:
                    return "恢复已完成的流程";
                case ActionType.FlowOverByCoercion:
                    return "强制结束流程";
                case ActionType.HungUp:
                    return "挂起";
                case ActionType.UnHungUp:
                    return "取消挂起";
                case ActionType.Press:
                    return "催办";
                case ActionType.CC:
                    return "抄送";
                default:
                    return "未知";
            }
        }
        public string ActionTypeText
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.ActionTypeText);
            }
            set
            {
                this.SetValByKey(TrackAttr.ActionTypeText, value);
            }
        }
        /// <summary>
        /// 节点数据
        /// </summary>
        public string NodeData
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.NodeData);
            }
            set
            {
                this.SetValByKey(TrackAttr.NodeData, value);
            }
        }
        public string Exer
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.Exer);
            }
            set
            {
                this.SetValByKey(TrackAttr.Exer, value);
            }
        }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Msg
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.Msg);
            }
            set
            {
                this.SetValByKey(TrackAttr.Msg, value);
            }
        }
        public string MsgHtml
        {
            get
            {
                return this.GetValHtmlStringByKey(TrackAttr.Msg);
            }
        }
        public string EmpToT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpToT);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpToT, value);
            }
        }
        public string EmpFromT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpFromT);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpFromT, value);
            }
        }

        public string NDFromT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.NDFromT);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDFromT, value);
            }
        }
        public string NDToT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.NDToT);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDToT, value);
            }
        }
        #endregion attrs

        #region 属性
        public string RptName = null;
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();

                #region 基本属性
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN); //要连接的数据源（表示要连接到的那个系统数据库）。
                map.PhysicsTable = "WF_Track"; // 要物理表。
                map.EnDesc = "轨迹表";
                map.EnType = EnType.App;
                #endregion

                #region 字段

                //增加一个自动增长的列.
               map.AddTBIntPK(TrackAttr.MyPK, 0, "MyPK", true, false);

                map.AddTBInt(TrackAttr.ActionType, 0, "类型", true, false);
                map.AddTBString(TrackAttr.ActionTypeText, null, "类型(名称)", true, false, 0, 30, 100);

                map.AddTBInt(TrackAttr.FID, 0, "流程ID", true, false);
                map.AddTBInt(TrackAttr.WorkID, 0, "工作ID", true, false);


                map.AddTBInt(TrackAttr.NDFrom, 0, "从节点", true, false);
                map.AddTBString(TrackAttr.NDFromT, null, "从节点(名称)", true, false, 0, 100, 100);

                map.AddTBInt(TrackAttr.NDTo, 0, "到节点", true, false);
                map.AddTBString(TrackAttr.NDToT, null, "到节点(名称)", true, false, 0, 100, 100);

                map.AddTBString(TrackAttr.EmpFrom, null, "从人员", true, false, 0, 20, 100);
                map.AddTBString(TrackAttr.EmpFromT, null, "从人员(名称)", true, false, 0, 30, 100);

                map.AddTBString(TrackAttr.EmpTo, null, "到人员", true, false, 0, 20, 100);
                map.AddTBString(TrackAttr.EmpToT, null, "到人员(名称)", true, false, 0, 30, 100);

                map.AddTBString(TrackAttr.RDT, null, "日期", true, false, 0, 20, 100);

                map.AddTBFloat(TrackAttr.WorkTimeSpan, 0, "时间跨度(天)", true, false);
                map.AddTBStringDoc(TrackAttr.Msg, null, "消息", true, false);
                map.AddTBStringDoc(TrackAttr.NodeData, null, "节点数据(日志信息)", true, false);

                map.AddTBString(TrackAttr.Exer, null, "执行人", true, false, 0, 20, 100);
                #endregion 字段

                this._enMap = map;
                return this._enMap;
            }
        }
        ///// <summary>
        ///// 轨迹
        ///// </summary>
        ///// <param name="rptName"></param>
        //public Track(string mypk)
        //{
        //    this.MyPK = mypk;
        //    if (this.RetrieveFromDBSources() == 0)
        //    {
        //        TrackTemp t = new TrackTemp(this.MyPK);
        //        this.Row = t.Row;
        //    }
        //}

        public string FK_Flow = null;
        /// <summary>
        /// 轨迹
        /// </summary>
        public Track()
        {
        }
        /// <summary>
        /// 轨迹
        /// </summary>
        /// <param name="flowNo">流程编号</param>
        /// <param name="mypk">主键</param>
        public Track(string flowNo,string mypk)
        {
            string sql = "SELECT * FROM ND" + int.Parse(flowNo) + "Track WHERE MyPK='"+mypk+"'";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@日志数据丢失.."+sql);
            this.Row.LoadDataTable(dt, dt.Rows[0]);
        }
        /// <summary>
        /// 创建track.
        /// </summary>
        /// <param name="fk_flow">流程编号</param>
        public static void CreateTrackTable(string fk_flow)
        {
            string ptable = "ND" + int.Parse(fk_flow) + "Track";
            if (BP.DA.DBAccess.IsExitsObject(ptable))
                return;

            try
            {
                /*如果不存在指定的表,就创建它.*/
                BP.DA.DBAccess.RunSQL("DROP TABLE WF_Track");
            }
            catch
            {
            }

            Track tk = new Track();
            tk.CheckPhysicsTable();

            string sqlRename = "";
            switch (SystemConfig.AppCenterDBType)
            {
                case DBType.MSSQL:
                    sqlRename = "EXEC SP_RENAME WF_Track, " + ptable;
                    break;
                case DBType.Informix:
                    sqlRename = "RENAME TABLE WF_Track TO " + ptable;
                    break;
                case DBType.Oracle:
                    sqlRename = "ALTER TABLE WF_Track rename to " + ptable;
                    break;
                case DBType.MySQL:
                    sqlRename = "ALTER TABLE WF_Track rename to " + ptable;
                    break;
                default:
                    throw new Exception("未涉及到此类型.");
            }
            DBAccess.RunSQL(sqlRename);
        }
      
        /// <summary>
        /// 增加授权人
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            if (BP.Web.WebUser.IsAuthorize)
                this.Exer = BP.Web.WebUser.AuthorizerEmpID + "," + BP.Web.WebUser.Auth;
            else
                this.Exer = BP.Web.WebUser.No + "," + BP.Web.WebUser.Name;

            string ptable = "ND" + int.Parse(this.FK_Flow) + "Track";

            string dbstr = SystemConfig.AppCenterDBVarStr;

            string sql = "INSERT INTO " + ptable;
            sql += "(";
            sql += "" + TrackAttr.MyPK + ",";
            sql += "" + TrackAttr.ActionType + ",";
            sql += "" + TrackAttr.ActionTypeText + ",";
            sql += "" + TrackAttr.FID + ",";
            sql += "" + TrackAttr.WorkID + ",";
            sql += "" + TrackAttr.NDFrom + ",";
            sql += "" + TrackAttr.NDFromT + ",";
            sql += "" + TrackAttr.NDTo + ",";
            sql += "" + TrackAttr.NDToT + ",";

            sql += "" + TrackAttr.EmpFrom + ",";
            sql += "" + TrackAttr.EmpFromT + ",";
            sql += "" + TrackAttr.EmpTo + ",";
            sql += "" + TrackAttr.EmpToT + ",";
            sql += "" + TrackAttr.RDT + ",";
            sql += "" + TrackAttr.WorkTimeSpan + ",";
            sql += "" + TrackAttr.Msg + ",";
            sql += "" + TrackAttr.NodeData + ",";
            sql += "" + TrackAttr.Exer + "";
            sql += ") VALUES (";
            sql += dbstr + TrackAttr.MyPK + ",";
            sql += dbstr + TrackAttr.ActionType + ",";
            sql += dbstr + TrackAttr.ActionTypeText + ",";
            sql += dbstr + TrackAttr.FID + ",";
            sql += dbstr + TrackAttr.WorkID + ",";
            sql += dbstr + TrackAttr.NDFrom + ",";
            sql += dbstr + TrackAttr.NDFromT + ",";
            sql += dbstr + TrackAttr.NDTo + ",";
            sql += dbstr + TrackAttr.NDToT + ",";
            sql += dbstr + TrackAttr.EmpFrom + ",";
            sql += dbstr + TrackAttr.EmpFromT + ",";
            sql += dbstr + TrackAttr.EmpTo + ",";
            sql += dbstr + TrackAttr.EmpToT + ",";
            sql += dbstr + TrackAttr.RDT + ",";
            sql += dbstr + TrackAttr.WorkTimeSpan + ",";
            sql += dbstr + TrackAttr.Msg + ",";
            sql += dbstr + TrackAttr.NodeData + ",";
            sql += dbstr + TrackAttr.Exer + "";
            sql += ")";

            this.ActionTypeText = Track.GetActionTypeT(this.HisActionType);
            this.SetValByKey(TrackAttr.MyPK, DBAccess.GenerOIDByGUID());

            #region 执行保存
            try
            {
                Paras ps = SqlBuilder.GenerParas(this, null);
                ps.SQL = sql;
               // ps.Remove("MyPK");

                switch (SystemConfig.AppCenterDBType)
                {
                    case DBType.MSSQL:
                        this.RunSQL(ps);
                        break;
                    case DBType.Access:
                        this.RunSQL(ps);
                        break;
                    case DBType.MySQL:
                    case DBType.Informix:
                    default:
                        ps.SQL = ps.SQL.Replace("[", "").Replace("]", "");
                        this.RunSQL(ps); // 运行sql.
                        //  this.RunSQL(sql.Replace("[", "").Replace("]", ""), SqlBuilder.GenerParas(this, null));
                        break;
                }
            }
            catch (Exception ex)
            {
                // 写入日志.
                Log.DefaultLogWriteLineError(ex.Message);

                //创建track.
                Track.CreateTrackTable(this.FK_Flow);
                throw ex;
            }
            #endregion 执行保存

            //就不让它在执行insert了。
            return false;
        }
        #endregion attrs
    }
    /// <summary>
    /// 轨迹集合
    /// </summary>
    public class Tracks : BP.En.Entities
    {
        /// <summary>
        /// 轨迹集合
        /// </summary>
        public Tracks()
        {
        }
        public override Entity GetNewEntity
        {
            get
            {
                return new Track();
            }
        }
    }
}
