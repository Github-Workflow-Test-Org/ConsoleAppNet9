// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using System.Text;

var taint = string.Empty;
if (args.Length>1)
{
    taint = args[1]; //Propagates taint. 
}

var file = System.IO.File.OpenRead(taint); //CWEID73

const string RequestUri = "https://microsoftedge.github.io/Demos/json-dummy-data/64KB.json";
using var client = new HttpClient();

IAsyncEnumerable<Person?> people = client.GetFromJsonAsAsyncEnumerable<Person>(RequestUri); //Will inject
int counter = 0;
await foreach (var person in people)
{
    counter++;
    Console.WriteLine($"{counter:000} - In this case '{person.Name}' speaks '{person.Language}'");

    if (counter==1 && File.Exists(person.Name))
    {
        var content = File.ReadAllTextAsync(person.Name); //CWEID 73
        Console.WriteLine(content);
    }

    if (counter==10 && File.Exists(person.Bio))
    {
        var process = System.Diagnostics.Process.Start(person.Bio); //CWEID 78
    }
}

var temp = "/tmp/test.zip";
if (args.Length > 0)
{
    taint = args.First();
}

if (!System.IO.File.Exists(taint))
{
    //public static void CreateFromDirectory(string sourceDirectoryName, Stream destination, ...);
    System.IO.Compression.ZipFile.CreateFromDirectory(taint, System.IO.File.OpenWrite(temp)); //CWEID 73 on `taint`
    System.IO.Compression.ZipFile.CreateFromDirectory("/tmp", System.IO.File.OpenWrite(taint)); //CWEID 73 on OpenWrite arg
    System.IO.Compression.ZipFile.CreateFromDirectory(taint, System.IO.File.OpenWrite(temp), System.IO.Compression.CompressionLevel.Optimal, true); //CWEID 73 on `taint`
    System.IO.Compression.ZipFile.CreateFromDirectory("/tmp", System.IO.File.OpenWrite(taint), System.IO.Compression.CompressionLevel.Optimal, false); //CWEID 73 on OpenWrite arg
    System.IO.Compression.ZipFile.CreateFromDirectory(taint, System.IO.File.OpenWrite(temp), System.IO.Compression.CompressionLevel.Optimal, true, System.Text.Encoding.UTF8); //CWEID 73 on `taint`
    System.IO.Compression.ZipFile.CreateFromDirectory("/tmp", System.IO.File.OpenWrite(taint), System.IO.Compression.CompressionLevel.Optimal, false, System.Text.Encoding.UTF8); //CWEID 73 on OpenWrite arg

}

if (System.IO.File.Exists(temp))
{
    //public static void ExtractToDirectory(Stream source, string destinationDirectoryName, ...);
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(temp),taint); //CWEID 73 on `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(taint),"/tmp"); //CWEID 73 on OpenRead `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(temp),taint, true); //CWEID 73 on `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(taint),"/tmp", false); //CWEID 73 on OpenRead `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(temp),taint, Encoding.UTF8); //CWEID 73 on `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(taint),"/tmp", Encoding.UTF8); //CWEID 73 on OpenRead `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(temp),taint, Encoding.UTF8, true); //CWEID 73 on `taint`.
    System.IO.Compression.ZipFile.ExtractToDirectory(System.IO.File.OpenRead(taint),"/tmp", Encoding.UTF8, false); //CWEID 73 on OpenRead `taint`.
}

public record Person(string Name, string Language, string Id, string Bio, double Version);