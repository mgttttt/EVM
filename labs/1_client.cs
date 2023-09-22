using System;
using System.IO.Pipes;
using System.Net.Mail;
using System.Runtime.CompilerServices;

public struct Data
{
    public int number;
    public int age;
}

class PipeClient
{
    static void Main()
    {
        using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "channel", PipeDirection.InOut);
        pipeClient.Connect();
        Console.WriteLine("Client connected to server");
        //StreamReader sr = new StreamReader(pipeClient);
        byte[] bytes = new byte[Unsafe.SizeOf<Data>()];
        pipeClient.Read(bytes, 0, bytes.Length);
        Data received_data = Unsafe.As<byte, Data>(ref bytes[0]);
        Console.WriteLine("Число: " + received_data.number);
        Console.WriteLine("Возраст: " + received_data.age);
        //Console.WriteLine($"Received data: name = {received_data.name}, age = {received_data.age}");
        received_data.number += received_data.age; 
        byte[] modified_bytes = new byte[Unsafe.SizeOf<Data>()];
        Unsafe.As<byte, Data>(ref modified_bytes[0]) = received_data;
        pipeClient.Write(modified_bytes, 0, modified_bytes.Length);
    }
}