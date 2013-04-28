using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;
using PartyLeaderBoardServices;
using ServiceStack.Configuration;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

[assembly: WebActivator.PreApplicationStartMethod(typeof(PartyLeaderboard.App_Start.AppHost), "Start")]

namespace PartyLeaderboard.App_Start
{
	public class AppHost
		: AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
			: base("Party Leader Board", typeof(ScoresService).Assembly) { }

		public override void Configure(Funq.Container container)
		{
			//Set JSON web services to return idiomatic JSON camelCase properties
			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ToString();
            Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider));

            Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[]
                {
                    new CredentialsAuthProvider(), 
                }));

            Plugins.Add(new RegistrationFeature());

            var userRep = new OrmLiteAuthRepository(container.Resolve<IDbConnectionFactory>());
            container.Register<IUserAuthRepository>(userRep);
            var redisCon = ConfigurationManager.AppSettings["redisUrl"].ToString();
            container.Register<IRedisClientsManager>(new PooledRedisClientManager(20, 60, redisCon));
            container.Register<ICacheClient>(c => (ICacheClient)c.Resolve<IRedisClientsManager>().GetCacheClient());

            userRep.CreateMissingTables();
			//Set MVC to use the same Funq IOC as ServiceStack
			ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
		}

		public static void Start()
		{
			new AppHost().Init();
		}
	}
}