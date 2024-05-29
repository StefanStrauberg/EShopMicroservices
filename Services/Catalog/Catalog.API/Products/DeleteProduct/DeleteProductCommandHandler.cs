
namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
  public DeleteProductCommandValidator()
  {
    RuleFor(x => x.Id).NotEmpty()
                      .WithMessage("Product ID is required");
  }
}

internal class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
  readonly IDocumentSession _session = session
    ?? throw new ArgumentNullException(nameof(session));

  public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
  {
    // Delete the product from the DB
    _session.Delete<Product>(request.Id);
    await _session.SaveChangesAsync(cancellationToken);

    // Return DeleteProductResult result
    return new DeleteProductResult(true);
  }
}
