using System;

class RandomCharacterGenerator
{
    static void Main(string[] args)
    {
        int sequenceLength = 20; // Define the length of the sequence
        char[] randomSequence = GenerateRandomSequence(sequenceLength);

        Console.WriteLine("Generated Sequence: " + new string(randomSequence));

        bool hasNoConsecutiveLetters = VerifyNoConsecutiveLetters(randomSequence);
        bool containsDEL = VerifyContainsSubstring(randomSequence, "DEL");

        if (hasNoConsecutiveLetters)
        {
            Console.WriteLine("The sequence is valid (no consecutive letters).");
        }
        else
        {
            Console.WriteLine("The sequence is invalid (contains consecutive letters).");
        }

        if (containsDEL)
        {
            Console.WriteLine("The sequence contains the substring 'DEL'.");
        }
        else
        {
            Console.WriteLine("The sequence does not contain the substring 'DEL'.");
        }
    }

    static char[] GenerateRandomSequence(int length)
    {
        char[] sequence = new char[length];
        Random random = new Random();
        string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        for (int i = 0; i < length; i++)
        {
            // Generate a random character from the combined upper and lower case alphabet
            sequence[i] = charSet[random.Next(charSet.Length)];
        }

        return sequence;
    }

    static bool VerifyNoConsecutiveLetters(char[] sequence)
    {
        for (int i = 0; i < sequence.Length - 1; i++)
        {
            if (sequence[i + 1] == sequence[i] + 1 || sequence[i + 1] == sequence[i] - 1)
            {
                return false; // Found consecutive letters
            }
        }
        return true; // No consecutive letters found
    }

    static bool VerifyContainsSubstring(char[] sequence, string substring)
    {
        string sequenceStr = new string(sequence);
        return sequenceStr.Contains(substring);
    }
}
