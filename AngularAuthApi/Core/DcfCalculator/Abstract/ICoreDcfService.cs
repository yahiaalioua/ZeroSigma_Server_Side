using AngularAuthApi.Core.DcfCalculator.Models;

namespace AngularAuthApi.Core.DcfCalculator.Abstract
{
    public interface ICoreDcfService
    {
        Task<List<CashFlowStatements>> CashFlowStatement(string ticker);
        Task<List<BalanceSheet>> BalanceSheet(string ticker);
        Task<List<IncomeStatements>> IncomeStatement(string ticker);
        Task<double> ReinvestementRate(string ticker);
        Task<double> ReturnOnCapital(string ticker);
        Task<double> ExpectedGrowthrate(string ticker);
        Task<double> FiveYearsExpectedReinvestementRate(string ticker);
        Task<double> AfterTaxOperatingIncome(string ticker);
        Task<List<double>> FreeCashFlow(string ticker);
        Task<List<double>> FutureFreeCashFlow(string ticker);
        Task<List<double>> DiscountFutureFreeCashFlow(string ticker);
        Task<double> TerminalValue(string ticker);
        Task<double> SumDiscountedFreeCashFlow(string ticker);
        Task<double> IntristicShareValue(string ticker);
    }
}