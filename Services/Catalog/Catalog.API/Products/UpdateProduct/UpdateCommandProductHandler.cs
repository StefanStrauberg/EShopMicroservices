namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,
                                   string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price) : ICommand<UpdateProductCommandResult>;

public record UpdateProductCommandResult(bool IsSuccess);

internal class UpdateCommandProductHandler(IDocumentSession session, ILogger<UpdateCommandProductHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));
  readonly ILogger<UpdateCommandProductHandler> _logger = logger
    ?? throw new ArgumentNullException(nameof(logger));

  public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", request);

    // Get product by ID from the DB
    var product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

    // Throw the exception if the product not found
    if (product is null)
      throw new ProductNotFoundException();

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
