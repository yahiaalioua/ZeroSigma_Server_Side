namespace AngularAuthApi.Core.DcfCalculator.Models
{
    public record FCFF
    {
        public double NetIncome { get; set; }
        public double InterestExpanse { get; set; }
        public double IncomeTaxExpense { get; set; }
        public double DepreciationAndAmortization { get; set; }
        public double CapitalExpenditure { get; set; }
        public double ChangeInWorkingCapital { get; set; }

    }
}
