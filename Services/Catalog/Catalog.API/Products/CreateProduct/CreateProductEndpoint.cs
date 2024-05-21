namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price)
                                   : ICommand<CreateProductResponse>;
public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : IQueryHandler<CreateProductRequest, CreateProductResponse>
{
  public Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
