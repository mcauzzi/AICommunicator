// See https://aka.ms/new-console-template for more information

using NAudio.CoreAudioApi;

var enumerator = new MMDeviceEnumerator();
foreach (var endpoint in
         enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
{
    Console.WriteLine($"{endpoint.FriendlyName} ({endpoint.ID})");
}