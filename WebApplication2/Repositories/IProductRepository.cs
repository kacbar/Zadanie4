namespace WebApplication2.Repositories
{
    public interface IProductRepository
    {
        Task<bool> ProductExistsAsync(int productId);
    }
}
