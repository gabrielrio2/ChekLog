using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string logFilePath = "C:/Users/bielr/Desktop/Teste/log.txt"; // MUDE O DIREITORIO PARA O SEU
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine("Arquivo de log não encontrado.");
            return;
        }

        string[] logLines = File.ReadAllLines(logFilePath);//LÊ AS LINHAS DO LOG
        Console.WriteLine($"Lendo arquivo de log: {logFilePath}");

        List<LogBlock> logBlocks = ExtractLogBlocks(logLines);//EXTRAI AS LINHAS DO LOG
        Console.WriteLine($"Total de blocos encontrados: {logBlocks.Count}");

        Dictionary<string, List<string>> errorBlocks = ProcessLogBlocks(logBlocks);//FALA O TOTAL DE LOGS COM FALHAS
        Console.WriteLine($"Total de blocos com erro encontrados: {errorBlocks.Count}");

        string resultFilePath = Path.Combine(Directory.GetCurrentDirectory(), "resultado.txt");//IMPRIME E GERA UM RESULTADO.TXT PODE SER ENCONTRADO NA PASTA ONDE O PROJETO ESTÁ SALVO \bin\Debug\net8.0
        WriteResultsToFile(resultFilePath, errorBlocks);
        Console.WriteLine($"Resultados escritos no arquivo {resultFilePath}");

        Console.WriteLine("\nConteúdo de resultado.txt:");
        PrintFileContent(resultFilePath);
    }

    static List<LogBlock> ExtractLogBlocks(string[] logLines)
    {
        List<LogBlock> logBlocks = new List<LogBlock>();
        LogBlock currentBlock = null;

        foreach (string line in logLines)
        {
            if (line.Contains("Iniciando interpretação da mensagem..."))
            {
                currentBlock = new LogBlock();
                currentBlock.Lines.Add(line);
                Console.WriteLine("Iniciando novo bloco.");
            }
            else if (line.Contains("Trabalho com pedidos foi finalizado."))
            {
                if (currentBlock != null)
                {
                    currentBlock.Lines.Add(line);
                    logBlocks.Add(currentBlock);
                    Console.WriteLine("Bloco finalizado e adicionado à lista.");
                    currentBlock = null;
                }
            }
            else if (currentBlock != null)
            {
                currentBlock.Lines.Add(line);
            }
        }

        return logBlocks;
    }

    static Dictionary<string, List<string>> ProcessLogBlocks(List<LogBlock> logBlocks)
    {
        Dictionary<string, List<string>> errorBlocks = new Dictionary<string, List<string>>();

        foreach (var block in logBlocks)
        {
            if (block.Lines.Any(line => line.Contains("System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.")))
            {
                string realmInicial = ExtractValue(block.Lines, "item.RealmInicial:");
                string mensagemHash = ExtractValue(block.Lines, "item.MensagemHash:");

                Console.WriteLine($"Bloco com erro encontrado: RealmInicial={realmInicial}, MensagemHash={mensagemHash}");

                if (realmInicial != null && mensagemHash != null)
                {
                    if (!errorBlocks.ContainsKey(realmInicial))
                    {
                        errorBlocks[realmInicial] = new List<string>();
                    }
                    errorBlocks[realmInicial].Add(mensagemHash);
                }
            }
        }

        return errorBlocks;
    }

    static string ExtractValue(List<string> lines, string prefix)
    {
        foreach (var line in lines)
        {
            if (line.Contains(prefix))
            {
                int startIndex = line.IndexOf(prefix) + prefix.Length;
                return line.Substring(startIndex).Trim();
            }
        }
        return null;
    }

    static void WriteResultsToFile(string filePath, Dictionary<string, List<string>> errorBlocks)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var entry in errorBlocks)
            {
                writer.WriteLine($"{entry.Key}");
                writer.WriteLine(string.Join(",\n", entry.Value.Select(hash => $"\"{hash}\"")));
                writer.WriteLine();
            }
        }
    }

    static void PrintFileContent(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
        else
        {
            Console.WriteLine($"O arquivo {filePath} não foi encontrado.");
        }
    }

    class LogBlock
    {
        public List<string> Lines { get; } = new List<string>();
    }
}
