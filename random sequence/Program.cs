class RandomCharacterGenerator
{
    static void Main()
    {
        int sequenceLength = 20;
        char[] randomSequence;

        // user options
        Console.WriteLine("Press 1 to generate a random sequence or 2 to enter a sequence manually.");
        string choice = Console.ReadLine()?.ToString() ?? string.Empty;
        switch (choice)
        {
            case "1":
                randomSequence = GenerateSequence(sequenceLength);
                break;
            case "2":
                Console.WriteLine("Enter a sequence: ");
                string input = Console.ReadLine()?.ToString() ?? string.Empty;
                randomSequence = input.ToCharArray();
                break;
            default:
                randomSequence = GenerateSequence(sequenceLength);
                Console.WriteLine("Invalid choice. Generating random sequence.");
                break;
        }

        Console.WriteLine("Generated Sequence: " + new string(randomSequence));

        // verify sequence
        bool hasNoRepeatation = VerifyRepeatedLetters(randomSequence);
        bool containsDEL = VerifyContainsSubstring(randomSequence, "DEL");

        if (hasNoRepeatation && containsDEL)
        {
            Console.WriteLine("Sequence is valid.");
        }
        else
        {
            Console.WriteLine("Sequence is invalid. (Repetition: " + !hasNoRepeatation + ", Contains 'DEL': " + containsDEL + ")");
            Console.WriteLine("Would you like to fix the sequence? (Y/N)");
            string fixChoice = Console.ReadLine()?.ToString() ?? string.Empty;
            if (fixChoice.ToUpper() == "Y")
            {
                if (!hasNoRepeatation)
                {
                    for (int i = 0; i < randomSequence.Length - 1; i++)
                    {
                        if (randomSequence[i + 1] == randomSequence[i])
                        {
                            randomSequence[i + 1] = (char)(randomSequence[i] + 1);
                        }
                    }
                }

                if (!containsDEL)
                {
                    for (int i = 0; i < randomSequence.Length - 2; i++)
                    {
                        if (randomSequence[i] == 'D' && randomSequence[i + 1] == 'E' && randomSequence[i + 2] == 'L')
                        {
                            randomSequence[i + 1] = (char)(randomSequence[i] + 1);
                            randomSequence[i + 2] = (char)(randomSequence[i] + 2);
                        }
                    }
                }

                Console.WriteLine("Fixed Sequence: " + new string(randomSequence));
            }
        }
    }

    static char[] GenerateSequence(int length)
    {
        char[] sequence = new char[length];
        Random random = new();

        for (int i = 0; i < length; i++)
        {
            sequence[i] = (char)random.Next('A', 'z' + 1);
        }

        return sequence;
    }

    static bool VerifyRepeatedLetters(char[] sequence)
    {
        for (int i = 0; i < sequence.Length - 1; i++)
        {
            if (sequence[i + 1] == sequence[i])
            {
                return false;
            }
        }
        return true;
    }

    static bool VerifyContainsSubstring(char[] sequence, string substring)
    {
        string sequenceStr = new(sequence);
        return sequenceStr.Contains(substring);
    }
}
