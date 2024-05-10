using WebApplication2.Dto;

namespace WebApplication2.Services
{
    namespace WebApplication2.Services
    {
        public interface IWarehouseService
        {
            Task<int> RegisterProductInWarehouseAsync(RegisterProductInWarehouseRequestDTO dto);
            Task<int> RegisterProductInWarehouseUsingProcedureAsync(RegisterProductInWarehouseRequestDTO dto);
        }
    }

}
