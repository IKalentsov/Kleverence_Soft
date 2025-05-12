using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testovoe.Task_3.Application;
using Testovoe.Task_3.Core.Interfaces;
using Testovoe.Task_3.Infrastructure.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ILogParser, LogParser>();
        services.AddSingleton<ILogWriter, LogWriter>();
        services.AddHostedService<LogStandardizerService>();
    })
    .Build();

await host.RunAsync();

    //static void Main()
    //{
    //    //string baseDir = AppContext.BaseDirectory;

    //    //string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\")); // Выходим из bin/Debug/netX.Y

    //    //string pathFiles = Path.Combine(projectDir, "Task_3\\_Files\\InputFormat_Type_INFO.txt");

    //    //string resultDir = Path.Combine(projectDir, "Task_3\\_ResultFiles");

    //    //// Создать папку, если её нет
    //    //Directory.CreateDirectory(resultDir);

    //    //Console.WriteLine(pathFiles);

    //    //if (File.Exists(pathFiles))
    //    //{
    //    //    string[] lines = File.ReadAllLines(pathFiles);

    //    //    foreach(var line in lines)
    //    //    {
    //    //        Console.WriteLine(line);
    //    //    }
    //    //}

    //    //var logProcessor = new LogProcessorNew();

    //    //logProcessor.ProcessLogs(pathFiles, resultDir, resultDir);

    //    ////Console.WriteLine(Directory.GetCurrentDirectory());
    //    ////Console.WriteLine("Task_3\\_Files");

    //    ////// Настройка DI контейнера
    //    //var services = new ServiceCollection();
    //    //ConfigureServices(services);
    //    ////using var serviceProvider = services.BuildServiceProvider();

    //    ////// Получение пути к файлам через консоль
    //    ////Console.WriteLine("Введите путь к входному файлу:");
    //    ////var inputPath = Console.ReadLine()!;

    //    ////Console.WriteLine("Введите путь для выходного файла:");
    //    ////var outputPath = Console.ReadLine()!;

    //    ////var problemsPath = Path.Combine(
    //    ////    Path.GetDirectoryName(outputPath) ?? Directory.GetCurrentDirectory(),
    //    ////    "problems.txt");

    //    ////// Обработка файла
    //    ////try
    //    ////{
    //    ////    var processor = serviceProvider.GetRequiredService<LogProcessor>();
    //    ////    processor.ProcessLogs(inputPath, outputPath, problemsPath);

    //    ////    Console.WriteLine("Обработка завершена успешно!");
    //    ////    Console.WriteLine($"Стандартизированные логи сохранены в: {outputPath}");
    //    ////    Console.WriteLine($"Проблемные записи сохранены в: {problemsPath}");
    //    ////}
    //    ////catch(Exception ex)
    //    ////{
    //    ////    Console.WriteLine($"Произошла ошибка: {ex.Message}");
    //    ////}
    //}

