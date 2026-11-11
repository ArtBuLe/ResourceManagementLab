namespace ResourceManagementLab.Services;

public static class MemoryDemo
{
    public static void Run()
    {
        Console.WriteLine("=== MEMORY DEMO ===");

        var first = new byte[1024];
        var second = new byte[1024 * 1024];
        var third = new byte[1024 * 1024 * 5];

        // П1: Начало изменений для Задания 4 (проверка поколений GC)
        Console.WriteLine($"Поколение first: {GC.GetGeneration(first)}");
        Console.WriteLine($"Поколение second: {GC.GetGeneration(second)}");
        Console.WriteLine($"Поколение third: {GC.GetGeneration(third)}");

        Console.WriteLine($"Сборки Gen 0: {GC.CollectionCount(0)}");
        Console.WriteLine($"Сборки Gen 1: {GC.CollectionCount(1)}");
        Console.WriteLine($"Сборки Gen 2: {GC.CollectionCount(2)}");

        // П1 TODO: Объяснить, почему размер объекта не определяет его поколение.
        // Размер не влияет на поколение. Поколение зависит от "возраста" объекта 
        // (сколько сборок мусора он пережил). Большой массив (third) может быть 
        // быстро удалён и остаться в Gen 0, а маленький объект может жить долго и перейти в Gen 2.
    }
}