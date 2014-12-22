using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Medici.Reflection
{    
    public static class DynamicLoader
    {
        public static Assembly LoadDll(string dllFullPath)
        {
            if (!File.Exists(dllFullPath))
                throw ExGen.Build<FileNotFoundException>(ErrorMessages.DllNotFound, dllFullPath);

            return Assembly.LoadFile(dllFullPath);            
        }

        public static Maybe<Assembly> TryLoadDll(string dllFullpath)
        {
            try
            {
                return new Maybe<Assembly>(LoadDll(dllFullpath));
            }
            catch (Exception ex)
            {
                return new Maybe<Assembly>(ex);
            }
        }

        public static Type[] ScanAssembly(Assembly asm, string fullTypeName)
        {
            Ensure.That(asm, "asm").IsNotNull();
            Ensure.That(fullTypeName, "type").IsNotNullOrEmpty();

            var typeQuery = from t in asm.GetTypes()
                where t.FullName == fullTypeName
                select t;

            return typeQuery.ToArray();
        }

        public static Type[] ScanAssemblyForImplementationsOf<TType>(Assembly asm) where TType : class
        {
            return ScanAssemblyForImplementations(asm, typeof (TType));
        }

        public static Type[] ScanAssemblyForImplementations(Assembly asm, Type searchType)
        {
            Ensure.That(asm, "asm").IsNotNull();
            Ensure.That(searchType, "searchType").IsNotNull();

            if (searchType.IsAbstract)
            {
                var concreteQuery = from t in asm.GetTypes()
                                    where !t.IsAbstract &&
                                            searchType.IsAssignableFrom(t)
                                    select t;
                
                return concreteQuery.ToArray();
            }
            else if (searchType.IsInterface)
            {
                var concreteQuery = from t in asm.GetTypes()
                                    where !t.IsAbstract &&
                                        t.GetInterfaces().Contains(searchType)
                                    select t;

                return concreteQuery.ToArray();

            }
            else
                throw ExGen.Build<MediciException>(ErrorMessages.InvalidAssemblyScanTargetType);
        }
    }
}
