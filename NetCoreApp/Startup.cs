using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using NetCore.Core.Extensions;
using NetCore.Domain.Domain;
using NetCore.Domain.Interface;
using NetCore.EntityFrameworkCore.Context;
using NetCore.Repository.Interface;
using NetCore.Repository.Repository;
using NetCore.Repository.UnitWork;
using Swashbuckle.AspNetCore.Swagger;
using NetCoreApp.Extensions;
using NetCoreApp.Filters;
using log4net;
using log4net.Config;
using log4net.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;
using NetCore.Core.Util;
using Quartz;
using Quartz.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NetCore.DTO.ViewModel;
using NetCore.Core.RedisUtil;
using NetCore.Core.RabbitMQ;
using NetCore.Services.IServices.I_RabbitMq;
using NetCore.Services.Services.S_RabbitMq;
using Microsoft.Extensions.Logging;

namespace NetCoreApp
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {


        /// <summary>
        /// /
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {

            //log4
            repository = LogManager.CreateRepository("CoreLog4");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));//配置文件路径可以自定义
            BasicConfigurator.Configure(repository);

            Configuration = configuration;

            //初始化自己配置的config文件
            AppConfigUtil.InitConfig(configuration);

            //redis 配置
            RedisConfig.InitConfig(configuration);
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// log4 接口
        /// </summary>
        public static ILoggerRepository repository { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            #region 读取配置信息

            services.Configure<JWTConfigViewModel>(Configuration.GetSection("JWT"));
            JWTConfigViewModel config = new JWTConfigViewModel();
            Configuration.GetSection("JWT").Bind(config);
            #endregion

            #region 启用JWT
            services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
             AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = config.Issuer,
                     ValidAudience = config.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.IssuerSigningKey))
                 };
             });
            #endregion

            #region 全局注册 异常过滤器  全局设置
            //取消自动校验模型  netcore  大于 2.0 自动校验模型
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(option =>
            {
                option.Filters.Add<AuthorizationFilter>();
                //option.Filters.Add<ResourceFilter>();
                option.Filters.Add<CustomerExceptionFilter>();
                option.Filters.Add<ActionFilter>();
                // option.Filters.Add<ResultFilter>();
                // option.Filters.Add(new AddHeaderResultFilterAttribute("name", "Jesen"));
            });


            //全局配置Json序列化处理
            services.AddMvc().AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //驼峰命名法
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            });

            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                    .AllowAnyOrigin() //允许任何来源的主机访问
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();//指定处理cookie
                });
            });
            #endregion

            #region automap 注入
            services.AddAutoMapperSetup();
            #endregion

            #region  DBContext   code-first使用
            //读取aoosettings.json里配置的数据库连接语句需要的代码
            var connection = Configuration.GetConnectionString("MySqlConnection");
            services.AddDbContext<DBContext>(options => options.UseMySql(connection));
            #endregion

            #region  监测性能
            services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");
            #endregion

            #region Swagger所需要的配置项
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                //添加Swagger.
                c.SwaggerDoc("CoreTest", new Info
                {
                    Version = "CoreTest",
                    Title = "netcore项目接口文档",
                    Description = "This CoreTest Api",
                    //服务条款
                    TermsOfService = "None",
                    //作者信息
                    Contact = new Contact
                    {
                        Name = "方向感",
                        Email = string.Empty,
                        Url = "http://fxg.fxg91.com/"
                    },
                    //许可证
                    License = new License
                    {
                        Name = "许可证名字",
                        Url = "http://fxg.fxg91.com/"
                    }
                });

                c.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题
                // 下面三个方法为 Swagger JSON and UI设置xml文档注释路径

                var xmlPath = Path.Combine(basePath, "NetCoreApp.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, "NetCore.EntityFrameworkCore.xml");
                c.IncludeXmlComments(xmlModelPath);

                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，CoreAPI。
                var security = new Dictionary<string, IEnumerable<string>> { { "CoreAPI", new string[] { } }, };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("CoreAPI", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"

                });

                #endregion

            });

            #endregion


            #region rabbitmq 配置
            //RabbitMQLoggerOptions producer = new RabbitMQLoggerOptions();
            //producer = Configuration.GetSection("ProducterMQ").Get<RabbitMQLoggerOptions>();
            //producer.Arguments= new Dictionary<string, object>() { { "x-queue-type", "classic" } };


            services.AddOptions<RabbitMQLoggerOptions>().Bind(Configuration.GetSection("ProducterMQ"));
            services.AddOptions<RabbitMQConsumerOptions>().Bind(Configuration.GetSection("ConsumerMQ"));
            //配置消息发布
            //services.Configure<RabbitMQLoggerOptions>(options =>
            //{
            //    options.Category = "Rabbit";
            //    options.Hosts = new string[] { "192.168.187.129" };
            //     options.MinLevel = LogLevel.Information;
            //    options.Password = "123456";
            //    options.Port = 5672;
            //    options.Queue = "queue1";
            //    options.UserName = "admin";
            //    options.VirtualHost = "/";
            //    options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };
            //    options.AutoDelete = false;
            //    options.Durable = true;
            //});
            //将RabbitLoggerProvider加入到容器中
            //  services.AddSingleton<ILoggerProvider, RabbitLoggerProvider>();

            //services.Configure<RabbitMQLoggerOptions>(Configuration.GetSection("ProducterMQ"));
            //RabbitMQLoggerOptions producer = new RabbitMQLoggerOptions();
            //producer.Arguments= new Dictionary<string, object>() { { "x-queue-type", "classic" } };
            //Configuration.GetSection("ProducterMQ").Bind(producer);
            // services.AddSingleton<IProducerMqServices, ProducerMqServices>();

            //配置消息消费
            //services.Configure<RabbitMQConsumerOptions>(options =>
            //{
            //    options.Hosts = new string[] { "192.168.187.129" };
            //    options.Password = "123456";
            //    options.Port = 5672;
            //    options.Queue = "queue1";
            //    options.UserName = "admin";
            //    options.VirtualHost = "/";
            //    options.Arguments = new Dictionary<string, object>() { { "x-queue-type", "classic" } };
            //    options.AutoDelete = false;
            //    options.Durable = true;
            //    options.AutoAck = false;
            //});

            //services.Configure<RabbitMQConsumerOptions>(Configuration.GetSection("ConsumerMQ"));
            //RabbitMQConsumerOptions consumer = new RabbitMQConsumerOptions();
            //Configuration.GetSection("ConsumerMQ").Bind(consumer);
            ////注入消费者
            //services.AddSingleton<IConsumerMqServices, ConsumerMqServices>();

            //注册单例 生产者和消费者
             services.AddSingleton<IProducerMqServices, ProducerMqServices>();
            //services.AddSingleton<IConsumerMqServices, ConsumerMqServices>();

            #endregion


            #region 版本信息
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

            #region 依赖注入
            services.ConfigureDynamicProxy();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #region 定时
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            #endregion
            //注册 IHttpContextAccessor 对象
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IBaseDomain<>), typeof(BaseDomain<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));

            var ignoredList = new List<string>() { "IProducerMqServices", "IConsumerMqServices" };

            foreach (var itemClass in GetClassInterfacePairs("NetCore.Services"))
            {
                foreach (var itemInterface in itemClass.Value)
                {
                    var name = itemInterface.Name;
                    if (!ignoredList.Contains(name))
                    {
                        services.AddScoped(itemInterface, itemClass.Key);
                    }
                }
            }

            foreach (var itemClass in GetClassInterfacePairs("NetCore.Domain"))
            {
                foreach (var itemInterface in itemClass.Value)
                {
                    services.AddScoped(itemInterface, itemClass.Key);
                }
            }
            foreach (var itemClass in GetClassInterfacePairs("NetCore.Repository"))
            {
                foreach (var itemInterface in itemClass.Value)
                {
                    services.AddScoped(itemInterface, itemClass.Key);
                }
            }

            return services.BuildDynamicProxyProvider();

            #endregion

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {



            //---
            AppConfigUtil._serviceProvider = app.ApplicationServices;




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // 配置Swagger  必须加在app.UseMvc前面 API文档中间件
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);

            });
            //Swagger Core需要配置的  必须加在app.UseMvc前面
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/CoreTest/swagger.json", "My API 1.0.1");//注意,中间那段的名字 (refuge) 要和 上面 SwaggerDoc 方法定义的 名字 (refuge)一样
                s.RoutePrefix = "swagger"; //默认值是 "swagger" ,需要这样请求:https://localhost:44300/
                s.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("NetCoreApp.index.html");
            });
            //automapper
            app.UseStateAutoMapper();
            app.UseHttpsRedirection();

            //
            app.UseMiniProfiler();


            //跨域 策略
            app.UseCors(MyAllowSpecificOrigins);

            //启用认证中间件
            app.UseAuthentication();

            app.UseMvc();

        }

        #region 辅助类
        private Dictionary<Type, List<Type>> GetClassInterfacePairs(string assemblyName)
        {
            //存储 实现类 以及 对应接口
            Dictionary<Type, List<Type>> dic = new Dictionary<Type, List<Type>>();
            Assembly assembly = GetAssembly(assemblyName);
            Type[] types = assembly.GetTypes();
            foreach (var item in types.AsEnumerable().Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericType))
            {
                dic.Add(item, item.GetInterfaces().Where(x => !x.IsGenericType).ToList());
            }
            return dic;
        }

        private List<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return list;
        }


        private Assembly GetAssembly(string assemblyName)
        {
            var ent = GetAllAssemblies().Where(p => p.FullName.Contains(assemblyName)).FirstOrDefault();
            return ent;
        }
        #endregion

        /// <summary>
        /// 小写输出
        /// </summary>
        public class LowerCasePropertyNamesContractResolver : DefaultContractResolver
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="propertyName"></param>
            /// <returns></returns>
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }
    }
}
