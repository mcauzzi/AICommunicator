using System;
using System.Reactive;
using ReactiveUI;

namespace Frontend.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _celsius;
    private string _fahrenheit;

    public string Celsius
    {
        get => _celsius;
        set
        {
            _celsius = value;
            ConvertCelsiusToFahrenheit();
        }
    }

    public string Fahrenheit
    {
        get => _fahrenheit;
        set
        {
            _fahrenheit = value;
            ConvertFahrenheitToCelsius();
        }
    }

    public MainWindowViewModel()
    {
        _celsius          = "0";
        ConvertCelsiusToFahrenheit();
    }

    private void ConvertCelsiusToFahrenheit()
    {
        if (double.TryParse(Celsius, out var C))
        {
            var F = C * (9d / 5d) + 32;
            this.RaiseAndSetIfChanged(ref _fahrenheit,F.ToString("0.0"),nameof(Fahrenheit));
        }
        else
        {
            this.RaiseAndSetIfChanged(ref _fahrenheit,"0",nameof(Fahrenheit));;
        }
    }
    private void ConvertFahrenheitToCelsius()
    {
        if (double.TryParse(Fahrenheit, out var F))
        {
            var C = (F - 32) * 5 / 9;
            this.RaiseAndSetIfChanged(ref _celsius,C.ToString("0.0"),nameof(Celsius));
        }
        else
        {
            this.RaiseAndSetIfChanged(ref _celsius,"0",nameof(Celsius));
        }
    }
}