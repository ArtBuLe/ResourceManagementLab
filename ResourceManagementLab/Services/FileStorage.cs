namespace ResourceManagementLab.Services;

// Теперь создадим класс, который самостоятельно управляет жизненным циклом файлового потока.
// Это важный пример разделения владения ресурсами:
// - `FileStorage` владеет `FileStream`;
// - `StreamReader` временно использует `FileStream`;
// - `StreamReader` не должен закрывать чужой ресурс.
public sealed class FileStorage : IDisposable
{
    // Класс владеет потоком. Значит, он обязан освободить его.
    private readonly FileStream _stream;

    // Позволяет контролировать повторный вызов методов после освобождения.
    private bool _disposed;

    public FileStorage(string filePath)
    {
        _stream = new FileStream(
            filePath,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.None);
    }

    public void Write(string text)
    {
        ThrowIfDisposed();

        byte[] data = System.Text.Encoding.UTF8.GetBytes(text);

        _stream.Write(data);
        _stream.Flush();
    }

    public string Read()
    {
        ThrowIfDisposed();

        _stream.Position = 0;

        // Почему в `StreamReader` указано `leaveOpen: true`?
        // Потому что `StreamReader` создаётся временно только для чтения. 
        // Он не должен закрывать поток, которым владеет `FileStorage`.
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
        if (_disposed)
        {
            return;
        }
        // Закрывает принадлежащий классу файловый поток.
        _stream.Dispose();

        _disposed = true;

        Console.WriteLine("FileStorage: ресурс освобождён.");
    }

    private void ThrowIfDisposed()
    {
        // Не позволяет использовать объект после `Dispose()`.
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);
    }
}