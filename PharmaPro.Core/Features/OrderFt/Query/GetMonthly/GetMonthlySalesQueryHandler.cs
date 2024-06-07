using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.OrderFt.Query.GetMonthly;
using PharmaPro.DS;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.OrderFt.Query.GetMonthlySales
{
    public class GetMonthlySalesQueryHandler : IRequestHandler<GetMonthlySalesQuery, APIResponse<GetMonthlySalesQueryResponse>>
    {
        private readonly AppDbContext _dbContext;

        public GetMonthlySalesQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<APIResponse<GetMonthlySalesQueryResponse>> Handle(GetMonthlySalesQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            var totalSales = await _dbContext.orders
                .Where(o => o.CreatedAt >= startOfMonth && o.CreatedAt < startOfNextMonth)
                .SumAsync(o => o.TotalPrice, cancellationToken);

            var totalOrders = await _dbContext.orders
                .Where(o => o.CreatedAt >= startOfMonth && o.CreatedAt < startOfNextMonth)
                .CountAsync(cancellationToken);

            var response = new GetMonthlySalesQueryResponse
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders
            };

            return new APIResponse<GetMonthlySalesQueryResponse>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
