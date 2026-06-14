using Basket.Api.Models;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.Api._Basket.GetBasket
{

    //public record GetBasketReequest(string userName);


    public record GetBasketResponse(ShoppingCart ShoppingCart);
    public class GetBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender)

                =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));

                var response = sender.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            }
             );

            
        }
    }
}
