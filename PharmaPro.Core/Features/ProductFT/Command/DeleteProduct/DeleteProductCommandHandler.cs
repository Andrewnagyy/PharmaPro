using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PharmaPro.Core.Features.ProductFT.Command.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, APIResponse<DeleteProductCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        public DeleteProductCommandHandler(AppDbContext appDbContext) => _dbContext = appDbContext;


        public async Task<APIResponse<DeleteProductCommandResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            int rowEffected = await _dbContext
                .products
                .Where(row => row.Id == request.Id)
                .ExecuteDeleteAsync();
            if (rowEffected == 0)
            {
                return new APIResponse<DeleteProductCommandResponse>
                {
                    Errors = new List<string> { "This Product is Not Found!" },
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                };
            }
            return new APIResponse<DeleteProductCommandResponse>
            {
                Data = new DeleteProductCommandResponse()
                {
                    Message = "Product Deleted Successfully!"
                },
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            };
        }
    }
}
