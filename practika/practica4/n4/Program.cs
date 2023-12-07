using System;
using System.Threading;
using System.Device.I2c;
using Iot.Device;
using Iot.Device.Adc;
using Raven.Iot.Device;
using Raven.Iot.Device.GpioExpander;
using Raven.Iot.Device.Ina219;
using UnitsNet;
using Spire.Xls;
using System.Device.Gpio;
using System.Text;

if (DeviceHelper.GetIna219Devices() is [I2cConnectionSettings settings])
{
    Memory<Storage> data = new Storage[6];

    var calibrator = Ina219Calibrator.Default with
    {
        VMax = ElectricPotential.FromVolts(5),
        IMax = ElectricCurrent.FromAmperes(0.6),
    };

    var ina219 = calibrator.CreateCalibratedDevice(settings);

    using GpioController controller = new GpioController();

    int pin = DeviceHelper.WiringPiToBcm(0);

    controller.OpenPin(pin, PinMode.Output);

    controller.Write(pin, PinValue.High);

    for (int i = 0; i < data.Length; i++)
    {
        data.Span[i] = new Storage(ina219.ReadBusVoltage(), ina219.ReadCurrent(), ina219.ReadPower(), TimeProvider.System.GetLocalNow());

        await Task.Delay(10000);
    }

    controller.Write(pin, PinValue.Low);

    controller.ClosePin(pin);

    WriteData(data);
}

void WriteData(Memory<Storage> data)
{
    string currentDirectory = Directory.GetCurrentDirectory();
    string[] csvFiles = Directory.GetFiles(currentDirectory, "data_*.csv");
    string[] csv = new string[data.Length + 1];
    csv[0] = $"Date, Time;Voltage, {UnitAbbreviationsCache.Default.GetDefaultAbbreviation(data.Span[0].Voltage.Unit)};Current, {UnitAbbreviationsCache.Default.GetDefaultAbbreviation(data.Span[0].Current.Unit)};Power, {UnitAbbreviationsCache.Default.GetDefaultAbbreviation(data.Span[0].Power.Unit)}\n";
    
    for (int i = 1; i < data.Length + 1; i++)
    {
        csv[i] = $"{data.Span[i - 1].DateTime};{data.Span[i - 1].Voltage.Value};{data.Span[i - 1].Current.Value};{data.Span[i - 1].Power.Value}\n";
    }
    using (var sw = new StreamWriter($"data_{csvFiles.Length}.csv"))
    {
        for (int i = 0; i < data.Length + 1; i++)
        {
            sw.Write(csv[i]);
        }
    }
}
public readonly record struct Storage(ElectricPotential Voltage, ElectricCurrent Current, Power Power, DateTimeOffset DateTime);
