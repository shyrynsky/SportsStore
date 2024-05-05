namespace Store.Models;

public interface IStoreRepository
{
    IQueryable<Product> Products { get; }
}