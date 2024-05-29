using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10)
  : IRequest<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session)
  : IRequestHandler<GetProductsQuery, GetProductsResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));

  public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
  {
    // Get all products from the DB
    var products = await _session.Query<Product>().ToPagedListAsync(request.PageNumber ?? 1, request.PageSize ?? 10, cancellationToken);

    // Return GetProductsResult result
    return new GetProductsResult(products);
  }
}
