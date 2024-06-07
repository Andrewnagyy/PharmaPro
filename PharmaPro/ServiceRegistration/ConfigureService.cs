using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory;
using PharmaPro.Core.Features.CategorysFT.Command.EditCategory;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList;
using PharmaPro.Core.Features.ContactFT.Command.AddContactUs;
using PharmaPro.Core.Features.ContactFT.Query.GetContactUs;
using PharmaPro.Core.Features.IdentityFt.isBlocked.blockUser;
using PharmaPro.Core.Features.IdentityFt.isBlocked.unblockUser;
using PharmaPro.Core.Features.OrderFt.Command.AddOrder;
using PharmaPro.Core.Features.OrderFt.Command.orderIsDone;
using PharmaPro.Core.Features.OrderFt.Query.GetDaily;
using PharmaPro.Core.Features.OrderFt.Query.GetDailySales;
using PharmaPro.Core.Features.OrderFt.Query.GetHistory;
using PharmaPro.Core.Features.OrderFt.Query.GetMonthly;
using PharmaPro.Core.Features.OrderFt.Query.GetMonthlySales;
using PharmaPro.Core.Features.OrderFt.Query.GetOrdersList;
using PharmaPro.Core.Features.ProductFT.Command.AddOffer;
using PharmaPro.Core.Features.ProductFT.Command.AddProduct;
using PharmaPro.Core.Features.ProductFT.Command.DeleteProduct;
using PharmaPro.Core.Features.ProductFT.Command.EditProduct;
using PharmaPro.Core.Features.ProductFT.Query.GetProductByCategory;
using PharmaPro.Core.Features.ProductFT.Query.GetProductById;
using PharmaPro.Core.Features.ProductFT.Query.GetProductList;
using PharmaPro.Core.Features.Storageft.Command.UploadImage;
using PharmaPro.Core.Features.Storageft.Query;
using PharmaPro.Core.Features.UserFt.Command.AddInfo;
using PharmaPro.Core.Features.UserFt.Query.GetUserList;
using System.Reflection;

namespace PharmaPRO.ServiceRegistration
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //User 
            services.AddTransient(typeof(IRequestHandler<AddUserInfoCommand, APIResponse<AddUserInfoCommandResponse>>), typeof(AddUserInfoCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetUserListQuery, APIResponse<GetUserListResponse>>), typeof(GetUserListQueryHandler));
            services.AddTransient(typeof(IRequestHandler<BlockUserCommand, APIResponse<BlockUserCommandResponse>>), typeof(BlockUserCommandHandler));
            services.AddTransient(typeof(IRequestHandler<unBlockUserCommand, APIResponse<unBlockUserCommandResponse>>), typeof(unBlockUserCommandHandler));

            //Category
            services.AddTransient(typeof(IRequestHandler<AddCategoryCommand, APIResponse<AddCategoryCommandResponse>>), typeof(AddCategoryCommandHandler));
            services.AddTransient(typeof(IRequestHandler<DeleteCategoryCommand, APIResponse<DeleteCategoryCommandResponse>>), typeof(DeleteCategoryCommandHandler));
            services.AddTransient(typeof(IRequestHandler<EditCategoryCommand, APIResponse<EditCategoryCommandResponse>>), typeof(EditCategoryCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetproductByIdQuery, APIResponse<GetCategoryByIdQueryResponse>>), typeof(GetCategoryByIdQueryHandler));
            services.AddTransient(typeof(IRequestHandler<GetCategoryListQuery, APIResponse<GetCategoryListQueryResponse>>), typeof(GetCategoryListQueryHandler));

            //Product
            services.AddTransient(typeof(IRequestHandler<AddProductCommand, APIResponse<AddProductCommandResponse>>), typeof(AddProductCommandHandler));
            services.AddTransient(typeof(IRequestHandler<EditProductCommand, APIResponse<EditProductCommandResponse>>), typeof(EditProductCommandHandler));
            services.AddTransient(typeof(IRequestHandler<DeleteProductCommand, APIResponse<DeleteProductCommandResponse>>), typeof(DeleteProductCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetProductByIdQuery, APIResponse<GetProductByIdQueryResponse>>), typeof(GetProductByIdQueryHandler));
            services.AddTransient(typeof(IRequestHandler<GetProductListQuery, APIResponse<GetProductListQueryResponse>>), typeof(GetProductListQueryHandler));
            services.AddTransient(typeof(IRequestHandler<AddOfferCommand, APIResponse<AddOfferCommandResponse>>), typeof(AddOfferCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetProductByCategoryQuery, APIResponse<GetProductByCategoryResponse>>), typeof(GetProductByCategoryHandler));


            //Order
            services.AddTransient(typeof(IRequestHandler<AddOrderCommand, APIResponse<AddOrderCommandResponse>>), typeof(AddOrderCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetOrderHistoryQuery, APIResponse<GetOrderHistoryQueryResponse>>), typeof(GetOrderHistoryQueryHandler));
            services.AddTransient(typeof(IRequestHandler<GetOrderListQuery, APIResponse<List<OrderDto>>>), typeof(GetOrderListQueryHandler));
            services.AddTransient(typeof(IRequestHandler<OrderIsDoneCommand, APIResponse<OrderIsDoneCommandResponse>>), typeof(OrderIsDoneCommandHandler));

            //Storage
            services.AddTransient(typeof(IRequestHandler<UploadImageCommand, APIResponse<UploadImageCommandResponse>>), typeof(UploadImageCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetImageQuery, GetImageQueryResponse>), typeof(GetImageQueryHandler));

            // contact us
            services.AddTransient(typeof(IRequestHandler<AddContactUsCommand, APIResponse<AddContactUsCommandResponse>>), typeof(AddContactUsCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetContactUsListQuery, APIResponse<List<ContactsDto>>>), typeof(GetContactUsListQueryHandler));


            //Dashboard
            services.AddTransient(typeof(IRequestHandler<GetDailySalesQuery, APIResponse<GetDailySalesQueryResponse>>), typeof(GetDailySalesQueryHandler));
            services.AddTransient(typeof(IRequestHandler<GetMonthlySalesQuery, APIResponse<GetMonthlySalesQueryResponse>>), typeof(GetMonthlySalesQueryHandler));



            return services;
        }
    }
}
