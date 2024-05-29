namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,
                                   string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price) : ICommand<UpdateProductCommandResult>;

public record UpdateProductCommandResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
  public UpdateProductCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty()
                      .WithMessage("Product ID is required");
    RuleFor(x => x.Name).NotEmpty()
                        .WithMessage("Name is required")
                        .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
    RuleFor(x => x.Price).GreaterThan(0)
                            .WithMessage("Price must be greater than 0");
  }
}

internal class UpdateCommandProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));

  public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
  {
    // Get product by ID from the DB
    var product = await _session.LoadAsync<Product>(request.Id, cancellationToken) ?? throw new ProductNotFoundException(request.Id);

    // Update the product
    product.Name = request.Name;
    product.Category = request.Category;
    product.Description = request.Description;
    product.ImageFile = request.ImageFile;
    product.Price = request.Price;

    // Save changes
    _session.Update(product);
    await _session.SaveChangesAsync(cancellationToken);

    // Return UpdateProductCommandResult result
    return new UpdateProductCommandResult(true);
  }
}
