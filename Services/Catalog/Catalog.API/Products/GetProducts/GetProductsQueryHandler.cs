namespace Catalog.API.Products.GetProducts;

public record GetProductQuery() : IRequest<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session) : IRequestHandler<GetProductQuery, GetProductsResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));

  public async Task<GetProductsResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
  {
    // Get all products from the DB
    var products = await _session.Query<Product>().ToListAsync(cancellationToken);

    // Return GetProductsResult result
    return new GetProductsResult(products);
  }
}
