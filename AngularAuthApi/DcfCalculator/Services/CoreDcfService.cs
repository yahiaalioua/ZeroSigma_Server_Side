using AngularAuthApi.DcfCalculator.Abstract;
using AngularAuthApi.DcfCalculator.Models;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text.Json;

namespace AngularAuthApi.DcfCalculator.Services
{
    public class CoreDcfService : ICoreDcfService
    {
        private readonly IFinancialPrepHttpCalls _httpCalls;


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

        public async Task<FCFF> GetDataCalcFcff(string Ticker)
        {
            List<IncomeStatements> incomeStatement = await IncomeStatement(Ticker);
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(Ticker);
            FCFF freeCashFlowObject = new FCFF()
            {
                NetIncome = cashFlowStatement[0].netIncome,
                InterestExpanse = incomeStatement[0].interestExpense,
                IncomeTaxExpense = incomeStatement[0].incomeTaxExpense,
                DepreciationAndAmortization = incomeStatement[0].depreciationAndAmortization,
                CapitalExpenditure = cashFlowStatement[0].capitalExpenditure,
                ChangeInWorkingCapital = cashFlowStatement[0].changeInWorkingCapital,
            };
            return freeCashFlowObject;
        }
        public async Task<double> Fcff(string ticker)
        {
            FCFF data = await GetDataCalcFcff(ticker);
            double fcff = ((data.NetIncome + data.InterestExpanse + data.IncomeTaxExpense)
                * (1 - 0.21) - (data.CapitalExpenditure - data.DepreciationAndAmortization)
                - data.ChangeInWorkingCapital);
            return fcff;
        }
        public async Task<double> Wacc(string ticker, double sharePrice)
        {
            List<BalanceSheet> balanceSheet = await BalanceSheet(ticker);
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            double marketCapital = sharePrice * incomeStatement[0].weightedAverageShsOut;
            double equityWeight = marketCapital + balanceSheet[0].totalDebt;
            double debtWeight = balanceSheet[0].totalDebt / marketCapital + balanceSheet[0].totalDebt;
            double wacc = equityWeight * debtWeight;
            return wacc;
        }
        public async Task<double> ReinvestementRate(string ticker)
        {
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(ticker);
            double reinvestementRate = (cashFlowStatement[0].capitalExpenditure - incomeStatement[0].depreciationAndAmortization
               + cashFlowStatement[0].changeInWorkingCapital) /
               (incomeStatement[0].netIncome + incomeStatement[0].interestExpense + incomeStatement[0].incomeTaxExpense);

            return reinvestementRate;
        }
        public async Task<double> ReturnOnCapital(string ticker)
        {
            List<BalanceSheet> balanceSheet = await BalanceSheet(ticker);
            List<IncomeStatements> incomeStatement = await IncomeStatement(ticker);
            List<CashFlowStatements> cashFlowStatement = await CashFlowStatement(ticker);
            double retrunOnCapital = (incomeStatement[0].netIncome + incomeStatement[0].interestExpense +
               incomeStatement[0].incomeTaxExpense) / (balanceSheet[1].totalStockholdersEquity + balanceSheet[1].totalDebt
               - balanceSheet[1].cashAndCashEquivalents);

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
            double afterTaxOperatingIncome = incomeStatement[0].netIncome + incomeStatement[0].incomeTaxExpense + incomeStatement[0].incomeTaxExpense * (1 - 0.21);
            return afterTaxOperatingIncome;
        }
        public async Task<List<List<double>>> ExpectedFcff(string ticker)
        {
            double fiveYearsexpectedReinvestementRate = await FiveYearsExpectedReinvestementRate(ticker);
            double expectedGrowthRate = await ExpectedGrowthrate(ticker);
            double afterTaxOperatingIncome = await AfterTaxOperatingIncome(ticker);
            int years = 6;
            List<double> afterTaxOperatingIncomeGrowth = new List<double>();
            List<double> reinvestementIncome = new List<double>();
            List<double> fcff = new List<double>();
            List<List<double>> expectedFinancials = new List<List<double>>();

            afterTaxOperatingIncomeGrowth.Add(afterTaxOperatingIncome);
            fcff.Add(0);
            for (int i = 1; i < years; i++)
            {
                double expectedAfterTaxOpIncome = Math.Pow((afterTaxOperatingIncome * (1 + expectedGrowthRate)), i);
                double expectedReinvestment = fiveYearsexpectedReinvestementRate * expectedAfterTaxOpIncome;
                double expectedFcff = expectedAfterTaxOpIncome - expectedReinvestment;
                afterTaxOperatingIncomeGrowth.Add(expectedAfterTaxOpIncome);
                reinvestementIncome.Add(expectedReinvestment);
                fcff.Add(expectedFcff);
            }
            expectedFinancials.Add(afterTaxOperatingIncomeGrowth);
            expectedFinancials.Add(reinvestementIncome);
            expectedFinancials.Add(fcff);
            return expectedFinancials;
        }
        public async Task<double> TerminalValue(string ticker, double sharePrice)
        {
            double wacc = await Wacc(ticker, sharePrice);
            double termianlGrowthRate = 0.3 / wacc;
            List<List<double>> expectedFinancials = await ExpectedFcff(ticker);
            double terminalAfterTaxOpIncome = expectedFinancials[0][^1] * (1 + 0.03);
            double terminalReinvestmentRate = terminalAfterTaxOpIncome * termianlGrowthRate;
            double TerminalFcff = terminalAfterTaxOpIncome - terminalReinvestmentRate;
            double TerminalValue = TerminalFcff / (wacc - terminalReinvestmentRate);
            double presentTerminalValue = Math.Pow((TerminalValue / (1 + wacc)), (expectedFinancials[2].Count)) + 1;
            return presentTerminalValue;
        }
        public async Task<double> PvFcff(string ticker, double sharePrice)
        {
            double wacc = await Wacc(ticker, sharePrice);
            List<List<double>> expectedFinancials = await ExpectedFcff(ticker);
            double pvFcff = 0;
            for (int i = 0; i < expectedFinancials[1].Count; i++)
            {
                pvFcff += expectedFinancials[2][i] / Math.Pow((1 + wacc), i);
            }
            return pvFcff;
        }
        public async Task<double> ValueOperatingAsset(string ticker, double sharePrice)
        {
            double pvFcff = await PvFcff(ticker, sharePrice);
            double terminalValue = await TerminalValue(ticker, sharePrice);
            double valueOperatingAsset = pvFcff + terminalValue;
            return valueOperatingAsset;
        }
        public async Task<double> ValueOfFirm(string ticker, double sharePrice)
        {
            List<BalanceSheet> balanceSheet = await BalanceSheet(ticker);
            double valueOperatingAsset = await ValueOperatingAsset(ticker, sharePrice);
            double valueOfFirm = valueOperatingAsset + balanceSheet[0].cashAndCashEquivalents;
            return valueOfFirm;
        }
        public async Task<double> ValueOfEquity(string ticker, double sharePrice)
        {
            List<BalanceSheet> balanceSheet = await BalanceSheet(ticker);
            double valueOfFirm = await ValueOfFirm(ticker, sharePrice);
            double valueOfEquity = valueOfFirm + balanceSheet[0].totalDebt;
            return valueOfEquity;
        }
    }
}
