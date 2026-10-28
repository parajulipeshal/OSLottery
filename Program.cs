
class Program
{
    const int VendorCount = 3;
    const int DefaultTicketsPerVendor = 10_000_000;

    static void Main(string[] args)
    {
        int ticketsPerVendor = args.Length > 0
            ? int.Parse(args[0])
            : DefaultTicketsPerVendor;

        var tickets = new Ticket[VendorCount * ticketsPerVendor];

        Parallel.For(0, VendorCount, vendorId =>
        {
            SellTickets(tickets, vendorId, ticketsPerVendor);
        });

        long totalSold = tickets.LongLength;
        Console.WriteLine($"\nTotal tickets sold: {totalSold:N0}");

        Ticket winningTicket = new Ticket(Random.Shared);
        var statistics = new long[VendorCount][];

        Parallel.For(0, VendorCount, workerId =>
        {
            statistics[workerId] = GatherStatistics(
                tickets,
                workerId,
                ticketsPerVendor,
                winningTicket);
        });

        PrintResults(statistics, totalSold);
    }

    static void SellTickets(Ticket[] tickets, int vendorId, int ticketsPerVendor)
    {
        Random random = new Random(Random.Shared.Next());
        int start = vendorId * ticketsPerVendor;

        for (int i = start; i < start + ticketsPerVendor; i++)
        {
            tickets[i] = new Ticket(random);
        }

        Console.WriteLine($"Vendor {vendorId + 1} finished selling.");
    }

    static long[] GatherStatistics(
        Ticket[] tickets,
        int workerId,
        int ticketsPerVendor,
        Ticket winningTicket)
    {
        long[] counts = new long[10];
        int start = workerId * ticketsPerVendor;

        for (int i = start; i < start + ticketsPerVendor; i++)
        {
            counts[tickets[i].GetWinnerLevel(winningTicket)]++;
        }

        Console.WriteLine($"Worker {workerId + 1} finished counting.");
        return counts;
    }

    static void PrintResults(long[][] statistics, long totalSold)
    {
        long[] totals = new long[10];

        foreach (long[] counts in statistics)
        {
            for (int level = 0; level < 10; level++)
            {
                totals[level] += counts[level];
            }
        }

        string[] prizes = {
            "Losing Ticket",
            "$4",
            "$4",
            "$7",
            "$7",
            "$100",
            "$100",
            "$50,000",
            "$1 Million",
            "Grand Prize"
        };

        long totalCounted = 0;
        long totalWinners = 0;

        Console.WriteLine("\n===== LOTTERY RESULTS =====");

        for (int level = 9; level >= 0; level--)
        {
            Console.WriteLine(
                $"Level {level} ({prizes[level]}): {totals[level]:N0}"
            );

            totalCounted += totals[level];

            if (level > 0)
                totalWinners += totals[level];
        }

        Console.WriteLine("\n===== SUMMARY =====");
        Console.WriteLine($"Total Sold: {totalSold:N0}");
        Console.WriteLine($"Total Counted: {totalCounted:N0}");
        Console.WriteLine($"Winning Tickets: {totalWinners:N0}");
        Console.WriteLine($"Losing Tickets: {totals[0]:N0}");

        if (totalCounted == totalSold)
            Console.WriteLine("All tickets counted successfully!");
        else
            Console.WriteLine("Error: Some tickets are missing!");
    }
}
 