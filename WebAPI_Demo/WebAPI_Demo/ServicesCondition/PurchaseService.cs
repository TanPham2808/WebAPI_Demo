
using WebAPI_Demo.Models;
using WebAPI_Demo.ServicesCondition.IServiceCondition;

namespace WebAPI_Demo.ServicesCondition
{
    public class PurchaseService
    {
        private readonly Func<UserLocations, ITaxCalculator> _accessor;

        public PurchaseService(Func<UserLocations, ITaxCalculator> accessor)
        {
            _accessor = accessor;
        }

        public int Checkout(UserLocations userLocations)
        {
            // Thuế được tính phụ thuộc vào location của user
            var tax = _accessor(userLocations);

            // Phương thức Caculate() sẽ lấy theo điều kiện UserLocation
            return tax.Caculate() + 100;
        }
    }
}
