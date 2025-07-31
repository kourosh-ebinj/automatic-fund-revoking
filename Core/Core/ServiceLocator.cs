using Microsoft.Extensions.DependencyInjection;
using System;
using Core.Models;

namespace Core
{
    /// <summary>
    /// An static object for getting services from DI
    /// </summary>
    public static class ServiceLocator
    {
        private static object locker = new object();
        private static IServiceProvider _serviceProvider;

        static ServiceLocator()
        {

        }

        /// <summary>
        /// Configures ServiceLocator static object
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        public static void Configure(IServiceProvider serviceProvider, ApplicationSettingModel configuration)
        {
            _serviceProvider = serviceProvider;

        }

        /// <summary>
        /// Use reflection to get a service from DI
        /// </summary>
        /// <param name="typeName">The type name to get from DI</param>
        /// <returns>returns System.Object</returns>
        public static object GetService(string typeName)
        {
            Type classType = Type.GetType(typeName);

            return GetService(classType);
        }

        /// <summary>
        /// Gets a service from DI
        /// </summary>
        /// <param name="type">The Type to get from DI</param>
        /// <returns>returns System.Object</returns>
        public static object GetService(Type type, bool scoped = true)
        {
            object result = null;
            var serviceScope = _serviceProvider
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            if (_serviceProvider == null && serviceScope == null)
                throw new NullReferenceException("ServiceLocator is not configured properly.");

            //try
            //{
            if (scoped)
                return serviceScope.ServiceProvider.GetService(type);
            else
                return _serviceProvider.GetService(type);

            //ActivatorUtilities.CreateInstance(_serviceProvider, type);
            //return result;
            //}
            //catch (Exception ex)
            //{
            //    result = _serviceProvider.GetService(type);
            //    if (result != null)
            //        return result;
            //    throw;
            //}
        }

        /// <summary>
        /// Gets a service from DI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>returns T</returns>
        public static T GetService<T>(bool scoped = true)
        {
            return (T)GetService(typeof(T), scoped);
        }

        //public static T GetApplicationSettings<T>(T configurationInstance)
        //{
        //    var applicationSettingsOptions = GetService<T>();
        //    configurationInstance = applicationSettingsOptions;
        //    //applicationSettingsOptions.OnChange(e =>
        //    //    configurationInstance = applicationSettingsOptions.CurrentValue
        //    //    );
        //    return configurationInstance;
        //}
    }

}
