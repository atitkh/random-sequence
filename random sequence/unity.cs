using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SignalValidator : MonoBehaviour
{
    public string signalInput;
    [SerializeField] private bool debugMode = false;

    [Tooltip("Fix the signal if it is invalid.")]
    public bool fixIfInvalid = false;

    [Tooltip("Length of the sequence to generate.")]
    [SerializeField] private int sequenceLength = 50;
    private System.Random random = new System.Random();
    private List<char> replacedChars = new List<char>();
    private string[] prohibitedSubstrings = { "xyz", "delete" };
    private string refinedSignal;
    char[] signalArray = new char[0];

    void Start()
    {
        Debug.Log("Signal Validator Loaded");
        if (debugMode)
        {
            if (signalInput.Length == 0)
            {
                signalArray = GenerateSequence(sequenceLength);
                Debug.Log("Current Signal " + new string(signalArray));
            }
            ValidateSignal();
        }
    }

    public void ValidateSignal()
    {
        if (signalInput.Length > 0)
        {
            signalArray = signalInput.ToCharArray();
            Debug.Log("Current Signal " + signalInput);
        }

        if (signalArray.Length > 0)
        {
            Debug.Log("Validating signal...");
            (bool hasConsecutiveChar, Dictionary<int, char> consecutiveChars) = VerifyConsecutiveChar(signalArray);
            (bool hasProhibitedSubstring, List<int> prohibitedSubstringIndices) = VerifySubstring(signalArray, prohibitedSubstrings[0]);

            // List<int> prohibitedSubstringIndices = new List<int>();
            // bool hasProhibitedSubstring = false;

            // foreach (string substring in prohibitedSubstrings)
            // {
            //     (bool hasSubstring, List<int> indices) = VerifySubstring(signalArray, substring);
            //     if (hasSubstring)
            //     {
            //         hasProhibitedSubstring = true;
            //         prohibitedSubstringIndices.AddRange(indices);
            //     }
            // }

            if (!hasConsecutiveChar && !hasProhibitedSubstring)
            {
                Debug.Log($"Sequence is valid.");
            }
            else
            {
                Debug.Log($"Sequence is invalid.");
                if (hasConsecutiveChar) Debug.Log($"{consecutiveChars.Count} Consecutive letters found: {string.Join(", ", consecutiveChars.Values)} at index {string.Join(", ", consecutiveChars.Keys)}");
                if (hasProhibitedSubstring) Debug.Log($"{prohibitedSubstringIndices.Count} Prohibited substring found: {prohibitedSubstrings[0]} at index {string.Join(", ", prohibitedSubstringIndices)}");

                if (fixIfInvalid)
                {
                    bool isValid = false;
                    replacedChars.Clear();
                    while (!isValid)
                    {
                        char[] newSignalArray = signalArray;
                        if (hasConsecutiveChar)
                        {
                            foreach (var item in consecutiveChars)
                            {
                                char newChar = (char)random.Next('A', 'z' + 1);
                                newSignalArray[item.Key] = newChar;
                                replacedChars.Add(newChar);
                            }
                        }

                        if (hasProhibitedSubstring)
                        {
                            foreach (int index in prohibitedSubstringIndices)
                            {
                                string newSubstring = new string(GenerateSequence(prohibitedSubstrings[0].Length));
                                for (int i = 0; i < prohibitedSubstrings[0].Length; i++)
                                {
                                    newSignalArray[index + i] = newSubstring[i];
                                    replacedChars.Add(newSubstring[i]);
                                }
                            }
                        }

                        (hasConsecutiveChar, consecutiveChars) = VerifyConsecutiveChar(newSignalArray);
                        (hasProhibitedSubstring, prohibitedSubstringIndices) = VerifySubstring(newSignalArray, prohibitedSubstrings[0]);

                        if (!hasConsecutiveChar && !hasProhibitedSubstring)
                        {
                            isValid = true;
                            refinedSignal = new string(newSignalArray);
                        }
                    }

                    Debug.Log("Fixed Sequence: " + refinedSignal);
                    Debug.Log("Replaced characters: " + string.Join(", ", replacedChars));
                }
            }
        }
        else
        {
            Debug.Log("No signal to validate.");
        }
    }

    private char[] GenerateSequence(int length)
    {
        return Enumerable.Range(0, length)
            .Select(r => (char)random.Next('A', 'z' + 1))
            .ToArray();
    }

    public (bool, Dictionary<int, char>) VerifyConsecutiveChar(char[] sequence)
    {
        var consecutiveChars = sequence
            .Select((c, i) => new { Char = c, Index = i })
            .Where(x => x.Index < sequence.Length - 1 && x.Char == sequence[x.Index + 1])
            .ToDictionary(x => x.Index, x => x.Char);

        if (consecutiveChars.Count > 0)
        {
            return (true, consecutiveChars);
        }

        return (false, null);
    }

    public (bool, List<int>) VerifySubstring(char[] sequence, string substring)
    {
        string sequenceStr = new string(sequence);
        List<int> indices = new List<int>();
        int index = sequenceStr.IndexOf(substring, StringComparison.CurrentCultureIgnoreCase);

        while (index >= 0)
        {
            indices.Add(index);
            index = sequenceStr.IndexOf(substring, index + 1, StringComparison.CurrentCultureIgnoreCase);
        }

        return (indices.Count > 0, indices);
    }
}