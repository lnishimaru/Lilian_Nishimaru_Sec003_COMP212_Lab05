using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lilian_Nishimaru_Sec003_COMP212_Lab05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Delegate for functions isEven and isOdd 
        //Receives one int and returns one bool

        public delegate bool NumberPredicate(int number);

        int[] int_array = new int[10];
        double[] double_array = new double[10];
        char[] char_split = new char[23];
        char[] char_array = new char[10];
        public MainWindow()
        {
            InitializeComponent();
        }
        //Event handler for calculateButton
        private async void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            //try to parse the input into a integer
            int number;
            if (Int32.TryParse(factorialTextBox.Text, out number))
            {
                //checks if the number is positive
                if (number < 0)
                {
                    factorialLabel.Content = "";
                    errorMessageLabel.Content = "Please check the value of the (1) factorial. Must be positive.";
                } else
                { 
                    //number must be less than 20 to fit the long variable
                    if (number > 20)
                    {
                        factorialLabel.Content = "";
                        errorMessageLabel.Content = "Please check the value of the (1) factorial. Must be less than 21.";
                    }
                    else
                    {
                        Task<long> task = Task.Run(() => CalculateFactorial(number));
                        await Task.WhenAll(task);
                        factorialLabel.Content = (task.Result.ToString());
                    }
                }
                
            } else
            {
                factorialLabel.Content = "";
                errorMessageLabel.Content = "Please check the value of the (1) factorial. Must be an integer.";
            }
            
        }
        //Event handler for evenOddButton
        private void evenOddButton_Click(object sender, RoutedEventArgs e)
        {
            //create an instance for the delegate
            NumberPredicate evenPredicate = IsEven;

            //try to parse the input into a integer
            int number;
            if (Int32.TryParse(numberTextBox.Text, out number))
            {
                if(evenPredicate(number) == true)
                {
                    errorMessageLabel.Content = null;
                    evenOddLabel.Content = "Even";
                }
                evenPredicate = IsOdd;
                if (evenPredicate(number) == true)
                {
                    errorMessageLabel.Content = null;
                    evenOddLabel.Content = "Odd";
                }
            }
            else
            {
                evenOddLabel.Content = null;
                errorMessageLabel.Content = "Please check the value of the (2) number. Must be an integer.";
            }

        }

        //Event handler to generate a list of numbers
        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            //clear the screen
            valuesListBox.Items.Clear();
            searchTextBox.Text = "";
            lowTextBox.Text = "";
            highTextBox.Text = "";
            outputValuesTextBox.Text = "";
            errorMessageLabel.Content = "";

            //creates a list of random integers
            if (integerRadioButton.IsChecked == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    int number = random.Next(10, 100);
                    int_array[i] = number;
                    valuesListBox.Items.Add(number.ToString());
                }
            }
            //creates a list of random doubles using extend method for NextDouble with max and min values
            if (doubleRadioButton.IsChecked == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    double number = random.NextDouble(10, 100);
                    number = Math.Round(number, 2);
                    double_array[i] = number;
                    valuesListBox.Items.Add(number.ToString("N2"));
                }
            }

            //creates a list of random chars
            string charSplit = "abcdefghijklmnopqrstuwxyz";
            char_split = charSplit.ToCharArray();
            
            if (charRadioButton.IsChecked == true)
            {
                for (int i = 0; i < 10; i++)
                {
                    int index = random.Next(0, 23);
                    valuesListBox.Items.Add(char_split[index].ToString());
                    char_array[i] = char_split[index];
                }
            }
            //Enables the search and display buttons 
            searchButton.IsEnabled = true;
            displayButton.IsEnabled = true;
        }

        //event handler for the search button
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == null || searchTextBox.Text == "")
            {
                errorMessageLabel.Content = "Please inform a value to search";
            } else
            {
                string[] arrayTest = valuesListBox.Items.OfType<string>().ToArray();
                int x = Search(arrayTest, searchTextBox.Text);
                if (x >= 0)
                {
                    MessageBox.Show($"Found in occurence {x}");
                } else
                {
                    MessageBox.Show($"Not found");
                }
            }
        }

        //event handler for displayButton
        private void displayButton_Click(object sender, RoutedEventArgs e)
        {
            //checking if the user informed correct parameters 
            if (string.IsNullOrEmpty(lowTextBox.Text))
            {
                errorMessageLabel.Content = "Invalid low value";
            }
            else
            {
                if (string.IsNullOrEmpty(highTextBox.Text))
                {
                    errorMessageLabel.Content = "Invalid high value";
                }
            }
            //check values for integers
            if (integerRadioButton.IsChecked == true)
            {
                int low, high;
                if (Int32.TryParse(lowTextBox.Text, out low) == false)
                {
                    errorMessageLabel.Content = "Invalid low value";
                }
                else
                {
                    if (Int32.TryParse(highTextBox.Text, out high) == false)
                    {
                        errorMessageLabel.Content = "Invalid high value";
                    }
                    else
                    {
                        Int32.TryParse(lowTextBox.Text, out low);
                        Int32.TryParse(highTextBox.Text, out high);
                        if (low > high)
                        {
                            errorMessageLabel.Content = "Low value has to be lower than high value";
                        }
                        else //no errors
                        {
                            errorMessageLabel.Content = null;
                            outputValuesTextBox.Text = Display(int_array, low, high);
                        }
                    }
                }
            }
            //check values for doubles
            if (doubleRadioButton.IsChecked == true)
            {
                double low, high;
                if (Double.TryParse(lowTextBox.Text, out low) == false)
                {
                    errorMessageLabel.Content = "Invalid low value";
                }
                else
                {
                    if (Double.TryParse(highTextBox.Text, out high) == false)
                    {
                        errorMessageLabel.Content = "Invalid high value";
                    }
                    else
                    {
                        Double.TryParse(lowTextBox.Text, out low);
                        Double.TryParse(highTextBox.Text, out high);
                        if (low > high)
                        {
                            errorMessageLabel.Content = "Low value has to be lower than high value";
                        }
                        else
                        {
                            errorMessageLabel.Content = null;

                            //  outputValuesTextBox.Text = Display(valuesListBox.Items.OfType<string>().ToArray(),
                            outputValuesTextBox.Text = Display(double_array, low, high);
                        }
                    }
                }
            }
            //check values for char
            if (charRadioButton.IsChecked == true)
            {
                char low, high;
                if (Char.TryParse(lowTextBox.Text, out low) == false)
                {
                    errorMessageLabel.Content = "Invalid low value";
                }
                else
                {
                    if (Char.TryParse(highTextBox.Text, out high) == false)
                    {
                        errorMessageLabel.Content = "Invalid high value";
                    } else
                    {
                        if (low.CompareTo(high) >= 1)
                        {
                            errorMessageLabel.Content = "Low value has to be lower than high value";
                        } else
                        {
                            errorMessageLabel.Content = null;
                            outputValuesTextBox.Text = Display(char_array, low, high);
                        }
                    }
                }

            }
        }

        //Method to calculate factorials
        private long CalculateFactorial(int number)
        {
            long result = 1;
            for (int i = 1; i <=number;i++)
            {
                result *= i;
            }          
            return result;
        }
        //Method to verify even numbers
        private static bool IsEven(int number) => number % 2 == 0;

        //Method to verify odd numbers;
        private static bool IsOdd(int number) => number % 2 == 1;

        //Method for search a generic list 
        public static int Search<T>(T[] dataArray, T searchKey) where T : IComparable<T>
        {
            int x = -1;
            int result = -1;
            // search the occurences of the data array and compares to the search value
            foreach (T i in dataArray)
            {
                x++;
                if (i.CompareTo(searchKey) == 0)
                {
                    result = x;
                    break;
                }
            }
            return result;
        }
        //display the array values for checking
        public static string Display<T>(T[] dataArray, T min, T max) where T : IComparable<T>
        {
            string result = "";
            int found = 0;
            foreach (T i in dataArray)
            {
                //compare to the low and high values
                if (i.CompareTo(min) >= 0 && i.CompareTo(max) < 1)
                {
                    result = result + " " + i;
                    found = found + 1;
                }
            }
            //when no value is found, return the message
            if (found == 0)
            {
                return "No value found";
            }
            else
            {
                return result;
            }            
        }

    

    } //end of class
    //Method Extension to get a random double within a range
    public static class RandomExtensions
    {
        public static double NextDouble(
            this Random random,
            double min,
            double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
