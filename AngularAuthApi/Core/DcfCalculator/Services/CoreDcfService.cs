using AngularAuthApi.Core.DcfCalculator.Abstract;
using AngularAuthApi.Core.DcfCalculator.Models;
using Newtonsoft.Json;


namespace AngularAuthApi.Core.DcfCalculator.Services
{
    public class CoreDcfService : ICoreDcfService
    {
        private readonly IFinancialPrepHttpCalls _httpCalls;
        public List<IncomeStatements> incomeStatement;

        private static double perpetualRate = 0.03;
        private static double requiredRate = 0.10;




        public CoreDcfService(IFinancialPrepHttpCalls httpCalls)
        {
            _httpCalls = httpCalls;
        }
        public async Task<List<IncomeStatements>> IncomeStatement(string ticker)
        {
            List<IncomeStatements> incomeStatement = JsonConvert.DeserializeObject<List<IncomeStatements>>(await _httpCalls.GetIncomeStatements(ticker));
            return incomeStatement;
        }
        public async Task<List<BalanceSheet>> BalanceSheet(string ticker)
        {
            List<BalanceSheet> balanceSheets = JsonConvert.DeserializeObject<List<BalanceSheet>>(await _httpCalls.GetBalanceSheet(ticker));
            return balanceSheets;
        }
        public async Task<List<CashFlowStatements>> CashFlowStatement(string ticker)
        {
            List<CashFlowStatements> cashFlowStatement = JsonConvert.DeserializeObject<List<CashFlowStatements>>(await _httpCalls.GetCashFlowStatements(ticker));
            return cashFlowStatement;
        }

        
        
        public async Task<double> ReinvestementRate(string ticker)
        {
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(ticker);
            double reinvestementRate = ((Math.Abs(cashFlowStatement[0].capitalExpenditure) - incomeStatement[0].depreciationAndAmortization
               + cashFlowStatement[0].changeInWorkingCapital)) /
               ((incomeStatement[0].netIncome + incomeStatement[0].interestExpense) * (1 - 0.21));

            return reinvestementRate;
        }
        public async Task<double> ReturnOnCapital(string ticker)
        {
            List<BalanceSheet> balanceSheet = await BalanceSheet(ticker);
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(ticker);
            double retrunOnCapital = (incomeStatement[0].netIncome + incomeStatement[0].interestExpense +
               incomeStatement[0].incomeTaxExpense) / ((balanceSheet[0].totalAssets - balanceSheet[0].totalCurrentAssets) + (balanceSheet[0].totalCurrentAssets - balanceSheet[0].totalCurrentLiabilities));

            return retrunOnCapital;
        }
        public async Task<double> ExpectedGrowthrate(string ticker)
        {
            double expectedGrowthRate = await ReinvestementRate(ticker) * await ReturnOnCapital(ticker);
            return expectedGrowthRate;

        }
        public async Task<double> FiveYearsExpectedReinvestementRate(string ticker)
        {
            double rRate = await ExpectedGrowthrate(ticker) / await ReturnOnCapital(ticker);
            return rRate;

        }
        public async Task<double> AfterTaxOperatingIncome(string ticker)
        {
            List<BalanceSheet> balanceSheet = await BalanceSheet(ticker);
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(ticker);
            double afterTaxOperatingIncome = incomeStatement[0].netIncome + incomeStatement[0].interestExpense + incomeStatement[0].incomeTaxExpense * (1 - 0.21);
            return afterTaxOperatingIncome;
        }
        public async Task<List<double>> FreeCashFlow(string ticker)
        {
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(ticker);
            List<double> freeCashFlow = new List<double>();
            foreach (var item in cashFlowStatement)
            {
                freeCashFlow.Add(item.freeCashFlow);
            }

            return freeCashFlow;
        }

        public async Task<List<double>> FutureFreeCashFlow(string ticker)
        {
            List<double>freeCashFlow=await FreeCashFlow(ticker);
            List<double> futureFreeCashFlow = new List<double>();
            List<double> discountFactor = new List<double>();

            double expectedGrowthRate=await ExpectedGrowthrate(ticker);
            for (int i = 1; i < freeCashFlow.Count; i++)
            {
                
                double cashFlow= (freeCashFlow[0] * Math.Pow((1 + expectedGrowthRate), i));
                futureFreeCashFlow.Add(cashFlow);

            }
            return futureFreeCashFlow;
        }
        public async Task<List<double>> DiscountFutureFreeCashFlow(string ticker)
        {
            List<double> futureFreeCashFlow = await FutureFreeCashFlow(ticker);
            List<double> discountedFreeCashFlow = new List<double>();
            List<double> discountFactor = new List<double>();
            for (int i = 0; i < futureFreeCashFlow.Count; i++)
            {
                discountFactor.Add(Math.Pow((1 + requiredRate), i));
                discountedFreeCashFlow.Add(futureFreeCashFlow[i] / discountFactor[i]);

            }
            return discountedFreeCashFlow;
        }
        public async Task<double> TerminalValue(string ticker)
        {

            
            List<double> fcff = await FreeCashFlow(ticker);
            double terminalValue = fcff[0] * (1 + perpetualRate) / (requiredRate - perpetualRate);
            return terminalValue;
        }
        public async Task<double> SumDiscountedFreeCashFlow(string ticker)
        {
            List<double> discountedFreeCashFlow = await DiscountFutureFreeCashFlow(ticker);
            double terminalValue = await TerminalValue(ticker);
            double discountedTerminalValue = terminalValue/ Math.Pow((1 + requiredRate), 5);
            discountedFreeCashFlow.Add(discountedTerminalValue);
            double sumAllCashFlow=discountedFreeCashFlow.Sum();
            return sumAllCashFlow;
        }
        public async Task<double> IntristicShareValue(string ticker)
        {
            double sumAllCashFlow = await SumDiscountedFreeCashFlow(ticker);
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            double totalSharesOustanding = incomeStatement[0].weightedAverageShsOut;
            return sumAllCashFlow/totalSharesOustanding;
        }

    }
}
