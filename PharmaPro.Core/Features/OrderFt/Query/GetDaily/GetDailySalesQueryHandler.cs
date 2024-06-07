using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.OrderFt.Query.GetDaily;
using PharmaPro.DS;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.OrderFt.Query.GetDailySales
{
    public class GetDailySalesQueryHandler : IRequestHandler<GetDailySalesQuery, APIResponse<GetDailySalesQueryResponse>>
    {
        private readonly AppDbContext _dbContext;

        public GetDailySalesQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<APIResponse<GetDailySalesQueryResponse>> Handle(GetDailySalesQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            var totalSales = await _dbContext.orders
                .Where(o => o.CreatedAt.Date == today)
                .SumAsync(o => o.TotalPrice, cancellationToken);

            var totalOrders = await _dbContext.orders
                .Where(o => o.CreatedAt.Date == today)
                .CountAsync(cancellationToken);

            var response = new GetDailySalesQueryResponse
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders
            };

            return new APIResponse<GetDailySalesQueryResponse>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
