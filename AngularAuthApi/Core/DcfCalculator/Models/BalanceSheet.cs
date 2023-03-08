namespace AngularAuthApi.Core.DcfCalculator.Models
{
    public class BalanceSheet
    {
        public string date { get; set; }
        public string symbol { get; set; }
        public string reportedCurrency { get; set; }
        public string cik { get; set; }
        public string fillingDate { get; set; }
        public string acceptedDate { get; set; }
        public string calendarYear { get; set; }
        public string period { get; set; }
        public double cashAndCashEquivalents { get; set; }
        public double shortTermInvestments { get; set; }
        public double cashAndShortTermInvestments { get; set; }
        public double netReceivables { get; set; }
        public double inventory { get; set; }
        public double otherCurrentAssets { get; set; }
        public double totalCurrentAssets { get; set; }
        public double propertyPlantEquipmentNet { get; set; }
        public double goodwill { get; set; }
        public double intangibleAssets { get; set; }
        public double goodwillAndIntangibleAssets { get; set; }
        public double longTermInvestments { get; set; }
        public double taxAssets { get; set; }
        public double otherNonCurrentAssets { get; set; }
        public double totalNonCurrentAssets { get; set; }
        public double otherAssets { get; set; }
        public double totalAssets { get; set; }
        public double accountPayables { get; set; }
        public double shortTermDebt { get; set; }
        public double taxPayables { get; set; }
        public double deferredRevenue { get; set; }
        public double otherCurrentLiabilities { get; set; }
        public double totalCurrentLiabilities { get; set; }
        public double longTermDebt { get; set; }
        public double deferredRevenueNonCurrent { get; set; }
        public double deferredTaxLiabilitiesNonCurrent { get; set; }
        public double otherNonCurrentLiabilities { get; set; }
        public double totalNonCurrentLiabilities { get; set; }
        public double otherLiabilities { get; set; }
        public double capitalLeaseObligations { get; set; }
        public double totalLiabilities { get; set; }
        public double preferredStock { get; set; }
        public double commonStock { get; set; }
        public double retainedEarnings { get; set; }
        public double accumulatedOtherComprehensiveIncomeLoss { get; set; }
        public double othertotalStockholdersEquity { get; set; }
        public double totalStockholdersEquity { get; set; }
        public double totalEquity { get; set; }
        public double totalLiabilitiesAndStockholdersEquity { get; set; }
        public double minorityInterest { get; set; }
        public double totalLiabilitiesAndTotalEquity { get; set; }
        public double totalInvestments { get; set; }
        public double totalDebt { get; set; }
        public double netDebt { get; set; }
        public string link { get; set; }
        public string finalLink { get; set; }
    }

}
