
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Repository;
using APiCatalogo.Extesions;
using AutoMapper;
using Cardapio.API.Context;
using Cardapio.API.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cardapio.API
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
            //Permissão de Cors
            services.AddCors(opt => 
            {
                opt.AddPolicy("EnableCors", 
                builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().Build());
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration["ConnectionStrings:CARDAPIODB"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            //JWT
            //adiciona o manipulador de autenticacao e define o esquema de autenticacao
            //usado : Bearer valida o emissor, a audiencia e a chave usando 
            //a chave secreta valida a assinatura
            services.AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme).
                    AddJwtBearer(options =>
                                options.TokenValidationParameters =
                                new TokenValidationParameters
                                {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime= true,
                                ValidAudience = Configuration["TokenConfiguration:Audience"],
                                ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(Configuration["Jwt:key"])
                                )});



            services.AddControllers()
                    .AddNewtonsoftJson(options => 
                                                    {
                                                        options.SerializerSettings.ReferenceLoopHandling = 
                                                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                                                    });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cardapio.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cardapio.API v1"));
            }

            //Adicionando um middleware de tratamento de erros
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("EnableCors");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
