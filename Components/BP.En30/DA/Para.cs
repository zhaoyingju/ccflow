using System;

namespace BP.DA
{ 
	/// <summary>
	/// ����
	/// </summary>
    public class Para
    {
        #region ����
        public System.Data.DbType DAType = System.Data.DbType.String;
        public System.Data.OracleClient.OracleType DATypeOfOra
        {
            get
            {
                switch (this.DAType)
                {
                    case System.Data.DbType.String:
                        return System.Data.OracleClient.OracleType.VarChar;
                    case System.Data.DbType.Int32:
                    case System.Data.DbType.Int16:
                        return System.Data.OracleClient.OracleType.Number;
                    case System.Data.DbType.Int64:
                        return System.Data.OracleClient.OracleType.UInt32;
                    case System.Data.DbType.Decimal:
                    case System.Data.DbType.Double:
                        return System.Data.OracleClient.OracleType.Double;
                    default:
                        throw new Exception("û���漰�������͡�typeof(obj)=");
                }
            }
        }
        public string ParaName = null;
        public int Size = 10;
        public Object val;
        #endregion

        /// <summary>
        /// ����
        /// </summary>
        public Para()
        {
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="_paraName">��������</param>
        /// <param name="_DAType">System.Data.SqlDbType</param>
        /// <param name="_val">ֵ</param>
        public Para(string _paraName, System.Data.DbType _DAType, Object _val)
        {
            this.ParaName = _paraName;
            this.DAType = _DAType;
            this.val = _val;
        }
    }
	/// <summary>
	/// ��������
	/// </summary>
	public class Paras : System.Collections.CollectionBase
    {
        #region ���⴦��
        public void AddFK_Emp(string userNo)
        {
            this.Add("FK_Emp", userNo);
        }
        public void AddFK_Emp()
        {
            this.Add("FK_Emp", Web.WebUser.No);
        }

        public void AddFK_NY(string fk_ny)
        {
            this.Add("FK_NY", fk_ny);
        }
        public void AddFK_NY()
        {
            this.Add("FK_NY", DataType.CurrentYearMonth);
        }


        public void AddFK_Dept(string val)
        {
            this.Add("FK_Dept", val);
        }
        public void AddFK_Dept()
        {
            this.Add("FK_Dept", BP.Web.WebUser.FK_Dept );
        }
        #endregion

        public string ToDesc()
        {
            string msg = "";
            foreach (Para p in this)
                msg += "@" + p.ParaName + " = " + p.val;

            return msg;
        }

        public string SQL = null;
        public string SQLNoPara
        {
            get
            {
                string mysql = this.SQL.Clone() as string;

                foreach (Para p in this)
                    mysql = mysql.Replace(":" + p.ParaName, "'" + p.val.ToString() + "'");

                return mysql;
            }
        }

		public Paras()
        {
        }
        public Paras(object o)
        {
            this.Add("p", o);
        }
        public string DBStr
        {
            get
            {
                return BP.SystemConfig.AppCenterDBVarStr;
            }
        }
        public Paras(string p,object v)
        {
            this.Add(p, v);
        }
        public Paras(string p1, object o1, string p2, object o2)
        {
            this.Add(p1, o1);
            this.Add(p2, o2);
        }
       
		public void Add(Para para)
		{
            foreach (Para p in this)
            {
                if (p.ParaName == para.ParaName)
                {
                    p.val = para.val;
                    return;
                }
            }

			this.InnerList.Add(para);
		}
        public void Add( object obj)
        {
            this.Add("p", obj);
        }
        public void Add(string _name, object obj)
        {
            if (_name == "abc")
                return;
            foreach (Para p in this)
            {
                if (p.ParaName == _name )
                {
                    p.val = obj;
                    return;
                }
            }

            if (obj.GetType() == typeof(string))
            {
                this.Add(_name, obj.ToString());
                return;
            }

            if (obj.GetType() == typeof(int)
                || obj.GetType() == typeof(Int32)
                || obj.GetType() == typeof(Int16))
            {
                this.Add(_name, Int32.Parse(obj.ToString()));
                return;
            }

            if (obj.GetType() == typeof(Int64))
            {
                this.Add(_name, Int64.Parse(obj.ToString()));
                return;
            }

            if (obj.GetType() == typeof(float))
            {
                this.Add(_name, float.Parse(obj.ToString()));
                return;
            }

            if (obj.GetType() == typeof(decimal))
            {
                this.Add(_name, decimal.Parse(obj.ToString()));
                return;
            }

            this.Add(_name, obj.ToString());
        }
		private void Add(string _name, string _val )
		{
			Para en = new Para();
			en.DAType=System.Data.DbType.String;
			en.val = _val;
			en.ParaName = _name;
			en.Size=_val.Length;
			this.Add(en);
		}

        private void Add(string _name, Int32 _val)
		{
			Para en = new Para();
			en.DAType=System.Data.DbType.Int32;
			en.val = _val;
			en.ParaName = _name ;
			this.Add(en);
		}
        private void Add(string _name, Int64 _val)
        {
            Para en = new Para();
            en.DAType = System.Data.DbType.Int64;
            en.val = _val;
            en.ParaName = _name;
            this.Add(en);
        }
        private void Add(string _name, float _val)
		{
			Para en = new Para();
			en.DAType=System.Data.DbType.Int64;
			en.val = _val;
			en.ParaName = _name ;
			this.Add(en);			
		}
        private void Add(string _name, decimal _val)
		{
			Para en = new Para();
			en.DAType=System.Data.DbType.Decimal;
			en.val = _val;
			en.ParaName = _name ;
			this.Add(en);			
		}
        public void Remove(string paraName)
        {
            int i=0;
            foreach (Para p in this)
            {
                if (p.ParaName == paraName)
                    break;
                i++;
            }
            this.RemoveAt(i);
        }
    }
}