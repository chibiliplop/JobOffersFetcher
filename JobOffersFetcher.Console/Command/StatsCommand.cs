using System.IO.Compression;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using JobOffersFetcher.Core.Entities;
using JobOffersFetcher.Core.Services;

namespace JobOffersFetcher.Console.Command;

[Command("stats", Description = "Log statistics of the job offers. If path is provided, the stats will be saved in a zip file as csv format")]
public class StatsCommand : ICommand
{
    private readonly StatisticServices _statisticServices;

    [CommandOption("path", 'p', Description = "Path to save the stats")]
    public string Path { get; set; }
    [CommandOption("filename", 'f', Description = "Filename to save the stats")]
    public string FileName { get; set; } = "JobOffersFetcherStats.zip";
    
    public StatsCommand(StatisticServices statisticServices)
    {
        _statisticServices = statisticServices;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        List<Statistic> statPerCommune = await _statisticServices.GetStatPerCommune();
        List<Statistic> statEntreprise = await _statisticServices.GetStatPerEntreprise();
        List<Statistic> statPerTypeContrat = await _statisticServices.GetStatPerTypeContrat();

        if (Path != null)
        {
            CreateArchive(statPerCommune, statEntreprise, statPerTypeContrat, $"{Path}/{FileName}");
        }
        else
        {
            WriteToConsole(statPerCommune, statEntreprise, statPerTypeContrat, console);
        }
        
    }

    public static void CreateArchive(List<Statistic> statPerCommune, List<Statistic> statEntreprise, List<Statistic> statPerTypeContrat, string path)
    {
        using (var fileStream = new FileStream(path, FileMode.Create))
        using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
        {
            WriteToCsv(statPerCommune, archive, "Commune", "StatPerCommune.csv");
            WriteToCsv(statEntreprise, archive, "Entrprise", "StatPerEntreprise.csv");
            WriteToCsv(statPerTypeContrat, archive, "Contrat type", "StatPerTypeContrat.csv");
        }
    }
    
    public static void WriteToCsv(List<Statistic> stats, ZipArchive archive, string headerName, string fileName)
    {
        var entry = archive.CreateEntry(fileName);
        using (var stream = entry.Open())
        using (var writer = new StreamWriter(stream))
        {
            writer.WriteLine($"{headerName},Count");
            foreach (Statistic stat in stats)
            {
                writer.WriteLine($"{stat.Key},{stat.Count}");
            }
        }
    }

    public static void WriteToConsole(List<Statistic> statPerCommune, List<Statistic> statEntreprise, List<Statistic> statPerTypeContrat, IConsole console)
    {
        DisplayConsoleHeader("Statistiques par commune", console);
        DisplayConsoleStats(statPerCommune, console);
        DisplayConsoleHeader("Statistiques par entreprise", console);
        DisplayConsoleStats(statEntreprise, console);
        DisplayConsoleHeader("Statistiques par type de contrat", console);
        DisplayConsoleStats(statPerTypeContrat, console);
    }

    public static void DisplayConsoleHeader(string header, IConsole console)
    {
        console.ForegroundColor = ConsoleColor.Yellow;
        console.Output.WriteLine(header);
        console.ResetColor();
    }
    
    public static void DisplayConsoleStats(List<Statistic> stats, IConsole console)
    {
        foreach (Statistic stat in stats)
        {
            console.Output.WriteLine($"{stat.Key.PadRight(30)} | {stat.Count.ToString().PadRight(10)}");
        }
        console.ResetColor();
    }
}