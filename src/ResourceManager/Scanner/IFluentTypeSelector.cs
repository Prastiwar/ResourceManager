using System;
using System.Collections.Generic;

namespace ResourceManager
{
    public interface IFluentTypeSelector
    {
        IFluentTypeSelector Select(params Type[] types);
        IFluentTypeSelector Ignore(HashSet<Type> types);
        IFluentTypeSelector Ignore(params Type[] types);
        IFluentTypeSelector ScanAbstract(bool isAbstract);
        IFluentTypeSelector ScanInterface(bool isInterface);
        IFluentTypeSelector ModifyGeneric(ModifyGenericHandler genericHandler);
        IFluentTypeSelector ScanTypes(Predicate<Type> predicate);
        IFluentTypeSelector ScanTypes(IFluentTypeSelector selector);
        TypeScanResult Get();
    }
}
