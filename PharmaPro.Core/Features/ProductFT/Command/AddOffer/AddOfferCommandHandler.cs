using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.ProductFT.Command.AddOffer
{
    public class AddOfferCommandHandler : IRequestHandler<AddOfferCommand, APIResponse<AddOfferCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        public AddOfferCommandHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<APIResponse<AddOfferCommandResponse>> Handle(AddOfferCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.products.FindAsync(request.ProductId);

            if (product == null)
            {
                return new APIResponse<AddOfferCommandResponse>
                {
                    Errors = new List<string> { "Product not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            product.Discount = request.Discount;
            product.OldPrice = product.Price;
            product.Price -= (product.Price * (decimal)request.Discount / 100);
            product.Offer = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new APIResponse<AddOfferCommandResponse>
            {
                Data = new AddOfferCommandResponse
                {
                    Message = "Offer added Successfully"
                },
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
