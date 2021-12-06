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
            // 1 способ - стандартный механизм маршрутизации в ASP.NET Core с помощью компонентов EndpointRoutingMiddleware и EndpointMiddleware
            
            // 2 способ построени€ маршрутизации - RouterMiddleware сравнивает запрашиваемый Url с зарегистрированными маршрутами, и если один из маршрутов подходит, то вызываетс€ обработчик этого маршрута
            // определ€ем обработчик маршрута
            var myRouterHandler = new RouteHandler(Handle); // в качестве параметра делегат RequestDelegate
            // создаем маршрут, использу€ обработчик
            var routeBuilder = new RouteBuilder(app, myRouterHandler);
            // определение шаблона маршрута первого
            routeBuilder.MapRoute("default", "{controller}/{action}/{id?}"); // сегмент раздел€етс€ слешом, параметры маршрута в фигурные скобки
                                                                             // RouterMiddleware не передает выполнение запроса дальше, если хот€
                                                                             // бы один из определенных маршрутов совпал со строкой запроса
            // определение шаблона маршрута второго
            routeBuilder.MapRoute("default2", "erp{controller}/{action}/{id?}");
            // подключаем компонент RouterMiddleware в конвейер обработки запроса
            app.UseRouter(routeBuilder.Build()); // Build() возвращает объект IRouter, который затем переходит в RouterMiddleware и используетс€ дл€ обработки запросов

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
