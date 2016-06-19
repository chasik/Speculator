using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using Speculator.MoexData;
using SpeculatorModel.MainData;
using SpeculatorModel.MoexHistory;

namespace Speculator.ViewModels.Data
{
    [POCOViewModel]
    public class MoexDataViewModel
    {
        protected MoexDataClient MoexDataClient { get; set; }
        protected virtual IOpenFileDialogService OpenFileDialogService => null;

        public virtual ObservableCollection<MoexSystem> MoexSystems { get; set; }
        public virtual ObservableCollection<MoexSymbol> MoexSymbols { get; set; }

        public MoexDataViewModel()
        {
            MoexDataClient = new MoexDataClient();
            MoexDataClient.SystemsCompleted +=
                (sender, eventArgs) => { MoexSystems = new ObservableCollection<MoexSystem>(eventArgs.Result); };
            MoexDataClient.SymbolsCompleted +=
                (sender, eventArgs) => { MoexSymbols = new ObservableCollection<MoexSymbol>(eventArgs.Result); };
            MoexDataClient.SystemsAsync();
            MoexDataClient.SymbolsAsync();
        }

        public void LoadHistoryFromFile()
        {
            if (OpenFileDialogService.ShowDialog())
            {
                BaseInfo lastBaseInfo = null;
                var allTrades = new List<MoexTrade>();
                var allClaims = new List<MoexClaim>();
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
                            {
                                var result = GetValuesInRow(columns, oneLine);
                                if (result is MoexClaim)
                                {
                                    // TODO пересмотреть связи между заявками и сделками в moex history файлах

                                    // в исходном файле бывают две записи с одинаковым id (одна из них сведа в сделку, одна нет)
                                    // что бы не дублировались - заменяем
                                    if ((lastBaseInfo as MoexClaim)?.Id != (result as MoexClaim)?.Id)
                                        allClaims.Add(result as MoexClaim);
                                    else if ((result as MoexClaim)?.PriceDeal != null)
                                    {
                                        var indexOldValue = allClaims.IndexOf(allClaims.Last());
                                        allClaims[indexOldValue] = result as MoexClaim;}
                                }
                                else
                                    allTrades.Add(result as MoexTrade);
                                lastBaseInfo = result;
                            }
                        }
                    }
                });

                // так как заявок даже за месяц по одному рынку очень много - делим на порции и отправляем частями для сохранения сервису
                var claimsPortionCounter = 0;
                while (claimsPortionCounter < allClaims.Count)
                {
                    MoexDataClient.AddClaims(allClaims.GetRange(claimsPortionCounter, 100000).ToArray());
                    claimsPortionCounter += 100000;
                }

                // ну а трейды можно сохранить одним заходом, так как их на много меньше. // Если будет и их большой объем - переделать сохранение на порционное 
                MoexDataClient.AddTradesAsync(allTrades.ToArray());
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
                var value = values[i];
                switch (columns[i])
                {
                    case "MOMENT":
                        entity.Moment = DateTime.ParseExact(value, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                        break;
                    case "SYMBOL":
                        if (MoexSymbols.All(s => s.Name != value))
                            MoexSymbols.Add(MoexDataClient.AddSymbol(new MoexSymbol {Name = value}));
                        entity.MoexSymbolId = MoexSymbols.Single(s => s.Name == value).Id;
                        break;
                    case "SYSTEM":
                        if (MoexSystems.All(s => s.Name != value))
                            MoexSystems.Add(MoexDataClient.AddSystem(new MoexSystem {Name = value}));
                        entity.MoexSystemId = MoexSystems.Single(s => s.Name == value).Id;
                        break;
                    case "ID":
                        entity.Id = long.Parse(value);
                        break;
                    case "ID_DEAL":
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (entity is MoexClaim)
                                ((MoexClaim) entity).MoexTradeId = long.Parse(value);
                            else
                                entity.Id = long.Parse(value);
                        }
                        break;
                    case "PRICE":
                        entity.Price = decimal.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "PRICE_DEAL":
                        if (!string.IsNullOrEmpty(value))
                        {
                            var price = decimal.Parse(value, CultureInfo.InvariantCulture);
                            if (entity is MoexClaim)
                                ((MoexClaim) entity).PriceDeal = price;
                            else
                                entity.Price = price;
                        }
                        break;
                    case "VOLUME":
                        entity.Volume = int.Parse(value);
                        break;
                    case "OPEN_POS":
                        ((MoexTrade)entity).OpenInterest = int.Parse(value);
                        break;
                    case "DIRECTION":
                    case "TYPE":
                        entity.DiractionId = value == "B" ? (byte)DiractionEnum.Buy : (byte)DiractionEnum.Sell;
                        break;
                    case "ACTION":
                        ((MoexClaim) entity).ClaimActionId = (byte)(int.Parse(value) + 1);
                        break;
                }
            }
            return entity;
        }
    }
}