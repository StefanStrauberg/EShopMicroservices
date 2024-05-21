using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price)
                                   : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

internal class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
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
    // return CreateProductResult result
    return new CreateProductResult(Guid.NewGuid());
  }
}
