using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Clients.Desktop.Mvvm;

[MarkupExtensionReturnType(typeof(object))]
public sealed class ResolveExtension : MarkupExtension
{
    public Type Type { get; set; } = null!;
    public Type? DesignType { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            var designType = DesignType ?? Type;
            if (designType != null && designType.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(designType)!;
            }

            return null!;
        }

        return Ioc.Default.GetRequiredService(Type);
    }
}
