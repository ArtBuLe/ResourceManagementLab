namespace ResourceManagementLab.Services;

public static class FileReaderWithoutUsing
{
    // П1: Начало изменений для Задания 1 (поиск проблемы и исправление)
    public static string ReadFile(string path)
    {
        // Проблема была здесь: создавались FileStream и StreamReader, но не вызывался Dispose().
        // FileStream владеет внешним ресурсом (дескриптором файла в ОС).
        // GC не гарантирует своевременное закрытие, так как финализация недетерминирована.
        // Исправление (Задание 1.4): добавлен using для гарантированного освобождения.

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}