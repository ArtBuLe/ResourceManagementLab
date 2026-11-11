// ОТВЕТЫ НА ВОПРОСЫ:
// 1. Стек: локальные переменные, вызовы методов (быстро, LIFO). Куча: объекты, управляется GC.
// 2. Достижимость: возможность добраться до объекта от корней GC (переменные стека, статические поля).
// 3. Поколения GC: оптимизация. Gen 0 проверяется часто (мусора много), Gen 2 — редко (мусора мало).
// 4. GC не закрывает файл сразу, так как управляет только памятью, а не внешними ресурсами ОС.
// 5. IDisposable: интерфейс для детерминированного (явного) освобождения ресурсов через Dispose().
// 6. using: синтаксический сахар для try/finally, гарантирующий вызов Dispose() при любом выходе.
// 7. Dispose вызывается при исключении, потому что using разворачивается в блок finally.
// 8. Dispose вызывается вручную (детерминированно), финализатор — автоматически GC (недетерминированно).
// 9. IAsyncDisposable: когда освобождение ресурса требует асинхронных операций (сеть, диск).
// 10. GC.Collect() вручную: убивает производительность и не гарантирует очистку внешних ресурсов.
// 11. Владение ресурсом: тот, кто создал или получил ресурс, обязан его освободить.
// 12. leaveOpen: запрещает StreamReader закрывать переданный ему поток, так как поток принадлежит другому владельцу.

using ResourceManagementLab.Services;

namespace ResourceManagementLab;

internal class Program
{
    private static void Main()
    {
        string dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDirectory);
        string filePath = Path.Combine(dataDirectory, "sample.txt");
        File.WriteAllText(filePath, "Hello from C#!\nResource management example.");

        Console.WriteLine("=== RESOURCE MANAGEMENT LAB ===");
        Console.WriteLine();

        Console.WriteLine("1. Чтение без using (исправлено на using по заданию 1.4, чтобы не блокировать файл):");
        string content1 = FileReaderWithoutUsing.ReadFile(filePath); // П1: начало изменений для Задания 1
        Console.WriteLine(content1);

        Console.WriteLine();
        Console.WriteLine("2. Чтение с using");
        string content2 = FileReaderWithUsing.ReadFile(filePath);
        Console.WriteLine(content2);

        Console.WriteLine();
        Console.WriteLine("3. Проверка поведения при исключении (Задание 2):"); // П1: начало изменений для Задания 2
        try
        {
            FileReaderWithUsing.ReadFileWithError(filePath);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Перехвачена ошибка: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("4. Проверка Dispose");
        using (var storage = new FileStorage(filePath))
        {
            storage.Write("Данные записаны через FileStorage.");
            Console.WriteLine(storage.Read());
        }

        Console.WriteLine();
        Console.WriteLine("5. Проверка повторного Dispose");
        var secondStorage = new FileStorage(filePath);
        secondStorage.Dispose();
        secondStorage.Dispose();

        Console.WriteLine();
        Console.WriteLine("6. Демонстрация GC (Задание 4):"); // П1: начало изменений для Задания 4
        MemoryDemo.Run();

        Console.WriteLine();
        Console.WriteLine("Работа программы завершена.");
    }
}