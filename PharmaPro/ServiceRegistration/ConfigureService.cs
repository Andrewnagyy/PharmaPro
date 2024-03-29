using MediatR;
using Microsoft.AspNetCore.Hosting;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory;
using PharmaPro.Core.Features.CategorysFT.Command.EditCategory;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList;
using System.Reflection;

namespace PharmaPRO.ServiceRegistration
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
 
            services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            //Category
            services.AddTransient(typeof(IRequestHandler<AddCategoryCommand, APIResponse<AddCategoryCommandResponse>>), typeof(AddCategoryCommandHandler));
            services.AddTransient(typeof(IRequestHandler<DeleteCategoryCommand, APIResponse<DeleteCategoryCommandResponse>>), typeof(DeleteCategoryCommandHandler));
            services.AddTransient(typeof(IRequestHandler<EditCategoryCommand, APIResponse<EditCategoryCommandResponse>>), typeof(EditCategoryCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetCategoryByIdQuery, APIResponse<GetCategoryByIdQueryResponse>>), typeof(GetCategoryByIdQueryHandler));
            services.AddTransient(typeof(IRequestHandler<GetCategoryListQuery, APIResponse<GetCategoryListQueryResponse>>), typeof(GetCategoryListQueryHandler));

            return services;
        }
    }
}
