using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Queries.Products
{
    public class GetProductByIdQuery : IRequest<ProductResponse>
    {
        public string Id { get; set; }
        public GetProductByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetProductById(request.Id);
            if (product == null) throw new Exception($"Product with id {request.Id} not found");
            return mapper.Map<ProductResponse>(product);
        }
    }
}
