using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Contract.Identity;
using PharmaPro.Domain.UserProducts;
using PharmaPro.DS;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, APIResponse<GetProductByIdQueryResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserToken _userToken;
        public GetProductByIdQueryHandler(AppDbContext appDbContext, IUserToken userToken)
        {
            _dbContext = appDbContext;
            _userToken = userToken;

        }
        public async Task<APIResponse<GetProductByIdQueryResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = await _userToken.GetUserIDFromToken();
            var productExist = await _dbContext.products
                .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (productExist == null)
            {
                return new APIResponse<GetProductByIdQueryResponse>()
                {
                    Errors = new List<string>()
                {
                    "Product is Not Found!"
                },
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                };
            }
            else
            {
                var product = await _dbContext.products.FindAsync(query.Id);

                var userProduct = new UserProduct
                {
                    ProductId = query.Id,
                    UserId = userId
                };

                _dbContext.userProducts.Add(userProduct);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var photoUrl = $"https://pharmapro.somee.com/api/Storage/GetImageById?id={product.Photo}";

                return new APIResponse<GetProductByIdQueryResponse>()
                {
                    Data = new GetProductByIdQueryResponse()
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Photo = photoUrl,
                        Amount = product.Amount,
                        BarCode = product.BarCode,
                        Active = product.Active,
                        SoldOut = product.SoldOut,
                        ExpirationDate = product.ExpirationDate,
                        Price = product.Price,
                        Offer = product.Offer,
                        Discount = product.Discount,
                        OldPrice = product.OldPrice,
                        CategoryId = product.CategoryId,
                    },
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                };

            }
        }
    }
}
