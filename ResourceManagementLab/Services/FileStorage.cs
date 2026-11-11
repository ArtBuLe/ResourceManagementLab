namespace ResourceManagementLab.Services;

// П2: Начало изменений для Заданий 5, 6, 7
public sealed class FileStorage : IDisposable
{
    private readonly FileStream _stream;
    private bool _disposed;

    // П2 TODO 1: Создать FileStream. Использовать FileMode.OpenOrCreate. Разрешить чтение и запись.
    public FileStorage(string path)
    {
        _stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    public void Write(string text)
    {
        // П2 TODO 2: Проверить, не был ли объект освобождён.
        ThrowIfDisposed();

        // П2 TODO 3: Преобразовать текст в байты.
        byte[] data = System.Text.Encoding.UTF8.GetBytes(text);

        // П2 TODO 4: Записать данные в поток.
        _stream.Write(data);
        _stream.Flush();
    }

    public string Read()
    {
        // П2 TODO 5: Проверить состояние объекта.
        ThrowIfDisposed();

        // П2 TODO 6: Установить позицию потока в начало.
        _stream.Position = 0;

        // П2 TODO 7: Прочитать содержимое (с leaveOpen: true, так как поток принадлежит FileStorage).
        using var reader = new StreamReader(
            _stream,
            System.Text.Encoding.UTF8,
            detectEncodingFromByteOrderMarks: true,
            bufferSize: 1024,
            leaveOpen: true);

        return reader.ReadToEnd();
    }

    public void Dispose()
    {
        // П2 TODO 8: Не допустить повторного освобождения.
        if (_disposed) return;

        // П2 TODO 9: Освободить FileStream.
        _stream.Dispose();

        // П2 TODO 10: Установить флаг _disposed.
        _disposed = true;
        Console.WriteLine("FileStorage: ресурс освобождён.");
    }

    // П2: Начало изменений для Задания 6 (Защита от использования после Dispose)
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}