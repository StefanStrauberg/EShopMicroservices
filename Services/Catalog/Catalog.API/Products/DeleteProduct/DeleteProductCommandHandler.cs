
namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));
  readonly ILogger<DeleteProductCommandHandler> _logger = logger
    ?? throw new ArgumentNullException(nameof(logger));

  public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
  {
    // Logging inforamtion that we going to delete a product
    _logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", request);

    // Delete the product from the DB
    _session.Delete<Product>(request.Id);
    await _session.SaveChangesAsync(cancellationToken);

    // Return DeleteProductResult result
    return new DeleteProductResult(true);
  }
}
