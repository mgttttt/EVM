using System.IO.Pipes;
using System.Runtime.CompilerServices;

public struct Data
{
    public int number;
    public int age;
}

class PipeServer
{
    static void Main()
    {
        using NamedPipeServerStream pipeServer = new("channel", PipeDirection.InOut);
        Console.WriteLine("Waiting for client connection...");
        pipeServer.WaitForConnection();
        Console.WriteLine("Client connected");
        StreamWriter sw = new(pipeServer)
        {
            AutoFlush = true
        };

        Console.Write("Введите число: ");
        int number_r = int.Parse(Console.ReadLine());
        Console.Write("Введите возраст: ");
        int age_r = int.Parse(Console.ReadLine());
        Data msg = new()
        {
            number = number_r,
            age = age_r
        };

        byte[] bytes = new byte[Unsafe.SizeOf<Data>()];
        Unsafe.As<byte, Data>(ref bytes[0]) = msg;
        sw.BaseStream.Write(bytes, 0, bytes.Length);

        byte[] received_bytes = new byte[Unsafe.SizeOf<Data>()];
        sw.BaseStream.Read(received_bytes, 0, received_bytes.Length);
        Data received_data = Unsafe.As<byte, Data>(ref received_bytes[0]);
        Console.WriteLine($"Received data: number = {received_data.number}, age = {received_data.age}");
    }
}