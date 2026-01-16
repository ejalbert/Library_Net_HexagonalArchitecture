using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

namespace LibraryManagement.Clients.Desktop.Mvvm;

internal static class ObservableRecipientActivation
{
    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(ObservableRecipientActivation),
            new PropertyMetadata(false, OnIsEnabledChanged));

    public static void SetIsEnabled(DependencyObject element, bool value)
    {
        element.SetValue(IsEnabledProperty, value);
    }

    public static bool GetIsEnabled(DependencyObject element)
    {
        return (bool)element.GetValue(IsEnabledProperty);
    }

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement element)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            element.Loaded += OnLoaded;
            element.Unloaded += OnUnloaded;
            element.DataContextChanged += OnDataContextChanged;

            if (element.IsLoaded)
            {
                Activate(element.DataContext);
            }
        }
        else
        {
            element.Loaded -= OnLoaded;
            element.Unloaded -= OnUnloaded;
            element.DataContextChanged -= OnDataContextChanged;
            Deactivate(element.DataContext);
        }
    }

    private static void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            Activate(element.DataContext);
        }
    }

    private static void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            Deactivate(element.DataContext);
        }
    }

    private static void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        Deactivate(e.OldValue);

        if (sender is FrameworkElement { IsLoaded: true } element)
        {
            Activate(element.DataContext);
        }
    }

    private static void Activate(object? dataContext)
    {
        if (dataContext is ObservableRecipient recipient)
        {
            recipient.IsActive = true;
        }
    }

    private static void Deactivate(object? dataContext)
    {
        if (dataContext is ObservableRecipient recipient)
        {
            recipient.IsActive = false;
        }
    }
}
