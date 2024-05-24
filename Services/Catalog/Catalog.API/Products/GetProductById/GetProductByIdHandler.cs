namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdHandler(IDocumentSession session) : IRequestHandler<GetProductByIdQuery, GetProductByIdResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));

  public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
  {
    // Get all product by ID from the DB
    var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

    // Throw an exception if the product is null
    if (product is null)
      throw new ProductNotFoundException();

    // Return GetProductByIdResult result
    return new GetProductByIdResult(product);
  }
}
