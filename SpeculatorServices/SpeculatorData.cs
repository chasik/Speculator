using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SpeculatorModel;
using SpeculatorModel.MainData;
using SpeculatorModel.MoexHistory;

namespace SpeculatorServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SpeculatorData : ISpeculatorData
    {
        static SpeculatorData()
        {
            SeedMethod();
        }

        public void GetHistory(string symbol)
        {
            throw new NotImplementedException();
        }

        public DataSource[] DataSources()
        {
            using (var dbContext = new SpeculatorContext())
            {
                return dbContext.DataSources.ToArray();
            }
        }

        public Symbol[] GetSymbols(DataSource selecteDataSource)
        {
            using (var dbContext = new SpeculatorContext())
            {
                if (selecteDataSource.Id == (byte) DataSourceEnum.SmartCom)
                {return dbContext.SmartComSymbols
                        .Select(
                            s =>
                                new Symbol
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    ShortName = s.ShortName,
                                    LongName = s.LongName,
                                    Step = s.Step,
                                    LotSize = s.LotSize,
                                    Punkt = s.Punkt
                                })
                        .ToArray();
                }
                return null;
            }
        }

        private static void SeedMethod()
        {
            using (var dbContext = new SpeculatorContext())
            {
                if (!dbContext.DataSources.Any())
                {
                    dbContext.DataSources.AddRange(new List<DataSource>
                    {
                        new DataSource {Id = (byte) DataSourceEnum.SmartCom, Name = "SmartCom"},
                        new DataSource {Id = (byte) DataSourceEnum.Transaq, Name = "Transaq"},
                        new DataSource {Id = (byte) DataSourceEnum.Quik, Name = "Quik"},
                        new DataSource {Id = (byte) DataSourceEnum.Plaza, Name = "Plaza"},
                        new DataSource {Id = (byte) DataSourceEnum.MoexHistory, Name = "MoexHistory"}
                    });
                }

                if (!dbContext.MoexClaimActions.Any())
                {
                    dbContext.MoexClaimActions.AddRange(new List<ClaimAction>
                    {
                        new ClaimAction {Id = (byte) ClaimActionEnum.Removed, Name = "Удалена" },
                        new ClaimAction {Id = (byte) ClaimActionEnum.Added, Name = "Добавлена" },
                        new ClaimAction {Id = (byte) ClaimActionEnum.Trade, Name = "Исполнена" }
                    });
                }

                if (!dbContext.MoexTradeDiractions.Any())
                {
                    dbContext.MoexTradeDiractions.AddRange(new List<Diraction>
                    {
                        new Diraction {Id = (byte) DiractionEnum.Buy, Name = "Покупка"},
                        new Diraction {Id = (byte) DiractionEnum.Sell, Name = "Продажа"}
                    });
                }


                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ee)
                {
                    
                }
            }
        }
    }
}