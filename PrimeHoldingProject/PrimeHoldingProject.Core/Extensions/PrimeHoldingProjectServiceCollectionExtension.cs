namespace Microsoft.Extensions.DependencyInjection
{
    public static class PrimeHoldingProjectServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient<IProductService, ProductService>();
            //services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
