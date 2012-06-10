using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Raven.Client.Document;

namespace StackClone
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
            DocumentStore store = new DocumentStore();
            store.Url = "http://localhost:8080";
            store.Initialize();

            AddIndexes(store);

            Application.Add("DocumentStore", store);
        }

        private void AddIndexes(DocumentStore store)
        {
            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(Question_ByDate).Assembly, store);
            new User_ByUserName().Execute(store);
        }

        protected void Application_End()
        {
            DocumentStore documentStore = Application.Get("DocumentStore") as DocumentStore;
            if(documentStore != null)
                documentStore.Dispose();
        }
    }
}