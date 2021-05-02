using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

// TODO: Things to implement
// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_ComplexType
// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_EnumerationType
// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_ContainmentNavigationProperty
// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_MediaEntityType
// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_OnDeleteAction
// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/odata-csdl-xml-v4.01.html#sec_ActionandFunction
namespace Corvus.Core.Services
{

    public interface IEDMService
    {
        Task AddEntityType(EntityType entityType);
        Task AddEntityNavigation(EntityNavigation navigationLink);
        Task AddService(EntitySet entitySet);
        Task AddServiceNavigation(NavigationPropertyBinding entitySet);
        Task EditEntityType(EntityType entityType);
        Task EditEntitySet(EntitySet entitySet);
        Task RemoveEntityType(string entityType);
        Task RemoveEntitySet(string entitySet);
        Task RemoveEntityNavigation(string entityType);
        IEnumerable<string> GetPrimitiveTypeNames();
        Task<IEdmModel> GetEdmModel();
    }

    public class EntitySet
    {
        public string Name { get; set; }
        public string EntityType { get; set; }
    }

    public class NavigationPropertyBinding
    {
        public string ServiceName { get; set; }
        public string EntityNavigation { get; set; }
        public string TargateServiceName { get; set; }
    }

    public class EntityType 
    {
        [Required]
        public string Name { get; set; }
        public string BaseType { get; set; }
        public bool? IsAbstract { get; set; }
        public bool? IsOpen { get; set; }
        public IEnumerable<EntityProperty> Properties { get; set; } = new List<EntityProperty>();
    }

    public class EntityProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? IsNullable { get; set; }
        public bool IsKey { get; set; }
        public int Maxlength { get; set; }
        public string DefaultValue { get; set; }
    }
    public class EnityNavigationLink
    {
        /// <summary>
        /// Constrainet Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constraint first entity
        /// </summary>
        public string SourceEntity { get; set; }

        /// <summary>
        /// Constrainet second entity
        /// </summary>
        public string TargateEntity { get; set; }

        /// <summary>
        /// Foregin key Property in source entity
        /// </summary>
        public string DependentProperty { get; set; }

        /// <summary>
        /// A Property of Targate Entity
        /// </summary>
        public string PrincipleProperty { get; set; }

        /// <summary>
        /// Cardinality 
        /// </summary>
        public string TargetMultiplicity { get; set; }
    }

    public class EntityNavigation
    {
        /// <summary>
        /// Constraint first entity
        /// </summary>
        public string SourceEntity { get; set; }

        /// <summary>
        /// Constrainet second entity
        /// </summary>
        public string TargateEntity { get; set; }

        public NavigationData ForwordNavigation { get; set; }
        public NavigationData BackwordNavigation { get; set; }
    }

    public class NavigationData
    {
        /// <summary>
        /// Foregin key Property in source entity
        /// </summary>
        public string DependentProperty { get; set; }

        /// <summary>
        /// A Property of Targate Entity
        /// </summary>
        public string PrincipleProperty { get; set; }

        /// <summary>
        /// Cardinality 
        /// </summary>
        public string Multiplicity { get; set; }
        /// <summary>
        /// Constrainet Name
        /// </summary>
        public string Name { get; set; }
    }
}
