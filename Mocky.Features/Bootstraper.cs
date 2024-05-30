using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mocky.Features
{
    public static class Bootstraper
    {
        public static void UseFeatures(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
