using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiMall.IRepository;
using MiMall.IService;
using MiMall.Model.Context;
using MiMall.Repository;
using MiMall.Service;
using MiMall.WebApi.AuthHelper;

namespace MiMall.WebApi
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MiMall API",
                    Version = "v1",
                    Description = "框架说明文档",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "2020520946@qq.com",
                        Name = "MiMall",
                        Url = new Uri("https://www.cnblogs.com/licm/")
                    }
                });

                //xml注释文档
                string basePath = Directory.GetCurrentDirectory();
                string fileName = Assembly.GetExecutingAssembly().GetName().Name;
                string xmlPath = Path.Combine(basePath, fileName + ".xml");
                options.IncludeXmlComments(xmlPath, true);

                //model xml注释文档

                string modelPath = basePath.Substring(0, basePath.LastIndexOf(fileName));
                string xmlModelPath = Path.Combine(basePath, "MiMall.Model.xml");
                options.IncludeXmlComments(xmlModelPath);

                //jwt 接口权限认证
                //添加安全定义
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",//提示信息描述
                    Name = "Authorization",//键名称
                    In = ParameterLocation.Header,//参数定义为头部请求参数
                    Type = SecuritySchemeType.ApiKey,//参数类型为apikey
                    BearerFormat = "JWT",//持票人格式为jwt
                    Scheme = "Bearer"//验证体系为Bearer
                });
                //添加安全需求
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                             Reference=new OpenApiReference()
                             {
                                  Type= ReferenceType.SecurityScheme,
                                  Id="Bearer"
                             }
                        } ,new string[]{ }
                    }
                });

            });
            #endregion

            #region Authentication && JWT
            JwtModel model = Configuration.GetSection("JwtModel").Get<JwtModel>();
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = model.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(model.SigningKey)),
                        ValidateAudience = true,
                        ValidAudience = model.Audience,
                        RequireExpirationTime = true,//是否验证过期时间
                        ValidateLifetime = true,//是否验证过期时间
                        ClockSkew = TimeSpan.Zero////这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                    };
                    //自定义Token获取方式
                    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            //接收到请求消息后调用 (在Url中添加access_token=[token])
                            context.Token = context.Request.Query["access_token"];
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            //在token验证成功后调用
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            //在token验证失败后调用
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            //未授权是调用
                            return Task.CompletedTask;
                        }

                    };
                });

            #endregion

            #region Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                //或
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("System", "Admin").Build());
                //并
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("System")
                                .RequireRole("Admin").Build());

            });
            #endregion

            #region Cors
            services.AddCors(options =>
            {
                //允许所有源，头文件、方法访问
                options.AddPolicy("any", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            #endregion


        }

        //Autofac DI
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MiMallContext>().As<DbContext>().InstancePerLifetimeScope();

            string path = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf(Assembly.GetExecutingAssembly().GetName().Name));
            List<Assembly> list = new List<Assembly>();
            Directory.GetDirectories(path).ToList()
                .ForEach(item =>
                {
                    string name = item.Substring(item.LastIndexOf("\\") + 1);
                    if (name.Contains("MiMall"))
                    {
                        list.Add(Assembly.Load(name));
                    }
                });

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>));


            builder.RegisterAssemblyTypes(list.ToArray())
                .Where(a => a.Name.EndsWith("Repository") || a.Name.EndsWith("Service"))
                .PublicOnly()
                .Where(s => s.IsClass)
                .AsImplementedInterfaces();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region swagger
                //只有在开发环境，才会启用swagger
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp v1");
                    //  //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，
                    //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可
                    //options.RoutePrefix = string.Empty;
                });
                #endregion
            }

            app.UseRouting();

            app.UseCors();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
