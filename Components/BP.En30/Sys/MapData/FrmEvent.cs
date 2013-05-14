using System;
using System.Text;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
using BP.Web;

namespace BP.Sys
{
    public enum EventDoType
    {
        /// <summary>
        /// ����
        /// </summary>
        Disable,
        /// <summary>
        /// ִ�д洢����
        /// </summary>
        SP,
        /// <summary>
        /// ����SQL
        /// </summary>
        SQL,
        /// <summary>
        /// �Զ���URL
        /// </summary>
        URLOfSelf,
        /// <summary>
        /// ϵͳ��url.
        /// </summary>
        URLOfSystem,
        /// <summary>
        /// �Զ���WS
        /// </summary>
        WSOfSelf,
        /// <summary>
        /// ϵͳ��WS
        /// </summary>
        WSOfSystem,
        /// <summary>
        /// EXE
        /// </summary>
        EXE,
        /// <summary>
        /// JS
        /// </summary>
        Javascript
    }
    public class FrmEventList
    {
        /// <summary>
        /// ����ǰ
        /// </summary>
        public const string FrmLoadBefore = "FrmLoadBefore";
        /// <summary>
        /// �����
        /// </summary>
        public const string FrmLoadAfter = "FrmLoadAfter";
        /// <summary>
        /// ����ǰ
        /// </summary>
        public const string SaveBefore = "SaveBefore";
        /// <summary>
        /// �����
        /// </summary>
        public const string SaveAfter = "SaveAfter";
    }
    /// <summary>
    /// �¼�����б�
    /// </summary>
    public class EventListOfNode : FrmEventList
    {
        #region �ڵ��¼�
        /// <summary>
        /// �ڵ㷢��ǰ
        /// </summary>
        public const string SendWhen = "SendWhen";
        /// <summary>
        /// �ڵ㷢�ͳɹ�
        /// </summary>
        public const string SendSuccess = "SendSuccess";
        /// <summary>
        /// �ڵ㷢��ʧ��
        /// </summary>
        public const string SendError = "SendError";
        /// <summary>
        /// ���ڵ��˻�ǰ
        /// </summary>
        public const string ReturnBefore = "ReturnBefore";
        /// <summary>
        /// ���ڵ��˺�
        /// </summary>
        public const string ReturnAfter = "ReturnAfter";
        /// <summary>
        /// ���ڵ㳷������ǰ
        /// </summary>
        public const string UndoneBefore = "UndoneBefore";
        /// <summary>
        /// ���ڵ㳷�����ͺ�
        /// </summary>
        public const string UndoneAfter = "UndoneAfter";
        #endregion �ڵ��¼�

        #region �����¼�
        /// <summary>
        /// �������ʱ
        /// </summary>
        public const string WhenFlowOver = "WhenFlowOver";
        /// <summary>
        /// ����ɾ��ǰ
        /// </summary>
        public const string BeforeFlowDel = "BeforeFlowDel";
        /// <summary>
        /// ����ɾ����
        /// </summary>
        public const string AfterFlowDel = "AfterFlowDel";
        #endregion �����¼�
    }
	/// <summary>
	/// �¼�����
	/// </summary>
    public class FrmEventAttr
    {
        /// <summary>
        /// �¼�����
        /// </summary>
        public const string FK_Event = "FK_Event";
        /// <summary>
        /// �ڵ�ID
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// ִ������
        /// </summary>
        public const string DoType = "DoType";
        /// <summary>
        /// ִ������
        /// </summary>
        public const string DoDoc = "DoDoc";
        /// <summary>
        /// ��ǩ
        /// </summary>
        public const string MsgOK = "MsgOK";
        /// <summary>
        /// ִ�д�����ʾ
        /// </summary>
        public const string MsgError = "MsgError";
    }
	/// <summary>
	/// �¼�
	/// �ڵ�Ľڵ㱣���¼������������.	 
	/// ��¼�˴�һ���ڵ㵽�����Ķ���ڵ�.
	/// Ҳ��¼�˵�����ڵ�������Ľڵ�.
	/// </summary>
    public class FrmEvent : EntityMyPK
    {
        #region ��������
        public override En.UAC HisUAC
        {
            get
            {
                UAC uac = new En.UAC();
                uac.IsAdjunct = false;
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        /// �ڵ�
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.FK_MapData, value);
            }
        }
        public string DoDoc
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.DoDoc).Replace("~", "'");
            }
            set
            {
                string doc = value.Replace("'", "~");
                this.SetValByKey(FrmEventAttr.DoDoc, doc);
            }
        }
        /// <summary>
        /// ִ�гɹ���ʾ
        /// </summary>
        public string MsgOK(Entity en)
        {
            string val = this.GetValStringByKey(FrmEventAttr.MsgOK);
            if (val.Trim() == "")
                return "";

            if (val.IndexOf('@') == -1)
                return val;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                val = val.Replace("@" + attr.Key, en.GetValStringByKey(attr.Key));
            }
            return val;
        }
        public string MsgOKString
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.MsgOK);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MsgOK, value);
            }
        }
        public string MsgErrorString
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.MsgError);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.MsgError, value);
            }
        }
        /// <summary>
        /// ������쳣��ʾ
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public string MsgError(Entity en)
        {
            string val = this.GetValStringByKey(FrmEventAttr.MsgError);
            if (val.Trim() == "")
                return null;

            if (val.IndexOf('@') == -1)
                return val;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                val = val.Replace("@" + attr.Key, en.GetValStringByKey(attr.Key));
            }
            return val;
        }

        public string FK_Event
        {
            get
            {
                return this.GetValStringByKey(FrmEventAttr.FK_Event);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.FK_Event, value);
            }
        }
        /// <summary>
        /// ִ������
        /// </summary>
        public EventDoType HisDoType
        {
            get
            {
                return (EventDoType)this.GetValIntByKey(FrmEventAttr.DoType);
            }
            set
            {
                this.SetValByKey(FrmEventAttr.DoType, (int)value);
            }
        }
        #endregion

        #region ���췽��
        /// <summary>
        /// �¼�
        /// </summary>
        public FrmEvent()
        {
        }
        public FrmEvent(string mypk)
        {
            this.MyPK = mypk;
            this.RetrieveFromDBSources();
        }
        public FrmEvent(string fk_mapdata, string fk_Event)
        {
            this.FK_Event = fk_Event;
            this.FK_MapData = fk_mapdata;
            this.MyPK = this.FK_MapData + "_" + this.FK_Event;
            this.RetrieveFromDBSources();
        }
        /// <summary>
        /// ��д���෽��
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Sys_FrmEvent");
                map.EnDesc = "�¼�";

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.AddMyPK();

                map.AddTBString(FrmEventAttr.FK_Event, null, "�¼�����", true, true, 0, 400, 10);
                map.AddTBString(FrmEventAttr.FK_MapData, null, "FK_MapData", true, true, 0, 400, 10);

                map.AddTBInt(FrmEventAttr.DoType, 0, "�¼�����", true, true);
                map.AddTBString(FrmEventAttr.DoDoc, null, "ִ������", true, true, 0, 400, 10);
                map.AddTBString(FrmEventAttr.MsgOK, null, "�ɹ�ִ����ʾ", true, true, 0, 400, 10);
                map.AddTBString(FrmEventAttr.MsgError, null, "�쳣��Ϣ��ʾ", true, true, 0, 400, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
        protected override bool beforeUpdateInsertAction()
        {
            this.MyPK = this.FK_MapData + "_" + this.FK_Event;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	/// �¼�
	/// </summary>
    public class FrmEvents : EntitiesOID
    {
        public string DoEventNode(string dotype, Entity en)
        {
           return DoEventNode(dotype,en,null);
        }
        public string DoEventNode(string dotype, Entity en, string atPara)
        {
            if (this.Count == 0)
                return null;
            return _DoEventNode(dotype, en, atPara);
        }

        /// <summary>
        /// ִ���¼����¼������ EventList.
        /// </summary>
        /// <param name="dotype">ִ������</param>
        /// <param name="en">����ʵ��</param>
        /// <param name="atPara">����Ĳ�����ʽ@key=value ��ʽ.</param>
        /// <returns></returns>
        private string _DoEventNode(string dotype, Entity en, string atPara)
        {
            if (this.Count == 0)
                return null;

            FrmEvent nev = this.GetEntityByKey(FrmEventAttr.FK_Event, dotype) as FrmEvent;
            if (nev == null || nev.HisDoType == EventDoType.Disable)
                return null;

             
            string doc = nev.DoDoc.Trim();
            if (doc == null || doc == "")
                return null;

            #region ����ִ������
            Attrs attrs = en.EnMap.Attrs;
            string MsgOK = "";
            string MsgErr = "";
            foreach (Attr attr in attrs)
            {
                if (doc.Contains("@" + attr.Key) == false)
                    continue;
                if (attr.MyDataType == DataType.AppString
                    || attr.MyDataType == DataType.AppDateTime
                    || attr.MyDataType == DataType.AppDate)
                    doc = doc.Replace("@" + attr.Key, "'" + en.GetValStrByKey(attr.Key) + "'");
                else
                    doc = doc.Replace("@" + attr.Key, en.GetValStrByKey(attr.Key));
            }

            doc = doc.Replace("~", "'");
            doc = doc.Replace("@WebUser.No", BP.Web.WebUser.No);
            doc = doc.Replace("@WebUser.Name", BP.Web.WebUser.Name);
            doc = doc.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
            doc = doc.Replace("@FK_Node", nev.FK_MapData.Replace("ND", ""));
            doc = doc.Replace("@FK_MapData", nev.FK_MapData);

            if (System.Web.HttpContext.Current != null)
            {
                /*����� bs ϵͳ, �п��ܲ���������url ,����url�Ĳ����滻���� .*/
                string url = System.Web.HttpContext.Current.Request.RawUrl;
                if (url.IndexOf('?') != -1)
                    url = url.Substring(url.IndexOf('?'));

                string[] paras = url.Split('&');
                foreach (string s in paras)
                {
                    if (doc.Contains("@" + s) == false)
                        continue;

                    string[] mys = s.Split('=');
                    if (doc.Contains("@" + mys[0]) == false)
                        continue;
                    doc = doc.Replace("@" + mys[0], mys[1]);
                }
            }

            if (nev.HisDoType == EventDoType.URLOfSelf)
            {
                if (doc.Contains("?") == false)
                    doc += "?1=2";

                doc += "&UserNo=" + WebUser.No;
                doc += "&SID=" + WebUser.SID;
                doc += "&FK_Dept=" + WebUser.FK_Dept;
                doc += "&FK_Unit=" + WebUser.FK_Unit;
                doc += "&OID=" + en.PKVal;

                if (SystemConfig.IsBSsystem)
                {
                    /*��bsϵͳ��������url����ִ������.*/
                    string url = System.Web.HttpContext.Current.Request.RawUrl;
                    if (url.IndexOf('?') != -1)
                        url = url.Substring(url.IndexOf('?'));
                    string[] paras = url.Split('&');
                    foreach (string s in paras)
                    {
                        if (doc.Contains(s))
                            continue;
                        doc += "&" + s;
                    }
                    doc = doc.Replace("&?", "&");
                }
                if (SystemConfig.IsBSsystem == false)
                {
                    /*��bsģʽ�µ���,������csģʽ�µ�����,����ȡ��������. */
                }

                if (doc.Contains("http") == false)
                {
                    /*���û�о���·�� */
                    if (SystemConfig.IsBSsystem)
                    {
                        /*��csģʽ���Զ���ȡ*/
                        string host = System.Web.HttpContext.Current.Request.Url.Host;
                        if (doc.Contains("@AppPath"))
                            doc = doc.Replace("@AppPath", "http://" + host + System.Web.HttpContext.Current.Request.ApplicationPath);
                        else
                            doc = "http://" +  System.Web.HttpContext.Current.Request.Url.Authority + doc;
                    }

                    if (SystemConfig.IsBSsystem == false)
                    {
                        /*��csģʽ������baseurl ��web.config�л�ȡ.*/
                        string cfgBaseUrl = SystemConfig.AppSettings["BaseUrl"];
                        if (string.IsNullOrEmpty(cfgBaseUrl))
                        {
                            string err = "����urlʧ��:û����web.config������BaseUrl,����url�¼����ܱ�ִ��.";
                            Log.DefaultLogWriteLineError(err);
                            throw new Exception(err);
                        }
                        doc = cfgBaseUrl + doc;
                    }
                }

                //������ϵͳԼ���Ĳ���.
                doc += "&EntityName=" + en.ToString() + "&EntityPK=" + en.PK + "&EntityPKVal=" + en.PKVal + "&FK_Event=" + nev.MyPK;
            }
            #endregion ����ִ������

            if (atPara != null)
            {
                AtPara ap = new AtPara(atPara);
                foreach (string s in ap.HisHT.Keys)
                    doc = doc.Replace("@" + s, ap.GetValStrByKey(s));
            }

            if (dotype == FrmEventList.FrmLoadBefore)
                en.Retrieve(); /*�����ִ�У��ͻ����ʵ����������ѯ�����ݲ�һ��.*/

            switch (nev.HisDoType)
            {
                case EventDoType.SP:
                    try
                    {
                        Paras ps = new Paras();
                        DBAccess.RunSP(doc, ps);
                        return nev.MsgOK(en);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                case EventDoType.SQL:
                    try
                    {
                        // ����ִ�д���GO��sql.
                        DBAccess.RunSQLs(doc);
                        return nev.MsgOK(en);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                case EventDoType.URLOfSelf:
                    string myURL = doc.Clone() as string;
                    if (myURL.Contains("http") == false)
                    {
                        if (SystemConfig.IsBSsystem)
                        {
                            string host = System.Web.HttpContext.Current.Request.Url.Host;
                            if (myURL.Contains("@AppPath"))
                                myURL = myURL.Replace("@AppPath", "http://" + host + System.Web.HttpContext.Current.Request.ApplicationPath);
                            else
                                myURL = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + myURL;
                        }
                        else
                        {
                            string cfgBaseUrl = SystemConfig.AppSettings["BaseUrl"];
                            if (string.IsNullOrEmpty(cfgBaseUrl))
                            {
                                string err = "����urlʧ��:û����web.config������BaseUrl,����url�¼����ܱ�ִ��.";
                                Log.DefaultLogWriteLineError(err);
                                throw new Exception(err);
                            }
                            myURL = cfgBaseUrl + myURL;
                        }
                    }

                    try
                    {
                        Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
                        string text = DataType.ReadURLContext(myURL, 8000, encode);
                        if (text == null)
                            throw new Exception("@������ƴ���ִ�е�URL����:" + myURL+", ����Ϊnull, ���������Ƿ���ȷ��");

                        if (text != null && text.Substring(0, 7).Contains("Err"))
                            throw new Exception(text);

                        if (text == null || text.Trim() == "")
                            return null;
                        return text;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@" + nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                case EventDoType.URLOfSystem:
                    string hos1t = System.Web.HttpContext.Current.Request.Url.Host;
                    string url = "http://" + hos1t + System.Web.HttpContext.Current.Request.ApplicationPath + "/DataUser/AppCoder/FrmEventHandle.aspx";
                    url += "?FK_MapData=" + en.ClassID + "&WebUseNo=" + WebUser.No + "&EventType=" + nev.FK_Event;
                    foreach (Attr attr in attrs)
                    {
                        if (attr.UIIsDoc || attr.IsRefAttr || attr.UIIsReadonly )
                            continue;
                        url += "&" + attr.Key + "=" + en.GetValStrByKey(attr.Key);
                    }

                    try
                    {
                        string text = DataType.ReadURLContext(url, 800, System.Text.Encoding.UTF8);
                        if (text != null && text.Substring(0, 7).Contains("Err"))
                            throw new Exception(text);

                        if (text == null || text.Trim() == "")
                            return null;
                        return text;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@" + nev.MsgError(en) + " Error:" + ex.Message);
                    }
                    break;
                default:
                    throw new Exception("@no such way.");
            }
        }
        /// <summary>
        /// �¼�
        /// </summary>
        public FrmEvents() { }
        /// <summary>
        /// �¼�
        /// </summary>
        /// <param name="FK_MapData">FK_MapData</param>
        public FrmEvents(string fk_MapData)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(FrmEventAttr.FK_MapData, fk_MapData);
            qo.DoQuery();
        }
        /// <summary>
        /// �õ����� Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmEvent();
            }
        }
    }
}