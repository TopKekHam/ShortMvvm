using ShortMvvm.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ShortMvvm
{

    public class ViewModel : INotifyPropertyChanged
    {

        public ViewModel()
        {
            InitCommands();
        }

        void InitCommands()
        {
            var methods = this.GetType().GetRuntimeMethods();
            InitCommands(methods);
            InitCanExecuteCommand(methods);
        }

        void InitCanExecuteCommand(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var commandExecuteAttr = method.GetCustomAttributes(typeof(CommandCanExecute));
                if (commandExecuteAttr.Any())
                {
                    if (method.Name.Substring(0, 3).ToLower() == "can" && method.Name.Length > 3)
                    {
                        try
                        {
                            var command = (Command)this[method.Name.Substring(3)];
                            command.CanExecuteCommand = () => (bool)method.Invoke(this, null);
                            OnNotify += () => command.TriggerCanExecuteChanged();
                        }
                        catch (Exception) { };
                    }
                }
            }
        }

        void InitCommands(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var commandExecuteAttr = method.GetCustomAttributes(typeof(CommandExecute));
                if (commandExecuteAttr.Any())
                {
                    var parameters = method.GetParameters();
                    if (parameters.Count() == 0)
                    {
                        Action<object> execute = (parameter) => method.Invoke(this, null);
                        this[method.Name] = new Command(execute);
                        
                    } else if(parameters.First().ParameterType is object)
                    {
                        Action<object> execute = (parameter) => method.Invoke(this, new object[] { parameter });
                        this[method.Name] = new Command(execute);
                    }
                }
            }
        }

        public dynamic Utilies;

        Dictionary<string, object> props = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        public object this[string key]
        {
            get
            {
                try
                {
                    return props[key];
                }
                catch (Exception)
                {
                    throw new Exception($"{key} doesn't exits");
                }
            }
            set
            {
                props[key] = value;
                Notify("Item[]");
            }
        }

        Action OnNotify;

        public void Notify(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            OnNotify?.Invoke();
        }

    }

}

