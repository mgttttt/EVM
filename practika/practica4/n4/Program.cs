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
        data.Span[i] = new Storage(ina219.ReadBusVoltage(), ina219.ReadCurrent(), ina219.ReadPower(), TimeProvider.System.GetUtcNow());

        await Task.Delay(10000);
    }

    controller.Write(pin, PinValue.Low);

    controller.ClosePin(pin);

    WriteData(data);
}

void WriteData(Memory<Storage> data)
{
    Workbook workbook = new Workbook();

    Worksheet worksheet = workbook.Worksheets[0];

    worksheet.Range[1, 1].Value = $"Date, Time";
    worksheet.Range[1, 2].Value = $"Voltage, {UnitAbbreviationsCache.Default.GetDefaultAbbreviation(data.Span[0].Voltage.Unit)}";
    worksheet.Range[1, 3].Value = $"Current, {UnitAbbreviationsCache.Default.GetDefaultAbbreviation(data.Span[0].Voltage.Unit)}";
    worksheet.Range[1, 4].Value = $"Power, {UnitAbbreviationsCache.Default.GetDefaultAbbreviation(data.Span[0].Voltage.Unit)}";
    CellStyle style = workbook.Styles.Add("newStyle");
    style.Font.IsBold = true;
    worksheet.Range[1, 1, 1, 3].Style = style;
    for (int i = 0; i < data.Length; i++)
    {
        worksheet.Range[i + 2, 1].Value = data.Span[i].DateTime.ToString();
        worksheet.Range[i + 2, 2].Value = data.Span[i].Voltage.Value.ToString();
        worksheet.Range[i + 2, 3].Value = data.Span[i].Current.Value.ToString();
        worksheet.Range[i + 2, 4].Value = data.Span[i].Power.Value.ToString();
    }

    worksheet.AllocatedRange.AutoFitColumns();

    workbook.SaveToFile("Results.xlsx");
}
public readonly record struct Storage(ElectricPotential Voltage, ElectricCurrent Current, Power Power, DateTimeOffset DateTime);
