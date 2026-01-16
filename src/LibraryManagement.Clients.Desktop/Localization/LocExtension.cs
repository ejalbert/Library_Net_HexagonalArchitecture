using System;
using System.ComponentModel;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

using LibraryManagement.Clients.Desktop.Resources;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LibraryManagement.Clients.Desktop.Localization;

[MarkupExtensionReturnType(typeof(string))]
public sealed class LocExtension : MarkupExtension
{
    public string? Key { get; set; }

    public Type? ResourceType { get; set; }

    public LocExtension()
    {
    }

    public LocExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrWhiteSpace(Key))
        {
            return string.Empty;
        }

        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            var resourceType = ResourceType ?? typeof(Strings);
            var resourceManager = new ResourceManager(resourceType);
            return resourceManager.GetString(Key) ?? Key;
        }

        var app = System.Windows.Application.Current;
        if (app is not App typedApp || typedApp.Services == null)
        {
            return Key;
        }

        var factory = typedApp.Services.GetService<IStringLocalizerFactory>();
        if (factory == null)
        {
            return Key;
        }

        var localizer = ResourceType != null
            ? factory.Create(ResourceType)
            : factory.Create(typeof(Strings));

        var localized = localizer[Key];
        return localized.ResourceNotFound ? Key : localized.Value;
    }
}
