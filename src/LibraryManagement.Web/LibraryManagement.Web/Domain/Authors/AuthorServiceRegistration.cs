namespace LibraryManagement.Web.Domain.Authors;

public static class AuthorServiceRegistration
{
    extension(IServiceCollection services){
        public IServiceCollection AddAuthorServices()
        {
            services.AddScoped<IAuthorModelMapper, AuthorModelMapper>();
            return services;
        }
    }
}
