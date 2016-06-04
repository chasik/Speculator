using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SpeculatorModel;
using SpeculatorModel.MainData;

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