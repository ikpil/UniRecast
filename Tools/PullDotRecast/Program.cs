using System.Diagnostics;

public class Program
{
    public static void InitWorkingDirectory()
    {
        for (int i = 0; i < 10; ++i)
        {
            var path = string.Join("../", Enumerable.Range(0, i).Select(x => "../"));
            var filePath = Path.Combine(path, "Program.cs");
            if (File.Exists(filePath))
            {
                var fullPath = Path.GetFullPath(filePath);
                var findlPAth = Path.GetDirectoryName(fullPath);
                Directory.SetCurrentDirectory(findlPAth);
            }

        }
    }
    
    public static void Main()
    {
        InitWorkingDirectory();
        Sync("../../../Sample1", "../../../Sample2", Array.Empty<string>());
    }

    private static void Sync(string srcRootPath, string dstRootPath, IList<string> ignoreFolders, string searchPattern = "*")
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
            Sync(sourceFolder.FullName, dest, ignoreFolders, searchPattern);
        }
    }
}