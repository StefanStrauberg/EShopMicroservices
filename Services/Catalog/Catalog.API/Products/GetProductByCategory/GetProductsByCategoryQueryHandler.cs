namespace Catalog.API.Products.GetProductByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));

  public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
  {
    // Get product by category from the DB
    var products = await _session.Query<Product>().Where(x => x.Category.Contains(request.Category)).ToListAsync(token: cancellationToken);
    // Return GetProductByCategoryResult result
    return new GetProductsByCategoryResult(products);
  }
}
