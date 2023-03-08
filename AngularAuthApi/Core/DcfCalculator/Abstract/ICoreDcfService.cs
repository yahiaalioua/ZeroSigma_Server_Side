using AngularAuthApi.Core.DcfCalculator.Models;

namespace AngularAuthApi.Core.DcfCalculator.Abstract
{
    public interface ICoreDcfService
    {
        Task<List<CashFlowStatements>> CashFlowStatement(string ticker);
        Task<List<BalanceSheet>> BalanceSheet(string ticker);
        Task<List<IncomeStatements>> IncomeStatement(string ticker);
        Task<FCFF> GetDataCalcFcff(string Ticker);
        Task<double> Fcff(string ticker);
        Task<double> Wacc(string ticker, double sharePrice);
        Task<double> ReinvestementRate(string ticker);
        Task<double> ReturnOnCapital(string ticker);
        Task<double> ExpectedGrowthrate(string ticker);
        Task<double> FiveYearsExpectedReinvestementRate(string ticker);
        Task<double> AfterTaxOperatingIncome(string ticker);
        Task<List<List<double>>> ExpectedFcff(string ticker);
        Task<double> TerminalValue(string ticker, double sharePrice);
        Task<double> PvFcff(string ticker, double sharePrice);
        Task<double> ValueOperatingAsset(string ticker, double sharePrice);
        Task<double> ValueOfFirm(string ticker, double sharePrice);
        Task<double> ValueOfEquity(string ticker, double sharePrice);
        Task<double> result(string ticker, double sharePrice);
    }
}