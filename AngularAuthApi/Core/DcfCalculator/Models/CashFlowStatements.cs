namespace AngularAuthApi.Core.DcfCalculator.Models
{
    public record CashFlowStatements
    {
        public string date { get; set; }
        public string symbol { get; set; }
        public string reportedCurrency { get; set; }
        public string cik { get; set; }
        public string fillingDate { get; set; }
        public string acceptedDate { get; set; }
        public string calendarYear { get; set; }
        public string period { get; set; }
        public double netIncome { get; set; }
        public double depreciationAndAmortization { get; set; }
        public double deferredIncomeTax { get; set; }
        public double stockBasedCompensation { get; set; }
        public double changeInWorkingCapital { get; set; }
        public double accountsReceivables { get; set; }
        public double inventory { get; set; }
        public double accountsPayables { get; set; }
        public double otherWorkingCapital { get; set; }
        public double otherNonCashItems { get; set; }
        public double netCashProvidedByOperatingActivities { get; set; }
        public double investmentsInPropertyPlantAndEquipment { get; set; }
        public double acquisitionsNet { get; set; }
        public double purchasesOfInvestments { get; set; }
        public double salesMaturitiesOfInvestments { get; set; }
        public double otherInvestingActivites { get; set; }
        public double netCashUsedForInvestingActivites { get; set; }
        public double debtRepayment { get; set; }
        public double commonStockIssued { get; set; }
        public double commonStockRepurchased { get; set; }
        public double dividendsPaid { get; set; }
        public double otherFinancingActivites { get; set; }
        public double netCashUsedProvidedByFinancingActivities { get; set; }
        public double effectOfForexChangesOnCash { get; set; }
        public double netChangeInCash { get; set; }
        public double cashAtEndOfPeriod { get; set; }
        public double cashAtBeginningOfPeriod { get; set; }
        public double operatingCashFlow { get; set; }
        public double capitalExpenditure { get; set; }
        public double freeCashFlow { get; set; }
        public string link { get; set; }
        public string finalLink { get; set; }
    }
}
