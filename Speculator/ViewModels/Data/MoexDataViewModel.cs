using System;
using System.Globalization;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using SpeculatorModel.MoexHistory;

namespace Speculator.ViewModels.Data
{
    [POCOViewModel]
    public class MoexDataViewModel
    {
        protected virtual IOpenFileDialogService OpenFileDialogService => null;


        public void LoadHistoryFromFile()
        {
            if (OpenFileDialogService.ShowDialog())
            {
                OpenFileDialogService.Files.ForEach(file =>
                {
                    using (var stream = file.OpenText())
                    {
                        string[] columns = null;
                        string oneLine;
                        while ((oneLine = stream.ReadLine()) != null)
                        {
                            if (oneLine[0] == '#')
                                columns = oneLine.Substring(1).ToUpper().Split(',');
                            else if (columns != null)
                                GetValuesInRow(columns, oneLine);
                        }
                    }});
            }
        }

        private BaseInfo GetValuesInRow(string[] columns, string oneLine)
        {
            BaseInfo entity = null;
            if (columns.Contains("OPEN_POS"))
                entity = new MoexTrade();
            else
                entity = new MoexClaim();

            var values = oneLine.Split(',');
            for (var i = 0; i < values.Length; i++)
            {
                switch (columns[i])
                {
                    case "MOMENT":
                        entity.Moment = DateTime.ParseExact(values[i], "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                        break;

                    case "SYMBOL":
                        break;
                    case "SYSTEM":
                        break;
                    case "ID_DEAL":
                        break;
                    case "PRICE_DEAL":
                        break;
                    case "VOLUME":
                        break;
                    case "OPEN_POS":
                        break;
                    case "DIRECTION":
                        break;
                    case "TYPE":
                        break;
                    case "ID":
                        break;
                    case "ACTION":
                        break;
                    case "PRICE":
                        break;
                }
            }
            return entity;
        }
    }
}