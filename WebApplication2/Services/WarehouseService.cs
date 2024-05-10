using WebApplication2.Dto;
using WebApplication2.Exceptions;
using WebApplication2.Models;
using WebApplication2.Repositories;
using WebApplication2.Services.WebApplication2.Services;

namespace WebApplication2.Services;



public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    public WarehouseService(IWarehouseRepository warehouseRepository, IProductRepository productRepository, IOrderRepository orderRepository)
    {
        _warehouseRepository = warehouseRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<int> RegisterProductInWarehouseAsync(RegisterProductInWarehouseRequestDTO dto)
    {
        // Example Flow:
        // check if product exists else throw NotFoundException
        // if (_productRepository.GetProductByIdAsync(dto.IdProduct) is null)
        //    throw new NotFoundException("adsasdasd");
        if (!await _productRepository.ProductExistsAsync(dto.IdProduct.Value))
            throw new NotFoundException("Product does not exist.");
        // check if warehouse exists else throw NotFoundException
        if (!await _warehouseRepository.WarehouseExistsAsync(dto.IdWarehouse.Value))
            throw new NotFoundException("Warehouse does not exist.");

        if (dto.Amount <= 0)
            throw new ConflictException("Amount must be greater than zero.");
        // get order if exists else throw NotFoundException
        var idOrder = await _orderRepository.OrderExistsAsync(dto.IdProduct.Value, dto.Amount, dto.CreatedAt.Value);
        if (idOrder == null) // Use == null instead of !idOrder.HasValue
            throw new NotFoundException("No valid order found for this product and amount before the given date.");
        // check if product is already in warehouse else throw ConflictException
        if (await _orderRepository.IsOrderFulfilledAsync(idOrder.Value))
            throw new ConflictException("This order has already been fulfilled.");

        var idProductWarehouse = await _warehouseRepository.RegisterProductInWarehouseAsync(
            idWarehouse: dto.IdWarehouse!.Value,
            idProduct: dto.IdProduct!.Value,
            idOrder: idOrder.Value,
            amount: dto.Amount,
            createdAt: DateTime.UtcNow);

        if (!idProductWarehouse.HasValue)
            throw new Exception("Failed to register product in warehouse");

        return idProductWarehouse.Value;
    }

    public async Task<int> RegisterProductInWarehouseUsingProcedureAsync(RegisterProductInWarehouseRequestDTO dto)
    {
        // Example Flow:
        // check if product exists else throw NotFoundException
        // if (_productRepository.GetProductByIdAsync(dto.IdProduct) is null)
        //    throw new NotFoundException("adsasdasd");
        if (!await _productRepository.ProductExistsAsync(dto.IdProduct.Value))
            throw new NotFoundException("Product does not exist.");
        // check if warehouse exists else throw NotFoundException
        if (!await _warehouseRepository.WarehouseExistsAsync(dto.IdWarehouse.Value))
            throw new NotFoundException("Warehouse does not exist.");

        if (dto.Amount <= 0)
            throw new ConflictException("Amount must be greater than zero.");
        // get order if exists else throw NotFoundException
        var idOrder = await _orderRepository.OrderExistsAsync(dto.IdProduct.Value, dto.Amount, dto.CreatedAt.Value);
        if (!idOrder.HasValue)
            throw new NotFoundException("No valid order found for this product and amount before the given date.");

        // check if product is already in warehouse else throw ConflictException
        if (await _orderRepository.IsOrderFulfilledAsync(idOrder.Value))
            throw new ConflictException("This order has already been fulfilled.");

        var idProductWarehouse = await _warehouseRepository.RegisterProductInWarehouseByProcedureAsync(
            idWarehouse: dto.IdWarehouse.Value,
            idProduct: dto.IdProduct.Value,
            amount: dto.Amount,
            createdAt: dto.CreatedAt.Value);

        return idProductWarehouse;
    }

}

