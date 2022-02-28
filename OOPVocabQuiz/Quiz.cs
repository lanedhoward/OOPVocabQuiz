using System;
using System.Collections.Generic;
using System.Text;
using static OOPVocabQuiz.ConsoleUtils;

namespace OOPVocabQuiz
{
    class Quiz
    {
        private string title = "Lane's OOP Vocab Quiz";
        private Dictionary<string, string> Terms = new Dictionary<string, string>();

        public void Start()
        {
            LoadData();
            Menu();
            //Print(ReadAllData());
        }

        private void LoadData()
        {
            
            string[] data = LoadTextLinesFromFile("../../../data/terms.txt");

            for (int i = 0; i < data.Length; i += 2)
            {
                Terms.Add(data[i], data[i + 1]);
            }

        }

        private string ReadAllData()
        {
            string output = "";
            foreach (string key in Terms.Keys)
            {
                output += key + ":  " + Terms[key] + "\n";
            }

            return output;
        }

        private void Menu()
        {
            
            Print("\t" + title);
            Print();

            string[] options = { "Quiz", "Show all terms and definitions", "Search for a term", "Exit" };

            for (int i = 0; i < options.Length; i++)
            {
                Print($"\t{i + 1}.  {options[i]}");
            }

            switch(GetInputIntKey(1,options.Length))
            {
                case 1:
                    //start quiz
                    PlayQuiz();
                    break;
                case 2:
                    //show all terms
                    ShowTerms();
                    break;
                case 3:
                    //search for term
                    Print("Error, not implemented");
                    WaitForKeyPress(true);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }

            ClearScrollable();
            Menu();
        }

        private void PlayQuiz()
        {
            Console.Clear();
            Print("How many questions would you like to be on the quiz? ");
            int quizLength = GetInputInt(1, Terms.Count);

            int score = 0;

            Random random = new Random();

            //get terms for quiz, make sure no repeats
            Queue<string> keysInQuestionSet = new Queue<string>();
            List<string> allKeys = new List<string>(Terms.Keys);

            for (int i = 0; i < quizLength; i++)
            {
                int r = random.Next(0, allKeys.Count);
                keysInQuestionSet.Enqueue(allKeys[r]);
                allKeys.RemoveAt(r);
            }

            //do quiz
            for (int i = 0; i < quizLength; i++)
            {
                Print($"Question {i + 1} / {quizLength}; Score = {score}");
                Print();

                string currentQuestionKey = keysInQuestionSet.Dequeue();

                Print("Which term best matches this definition : ");
                Print(Terms[currentQuestionKey]);

                //get all the keys again
                allKeys = new List<string>(Terms.Keys);

                //remove the correct key so it doesn't get chosen randomly
                allKeys.Remove(currentQuestionKey);

                //fill up a 4 slot array with random keys
                int numberOfChoices = 4;
                string[] answers = new string[numberOfChoices];

                for (int j = 0; j < numberOfChoices; j++)
                {
                    int r = random.Next(0, allKeys.Count);
                    answers[j] = allKeys[r];
                    allKeys.RemoveAt(r);
                }

                //replace one of those keys with the real key
                answers[random.Next(0, answers.Length - 1)] = currentQuestionKey;


                //display menu
                for (int k = 0; k < answers.Length; k++)
                {
                    Print($"\t{k + 1}.  {answers[k]}");
                }

                int choice = GetInputIntKey(1, answers.Length);

                if (answers[choice-1] == currentQuestionKey)
                {
                    Print("Correct! +1 point.");
                    score += 1;
                }
                else
                {
                    Print($"Incorrect, the answer was {currentQuestionKey}");
                }

                WaitForKeyPress(true);
                ClearScrollable();

            }

            //once all questions are done, display summary
            Print($"You answered {score} questions correctly out of {quizLength} questions. ");
            Print("Thanks for playing!");
            WaitForKeyPress(true);


        }

        private void ShowTerms()
        {
            Console.Clear();
            Print(ReadAllData());
            WaitForKeyPress(true);
        }

        private void TermSearch()
        {

        }

    }
}
