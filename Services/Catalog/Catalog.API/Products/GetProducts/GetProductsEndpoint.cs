
namespace Catalog.API.Products.GetProducts;

// public record GetProductsRequest();

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/products", async (ISender sender) =>
    {
      var result = await sender.Send(new GetProductQuery());
      var response = result.Adapt<GetProductsResponse>();
      return Results.Created($"/products", response);
    }).WithName("GetProducts")
      .Produces<GetProductsResponse>(StatusCodes.Status201Created)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .WithSummary("Get Products")
      .WithDescription("Get Products");
  }
}
