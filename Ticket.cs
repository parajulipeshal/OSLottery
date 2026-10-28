
using System;

public struct Ticket
{
    public byte N1, N2, N3, N4, N5;
    public byte Powerball;

    public Ticket(Random random)
    {
        // Generate 5 different white balls (1-69)
        N1 = (byte)random.Next(1, 70);

        do {
            N2 = (byte)random.Next(1, 70);
        } while (N2 == N1);

        do {
            N3 = (byte)random.Next(1, 70);
        } while (N3 == N1 || N3 == N2);

        do {
            N4 = (byte)random.Next(1, 70);
        } while (N4 == N1 || N4 == N2 || N4 == N3);

        do {
            N5 = (byte)random.Next(1, 70);
        } while (N5 == N1 || N5 == N2 ||
                 N5 == N3 || N5 == N4);

        // Generate Powerball (1-26)
        Powerball = (byte)random.Next(1, 27);
    }

    // Check whether a white ball matches
    public bool Contains(byte number)
    {
        return N1 == number || N2 == number ||
               N3 == number || N4 == number ||
               N5 == number;
    }

    // Find winning level (0-9)
    public int GetWinnerLevel(Ticket winning)
    {
        int matches = 0;

        if (winning.Contains(N1)) matches++;
        if (winning.Contains(N2)) matches++;
        if (winning.Contains(N3)) matches++;
        if (winning.Contains(N4)) matches++;
        if (winning.Contains(N5)) matches++;

        bool pb = Powerball == winning.Powerball;

        if (matches == 5 && pb) return 9;
        if (matches == 5) return 8;
        if (matches == 4 && pb) return 7;
        if (matches == 4) return 6;
        if (matches == 3 && pb) return 5;
        if (matches == 3) return 4;
        if (matches == 2 && pb) return 3;
        if (matches == 1 && pb) return 2;
        if (matches == 0 && pb) return 1;

        return 0; // Losing ticket
    }
}
