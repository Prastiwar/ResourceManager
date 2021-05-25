using MediatR;

namespace ResourceManager.Services
{
    public class MutableServiceFactory
    {
        public MutableServiceFactory() { }
        public MutableServiceFactory(ServiceFactory factory) => Factory = factory;

        public ServiceFactory Factory { get; set; }
    }
}
