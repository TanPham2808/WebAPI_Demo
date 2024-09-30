using WebAPI_Demo.ServicesCondition.IServiceCondition;

namespace WebAPI_Demo.ServicesCondition
{
    public class VietNamTaxCaculator : ITaxCalculator
    {
        public int Caculate()
        {
            return 10;
        }
    }
}
