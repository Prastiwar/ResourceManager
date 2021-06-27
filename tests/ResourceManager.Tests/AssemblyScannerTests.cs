using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ResourceManager.Tests
{
    public class AssemblyScannerTests
    {
        public AssemblyScannerTests() => Scanner = new FluentAssemblyScanner(typeof(AssemblyScannerTests).Assembly);

        protected IFluentAssemblyScanner Scanner { get; }

        [Fact]
        public void ScanAbstractIService()
        {
            Type scanType = typeof(IService);

            TypeScanResult result = Scanner.Scan().Select(scanType).ScanAbstract(true).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);
            Assert.Single(resultTypes);
            Assert.Contains(resultTypes, x => x == typeof(AbstractTestService));
        }

        [Fact]
        public void ScanIService()
        {
            Type scanType = typeof(IService);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);
            Assert.Equal(3, resultTypes.Count);
            Assert.Contains(resultTypes, x => x == typeof(TestService1));
            Assert.Contains(resultTypes, x => x == typeof(TestService2));
            Assert.Contains(resultTypes, x => x == typeof(CombinedServiceQueryHandler));
        }

        [Fact]
        public void ScanIEmptyInterface()
        {
            Type scanType = typeof(IEmptyInterface);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.False(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);
            Assert.Empty(resultTypes);
        }

        [Fact]
        public void ScanIOtherInheritance()
        {
            Type scanType = typeof(IOtherInheritance);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);
            Assert.Single(resultTypes);

            Assert.Contains(resultTypes, x => x == typeof(OpenGeneric<>));
        }

        [Fact]
        public void ScanIPartiallyClosedGeneric()
        {
            Type scanType = typeof(IPartiallyClosedGeneric<,>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanIPartiallyConcreteClosedGeneric()
        {
            //Type scanType = typeof(IPartiallyClosedGeneric<T, int>);

            //TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            //List<TypeScan> scans = result.Scans.ToList();
            //TypeScan scanResult = scans.FirstOrDefault();
            //List<Type> resultTypes = scanResult.ResultTypes.ToList();

            //Assert.True(result.HasResults);
            //Assert.Single(scans);
            //Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanIQueryHandler()
        {
            Type scanType = typeof(IQueryHandler<,>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanConcreteIQueryHandler()
        {
            Type scanType = typeof(IQueryHandler<string, int>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanIDuplicateInheritance()
        {
            Type scanType = typeof(IDuplicateInheritance);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanITestService()
        {
            Type scanType = typeof(ITestService<>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanConcreteITestService()
        {
            Type scanType = typeof(ITestService<int>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanOpenGeneric()
        {
            Type scanType = typeof(IOpenGeneric<>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }

        [Fact]
        public void ScanConcreteOpenGeneric()
        {
            Type scanType = typeof(IOpenGeneric<string>);

            TypeScanResult result = Scanner.Scan().Select(scanType).Get();
            List<TypeScan> scans = result.Scans.ToList();
            TypeScan scanResult = scans.FirstOrDefault();
            List<Type> resultTypes = scanResult.ResultTypes.ToList();

            Assert.True(result.HasResults);
            Assert.Single(scans);
            Assert.Equal(scanResult.ScannedType, scanType);

            // TEST: Test resultTypes
        }
    }
}
