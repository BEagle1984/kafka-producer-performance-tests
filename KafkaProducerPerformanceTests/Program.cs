using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Broker;
using Silverback.Messaging.Configuration;

namespace KafkaProducerPerformanceTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var statsList = new List<Stats>();

            var messageSizes = new[] {50, 100, 1_000, 10_000};
            // var messageSizes = new[] {50, 100};
            var messagesCount = 100_000;

            // Warmup
            await RunWithCallbacks(messagesCount, CreateMessage(10), null, null);

            foreach (var messageSize in messageSizes)
            {
                Console.WriteLine();
                Console.WriteLine();
                WriteTitle($"{messagesCount:#,###} messages of {messageSize:#,###} bytes");

                statsList.Add(await RunWithCallbacks(messagesCount, CreateMessage(messageSize), null, null));
                statsList.Add(await RunWithCallbacks(messagesCount, CreateMessage(messageSize), 100, null));
                statsList.Add(await RunWithCallbacks(messagesCount, CreateMessage(messageSize), 100, 50_000_000));

                statsList.Add(await RunSilverbackProduceAsyncWithCallbacks(messagesCount, CreateMessage(messageSize), null, null));
                statsList.Add(await RunSilverbackProduceAsyncWithCallbacks(messagesCount, CreateMessage(messageSize), 100, null));
                statsList.Add(await RunSilverbackProduceAsyncWithCallbacks(messagesCount, CreateMessage(messageSize), 100, 50_000_000));

                statsList.Add(await RunSilverbackRawProduceAsyncWithCallbacks(messagesCount, CreateMessage(messageSize), null, null));
                statsList.Add(await RunSilverbackRawProduceAsyncWithCallbacks(messagesCount, CreateMessage(messageSize), 100, null));
                statsList.Add(await RunSilverbackRawProduceAsyncWithCallbacks(messagesCount, CreateMessage(messageSize), 100, 50_000_000));

                statsList.Add(await RunSilverbackRawProduceWithCallbacks(messagesCount, CreateMessage(messageSize), null, null));
                statsList.Add(await RunSilverbackRawProduceWithCallbacks(messagesCount, CreateMessage(messageSize), 100, null));
                statsList.Add(await RunSilverbackRawProduceWithCallbacks(messagesCount, CreateMessage(messageSize), 100, 50_000_000));
            }

            WriteSummary(statsList);
        }

        private static async Task<Stats> RunBaseline(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Confluent.Kafka ProduceAsync Baseline", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "PLAINTEXT://localhost:9092",
                LingerMs = lingerMs,
                BatchSize = batchSize
            };
            var producer = new ProducerBuilder<byte[], byte[]>(producerConfig).Build();
            var stopwatch = Stopwatch.StartNew();

            for (int i = 1; i <= count; i++)
            {
                await producer.ProduceAsync("test", message);
                NotifyProduced(i);
            }

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunWithCallbacks(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Confluent.Kafka Produce w/ callback", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "PLAINTEXT://localhost:9092",
                LingerMs = lingerMs,
                BatchSize = batchSize
            };
            var producer = new ProducerBuilder<byte[], byte[]>(producerConfig).Build();
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                producer.Produce("test", message, report =>
                {
                    Interlocked.Increment(ref produced);
                    NotifyProduced(produced);

                    if (produced == count)
                        taskCompletionSource.TrySetResult();
                });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunWithoutAwait(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Confluent.Kafka ProduceAsync not awaited", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "PLAINTEXT://localhost:9092",
                LingerMs = lingerMs,
                BatchSize = batchSize
            };
            var producer = new ProducerBuilder<byte[], byte[]>(producerConfig).Build();
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                producer.ProduceAsync("test", message).ContinueWith(report =>
                {
                    Interlocked.Increment(ref produced);
                    NotifyProduced(produced);

                    if (produced == count)
                        taskCompletionSource.TrySetResult();
                });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunSilverbackProduceAsync(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Silverback ProduceAsync not awaited", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSilverback()
                .WithConnectionToMessageBroker(options => options.AddKafka())
                .AddEndpoints(endpoints => endpoints
                    .AddKafkaEndpoints(kafkaEndpoints => kafkaEndpoints
                        .Configure(config => { config.BootstrapServers = "PLAINTEXT://localhost:9092"; })
                        .AddOutbound<object>(endpoint => endpoint
                            .ProduceTo("test")
                            .Configure(config =>
                            {
                                config.LingerMs = lingerMs;
                                config.BatchSize = batchSize;
                            }))))
                .Services.BuildServiceProvider();

            var broker = serviceProvider.GetRequiredService<IBroker>();
            await broker.ConnectAsync();
            var producer = broker.GetProducer("test");
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                producer.ProduceAsync(message.Value).ContinueWith(report =>
                {
                    Interlocked.Increment(ref produced);
                    NotifyProduced(produced);

                    if (produced == count)
                        taskCompletionSource.TrySetResult();
                });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunSilverbackProduceAsyncWithCallbacks(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Silverback ProduceAsync w/ callbacks", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSilverback()
                .WithConnectionToMessageBroker(options => options.AddKafka())
                .AddEndpoints(endpoints => endpoints
                    .AddKafkaEndpoints(kafkaEndpoints => kafkaEndpoints
                        .Configure(config => { config.BootstrapServers = "PLAINTEXT://localhost:9092"; })
                        .AddOutbound<object>(endpoint => endpoint
                            .ProduceTo("test")
                            .Configure(config =>
                            {
                                config.LingerMs = lingerMs;
                                config.BatchSize = batchSize;
                            }))))

                .Services.BuildServiceProvider();

            var broker = serviceProvider.GetRequiredService<IBroker>();
            await broker.ConnectAsync();
            var producer = broker.GetProducer("test");
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                await producer.ProduceAsync(message.Value,
                    null,
                    () =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        if (produced == count)
                            taskCompletionSource.TrySetResult();
                    },
                    ex =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        taskCompletionSource.TrySetException(ex);
                    });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunSilverbackProduceWithCallbacks(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Silverback Produce w/ callbacks", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSilverback()
                .WithConnectionToMessageBroker(options => options.AddKafka())
                .AddEndpoints(endpoints => endpoints
                    .AddKafkaEndpoints(kafkaEndpoints => kafkaEndpoints
                        .Configure(config => { config.BootstrapServers = "PLAINTEXT://localhost:9092"; })
                        .AddOutbound<object>(endpoint => endpoint
                            .ProduceTo("test")
                            .Configure(config =>
                            {
                                config.LingerMs = lingerMs;
                                config.BatchSize = batchSize;
                            }))))
                .Services.BuildServiceProvider();

            var broker = serviceProvider.GetRequiredService<IBroker>();
            await broker.ConnectAsync();
            var producer = broker.GetProducer("test");
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                producer.Produce(message.Value,
                    null,
                    () =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        if (produced == count)
                            taskCompletionSource.TrySetResult();
                    },
                    ex =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        taskCompletionSource.TrySetException(ex);
                    });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunSilverbackRawProduceAsyncWithCallbacks(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Silverback RawProduceAsync w/ callbacks", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSilverback()
                .WithConnectionToMessageBroker(options => options.AddKafka())
                .AddEndpoints(endpoints => endpoints
                    .AddKafkaEndpoints(kafkaEndpoints => kafkaEndpoints
                        .Configure(config => { config.BootstrapServers = "PLAINTEXT://localhost:9092"; })
                        .AddOutbound<object>(endpoint => endpoint
                            .ProduceTo("test")
                            .Configure(config =>
                            {
                                config.LingerMs = lingerMs;
                                config.BatchSize = batchSize;
                            }))))
                .Services.BuildServiceProvider();

            var broker = serviceProvider.GetRequiredService<IBroker>();
            await broker.ConnectAsync();
            var producer = broker.GetProducer("test");
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                await producer.RawProduceAsync(message.Value,
                    null,
                    () =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        if (produced == count)
                            taskCompletionSource.TrySetResult();
                    },
                    ex =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        taskCompletionSource.TrySetException(ex);
                    });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static async Task<Stats> RunSilverbackRawProduceWithCallbacks(int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            var runTitle = GetRunTitle("Silverback RawProduce w/ callbacks", count, message, lingerMs, batchSize);
            WriteTitle(runTitle);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSilverback()
                .WithConnectionToMessageBroker(options => options.AddKafka())
                .AddEndpoints(endpoints => endpoints
                    .AddKafkaEndpoints(kafkaEndpoints => kafkaEndpoints
                        .Configure(config => { config.BootstrapServers = "PLAINTEXT://localhost:9092"; })
                        .AddOutbound<object>(endpoint => endpoint
                            .ProduceTo("test")
                            .Configure(config =>
                            {
                                config.LingerMs = lingerMs;
                                config.BatchSize = batchSize;
                            }))))
                .Services.BuildServiceProvider();

            var broker = serviceProvider.GetRequiredService<IBroker>();
            await broker.ConnectAsync();
            var producer = broker.GetProducer("test");
            var stopwatch = Stopwatch.StartNew();

            var produced = 0;
            var taskCompletionSource = new TaskCompletionSource();

            for (int i = 1; i <= count; i++)
            {
                producer.RawProduce(message.Value,
                    null,
                    () =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        if (produced == count)
                            taskCompletionSource.TrySetResult();
                    },
                    ex =>
                    {
                        Interlocked.Increment(ref produced);
                        NotifyProduced(produced);

                        taskCompletionSource.TrySetException(ex);
                    });
            }

            await taskCompletionSource.Task;

            return new Stats(runTitle, count, message.Value.Length, stopwatch.Elapsed);
        }

        private static Message<byte[], byte[]> CreateMessage(int size)
        {
            var rawMessage = Enumerable.Range(1, size).Select(i => (byte) i).ToArray();
            return new Message<byte[], byte[]>
            {
                Value = rawMessage
            };
        }

        private static void WriteTitle(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static string GetRunTitle(string title, int count, Message<byte[], byte[]> message, int? lingerMs, int? batchSize)
        {
            if (lingerMs != null)
                title += $", LingerMs={lingerMs:#,###}";

            if (batchSize != null)
                title += $", BatchSize={batchSize:#,###}";

            return title;
        }

        private static void NotifyProduced(int i)
        {
            if (i % 10000 == 0)
                Console.WriteLine($"Produced {i:#,###} messages");
        }

        private static void WriteSummary(List<Stats> statsList)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            WriteTitle("** RESULTS **");

            var maxLabelLength = statsList.Max(stats => stats.Label.Length);

            var groupedStats = statsList.GroupBy(stats => new
            {
                stats.Count,
                stats.MessageSize
            });

            foreach (var statsGroup in groupedStats)
            {
                WriteTitle($"{statsGroup.Key.Count:#,###} messages of {statsGroup.Key.MessageSize:#,###} bytes");

                var minElapsed = statsGroup.Min(stats => stats.Elapsed);

                foreach (var stats in statsGroup)
                {
                    Console.ForegroundColor = stats.Elapsed == minElapsed ? ConsoleColor.Green : ConsoleColor.White;

                    var performance = (stats.Count / stats.Elapsed.TotalSeconds).ToString("#,##0.00").PadLeft(10);

                    Console.Write(stats.Label.PadRight(maxLabelLength));
                    Console.Write(" : ");
                    Console.Write($"{performance} msg/s");
                    Console.Write($" | {stats.Count:#,###} in {stats.Elapsed.TotalSeconds:0.000}s");

                    if (stats.Elapsed > minElapsed)
                    {
                        Console.Write(" -> ");
                        Console.Write(((stats.Elapsed.TotalMilliseconds - minElapsed.TotalMilliseconds) / minElapsed.TotalMilliseconds).ToString("+0.00%").PadLeft(10));
                    }

                    Console.WriteLine();
                }
            }
        }
    }

    record Stats (string Label, int Count, int MessageSize, TimeSpan Elapsed);
}
