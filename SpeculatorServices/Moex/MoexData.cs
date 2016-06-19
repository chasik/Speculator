using System.Linq;
using System.ServiceModel;
using SpeculatorModel;
using SpeculatorModel.MoexHistory;

namespace SpeculatorServices.Moex
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MoexData : IMoexData
    {
        public MoexSystem AddSystem(MoexSystem system)
        {
            using (var dbContext = new SpeculatorContext())
            {
                var result = dbContext.MoexSystems.Add(system).Entity;
                dbContext.SaveChanges();
                return result;
            }
        }

        public MoexSymbol AddSymbol(MoexSymbol symbol)
        {
            using (var dbContext = new SpeculatorContext())
            {
                var result = dbContext.MoexSymbols.Add(symbol).Entity;
                dbContext.SaveChanges();
                return result;
            }
        }

        public void AddClaims(MoexClaim[] claims)
        {
            using (var dbContext = new SpeculatorContext())
            {
                var counter = 0;
                var existClaims = dbContext.MoexClaims.Select(c => c.Id).ToList();
                foreach (var claim in claims.Where(claim => !existClaims.Contains(claim.Id)))
                {
                    counter++;
                    dbContext.MoexClaims.Add(claim);
                    if (counter % 200 == 0)
                        dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }
        }
        
        public void AddTrades(MoexTrade[] trades)
        {
            using (var dbContext = new SpeculatorContext())
            {
                var counter = 0;
                var existTrades = dbContext.MoexTrades.Select(t => t.Id).ToList();
                foreach (var trade in trades.Where(trade => !existTrades.Contains(trade.Id)))
                {
                    counter++;
                    dbContext.MoexTrades.Add(trade);
                    if (counter % 200 == 0)
                        dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }
        }

        public MoexSystem[] Systems()
        {
            using (var dbContext = new SpeculatorContext())
            {
                return dbContext.MoexSystems.ToArray();
            }
        }

        public MoexSymbol[] Symbols()
        {
            using (var dbContext = new SpeculatorContext())
            {
                return dbContext.MoexSymbols.ToArray();
            }
        }
    }
}
