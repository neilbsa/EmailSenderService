using EmailSenderService.Models;
using EmailSenderService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmailSenderService", Version = "v1" });
            });

            var emailConfig = Configuration
             .GetSection("EmailConfiguration")
             .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
                o.KeyLengthLimit = int.MaxValue;
                o.ValueCountLimit = int.MaxValue;
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;

            });


            var issuer = Configuration["Issuer"];
            var audience = Configuration["Audience"];
            var sec = Configuration["Secret"];
            bool ValidateIssuer = Boolean.Parse(Configuration["ValidateIssuer"]);
            bool ValidateAudience = Boolean.Parse(Configuration["ValidateAudience"]);
            bool ValidateLifeTime = Boolean.Parse(Configuration["ValidateLifeTime"]);
            bool ValidateIssuerSigningKey = Boolean.Parse(Configuration["ValidateIssuerSigningKey"]);

            var tokenParam = new TokenValidationParameters()
            {
                ValidateIssuer = ValidateIssuer,
                ValidateAudience = ValidateAudience,
                ValidateLifetime = ValidateLifeTime,
                ValidateIssuerSigningKey = ValidateIssuerSigningKey,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sec))
            };

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
         .AddJwtBearer(
         opt =>
         {
             opt.RequireHttpsMetadata = false;
             opt.SaveToken = true;
             opt.TokenValidationParameters = tokenParam;
         });


            services.AddScoped<ISender, Sender>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmailSenderService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }





}
