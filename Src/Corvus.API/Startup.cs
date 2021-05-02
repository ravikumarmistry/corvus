using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.OData;

namespace Corvus.API
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
            services.AddOData();
            //services.AddCorvusStore();
            //services.AddCorvus();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private static IEdmModel GetEdmModel()
        //{
        //    var edmModel = new EdmModel();
        //    #region data source entity type
        //    var dataSourceEntity = edmModel.AddEntityType("Corvus.Meta", "DataSource");
        //    var dataSourceId = dataSourceEntity.AddStructuralProperty("Id", EdmPrimitiveTypeKind.String, false);
        //    dataSourceEntity.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String, false);
        //    dataSourceEntity.AddStructuralProperty("Provider", EdmPrimitiveTypeKind.String, false);
        //    dataSourceEntity.AddStructuralProperty("ConnectionStringProvider", EdmPrimitiveTypeKind.String, false);
        //    dataSourceEntity.AddStructuralProperty("JsonSettings", EdmPrimitiveTypeKind.String, false);
        //    dataSourceEntity.AddKeys(dataSourceId);

        //    var edm = new EdmEntityType("Corvus.Meta", "EDM", null, false, true);
        //    var edmf = new EdmFunction("Corvus.Meta", "SaveEntityType", new EdmEntityTypeReference(edm, false));
        //    #endregion
        //    var container = edmModel.AddEntityContainer("Corvus.Meta", "Default");
        //    container.AddEntitySet("DataSources", dataSourceEntity);
        //    container.AddSingleton("EDMX", edm);

        //    edmModel.AddElement(edmf);
        //    return edmModel;
        //}
    }
}
