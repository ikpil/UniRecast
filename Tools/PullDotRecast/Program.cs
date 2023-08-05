using System.Collections.Immutable;
using System.Diagnostics;

public class CsProj
{
    public string Name { get; init; }

    public CsProj(string name)
    {
        Name = name;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var directoryName = GetProgramDirectory("Program.cs");
        Directory.SetCurrentDirectory(directoryName);
        
        // for dotnet run
        var safeArgs = args
            .Where(x => x != "--argument")
            .ToList();

        if (0 >= safeArgs.Count)
        {
            throw new Exception("not found source directory");
        }
            

        var ignorePaths = ImmutableArray.Create("bin", "obj");
        var projs = ImmutableArray.Create(
            new CsProj("DotRecast.Core"),
            new CsProj("DotRecast.Recast"),
            new CsProj("DotRecast.Detour"),
            new CsProj("DotRecast.Detour.Crowd"),
            new CsProj("DotRecast.Detour.Dynamic"),
            new CsProj("DotRecast.Detour.Extras"),
            new CsProj("DotRecast.Detour.TileCache"),
            new CsProj("DotRecast.Recast.Toolset")
        );

        string dotRecastPath = safeArgs[0];
        dotRecastPath = Path.GetFullPath(dotRecastPath);
        if (!Directory.Exists(dotRecastPath))
        {
            throw new Exception("not found source directory");
        }

        string destRoot = "../../UniRecast/Assets/Plugins/UniRecast/Contrib/DotRecast";
        destRoot = Path.GetFullPath(destRoot);
        if (Directory.Exists(destRoot))
        {
            Directory.Delete(destRoot, true);
        }

        foreach (var proj in projs)
        {
            var sourcePath = $"../../../DotRecast/src/{proj.Name}";
            var destPath = $"{destRoot}/src/{proj.Name}";
            SyncFiles(sourcePath, destPath, ignorePaths, "*.cs");
        }
    }

    public static string GetProgramDirectory(string fileName)
    {
        for (int i = 0; i < 10; ++i)
        {
            var path = string.Join("", Enumerable.Range(0, i).Select(x => "../"));


            var filePath = Path.Combine(path, fileName);
            if (File.Exists(filePath))
            {
                var fullPath = Path.GetFullPath(filePath);
                var directoryName = Path.GetDirectoryName(fullPath);
                return directoryName;
            }
        }

        return Directory.GetCurrentDirectory();
    }

    private static void SyncFiles(string srcRootPath, string dstRootPath, IList<string> ignoreFolders, string searchPattern = "*")
    {
        // 끝에서부터 이그노어 폴더일 경우 패스
        var destLastFolderName = Path.GetFileName(dstRootPath);
        if (ignoreFolders.Any(x => x == destLastFolderName))
            return;

        if (!Directory.Exists(dstRootPath))
            Directory.CreateDirectory(dstRootPath);

        // 소스파일 추출
        var sourceFiles = Directory.GetFiles(srcRootPath, searchPattern).ToList();
        var sourceFolders = Directory.GetDirectories(srcRootPath)
            .Select(x => new DirectoryInfo(x))
            .ToList();

        // 대상 파일 추출
        var destinationFiles = Directory.GetFiles(dstRootPath, searchPattern).ToList();
        var destinationFolders = Directory.GetDirectories(dstRootPath)
            .Select(x => new DirectoryInfo(x))
            .ToList();

        // 대상에 파일이 있는데, 소스에 없을 경우, 대상 파일을 삭제 한다.
        foreach (var destinationFile in destinationFiles)
        {
            var destName = Path.GetFileName(destinationFile);
            var found = sourceFiles.Any(x => Path.GetFileName(x) == destName);
            if (found)
                continue;

            File.Delete(destinationFile);
            Console.WriteLine($"delete file - {destinationFile}");
        }

        // 대상에 폴더가 있는데, 소스에 없을 경우, 대상 폴더를 삭제 한다.
        foreach (var destinationFolder in destinationFolders)
        {
            var found = sourceFolders.Any(sourceFolder => sourceFolder.Name == destinationFolder.Name);
            if (found)
                continue;

            Directory.Delete(destinationFolder.FullName, true);
            Console.WriteLine($"delete folder - {destinationFolder.FullName}");
        }

        // 소스 파일을 복사 한다.
        foreach (var sourceFile in sourceFiles)
        {
            var name = Path.GetFileName(sourceFile);
            var dest = Path.Combine(dstRootPath, name);
            File.Copy(sourceFile, dest, true);
            Console.WriteLine($"copy - {sourceFile} => {dest}");
        }

        // 대상 폴더를 복사 한다
        foreach (var sourceFolder in sourceFolders)
        {
            var dest = Path.Combine(dstRootPath, sourceFolder.Name);
            SyncFiles(sourceFolder.FullName, dest, ignoreFolders, searchPattern);
        }
    }
}