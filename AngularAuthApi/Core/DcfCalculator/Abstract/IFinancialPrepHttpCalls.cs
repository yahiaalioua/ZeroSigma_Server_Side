﻿
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthApi.Core.DcfCalculator.Abstract
{
    public interface IFinancialPrepHttpCalls
    {
        Task<string> GetIncomeStatements(string Ticker);
        Task<string> GetBalanceSheet(string Ticker);
        Task<string> GetCashFlowStatements(string Ticker);
    }
}