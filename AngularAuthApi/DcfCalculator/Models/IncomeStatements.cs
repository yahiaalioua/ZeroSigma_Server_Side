namespace AngularAuthApi.DcfCalculator.Models
{
    public record IncomeStatements
    {
        public string date { get; set; }
        public string symbol { get; set; }
        public string reportedCurrency { get; set; }
        public string cik { get; set; }
        public string fillingDate { get; set; }
        public string acceptedDate { get; set; }
        public string calendarYear { get; set; }
        public string period { get; set; }
        public double revenue { get; set; }
        public double costOfRevenue { get; set; }
        public double grossProfit { get; set; }
        public double grossProfitRatio { get; set; }
        public double researchAndDevelopmentExpenses { get; set; }
        public double generalAndAdministrativeExpenses { get; set; }
        public double sellingAndMarketingExpenses { get; set; }
        public double sellingGeneralAndAdministrativeExpenses { get; set; }
        public double otherExpenses { get; set; }
        public double operatingExpenses { get; set; }
        public double costAndExpenses { get; set; }
        public double interestIncome { get; set; }
        public double interestExpense { get; set; }
        public double depreciationAndAmortization { get; set; }
        public double ebitda { get; set; }
        public double ebitdaratio { get; set; }
        public double operatingIncome { get; set; }
        public double operatingIncomeRatio { get; set; }
        public double totalOtherIncomeExpensesNet { get; set; }
        public double incomeBeforeTax { get; set; }
        public double incomeBeforeTaxRatio { get; set; }
        public double incomeTaxExpense { get; set; }
        public double netIncome { get; set; }
        public double netIncomeRatio { get; set; }
        public double eps { get; set; }
        public double epsdiluted { get; set; }
        public double weightedAverageShsOut { get; set; }
        public double weightedAverageShsOutDil { get; set; }
        public string link { get; set; }
        public string finalLink { get; set; }
    }
      
    
    
}
