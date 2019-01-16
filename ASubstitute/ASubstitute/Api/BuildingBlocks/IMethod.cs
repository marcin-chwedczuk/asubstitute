using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace ASubstitute.Api.BuildingBlocks {
    public interface IMethod {
        string ProxyName { get; }

        string Name { get; }

        IImmutableList<Type> ParameterTypes { get; }
    }

    public static class IMethodExtensionMethods {
        public static bool HasSameSignatureAs(this IMethod @this, IMethod other) {
            if (!string.Equals(@this.Name, other.Name, StringComparison.Ordinal)) 
                return false;

            return @this.ParameterTypes.SequenceEqual(other.ParameterTypes);
        }
    }
}