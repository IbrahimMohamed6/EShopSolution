using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.Api._Basket.DeleteBasket
{
    // public record DeleteBasketRequest (string userName);

    public record DeleteBasketResponse(bool IsSuccess, string userName);
    public class DeleteBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketRequest(userName));

                var response = sender.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            });
        }
    }
}
