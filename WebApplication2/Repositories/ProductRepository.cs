using System.Data.SqlClient;
using WebApplication2.Exceptions;

namespace WebApplication2.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> ProductExistsAsync(int productId)
        {
            try
            {
                await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
                await connection.OpenAsync();

                var query = "SELECT COUNT(1) FROM Product WHERE IdProduct = @IdProduct";
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProduct", productId);

                var exists = (int)await command.ExecuteScalarAsync() > 0;
                return exists;
            }
            catch
            {
                throw new ConflictException("An error occurred while checking product existence.");
            }
            
        }
    }
}
