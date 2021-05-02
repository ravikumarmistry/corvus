using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corvus.Core.Extenstions
{
    public static class EdmExtensions
    {
        public static EdmEntityType GetEntityType(this EdmModel edm, string name, string ns = "Corvus.App")
        {
            var edmSchema = edm.FindDeclaredType($"{ns}.{name}");
            if (edmSchema == null || edmSchema.TypeKind != EdmTypeKind.Entity)
                return null;
            return (edmSchema as EdmEntityType);
        }
    }
}
