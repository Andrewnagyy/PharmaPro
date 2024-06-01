using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.ProductFT.Command.AddOffer
{
    public class AddOfferCommand : IRequest<APIResponse<AddOfferCommandResponse>>
    {
        public Guid ProductId { get; set; }
        public int Discount { get; set; }
        //  public bool Offer { get; set; }
    }
}
