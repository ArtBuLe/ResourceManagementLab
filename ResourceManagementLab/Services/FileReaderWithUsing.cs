namespace ResourceManagementLab.Services;

public static class FileReaderWithUsing
{
    // П1: Начало изменений для Задания 1.4 (исправленный метод)
    public static string ReadFile(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    // П1: Начало изменений для Задания 2 (проверка исключения)
    public static string ReadFileWithError(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);

        string content = reader.ReadToEnd();

        // П1 TODO: Создать искусственное исключение после чтения файла.
        throw new InvalidOperationException("Искусственная ошибка после чтения файла.");
    }
}