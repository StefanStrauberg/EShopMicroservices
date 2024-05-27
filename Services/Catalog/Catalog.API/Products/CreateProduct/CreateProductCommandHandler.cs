namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));
  public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
  {
    // create Product entity from command object
    var product = new Product()
    {
      Name = request.Name,
      Category = request.Category,
      Description = request.Description,
      ImageFile = request.ImageFile,
      Price = request.Price
    };

    // save to database
    _session.Store(product);
    await session.SaveChangesAsync(cancellationToken);

    // return CreateProductResult result
    return new CreateProductResult(product.Id);
  }
}
