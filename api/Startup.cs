using System;
using api.binder;
using api.model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    // This does NOT work if body is only a DateTime (no Model)
                    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());                    
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    
                    // This didn't seem to have any effect...
                    // options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
                    // {
                    //     DateTimeStyles = DateTimeStyles.AdjustToUniversal
                    // });
                });
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration  configuration)
        {
            var section1 = configuration.GetSection("CustomSection1");
            var setting11 = section1["CustomSetting11"];
            Console.Out.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>CustomSetting11={setting11}");
            var customSection1 = new CustomSection1();
            configuration.Bind("CustomSection1", customSection1);
            Console.Out.WriteLine($">>>>>>>> Via binding >>>>>CustomSetting11={customSection1.CustomSetting11}");

            app.UseSwagger();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}