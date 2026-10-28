
using System;

public struct Ticket
{
    public byte N1, N2, N3, N4, N5;
    public byte Powerball;

    public Ticket(Random random)
    {
        var usedNumbers = new HashSet<byte>();

        N1 = GetUniqueNumber(random, usedNumbers);
        N2 = GetUniqueNumber(random, usedNumbers);
        N3 = GetUniqueNumber(random, usedNumbers);
        N4 = GetUniqueNumber(random, usedNumbers);
        N5 = GetUniqueNumber(random, usedNumbers);

        Powerball = (byte)random.Next(1, 27);
    }

    private static byte GetUniqueNumber(Random random, HashSet<byte> usedNumbers)
    {
        byte number;

        do
        {
            number = (byte)random.Next(1, 70);
        }
        while (!usedNumbers.Add(number));

        return number;
    }

    public bool Contains(byte number)
    {
        return N1 == number || N2 == number ||
               N3 == number || N4 == number ||
               N5 == number;
    }

    public int GetWinnerLevel(Ticket winning)
    {
        int matches = (winning.Contains(N1) ? 1 : 0) +
                      (winning.Contains(N2) ? 1 : 0) +
                      (winning.Contains(N3) ? 1 : 0) +
                      (winning.Contains(N4) ? 1 : 0) +
                      (winning.Contains(N5) ? 1 : 0);

        bool powerballMatch = Powerball == winning.Powerball;

        return matches switch
        {
            5 when powerballMatch => 9,
            5 => 8,
            4 when powerballMatch => 7,
            4 => 6,
            3 when powerballMatch => 5,
            3 => 4,
            2 when powerballMatch => 3,
            1 when powerballMatch => 2,
            0 when powerballMatch => 1,
            _ => 0
        };
    }
}
