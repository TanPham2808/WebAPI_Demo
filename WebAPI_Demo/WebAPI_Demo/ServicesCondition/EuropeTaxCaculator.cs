using WebAPI_Demo.ServicesCondition.IServiceCondition;

namespace WebAPI_Demo.ServicesCondition
{
    public class EuropeTaxCaculator : ITaxCalculator
    {
        public int Caculate()
        {
            return 20;
        }
    }
}
