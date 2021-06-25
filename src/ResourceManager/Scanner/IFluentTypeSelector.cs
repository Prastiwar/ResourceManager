using System;
using System.Collections.Generic;

namespace ResourceManager
{
    public interface IFluentTypeSelector
    {
        IFluentTypeSelector Select(Type[] types);
        IFluentTypeSelector Ignore(HashSet<Type> types);

        IFluentTypeSelector ScanAbstract(bool isAbstract);
        IFluentTypeSelector ScanInterface(bool isInterface);
        IFluentTypeSelector ModifyGeneric(ModifyGenericHandler genericHandler);
        IFluentTypeSelector ScanTypes(Predicate<Type> predicate);
        IFluentTypeSelector ScanTypes(IFluentTypeSelector selector);

        IFluentTypeSelector Reset();
        TypeScanResult Get();
    }
}
