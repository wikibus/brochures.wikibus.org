﻿using Anotar.Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Serilog;
using Wikibus.Sources.EF;

namespace Brochures.Wikibus.Org
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            LogTo.Information("Starting app");
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string domain = $"https://{this.Configuration["authentication:Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = this.Configuration["authentication:Auth0:ApiIdentifier"];
            });

            services.AddDbContext<SourceContext>(
                options => options.UseSqlServer(
                    this.Configuration["wikibus:sources:sql"],
                    builder => builder.UseRowNumberForPaging()));

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            if (this.Configuration.GetValue<bool>("authentication:backdoor"))
            {
                app.UseMiddleware<AuthorizationHeaders>();
            }

            app.UseWhen(context => context.Request.Path.ToString().EndsWith("/file"), fileApp =>
            {
                fileApp.Run(async context =>
                {
                    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 100_000_000;
                });
            });

            app.UseOwin(owin => owin.UseNancy(
                new NancyOptions
                {
                    Bootstrapper = new Bootstrapper(this.Configuration, loggerFactory)
                }));
        }
    }
}
