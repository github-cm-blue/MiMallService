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
                    Description = "���˵���ĵ�",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "2020520946@qq.com",
                        Name = "MiMall",
                        Url = new Uri("https://www.cnblogs.com/licm/")
                    }
                });

                //xmlע���ĵ�
                string basePath = Directory.GetCurrentDirectory();
                string fileName = Assembly.GetExecutingAssembly().GetName().Name;
                string xmlPath = Path.Combine(basePath, fileName + ".xml");
                options.IncludeXmlComments(xmlPath, true);

                //model xmlע���ĵ�

                string modelPath = basePath.Substring(0, basePath.LastIndexOf(fileName));
                string xmlModelPath = Path.Combine(basePath, "MiMall.Model.xml");
                options.IncludeXmlComments(xmlModelPath);

                //jwt �ӿ�Ȩ����֤
                //��Ӱ�ȫ����
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",//��ʾ��Ϣ����
                    Name = "Authorization",//������
                    In = ParameterLocation.Header,//��������Ϊͷ���������
                    Type = SecuritySchemeType.ApiKey,//��������Ϊapikey
                    BearerFormat = "JWT",//��Ʊ�˸�ʽΪjwt
                    Scheme = "Bearer"//��֤��ϵΪBearer
                });
                //��Ӱ�ȫ����
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
                        RequireExpirationTime = true,//�Ƿ���֤����ʱ��
                        ValidateLifetime = true,//�Ƿ���֤����ʱ��
                        ClockSkew = TimeSpan.Zero////����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
                    };
                    //�Զ���Token��ȡ��ʽ
                    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            //���յ�������Ϣ����� (��Url�����access_token=[token])
                            context.Token = context.Request.Query["access_token"];
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            //��token��֤�ɹ������
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            //��token��֤ʧ�ܺ����
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            //δ��Ȩ�ǵ���
                            return Task.CompletedTask;
                        }

                    };
                });

            #endregion

            #region Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                //��
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("System", "Admin").Build());
                //��
                options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("System")
                                .RequireRole("Admin").Build());

            });
            #endregion

            #region Cors
            services.AddCors(options =>
            {
                //��������Դ��ͷ�ļ�����������
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
                //ֻ���ڿ����������Ż�����swagger
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp v1");
                    //  //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�
                    //���ʱ��ȥlaunchSettings.json�а�"launchUrl": "swagger/index.html"ȥ���� Ȼ��ֱ�ӷ���localhost:8001/index.html����
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
