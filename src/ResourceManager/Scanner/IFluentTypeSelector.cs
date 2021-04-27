using System;

namespace ResourceManager
{
    public interface IFluentTypeSelector
    {
        IFluentTypeSelector Select(params Type[] types);
        IFluentTypeSelector ScanAbstract(bool isAbstract);
        IFluentTypeSelector ScanInterface(bool isInterface);
        IFluentTypeSelector ScanTypes(Predicate<Type> predicate);
        IFluentTypeSelector ScanTypes(IFluentTypeSelector selector);
        TypeScanResult Get();
    }
}
