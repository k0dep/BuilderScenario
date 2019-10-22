using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BuilderScenario
{
    /// <summary>
    /// Simple IoC Container based on constructor injection.
    /// </summary>
    public class ServiceCollection : IServiceCollection
    {
        Dictionary<Type, Type> registeredTypes = new Dictionary<Type, Type>();
        HashSet<Type> singletonTypes = new HashSet<Type>();

        Dictionary<Type, object> instances = new Dictionary<Type, object>();

        Type tmp;

        public T Resolve<T>() where T : class
        {
            if (!registeredTypes.Any())
                throw new Exception("No entity has been registered yet.");

            T s = (T)ResolveParameter(typeof(T));

            if (singletonTypes.Contains(typeof(T)) && !instances.ContainsKey(typeof(T)))
                instances.Add(typeof(T), s);

            return s;
        }
        
        public ServiceCollection Register<Tfrom, TTo>() where TTo : Tfrom
        {
            return For<Tfrom>().Inject<TTo>();
        }
        
        public ServiceCollection RegisterSingleton<Tfrom, TTo>() where TTo : Tfrom
        {
            singletonTypes.Add(typeof(Tfrom));
            return For<Tfrom>().Inject<TTo>();
        }
        
        public ServiceCollection Register<TFrom>(TFrom instance)
        {
            For<TFrom>().Inject(instance.GetType());
            singletonTypes.Add(typeof(TFrom));
            instances.Add(typeof(TFrom), instance);
            return this;
        }

        public object[] ResolveArguments(MethodInfo method)
        {
            var result = new List<object>();
            
            foreach (var parameter in method.GetParameters())
            {
                result.Add(ResolveParameter(parameter.ParameterType));
            }

            return result.ToArray();
        }

        private object ResolveParameter(Type type)
        {
            try
            {
                Type resolved = null;

                if (singletonTypes.Contains(type) && instances.ContainsKey(type))
                {
                    return instances[type];
                }

                resolved = registeredTypes[type];

                var cnstr = resolved.GetConstructors().First();
                var cnstrParams = cnstr.GetParameters().Where(w => w.GetType().IsClass);

                // If constructor hasn't parameter, Create an instance of object
                if (!cnstrParams.Any())
                    return Activator.CreateInstance(resolved);

                var paramLst = new System.Collections.Generic.List<object>(cnstrParams.Count());

                // Iterate through parameters and resolve each parameter
                for (int i = 0; i < cnstrParams.Count(); i++)
                {
                    var paramType = cnstrParams.ElementAt(i).ParameterType;
                    var resolvedParam = ResolveParameter(paramType);
                    paramLst.Add(resolvedParam);
                }

                return cnstr.Invoke(paramLst.ToArray());
            }
            catch (Exception)
            {
                var err = string.Format("'{0}' Cannot be resolved. Check your registered types", type.FullName);
                throw new Exception(err);
            }
        }
        
        private ServiceCollection For<Tfrom>()
        {
            tmp = typeof(Tfrom);
            return this;
        }
        
        
        private ServiceCollection Inject(Type type)
        {
            // If Already added, Just update the injection type
            if (registeredTypes.ContainsKey(tmp))
                registeredTypes[tmp] = type;
            else
            {
                registeredTypes.Add(tmp, type);
                tmp = null;
            }

            return this;
        }
        
        private ServiceCollection Inject<TTo>()
        {
            return Inject(typeof(TTo));
        }
    }
}