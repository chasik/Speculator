using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace SpeculatorServices
{
    public static class TransaqConnector
    {
        public delegate bool CallBackDelegate(IntPtr pData);

        public delegate bool CallBackExDelegate(IntPtr pData, IntPtr userData);

        private static readonly CallBackDelegate TransaqCallbackDelegate = TransaqCallBack;

        [DllImport("txmlconnector64.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetCallback(CallBackDelegate pCallback);

        [DllImport("txmlconnector64.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetCallbackEx(CallBackExDelegate pCallbackEx, IntPtr userData);

        [DllImport("txmlconnector64.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr SendCommand(IntPtr pData);

        [DllImport("txmlconnector64.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool FreeMemory(IntPtr pData);

        [DllImport("TXmlConnector64.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr Initialize(IntPtr pPath, int logLevel);

        [DllImport("TXmlConnector64.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr UnInitialize();

        [DllImport("TXmlConnector64.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr SetLogLevel(int logLevel);


        public static string ConnectorSendCommand(string command)
        {
            var pData = MarshalUtf8.StringToHGlobalUtf8(command);
            var pResult = SendCommand(pData);
            var result = MarshalUtf8.PtrToStringUtf8(pResult);

            Marshal.FreeHGlobal(pData);
            FreeMemory(pResult);

            return result;
        }


        public static bool ConnectorInitialize(string path, short logLevel)
        {
            var pPath = MarshalUtf8.StringToHGlobalUtf8(path);
            var pResult = Initialize(pPath, logLevel);

            if (!pResult.Equals(IntPtr.Zero))
            {
                Marshal.FreeHGlobal(pPath);
                FreeMemory(pResult);
                return false;
            }
            else
            {
                Marshal.FreeHGlobal(pPath);
                return true;
            }
        }

        public static void ConnectorUnInitialize()
        {

            //if (statusDisconnected.WaitOne(statusTimeout))
            //{
                var pResult = UnInitialize();

                if (!pResult.Equals(IntPtr.Zero))
                {
                    var result = MarshalUtf8.PtrToStringUtf8(pResult);
                    FreeMemory(pResult);
                    //log.WriteLog(result);
                }
                else
                {
                    //log.WriteLog("UnInitialize() OK");
                }
            //}
            //else
            //{
            //    //log.WriteLog("WARNING! Не дождались статуса disconnected. UnInitialize() не выполнено.");
            //}
        }

        public static void ConnectorSetCallback()
        {
            if (!SetCallback(TransaqCallbackDelegate))
            {
                throw new Exception();
            }
            //if (!SetCallbackEx(myCallbackExDelegate, IntPtr.Zero))
            //{
            //    throw (new Exception(EX_SETTING_CALLBACK));
            //}
        }

        public static bool TransaqCallBack(IntPtr pData)
        {
            var data = MarshalUtf8.PtrToStringUtf8(pData);
            FreeMemory(pData);

            var res = TransaqHandleData(data);
            //if (res == "server_status")
            //    New_Status();
            return true;
        }

        public static string TransaqHandleData(string data)
        {
            // обработка данных, полученных коннектором от сервера Транзак
            var info = "";

            var textForWindow = data;

            var xs = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                DtdProcessing = DtdProcessing.Ignore
            };

            var xr = XmlReader.Create(new System.IO.StringReader(data), xs);
            var section = "";
            var line = "";
            var str = "";
            var ename = "";
            var evalue = "";
            //string values = "";

            // обработка "узлов" 
            while (xr.Read())
            {
                switch (xr.NodeType)
                {
                    case XmlNodeType.Element:
                    case XmlNodeType.EndElement:
                        ename = xr.Name; break;
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Comment:
                    case XmlNodeType.XmlDeclaration:
                        evalue = xr.Value; break;
                    case XmlNodeType.DocumentType:
                        ename = xr.Name; evalue = xr.Value; break;
                    default: break;
                }

                // определяем узел верхнего уровня - "секцию"
                if (xr.Depth == 0)
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        section = ename;

                        if ((section != "boards") && (section != "securities") && (section != "pits") && (section != "sec_info_upd") && (textForWindow.Length > 0))
                        {
                            //Form_AddText(textForWindow);
                            textForWindow = "";
                        }

                        line = "";
                        str = "";
                        for (var i = 0; i < xr.AttributeCount; i++)
                        {
                            str = str + xr.GetAttribute(i) + ";";
                        }
                    }
                    if (xr.NodeType == XmlNodeType.EndElement)
                    {
                        //line = "";
                        //section = "";
                    }
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        str = str + evalue + ";";
                    }
                }

                // данные для рынков
                if (section == "markets")
                {
                    //xe = (XElement)XNode.ReadFrom(xr);

                    if (ename == "market")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (var i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add market: " + str;
                            str = "";
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }

                // данные для таймфреймов
                if (section == "candlekinds")
                {
                    if (ename == "kind")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add kind: " + str;
                            //On_New_Timeframe(str);
                            str = "";
                        }
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }

                // данные для инструментов
                if (section == "securities")
                {
                    if (ename == "security")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (var i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.EndElement)
                        {
                            line = "add security: " + str;
                            //On_New_Security(str);
                            str = "";
                        }
                    }
                    else
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            for (var i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                        }
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                        }
                    }
                }

                // данные по свечам
                if (section == "candles")
                {
                    if (ename == "candles")
                    {

                    }
                    if (ename == "candle")
                    {

                    }
                }

                // данные по клиенту
                if (section == "client")
                {
                    if (ename == "client")
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            line = "";
                            str = "";
                            for (var i = 0; i < xr.AttributeCount; i++)
                            {
                                str = str + xr.GetAttribute(i) + ";";
                            }
                            // определение параметров клиента
                            //string[] с_attrs = str.Split(';');
                            //if (с_attrs.Length > 0)
                            //{
                            //    ClientCode = с_attrs[0];
                            //}
                            line = "add client: " + str;
                        }
                    }
                    else
                    {
                        line = "";
                        if (xr.NodeType == XmlNodeType.Text)
                        {
                            str = str + evalue + ";";
                            line = "set: " + ename + "=" + evalue;
                        }
                    }
                }

                // данные для позиций
                if (section == "positions")
                {
                    line = "";
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        line = ename + ": " + evalue;
                    }
                }

                if (section == "overnight")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        line = "";
                        str = "";
                        for (var i = 0; i < xr.AttributeCount; i++)
                        {
                            str = str + "<" + xr.GetAttribute(i) + ">;";
                        }
                        line = "set overnight status: " + str;
                    }
                }

                // данные о статусе соединения с сервером
                if (section == "server_status")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        line = "";
                        str = "";
                        var attr_connected = xr.GetAttribute("connected");
                        //if (attr_connected == "true") bConnected = true;
                        //if (attr_connected == "false") bConnected = false;

                        for (var i = 0; i < xr.AttributeCount; i++)
                        {
                            str = str + i + ":<" + xr.GetAttribute(i) + ">;";
                        }
                        line = "set server_status: " + str;
                    }
                }

                if (section == "orders") //обрабатываем заявки
                {

                }

                if (section == "alltrades")
                {

                }
                if (section == "ticks")
                {

                }

                if (line.Length > 0)
                {
                    //line = new string(' ',xr.Depth*2) + line;
                    if (info.Length > 0) info = info + (char)13 + (char)10;
                    info = info + line;
                }
            }

            return section;
        }
    }
}
