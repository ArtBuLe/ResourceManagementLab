// ОТВЕТЫ НА ВОПРОСЫ П2:
// 1. Использование после Dispose: объект должен выбрасывать ObjectDisposedException.
// 2. Повторный Dispose: должен быть идемпотентным (ничего не делать и не вызывать ошибок).
// 3. leaveOpen: указывает StreamReader НЕ закрывать переданный поток, так как владелец потока — другой объект.
// 4. IAsyncDisposable: используется для асинхронного освобождения ресурсов (DisposeAsync), чтобы не блокировать поток выполнения.

// П2: Задания 9
/*
 * Анализ необходимости реализации IDisposable:
 * 
 * 1. Player (string Name, int Score): Нет.
 *    Объяснение: содержит только управляемые типы. GC автоматически очистит память.
 * 
 * 2. FileStorage (владеет FileStream): Да.
 *    Объяснение: владеет неуправляемым ресурсом (дескриптор файла ОС), который требует явного и своевременного освобождения.
 * 
 * 3. Report (List<string>): Обычно нет.
 *    Объяснение: List управляется GC. Если бы внутри хранился Stream или другая неуправляемая сущность, тогда да.
 * 
 * 4. SocketManager (владеет Socket): Да.
 *    Объяснение: Сокет — это неуправляемый сетевой ресурс, требующий явного закрытия соединения.
 * 
 * 5. Класс, принимающий чужой Stream и читающий его: Не обязательно.
 *    Объяснение: Если класс не владеет потоком (использует leaveOpen: true), он не должен его закрывать. 
 *    Ответственность за освобождение лежит на создателе (владельце) потока.
 */

using ResourceManagementLab.Services;

namespace ResourceManagementLab;

internal class Program
{
    private static async Task Main() // П2: изменено на async Task для Задания 10
    {
        string dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDirectory);
        string filePath = Path.Combine(dataDirectory, "sample.txt");
        File.WriteAllText(filePath, "Hello from C#!\nResource management example.");

        Console.WriteLine("=== П2: RESOURCE MANAGEMENT LAB ===");
        Console.WriteLine();

        // П2: Начало изменений для Заданий 5, 6, 7 (FileStorage, защита от использования, повторный Dispose)
        Console.WriteLine("1. Проверка FileStorage и повторного Dispose:");
        var storage = new FileStorage(filePath);
        storage.Write("Hello from FileStorage");
        Console.WriteLine(storage.Read());

        storage.Dispose();
        Console.WriteLine("Первый Dispose выполнен.");

        storage.Dispose(); // Повторный вызов не должен вызывать ошибку
        Console.WriteLine("Повторный Dispose выполнен без ошибок.");

        try
        {
            storage.Write("Тест после Dispose");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"Перехвачено ожидаемое исключение: {ex.GetType().Name}");
        }

        Console.WriteLine();

        // П2: Начало изменений для Задания 8 (Проверка владения ресурсом)
        Console.WriteLine("2. Проверка владения ресурсом (leaveOpen: true):");
        using var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("Владелец потока - Program"));
        stream.Position = 0;

        // П2 TODO: Передать stream другому компоненту и не допустить, чтобы он закрыл поток.
        using var reader = new StreamReader(stream, leaveOpen: true); // П2 TODO
        string content = reader.ReadToEnd();
        Console.WriteLine($"Прочитано через внешний StreamReader: {content}");

        // П2 TODO: Использовать переданный поток после закрытия компонента-пользователя.
        stream.Position = 0; // П2 TODO
        Console.WriteLine("Поток всё ещё открыт и доступен владельцу после закрытия StreamReader.");

        Console.WriteLine();

        // П2: Начало изменений для Задания 10 (Асинхронное освобождение)
        Console.WriteLine("4. Проверка IAsyncDisposable:");
        await using (var asyncStorage = new AsyncFileStorage(filePath)) // П2 TODO
        {
            await asyncStorage.WriteAsync("Асинхронная запись данных."); // П2 TODO
            string asyncContent = await asyncStorage.ReadAsync(); // П2 TODO
            Console.WriteLine($"Асинхронно прочитано: {asyncContent}");
        }
        // DisposeAsync вызывается автоматически здесь

        Console.WriteLine();
        Console.WriteLine("Работа программы завершена.");
    }
}