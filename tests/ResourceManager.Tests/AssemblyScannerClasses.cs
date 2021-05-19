namespace ResourceManager.Tests
{
    public interface IService { }

    public abstract class TestService : IService { }

    public class TestService1 : IService { }

    public class TestService2 : IService { }

    public interface IQueryHandler<TQuery, TResult> { }

    public class QueryHandler : IQueryHandler<string, int> { }

    public interface IOpenGeneric<T> : IOtherInheritance { }

    public class OpenGeneric<T> : IOpenGeneric<T> { }

    public interface IPartiallyClosedGeneric<T1, T2> { }

    public class PartiallyClosedGeneric<T> : IPartiallyClosedGeneric<T, int> { }

    public class CombinedService : IService, IQueryHandler<object, CombinedService> { }

    public interface IEmptyInterface { }

    public interface IDuplicateInheritance { }

    public interface IOtherInheritance { }

    public class DuplicateInheritance : IDuplicateInheritance { }

    public class DuplicateInheritance1 : DuplicateInheritance, IDuplicateInheritance { }

    public interface ITestService<T> { }

    public class TestService<T, U> : ITestService<T> { }

}
