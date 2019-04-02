using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Windows.UI.Xaml;

namespace ShortMvvm
{
    public abstract class Locator
    {
        Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        Dictionary<Type, Type> _singletonServices = new Dictionary<Type, Type>();
        Dictionary<Type, Type> _services = new Dictionary<Type, Type>();

        protected Type ViewModelUtilityClass { get; set; }

        public Locator()
        {

            RegisterSingleton<Locator>();
            _singletons[typeof(Locator)] = this;
            RegisterSingleton<Application>();
            _singletons[typeof(Application)] = Application.Current;
        }

        protected void Register<ServiceInterface, Service>()
        {
            _services.Add(typeof(ServiceInterface), typeof(Service));
        }

        protected void RegisterSingleton<ServiceInterface, Service>()
        {
            _singletonServices.Add(typeof(ServiceInterface), typeof(Service));
        }

        protected void Register<Service>()
        {
            _services.Add(typeof(Service), typeof(Service));
        }

        protected void RegisterSingleton<Service>()
        {
            _singletonServices.Add(typeof(Service), typeof(Service));
        }

        protected InstanceType Create<InstanceType>()
        {
            return (InstanceType)Create(typeof(InstanceType));
        }

        object Create(Type instanceType)
        {
            if (_singletonServices.ContainsKey(instanceType))
            {
                if (!_singletons.ContainsKey(instanceType))
                {
                    _singletons.Add(instanceType, CreateType(_singletonServices[instanceType]));
                }

                return _singletons[instanceType];
            }
            else
            {
                return CreateType(_services[instanceType]);
            }
        }

        object CreateType(Type instanceType)
        {

            var constractor = instanceType.GetConstructors().First();
            var arguments = constractor.GetParameters();

            foreach (var arg in arguments)
            {
                if (!_services.ContainsKey(arg.ParameterType) &&
                    !_singletonServices.ContainsKey(arg.ParameterType))
                {
                    throw new Exception($"No registered type: {arg.ParameterType.Name}");
                }
            }

            List<object> prms = new List<object>();

            foreach (var arg in arguments)
            {
                var argType = arg.ParameterType;
                var newArg = Create(argType);
                prms.Add(newArg);
            }
            try
            {
                var instance = Activator.CreateInstance(instanceType, prms.ToArray());
                return instance;
            }
            catch (Exception)
            {
                throw new Exception($"No registered type: {instanceType.Name}");
            }

        }

    }
}
