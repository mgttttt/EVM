using Iot.Device.OneWire;

bool flag = true;
Console.CancelKeyPress += (sender, eventArgs) =>
{
    flag = false;
};
while (flag)
{
    foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
    {
        Console.WriteLine("Name: " + dev.DeviceId.ToString());
        Console.WriteLine("Temperature" + dev.ReadTemperature().DegreesCelsius.ToString());
        Console.WriteLine();
    }
}