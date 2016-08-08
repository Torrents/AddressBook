using AddressBook.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AddressBook.Web
{
	public sealed class AddressBookControllerFactory : DefaultControllerFactory
	{
		private sealed class ControllerCreationInfo
		{
			public Type Type { get; set; }

			public string[] ConstructorParameterKeys { get; set; }
		}

		private const string IdentityKeyPrefix = "AspNet.Identity.Owin:";

		private static string GetKey(Type type)
		{
			return IdentityKeyPrefix + type.AssemblyQualifiedName;
		}

		private static string[] GetConstructorParameterKeys(Type controllerType)
		{
			var constructorInfos = controllerType.GetConstructors();

			if (constructorInfos.Length != 0)
			{
				var selectedConstructorInfo = constructorInfos.OrderBy(constructorInfo => constructorInfo.GetParameters().Length).FirstOrDefault();

				if (selectedConstructorInfo != null)
				{
					var constructorParameterInfos = selectedConstructorInfo.GetParameters();
					var constructorParameterCount = constructorParameterInfos.Length;

					if (constructorParameterCount != 0)
					{
						var constructorParameterKeys = new string[constructorParameterCount];

						for (var index = 0; index < constructorParameterCount; index++)
						{
							var constructorParameterInfo = constructorParameterInfos[index];
							constructorParameterKeys[index] = GetKey(constructorParameterInfo.ParameterType);
						}

						return constructorParameterKeys;
					}
				}
			}

			return null;
		}

		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			var controllers = MemoryCache.Default.GetOrGenerate("OwinControllerFactory.Controllers", TimeSpan.FromDays(1), () =>
			{
				var controllersQuery =
					from type in Assembly.GetExecutingAssembly().GetTypes()
					where type.GetInterface("IController") != null
					let constructorParameterKeys = GetConstructorParameterKeys(type)
					where constructorParameterKeys != null
					select new
					{
						Name = type.Name,
						CreationInfo = new ControllerCreationInfo
						{
							Type = type,
							ConstructorParameterKeys = constructorParameterKeys
						}
					};
				return controllersQuery.ToSortedDictionary(controller => controller.Name, controller => controller.CreationInfo);
			});

			var controllerClassName = controllerName + "Controller";
			ControllerCreationInfo controllerCreationInfo;

			if (controllers.TryGetValue(controllerClassName, out controllerCreationInfo))
			{
				var owinContext = requestContext.HttpContext.GetOwinContext();
				var constructorParameterKeys = controllerCreationInfo.ConstructorParameterKeys;
				var parameterCount = constructorParameterKeys.Length;
				var parameters = new object[parameterCount];

				for (var index = 0; index != parameterCount; ++index)
				{
					parameters[index] = owinContext.Get<object>(constructorParameterKeys[index]);
				}

				var controller = Activator.CreateInstance(controllerCreationInfo.Type, parameters) as IController;

				if (controller != null)
				{
					return controller;
				}

				controllers.Remove(controllerClassName);
			}

			return base.CreateController(requestContext, controllerName);
		}
	}
}