using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using SpeculatorModel;
using SpeculatorModel.Transaq;


namespace SpeculatorServices.Transaq
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

        public static void TransaqHandleData(string data)
        {
            // обработка данных, полученных коннектором от сервера Транзак

            var xs = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                DtdProcessing = DtdProcessing.Ignore
            };

            var xmlReader = XmlReader.Create(new System.IO.StringReader(data), xs);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            var xmlRoot = xmlDocument.DocumentElement;
            if (xmlRoot == null)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                foreach (XmlNode xmlNode in xmlRoot)
                {
                    switch (xmlNode.Name)
                    {
                        case "market":          // доступные рынки
                            int id;
                            int.TryParse(xmlNode.Attributes?.GetNamedItem("id").Value, out id);
                            var market = new Market {Id = id, Name = xmlNode.InnerText};
                            if (!dbContext.Markets.Any(m => m.Id == market.Id && m.Name == market.Name))
                                dbContext.Markets.Add(market);
                            break;
                        case "board":           // справочник режимов торгов
                            break;
                        case "candlekind":     // информация о доступных периодах свечей
                            break;
                        case "security":      // список инструментов
                            break;
                        case "candles":         // исторические данные
                            break;
                        case "client":          // клиентские счета
                            break;
                        case "positions":       // позиции клиентов
                            break;
                        case "overnight":       // ночной или дневной режим кредитования (больше не используется в системе)
                            break;
                        case "server_status":   // получить информацию о текущем состоянии соединения с Сервером
                            break;
                        case "order":           // заявки клиентов
                            break;
                        case "stoporder":       // стоп ордера клиентов
                            break;
                        case "negdeal":         // адресные заявки клиентов
                            break;
                        case "trade":           // сделки рынка по инструментам
                            if (xmlRoot.Name == "trades")
                            {
                                
                            }
                            else if (xmlRoot.Name == "alltrades")
                            {
                                
                            }
                            break;
                        case "tick":            // тиковые данные
                            break;
                        case "quote":           // глубина рынка по инструментам (“стакан”)
                            break;
                        default:
                            break;
                    }
                }
                dbContext.SaveChangesAsync();
            }
        }
    }
}
