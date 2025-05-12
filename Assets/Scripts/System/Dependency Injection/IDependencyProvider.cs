using UnityEngine;

namespace Bap.DependencyInjection
{
    public interface IDependencyProvider
    {
        public IDependencyProvider ProvideMyself();
    }
}