using Corvus.Core.Exceptions;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Corvus.Core.Extenstions;

namespace Corvus.Core.Services
{
    public class EDMService : IEDMService
    {
        const string NAMESPACE = "Corvus.App";

        private EdmModel _edm;

        public EDMService()
        {
            if (_edm == null)
            {
                _edm = new EdmModel();
                _edm.AddEntityContainer("Corvus.App", "Default");
            }
        }

        public async Task AddService(EntitySet entitySet)
        {
            var entityType = _edm.GetEntityType(entitySet.EntityType);
            if (entityType == null)
                throw new EDMValidationException($"Source entity '{ entitySet.EntityType }' not found.");
            EdmEntityContainer container = _edm.EntityContainer as EdmEntityContainer;
            container.AddEntitySet(entitySet.Name, entityType);
            await Task.CompletedTask;
        }

        public async Task AddServiceNavigation(NavigationPropertyBinding navigationPropertyBinding)
        {

            EdmEntityContainer container = _edm.EntityContainer as EdmEntityContainer;
            EdmEntitySet srcService = container.FindEntitySet(navigationPropertyBinding.ServiceName) as EdmEntitySet;
            EdmEntitySet trgtService = container.FindEntitySet(navigationPropertyBinding.TargateServiceName) as EdmEntitySet;
            EdmEntityType entity = srcService.EntityType() as EdmEntityType;
            var navlink = entity.FindProperty(navigationPropertyBinding.EntityNavigation);
            if (navlink == null || navlink.PropertyKind != EdmPropertyKind.Navigation)
            {
                throw new EDMValidationException($"navigation property '{navigationPropertyBinding.EntityNavigation}' is not valid.");
            }

            srcService.AddNavigationTarget(navlink as EdmNavigationProperty, trgtService);
            await Task.CompletedTask;
        }

        public async Task AddEntityType(EntityType entityType)
        {
            EdmEntityType edmEntityType = new EdmEntityType(NAMESPACE, entityType.Name);

            foreach (var item in entityType.Properties)
            {
                Enum.TryParse(item.Type, out EdmPrimitiveTypeKind type);
                var isPropertynullable = item.IsKey ? false : item.IsNullable.HasValue ? item.IsNullable.Value : true;
                var property = edmEntityType.AddStructuralProperty(item.Name, type, isPropertynullable);
                if (item.IsKey)
                {
                    edmEntityType.AddKeys(property);
                }
            }
            _edm.AddElement(edmEntityType);

            await Task.CompletedTask;
        }

        //public async Task AddUniDirectionalNavigation(EntityNavigation navigationLink)
        //{
        //    var srcEntityType = _edm.GetEntityType(navigationLink.SourceEntity);
        //    if (srcEntityType == null)
        //        throw new EDMValidationException($"Source entity '{ navigationLink.SourceEntity }' not found.");

        //    var navInfo = GetNavigationInfo(navigationLink, srcEntityType);

        //    srcEntityType.AddUnidirectionalNavigation(navInfo);


        //    //navigationProperty.
        //    await Task.CompletedTask;
        //}

        public Task EditEntitySet(EntitySet entitySet)
        {
            throw new NotImplementedException();
        }

        public Task EditEntityType(EntityType entityType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEdmModel> GetEdmModel()
        {
            return await Task.FromResult(_edm);
        }

        public IEnumerable<string> GetPrimitiveTypeNames()
        {
            return Enum.GetNames(typeof(EdmPrimitiveTypeKind));
        }

        public async Task RemoveEntitySet(string name)
        {
            EdmEntityContainer container = _edm.EntityContainer as EdmEntityContainer;
            // container.Elements
            await Task.CompletedTask;
        }

        public async Task RemoveEntityType(string name)
        {
            EdmEntityContainer container = _edm.EntityContainer as EdmEntityContainer;

            await Task.CompletedTask;
        }

        public Task RemoveEntityNavigation(string entityType)
        {
            throw new NotImplementedException();
        }

        public async Task AddEntityNavigation(EntityNavigation navigationLink)
        {
            var srcEntityType = _edm.GetEntityType(navigationLink.SourceEntity);
            if (srcEntityType == null)
                throw new EDMValidationException($"Source entity '{ navigationLink.SourceEntity }' not found.");

            var forwordNav = GetNavigationInfo(new EnityNavigationLink()
            {
                SourceEntity = navigationLink.SourceEntity,
                Name = navigationLink.ForwordNavigation.Name,
                TargetMultiplicity = navigationLink.ForwordNavigation.Multiplicity,
                DependentProperty = navigationLink.ForwordNavigation.DependentProperty,
                TargateEntity = navigationLink.TargateEntity,
                PrincipleProperty = navigationLink.ForwordNavigation.PrincipleProperty
            }, srcEntityType);

            var reverseNav = GetNavigationInfo(new EnityNavigationLink()
            {
                //Reverse
                SourceEntity = navigationLink.TargateEntity,
                TargateEntity = navigationLink.SourceEntity,
                Name = navigationLink.BackwordNavigation.Name,
                TargetMultiplicity = navigationLink.BackwordNavigation.Multiplicity,
                DependentProperty = navigationLink.BackwordNavigation.DependentProperty,
                PrincipleProperty = navigationLink.BackwordNavigation.PrincipleProperty
            }, srcEntityType);

            var nav = srcEntityType.AddBidirectionalNavigation(forwordNav, reverseNav);

            var a = nav.PrincipalProperties();

            await Task.CompletedTask;
        }

        private EdmNavigationPropertyInfo GetNavigationInfo(EnityNavigationLink navigationLink, EdmEntityType srcEntityType)
        {
            Enum.TryParse(navigationLink.TargetMultiplicity, out EdmMultiplicity multiplicity);
            IEdmProperty dependentProperty = null;
            if (navigationLink.DependentProperty != null && multiplicity != EdmMultiplicity.Many)
            {
                dependentProperty = srcEntityType.FindProperty(navigationLink.DependentProperty);
                if (dependentProperty == null)
                    throw new EDMValidationException($"Property '{navigationLink.DependentProperty}' not found in entity '{navigationLink.SourceEntity}'.");
            }


            var trgtEntityType = _edm.GetEntityType(navigationLink.TargateEntity);
            if (trgtEntityType == null)
                throw new EDMValidationException($"Source entity '{ navigationLink.TargateEntity }' not found.");


            IEdmProperty principalProperty = null;
            if (navigationLink.PrincipleProperty != null && multiplicity != EdmMultiplicity.Many)
            {
                principalProperty = trgtEntityType.FindProperty(navigationLink.PrincipleProperty);
                if (principalProperty == null)
                    throw new EDMValidationException($"Property '{navigationLink.PrincipleProperty}' not found in entity '{navigationLink.TargateEntity}'.");
            }

            List<IEdmStructuralProperty> dependentProperties = null;
            if (dependentProperty != null)
            {
                dependentProperties = new List<IEdmStructuralProperty>() { dependentProperty as IEdmStructuralProperty };
            }

            List<IEdmStructuralProperty> principalProperties = null;
            if (principalProperty != null)
            {
                principalProperties = new List<IEdmStructuralProperty>() { principalProperty as IEdmStructuralProperty };
            }

            var navInfo = new EdmNavigationPropertyInfo()
            {
                Name = navigationLink.Name,
                TargetMultiplicity = multiplicity,
                PrincipalProperties = principalProperties,
                Target = trgtEntityType,
                DependentProperties = dependentProperties,
            };
            return navInfo;
        }
    }
}
