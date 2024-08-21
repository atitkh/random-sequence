class RandomSequenceGenerator
{
    static void Main()
    {
        int sequenceLength = 50;
        char[] randomSequence;
        List<char> replacedChars = [];
        Random random = new();

        // user options
        Console.WriteLine("Press 1 to generate a random sequence or 2 to enter a sequence manually.");
        string choice = Console.ReadLine() ?? string.Empty;

        switch (choice)
        {
            case "1":
                randomSequence = GenerateSequence(sequenceLength);
                break;
            case "2":
                Console.WriteLine("Enter a sequence: ");
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid input. Generating random sequence.");
                    randomSequence = GenerateSequence(sequenceLength);
                    break;
                }

                randomSequence = input.ToCharArray();
                break;
            default:
                Console.WriteLine("Invalid choice. Generating random sequence.");
                randomSequence = GenerateSequence(sequenceLength);
                break;
        }

        Console.WriteLine("Generated Sequence: " + new string(randomSequence));

        // verify sequence
        bool hasRepetition = VerifyRepeatedLetters(randomSequence);
        bool hasProhibitedSubstring = VerifySubstring(randomSequence, "xyz");

        if (!hasRepetition && !hasProhibitedSubstring)
        {
            Console.WriteLine($"Sequence is valid. (Repetition: {hasRepetition}, Contains 'xyz': {hasProhibitedSubstring})");
        }
        else
        {
            // fix sequence
            Console.WriteLine($"Sequence is invalid. (Repetition: {hasRepetition}, Contains 'xyz': {hasProhibitedSubstring})");
            Console.WriteLine("Would you like to fix the sequence? (Y/N)");
            string fixChoice = Console.ReadLine() ?? string.Empty;
            if (fixChoice.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                bool isValid = false;
                while (!isValid)
                {
                    replacedChars.Clear();
                    for (int i = 0; i < randomSequence.Length - 2; i++)
                    {
                        if (hasRepetition && char.ToLower(randomSequence[i]) == char.ToLower(randomSequence[i + 1]))
                        {
                            replacedChars.Add(randomSequence[i + 1]);
                            randomSequence[i + 1] = (char)random.Next('a', 'z' + 1);
                        }
            
                        if (hasProhibitedSubstring && char.ToLower(randomSequence[i]) == 'x' && char.ToLower(randomSequence[i + 1]) == 'y' && char.ToLower(randomSequence[i + 2]) == 'z')
                        {
                            replacedChars.Add(randomSequence[i + 1]);
                            replacedChars.Add(randomSequence[i + 2]);
                            randomSequence[i + 1] = (char)random.Next('a', 'z' + 1);
                            randomSequence[i + 2] = (char)random.Next('a', 'z' + 1);
                        }
                    }
            
                    isValid = !VerifyRepeatedLetters(randomSequence) && !VerifySubstring(randomSequence, "xyz");
                }
            
                Console.WriteLine("Fixed Sequence: " + new string(randomSequence));
                Console.WriteLine("Replaced characters: " + string.Join(", ", replacedChars));
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
            if (char.ToLower(sequence[i]) == char.ToLower(sequence[i + 1]))
            {
                return true;
            }
        }
        return false;
    }

    static bool VerifySubstring(char[] sequence, string substring)
    {
        string sequenceStr = new(sequence);
        return sequenceStr.Contains(substring, StringComparison.CurrentCultureIgnoreCase);
    }
}
