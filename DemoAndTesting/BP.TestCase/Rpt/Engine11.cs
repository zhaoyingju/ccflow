using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using BP.En;
using BP.DA;
using BP.Port;
using BP.Sys;

namespace BP.Rpt
{
    public class RepBill : BP.DTS.DataIOEn
    {
        public RepBill()
        {
            this.Title = "CCFlow5单据自动修复线。";
        }
        public override void Do()
        {
            string msg = "";
            string sql = "  SELECT * FROM WF_BillTemplate";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string file = SystemConfig.PathOfCyclostyleFile + dr["URL"].ToString() + ".rtf";
                msg += RepBill.RepairBill(file);
            }
            PubClass.ResponseWriteBlueMsg(msg);
        }

        public static string RepairBill(string file)
        {
            string msg = "";
            string docs;

            // 读取文件。
            try
            {
                StreamReader read = new StreamReader(file, System.Text.Encoding.ASCII); // 文件流.
                docs = read.ReadToEnd();  // 读取完毕
                read.Close(); // 关闭
            }
            catch (Exception ex)
            {
                return "@读取单据模板时出现错误。cfile=" + file + " @Ex=" + ex.Message;
            }

            // 修复。
            docs = RepairLineV2(docs);

            // 写入。
            try
            {
                StreamWriter mywr = new StreamWriter(file, false);
                mywr.Write(docs);
                mywr.Close();
            }
            catch (Exception ex)
            {
                return "@写入单据模板时出现错误。cfile=" + file + " @Ex=" + ex.Message;
            }
            msg += "@单据:[" + file + "]成功修复。";
            return msg;
        }

        public static string RepairLine(string line)//str
        {
            char[] chs = line.ToCharArray();
            string str = "";
            foreach (char ch in chs)
            {
                if (ch == '\\')
                {
                    line = line.Replace("\\" + str, "");
                    str = "";
                }
                else if (ch == ' ')
                {
                    /* 如果等于空格， 直接替换原来的 str */
                    line = line.Replace("\\" + str + ch, "");
                    str = "sssssssssssssssssss";
                }
                else
                    str += ch.ToString();
            }

            line = line.Replace("{", "");
            line = line.Replace("}", "");
            line = line.Replace("\r", "");
            line = line.Replace("\n", "");
            line = line.Replace(" ", "");
            line = line.Replace("..", ".");
            return line;
        }
        /// <summary>
        /// RepairLineV2
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public static string RepairLineV2(string docs)//str
        {
            char[] chars = docs.ToCharArray();
            string strs = "";
            foreach (char c in chars)
            {
                if (c == '<')
                {
                    strs = "<";
                    continue;
                }
                if (c == '>')
                {
                    strs += c.ToString();
                    string line = strs.Clone().ToString();
                    line = RepairLine(line);
                    docs = docs.Replace(strs, line);
                    strs = "";
                    continue;
                }
                strs += c.ToString();
            }
            return docs;
        }
    }
	/// <summary>
	/// WebRtfReport 的摘要说明。
	/// </summary>
	public class RTFEngine 
	{

		#region 数据明细实体
		private System.Text.Encoding _encoder= System.Text.Encoding.GetEncoding("GB2312") ;

        public string GetCode(string str)
        {
            if (str == "")
                return str;

            string rtn = "";
            byte[] rr = _encoder.GetBytes(str);
            foreach (byte b in rr)
            {
                if (b > 122)
                    rtn += "\\'" + b.ToString("x");
                else
                    rtn += (char)b;
            }
            return rtn.Replace("\n", " \\par ");
        }
		#endregion 数据明细实体
		 
		public string CyclostyleFilePath="";
		public string TempFilePath="";

		#region 获取特殊要处理的流程节点信息.
        public static string GetImgHexString(System.Drawing.Image img,System.Drawing.Imaging.ImageFormat ext)
        {
            StringBuilder imgs = new StringBuilder();
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ext);
            stream.Close();

            byte[] buffer = stream.ToArray();

            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i % 32) == 0)
                {
                    imgs.AppendLine();
                }
                else if ((i % 8) == 0)
                {
                    imgs.Append(" ");
                }
                byte num2 = buffer[i];
                int num3 = (num2 & 240) >> 4;
                int num4 = num2 & 15;
                imgs.Append("0123456789abcdef"[num3]);
                imgs.Append("0123456789abcdef"[num4]);
            }
            return imgs.ToString();
        }
        /// <summary>
        /// 获取ICON图片的数据。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueImgStrs(string key)
        {
            key = key.Replace(" ", "");
            key = key.Replace("\r\n", "");

            /*说明是图片文件.*/
            string path = key.Replace("OID.Img@AppPath", SystemConfig.PathOfWebApp);

            //定义rtf中图片字符串
            StringBuilder pict = new StringBuilder();
            //获取要插入的图片
            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

            //将要插入的图片转换为16进制字符串
            string imgHexString;
            key = key.ToLower();

            if (key.Contains(".png"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Png);
            else if (key.Contains(".jp"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (key.Contains(".gif"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Gif);
            else if (key.Contains(".ico"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Icon);
            else
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);

            //生成rtf中图片字符串
            pict.AppendLine();
            pict.Append(@"{\pict");
            pict.Append(@"\jpegblip");
            pict.Append(@"\picscalex100");
            pict.Append(@"\picscaley100");
            pict.Append(@"\picwgoal" + img.Size.Width * 15);
            pict.Append(@"\pichgoal" + img.Size.Height * 15);
            pict.Append(imgHexString + "}");
            pict.AppendLine();
            return pict.ToString();
        }
        /// <summary>
        /// 获取M2M数据并输出
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueM2MStrs(string key)
        {
            string[] strs = key.Split('.');
            string sql = "SELECT ValsName FROM SYS_M2M WHERE FK_MapData='" + strs[0] + "' AND M2MNo='" + strs[2] + "' AND EnOID='" + this.HisGEEntity.PKVal + "'";
            string vals = DBAccess.RunSQLReturnStringIsNull(sql, null);
            if (vals == null)
                return "无数据";

            vals = vals.Replace("@","  ");
            vals = vals.Replace("<font color=green>", "");
            vals = vals.Replace("</font>", "");
            return vals;


            string val = "";
            string[] objs = vals.Split('@');
            foreach (string obj in objs)
            {
                string[] noName = obj.Split(',');
                val += noName[1];
            }
            return val;
        }
        /// <summary>
        /// 获取写字版的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueBPPaintStrs(string key)
        {
            key = key.Replace(" ", "");
            key = key.Replace("\r\n", "");

            string[] strs = key.Split('.');
            string filePath = "";
            try
            {
                filePath = DBAccess.RunSQLReturnString("SELECT Tag2 From Sys_FrmEleDB WHERE RefPKVal=" + this.PKVal+ " AND EleID='" + strs[2].Trim() + "'");
                if (filePath == null)
                    return "";
            }
            catch
            {
                return "";
            }

            //定义rtf中图片字符串
            StringBuilder pict = new StringBuilder();
            //获取要插入的图片
            System.Drawing.Image img = System.Drawing.Image.FromFile(filePath);

            //将要插入的图片转换为16进制字符串
            string imgHexString;
            filePath = filePath.ToLower();

            if (filePath.Contains(".png"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Png);
            else if (filePath.Contains(".jp"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (filePath.Contains(".gif"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Gif);
            else if (filePath.Contains(".ico"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Icon);
            else
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);

            //生成rtf中图片字符串
            pict.AppendLine();
            pict.Append(@"{\pict");
            pict.Append(@"\jpegblip");
            pict.Append(@"\picscalex100");
            pict.Append(@"\picscaley100");
            pict.Append(@"\picwgoal" + img.Size.Width * 15);
            pict.Append(@"\pichgoal" + img.Size.Height * 15);
            pict.Append(imgHexString + "}");
            pict.AppendLine();
            return pict.ToString();
        }
        public string GetValByKeyFromMainTable(string key)
        {
            //foreach (DataColumn dc in DtlOpenType.ForEmp )
            //{
            //}

            return null;
        }
		/// <summary>
		/// 审核节点的表示方法是 节点ID.Attr.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetValueByKey(string key)
		{
			key=key.Replace(" ","");
			key=key.Replace("\r\n","");

            string[] strs = key.Split('.');

            // 如果不包含 . 就说明他是从Rpt中取数据。
            if (this.HisGEEntity != null && key.Contains("ND") == false)
            {
                if (strs.Length == 1)
                    return this.HisGEEntity.GetValStringByKey(key);

                if (strs[1].Trim() == "ImgAth")
                {
                    string path1 = BP.SystemConfig.PathOfDataUser + "\\ImgAth\\Data\\" + strs[0].Trim() + "_" + this.HisGEEntity.PKVal + ".png";

                    //定义rtf中图片字符串.
                    StringBuilder mypict = new StringBuilder();

                    //获取要插入的图片
                    System.Drawing.Image imgAth = System.Drawing.Image.FromFile(path1);

                    //将要插入的图片转换为16进制字符串
                    string imgHexStringImgAth = GetImgHexString(imgAth, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //生成rtf中图片字符串
                    mypict.AppendLine();
                    mypict.Append(@"{\pict");
                    mypict.Append(@"\jpegblip");
                    mypict.Append(@"\picscalex100");
                    mypict.Append(@"\picscaley100");
                    mypict.Append(@"\picwgoal" + imgAth.Size.Width * 15);
                    mypict.Append(@"\pichgoal" + imgAth.Size.Height * 15);
                    mypict.Append(imgHexStringImgAth + "}");
                    mypict.AppendLine();
                    return mypict.ToString();
                }

                if (strs[1].Trim() == "BPPaint")
                {
                    string path1 = DBAccess.RunSQLReturnString("SELECT  Tag2 FROM Sys_FrmEleDB WHERE REFPKVAL=" + this.HisGEEntity.PKVal + " AND EleID='" + strs[0].Trim() + "'");
                  //  string path1 = BP.SystemConfig.PathOfDataUser + "\\BPPaint\\" + this.HisGEEntity.ToString().Trim() + "\\" + this.HisGEEntity.PKVal + ".png";

                    //定义rtf中图片字符串.
                    StringBuilder mypict = new StringBuilder();
                    //获取要插入的图片
                    System.Drawing.Image myBPPaint = System.Drawing.Image.FromFile(path1);

                    //将要插入的图片转换为16进制字符串
                    string imgHexStringImgAth = GetImgHexString(myBPPaint, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //生成rtf中图片字符串
                    mypict.AppendLine();
                    mypict.Append(@"{\pict");
                    mypict.Append(@"\jpegblip");
                    mypict.Append(@"\picscalex100");
                    mypict.Append(@"\picscaley100");
                    mypict.Append(@"\picwgoal" + myBPPaint.Size.Width * 15);
                    mypict.Append(@"\pichgoal" + myBPPaint.Size.Height * 15);
                    mypict.Append(imgHexStringImgAth + "}");
                    mypict.AppendLine();
                    return mypict.ToString();
                }


                if (strs.Length == 2)
                {
                    string val = this.HisGEEntity.GetValStringByKey(strs[0].Trim());
                    switch (strs[1].Trim())
                    {
                      
                        case "Text":
                            if (val == "0")
                                return "否";
                            else
                                return "是";
                        case "Year":
                            return val.Substring(0, 4);
                        case "Month":
                            return val.Substring(5, 2);
                        case "Day":
                            return val.Substring(8, 2);
                        case "NYR":
                            return DA.DataType.ParseSysDate2DateTime(val).ToString("yyyy年MM月dd日");
                        case "RMB":
                            return float.Parse(val).ToString("0.00");
                        case "RMBDX":
                            return DA.DataType.ParseFloatToCash(float.Parse(val));
                        case "Siganture":
                            string path = BP.SystemConfig.PathOfDataUser + "\\Siganture\\" + val + ".jpg";
                            //定义rtf中图片字符串
                            StringBuilder pict = new StringBuilder();
                            //获取要插入的图片
                            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                            //将要插入的图片转换为16进制字符串
                            string imgHexString = GetImgHexString(img,System.Drawing.Imaging.ImageFormat.Jpeg);
                            //生成rtf中图片字符串
                            pict.AppendLine();
                            pict.Append(@"{\pict");
                            pict.Append(@"\jpegblip");
                            pict.Append(@"\picscalex100");
                            pict.Append(@"\picscaley100");
                            pict.Append(@"\picwgoal" + img.Size.Width * 15);
                            pict.Append(@"\pichgoal" + img.Size.Height * 15);
                            pict.Append(imgHexString + "}");
                            pict.AppendLine();
                            return pict.ToString();
                        //替换rtf模板文件中的签名图片标识为图片字符串
                        // str = str.Replace(imgMark, pict.ToString());
                        case "YesNo":
                            if (val == "1")
                                return "[√]";
                            else
                                return "[×]";
                            break;
                        case "Yes":
                            if (val == "0")
                                return "[×]";
                            else
                                return "[√]";
                        case "No":
                            if (val == "0")
                                return "[√]";
                            else
                                return "[×]";
                        default:
                            throw new Exception("参数设置错误，特殊方式取值错误：" + key);
                    }
                }
                else
                {
                    throw new Exception("参数设置错误，特殊方式取值错误：" + key);
                }
            }

			foreach(Entity en in this.HisEns)
			{
                string enKey = en.ToString();
                if (enKey.Contains("."))
                    enKey = en.GetType().Name;
                if (key.Contains(en.ToString() + ".") == false)
                    continue;
               
				/*说明就在这个字段内*/
				if (strs.Length==1)
					throw new Exception("参数设置错误，strs.length=1 。"+key);

				if (strs.Length==2)
					return en.GetValStringByKey(strs[1].Trim() );

				if (strs.Length==3)
				{
                    if (strs[2].Trim() == "ImgAth")
                    {
                        string path1 = BP.SystemConfig.PathOfDataUser + "\\ImgAth\\Data\\" + strs[1].Trim() + "_" + en.PKVal + ".png";
                        //定义rtf中图片字符串.
                        StringBuilder mypict = new StringBuilder();
                        //获取要插入的图片
                        System.Drawing.Image imgAth = System.Drawing.Image.FromFile(path1);

                        //将要插入的图片转换为16进制字符串
                        string imgHexStringImgAth = GetImgHexString(imgAth, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //生成rtf中图片字符串
                        mypict.AppendLine();
                        mypict.Append(@"{\pict");
                        mypict.Append(@"\jpegblip");
                        mypict.Append(@"\picscalex100");
                        mypict.Append(@"\picscaley100");
                        mypict.Append(@"\picwgoal" + imgAth.Size.Width * 15);
                        mypict.Append(@"\pichgoal" + imgAth.Size.Height * 15);
                        mypict.Append(imgHexStringImgAth + "}");
                        mypict.AppendLine();
                        return mypict.ToString();
                    }


					string val=en.GetValStringByKey(strs[1].Trim() );
					switch( strs[2].Trim() )
					{
						case "Text":
							if ( val=="0")
								return "否";
							else
								return "是";
						case "Year":
							return val.Substring(0,4);
						case "Month":
							return val.Substring(5,2);
						case "Day":
							return val.Substring(8,2);
						case "NYR":
							return DA.DataType.ParseSysDate2DateTime(val).ToString("yyyy年MM月dd日");
						case "RMB":
							return float.Parse(val).ToString("0.00");
						case "RMBDX":
							return DA.DataType.ParseFloatToCash( float.Parse(val)) ;
                        case "ImgAth":
                            string path1 = BP.SystemConfig.PathOfDataUser + "\\ImgAth\\Data\\" + strs[0].Trim() + "_" + this.HisGEEntity.PKVal + ".png";
                        
                           //定义rtf中图片字符串.
                            StringBuilder mypict = new StringBuilder();
                            //获取要插入的图片
                            System.Drawing.Image imgAth = System.Drawing.Image.FromFile(path1);

                            //将要插入的图片转换为16进制字符串
                            string imgHexStringImgAth = GetImgHexString(imgAth, System.Drawing.Imaging.ImageFormat.Jpeg);
                            //生成rtf中图片字符串
                            mypict.AppendLine();
                            mypict.Append(@"{\pict");
                            mypict.Append(@"\jpegblip");
                            mypict.Append(@"\picscalex100");
                            mypict.Append(@"\picscaley100");
                            mypict.Append(@"\picwgoal" + imgAth.Size.Width * 15);
                            mypict.Append(@"\pichgoal" + imgAth.Size.Height * 15);
                            mypict.Append(imgHexStringImgAth + "}");
                            mypict.AppendLine();
                            return mypict.ToString();
                        case "Siganture":
                            string path = BP.SystemConfig.PathOfDataUser + "\\Siganture\\" + val + ".jpg";
                            //定义rtf中图片字符串.
                            StringBuilder pict = new StringBuilder();
                            //获取要插入的图片
                            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                            //将要插入的图片转换为16进制字符串
                            string imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                            //生成rtf中图片字符串
                            pict.AppendLine();
                            pict.Append(@"{\pict");
                            pict.Append(@"\jpegblip");
                            pict.Append(@"\picscalex100");
                            pict.Append(@"\picscaley100");
                            pict.Append(@"\picwgoal" + img.Size.Width * 15);
                            pict.Append(@"\pichgoal" + img.Size.Height * 15);
                            pict.Append(imgHexString + "}");
                            pict.AppendLine();
                            return pict.ToString();
                        //替换rtf模板文件中的签名图片标识为图片字符串
                        // str = str.Replace(imgMark, pict.ToString());
						default:
							throw new Exception("参数设置错误，特殊方式取值错误："+key);
					}
				}
			}
			throw new Exception("参数设置错误 GetValueByKey ："+key);
		}
		#endregion
	 

        #region 生成单据
        public string PK = null;
        public string PKVal = null;
        public DataTable mainDT = null;
        public DataSet dtlsDS = null;

        /// <summary>
        /// 生成单据根据
        /// </summary>
        /// <param name="templeteFile">模板文件</param>
        /// <param name="saveToFile"></param>
        /// <param name="mainDT"></param>
        /// <param name="dtls"></param>
        public void MakeDoc(string templeteFile, string saveToFile,DataTable mainDT, DataSet dtlsDS)
        {

            this.mainDT  = mainDT;
            this.dtlsDS = dtlsDS;


           #region 生成参数。
            
            string str = Cash.GetBillStr(templeteFile, false).Substring(0);

            string error = "";
            string[] paras = null;

            DataSet dsTemp = new DataSet();
            dsTemp.Tables.Add(mainDT);
            foreach (DataTable dttemp in dsTemp.Tables)
            {
                dsTemp.Tables.Add(dttemp);
            }

            Attrs attrs = new Attrs();
            foreach (DataTable dt in dsTemp.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    Attr attr = new Attr();
                    attr.Field = dc.ColumnName;
                    attr.Key = dc.ColumnName;

                    switch (dc.DataType.ToString())
                    {
                        case "int":
                            attr.MyDataType = DataType.AppInt;
                            break;
                        case "decimal":
                        case "float":
                            attr.MyDataType = DataType.AppFloat;
                            break;
                        case "string":
                        default:
                            attr.MyDataType = DataType.AppString;
                            break;
                    }
                }
            }
            paras = Cash.GetBillParas_Gener(templeteFile, attrs);
           #endregion

            try
            {
                string key = "";
                string ss = "";

                #region 主表
                foreach (string para in paras)
                {
                    if (para == null || para == "")
                        continue;
                    try
                    {
                        if (para.Contains("ImgAth"))
                            str = str.Replace("<" + para + ">", this.GetValueByKey(para));
                        else if (para.Contains("Siganture"))
                            str = str.Replace("<" + para + ">", this.GetValueByKey(para));
                        else if (para.Contains("Img@AppPath"))
                            str = str.Replace("<" + para + ">", this.GetValueImgStrs(para));
                        else if (para.Contains(".BPPaint"))
                            str = str.Replace("<" + para + ">", this.GetValueBPPaintStrs(para));
                        else if (para.Contains(".M2M"))
                            str = str.Replace("<" + para + ">", this.GetValueM2MStrs(para));
                        else
                            str = str.Replace("<" + para + ">", this.GetCode(this.GetValueByKey(para)));
                    }
                    catch (Exception ex)
                    {
                        error += "取参数[" + para + "]出现错误：有以下情况导致此错误;1你用Text取值时间，此属性不是外键。2,类无此属性。<br>更详细的信息：<br>" + ex.Message;
                        if (SystemConfig.IsDebug)
                            throw new Exception(error);
                        Log.DebugWriteError(error);
                    }
                }
                #endregion

                #region 从表
                string shortName = "";
                ArrayList al = this.EnsDataDtls;
                foreach (Entities dtls in al)
                {
                    //  shortName = dtls.GetNewEntity.ToString().Substring(dtls.GetNewEntity.ToString().LastIndexOf(".") + 1);

                    Entity dtl = dtls.GetNewEntity;
                    string dtlEnName = dtl.ToString();
                    shortName = dtlEnName.Substring(dtlEnName.LastIndexOf(".") + 1);

                    if (str.IndexOf(shortName) == -1)
                        continue;

                    int pos_rowKey = str.IndexOf(shortName);
                    int row_start = -1, row_end = -1;
                    if (pos_rowKey != -1)
                    {
                        row_start = str.Substring(0, pos_rowKey).LastIndexOf("\\row");
                        row_end = str.Substring(pos_rowKey).IndexOf("\\row");
                    }

                    if (row_start != -1 && row_end != -1)
                    {
                        string row = str.Substring(row_start, (pos_rowKey - row_start) + row_end);
                        str = str.Replace(row, "");

                        Map map = dtls.GetNewEntity.EnMap;
                        int i = dtls.Count;
                        while (i > 0)
                        {
                            i--;
                            string rowData = row.Clone() as string;
                            dtl = dtls[i];
                            foreach (Attr attr in map.Attrs)
                            {
                                switch (attr.MyDataType)
                                {
                                    case DataType.AppDouble:
                                    case DataType.AppFloat:
                                    case DataType.AppRate:
                                        rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValStringByKey(attr.Key));
                                        break;
                                    case DataType.AppMoney:
                                        rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValDecimalByKey(attr.Key).ToString("0.00"));
                                        break;
                                    case DataType.AppInt:
                                        if (attr.MyDataType == DataType.AppBoolean)
                                        {
                                            rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValStrByKey(attr.Key));
                                            int v = dtl.GetValIntByKey(attr.Key);
                                            if (v == 1)
                                                rowData = rowData.Replace("<" + shortName + "." + attr.Key + "Text>", "是");
                                            else
                                                rowData = rowData.Replace("<" + shortName + "." + attr.Key + "Text>", "否");
                                        }
                                        else
                                        {
                                            rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValStrByKey(attr.Key));
                                        }
                                        break;
                                    default:
                                        rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", GetCode(dtl.GetValStrByKey(attr.Key)));
                                        break;
                                }
                            }
                            str = str.Insert(row_start, rowData);
                        }
                    }
                }
                #endregion 从表

                #region 明细 合计信息。
                al = this.EnsDataDtls;
                foreach (Entities dtls in al)
                {
                    Entity dtl = dtls.GetNewEntity;
                    string dtlEnName = dtl.ToString();
                    shortName = dtlEnName.Substring(dtlEnName.LastIndexOf(".") + 1);
                    //shortName = dtls.ToString().Substring(dtls.ToString().LastIndexOf(".") + 1);
                    Map map = dtl.EnMap;
                    foreach (Attr attr in map.Attrs)
                    {
                        switch (attr.MyDataType)
                        {
                            case DataType.AppDouble:
                            case DataType.AppFloat:
                            case DataType.AppMoney:
                            case DataType.AppRate:
                                key = "<" + shortName + "." + attr.Key + ".SUM>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key, dtls.GetSumFloatByKey(attr.Key).ToString());

                                key = "<" + shortName + "." + attr.Key + ".SUM.RMB>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key, dtls.GetSumFloatByKey(attr.Key).ToString("0.00"));

                                key = "<" + shortName + "." + attr.Key + ".SUM.RMBDX>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key,
                                        GetCode(DA.DataType.ParseFloatToCash(dtls.GetSumFloatByKey(attr.Key))));
                                break;
                            case DataType.AppInt:
                                key = "<" + shortName + "." + attr.Key + ".SUM>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key, dtls.GetSumIntByKey(attr.Key).ToString());
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion 从表合计

                StreamWriter wr = new StreamWriter(this.TempFilePath, false, Encoding.ASCII);
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                wr.Write(str);
                wr.Close();
            }
            catch (Exception ex)
            {
                string msg = "";
                if (SystemConfig.IsDebug)
                {  // 异常可能与单据的配置有关系。
                    try
                    {
                        this.CyclostyleFilePath = SystemConfig.PathOfDataUser + "\\CyclostyleFile\\" + templeteFile;
                        str = Cash.GetBillStr(templeteFile, false);
                        string s = RepBill.RepairBill(this.CyclostyleFilePath);
                        msg = "@已经成功的执行修复线  RepairLineV2，您重新发送一次或者，退后重新在发送一次，是否可以解决此问题。@" + s;
                    }
                    catch (Exception ex1)
                    {
                        msg = "执行修复线失败.  RepairLineV2 " + ex1.Message;
                    }
                }
                throw new Exception("生成文档失败：单据名称[" + this.CyclostyleFilePath + "] 异常信息：" + ex.Message + " @自动修复单据信息：" + msg);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// RTFEngine
        /// </summary>
        public RTFEngine()
        {
            this._EnsDataDtls = null;
            this._HisEns = null;
        }
        /// <summary>
        /// 修复线
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
		
	
		#endregion
	}

     
}
