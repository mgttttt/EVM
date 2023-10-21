using System;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.IO;

public struct Structure
{
    public int n;
    public int m;
}

class PipeServer
{
    private static PriorityQueue<Structure, int> dataQueue = new PriorityQueue<Structure, int>();
    private static Mutex mutex = new Mutex();

    private static Task Main()
    {
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;
        using NamedPipeServerStream pipeServer = new("channel", PipeDirection.InOut);
        Console.WriteLine("Waiting for client connection...");
        pipeServer.WaitForConnection();
        string fileName = "output.txt";
        string str = string.Empty;
        Console.WriteLine("Client connected");

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            source.Cancel();
            SaveToFile(fileName, str);
        };

        return Task.WhenAll(SenderTask(pipeServer, token), ReceiverTask(pipeServer, token));

        Task SenderTask(NamedPipeServerStream pipeServer, CancellationToken token)
        {
            return Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    int _n, _m, _priority;
                    Console.Write("Enter n: ");
                    int.TryParse(Console.ReadLine(), out _n);
                    Console.Write("Enter m: ");
                    int.TryParse(Console.ReadLine(), out _m);
                    Console.Write("Enter priority: ");
                    if (!int.TryParse(Console.ReadLine(), out _priority))
                        _priority = 0;
                    Structure data = new Structure
                    {
                        n = _n,
                        m = _m,
                    };
                    mutex.WaitOne();
                    dataQueue.Enqueue(data, _priority);
                    Console.WriteLine(dataQueue.Count);
                    mutex.ReleaseMutex();

                }
            });
            
        }
        async Task ReceiverTask(NamedPipeServerStream pipeServer, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Structure st;
                int pr;
                mutex.WaitOne();
                bool flag = dataQueue.TryDequeue(out st, out pr);
                mutex.ReleaseMutex();
                if (flag)
                {
                    byte[] dataBytes = new byte[Unsafe.SizeOf<Structure>()];
                    Unsafe.As<byte, Structure>(ref dataBytes[0]) = st;
                    pipeServer.Write(dataBytes, 0, dataBytes.Length);
                    byte[] receivedBytes = new byte[Unsafe.SizeOf<Structure>()];
                    if (pipeServer.Read(receivedBytes, 0, receivedBytes.Length) == receivedBytes.Length)
                    {
                        st = Unsafe.As<byte, Structure>(ref receivedBytes[0]);
                    }
                    str += $"n = {st.n}; m = {st.m}; priority= {pr}\n"; 
                }
            }
        }
    }

    private static void SaveToFile(string name, string str)
    {
        File.AppendAllText(name, str);
    }
}
