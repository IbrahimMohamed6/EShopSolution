using Basket.Api.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api._Basket.StoreBasket
{

    public record StoreBasketRequest(ShoppingCart ShoppingCart);

    public record StoreBasketResponse(bool IsSuccess, string userName);
    public class StoreBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var commend = sender.Adapt<StoreBasketCommand>();
                var result = sender.Send(commend);

                var response = sender.Adapt<StoreBasketResponse>();

                return Results.Created($"/basket/{response.userName}",response);
            });
        }
    }
}
