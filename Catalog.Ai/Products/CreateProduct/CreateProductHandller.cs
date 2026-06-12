using BuildingBlocks.CQRS;
using Catalog.Ai.Models;
using FluentValidation;
using Marten;
using MediatR;
using System.Windows.Input;

namespace Catalog.Ai.Products.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>
        ;
    public record CreateProductResult(Guid Id);

    public class CreateProductValidation
     : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
    public class CreateProductHandller  : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IDocumentSession _session;
        

        public CreateProductHandller(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Create New Product
            
            var product=new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            // Save To Database
            _session.Store(product);
            await _session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);


        }
    }
}
