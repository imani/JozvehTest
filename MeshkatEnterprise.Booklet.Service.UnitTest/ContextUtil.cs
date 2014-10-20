using Compositional.Composer;
using Compositional.Composer.Utility;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service.UnitTest
{
    public static class ContextUtil
    {
        private static bool _init;
        private static readonly object LockObject = new object();

        public static void Init()
        {
            lock (LockObject)
            {
                if (!_init)
                {
                    var context = new ComponentContext();
                    context.ProcessCompositionXmlFromResource(
                        "MeshkatEnterprise.Booklet.Service.UnitTest.Composition.xml");
                    ComponentContextBinder.Bind(context);

                    _init = true;
                }
            }
        }
    }
}