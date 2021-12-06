using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace part7
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 1 ������ - ����������� �������� ������������� � ASP.NET Core � ������� ����������� EndpointRoutingMiddleware � EndpointMiddleware
            
            // 2 ������ ���������� ������������� - RouterMiddleware ���������� ������������� Url � ������������������� ����������, � ���� ���� �� ��������� ��������, �� ���������� ���������� ����� ��������
            // ���������� ���������� ��������
            var myRouterHandler = new RouteHandler(Handle); // � �������� ��������� ������� RequestDelegate
            // ������� �������, ��������� ����������
            var routeBuilder = new RouteBuilder(app, myRouterHandler);
            // ����������� ������� �������� �������
            routeBuilder.MapRoute("default", "{controller}/{action}/{id?}"); // ������� ����������� ������, ��������� �������� � �������� ������
                                                                             // RouterMiddleware �� �������� ���������� ������� ������, ���� ����
                                                                             // �� ���� �� ������������ ��������� ������ �� ������� �������
            // ����������� ������� �������� �������
            routeBuilder.MapRoute("default2", "erp{controller}/{action}/{id?}");
            // ���������� ��������� RouterMiddleware � �������� ��������� �������
            app.UseRouter(routeBuilder.Build()); // Build() ���������� ������ IRouter, ������� ����� ��������� � RouterMiddleware � ������������ ��� ��������� ��������

            var myRouter = new RouteBuilder(app);
            myRouter.MapRoute("re{year:int}-{month}/{*all}",
                async context =>
                {
                    await context.Response.WriteAsync("Smth in Engilsh " + context.GetRouteData().Values["year"]);
                });
            app.UseRouter(myRouter.Build());

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello world");
            });
        }

        private async Task Handle(HttpContext context)
        {
            await context.Response.WriteAsync("Hello ASP.NET");
        }
    }
}
