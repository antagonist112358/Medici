using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Medici.Reflection
{
    public static class SafeAssemblyScanner
    {
        // path to the core library
        private static readonly string CoreLibraryPath = Assembly.GetExecutingAssembly().Location;

        public static bool DoesAssemblyContainAnyOf(string asmPath, params Type[] implementationTypes)
        {
            Ensure.That(asmPath, "asmPath").IsNotNullOrEmpty();

            // Check to see if there are actually any types to search for
            if (implementationTypes.IsNullOrEmpty())
                return false;

            // Check that each type is an interface or an abstract type
            if (implementationTypes.Any(typeOfTType => !typeOfTType.IsInterface && !typeOfTType.IsAbstract))
            {
                throw ExGen.Build<MediciException>(ErrorMessages.InvalidAssemblyScanTargetType);
            }

            // Create the temporary AppDomain and the proxy object in that domain
            AppDomain tempDomain; AppDomainProxy proxy;
            CreateTempAppDomain().To(out tempDomain, out proxy);

            try
            {
                // Use the proxy to scan for the requested types
                return ProxyDomainTest(asmPath, tempDomain, proxy, (testAsmName) =>
                    proxy.ScanForImplementations(testAsmName, implementationTypes));
            }
            catch (Exception proxyEx)
            {
                throw ExGen.Build<MediciException>("Encountered an exception while querying the assembly '{0}' " +
                    "for types inheriting or implementing types: {1}. See inner exception for more details.",
                    proxyEx, asmPath, String.Join(",", implementationTypes.Select(t => t.Name)));
            }
        }

        public static bool DoesAssemblyContainImplementationsOf<TType>(string asmPath)
        {
            Ensure.That(asmPath, "asmPath").IsNotNullOrEmpty();
            
            // Check that TType is an interface or an abstract type
            var typeOfTType = typeof (TType);
            if (!typeOfTType.IsInterface && !typeOfTType.IsAbstract)
            {
                throw ExGen.Build<MediciException>(ErrorMessages.InvalidAssemblyScanTargetType);
            }

            AppDomain tempDomain; AppDomainProxy proxy;
            CreateTempAppDomain().To(out tempDomain, out proxy);

            try
            {
                // Use the proxy to scan for the requested type
                return ProxyDomainTest(asmPath, tempDomain, proxy, (testAsmName) => 
                    proxy.ScanForImplementationsOf(testAsmName, typeOfTType));
            }
            catch (Exception proxyEx)
            {                
                throw ExGen.Build<MediciException>("Encountered an exception while querying the assembly '{0}' " +
                    "for types inheriting or implementing type: {1}. See inner exception for more details.",
                    proxyEx, asmPath, typeOfTType.FullName);
            }
        }

        public static bool DoesAssemblyContainType(string asmPath, string typeName)
        {
            Ensure.That(asmPath, "asmPath").IsNotNullOrEmpty();
            Ensure.That(typeName, "typeName").IsNotNullOrEmpty();

            AppDomain tempDomain; AppDomainProxy proxy;
            CreateTempAppDomain().To(out tempDomain, out proxy);

            try
            {
                // Use the proxy to scan for the requested type
                return ProxyDomainTest(asmPath, tempDomain, proxy, (testAsmName) =>
                    proxy.ScanForSpecificType(testAsmName, typeName));
            }
            catch (Exception proxyEx)
            {
                throw ExGen.Build<MediciException>("Encountered an exception while querying the assembly '{0}' " +
                    "for specific type named: {1}. See inner exception for more details.",
                    proxyEx, asmPath, typeName);
            }
        }


        private static Tuple<AppDomain, AppDomainProxy> CreateTempAppDomain()
        {
            // Unique ID for temp ad name
            var uniqueName = (Guid.NewGuid()).ToString();

            // Create a temporary appdomain
            var tempDomain = AppDomain.CreateDomain(uniqueName);

            // Create the proxy
            var proxy = (AppDomainProxy) tempDomain.CreateInstanceFromAndUnwrap(
                CoreLibraryPath, "Medici.AppDomainProxy");

            return Tuple.Create(tempDomain, proxy);
        }

        private static bool ProxyDomainTest(string asmPath, AppDomain tempDomain, AppDomainProxy proxy, Func<string, bool> test)
        {
            try
            {
                // Load the assembly into the appDomain
                var testAsmName = proxy.LoadAssembly(asmPath);

                // If it wasn't loaded, return false
                if (String.IsNullOrEmpty(testAsmName))
                {
                    return false;
                }

                // Execute the proxy test
                return test(testAsmName);
            }
            finally
            {
                AppDomain.Unload(tempDomain);
            }
        }

    }

    public class AppDomainProxy : MarshalByRefObject
    {
        public string LoadAssembly(string asmPath)
        {
            // Check for the file
            if (!System.IO.File.Exists(asmPath))
                return null;

            try
            {
                // Attempt to load the assembly
                Assembly asm = Assembly.LoadFrom(asmPath);

                // Return the fully qualified name
                return asm.FullName;
            }
            catch
            {
                return null;
            }
        }

        public bool ScanForSpecificType(string fullName, string typeName)
        {
            // Try to find the assembly in the appdomain
            Assembly foundAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == fullName);

            if (foundAsm == null) { return false; }

            // Scan the assmebly for the specified type
            Type[] foundTypes = DynamicLoader.ScanAssembly(foundAsm, typeName);

            // Return results
            return (foundTypes != null && foundTypes.Length > 0);
        }

        public bool ScanForImplementationsOf(string fullName, Type implType)
        {
            // Try to find the assembly in the appdomain
            Assembly foundAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == fullName);

            if (foundAsm == null) { return false; }

            // Scan the assmebly for the specified type
            Type[] foundTypes = DynamicLoader.ScanAssemblyForImplementations(foundAsm, implType);

            // Return results
            return (foundTypes != null && foundTypes.Length > 0);
        }

        public bool ScanForImplementations(string fullName, IEnumerable<Type> types)
        {
            // Try to find the assembly in the appdomain
            Assembly foundAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == fullName);

            if (foundAsm == null) { return false; }

            var search = types.Select(t => DynamicLoader.ScanAssemblyForImplementations(foundAsm, t))
                .Any(arr => arr != null && arr.Length > 0);

            return search;
        }
    }
}
