using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator_Extended
{
    /// <summary>
    /// This is the main calculator class that house the entire calculator functions
    /// </summary>
    public class CalculatorStructure
    {


        #region Public Fields
        //top block string 
        public string topBlock { get; set; }
        public decimal mainBlock { get; set; }
        //stores if the calculator is currently accepting inputs
        public bool AcceptsInput { get; set; }
        //stores if the current input has a decimal or not
        public bool HasDecimal { get; set; }
        //stores if the input is Negative or not
        public bool IsNegative { get; set; }
        //stores if a memory item exists or not
        public bool MemoryExist { get; set; }

        #endregion

        #region Private Fields
        //the main input block that holds the current input
        private string mainBlockInput { get; set; }
        // the current solution
        private string theSolution { get; set; }

        //holds the list of the curent input see the Digit Settet Method to understand the use.
        private List<string> digitList = new List<string>();
        //strore the list of entries for display on top block 
        private List<string> displayList = new List<string>();
        //hold a single item of the digit list with a default value of 0
        private string[] digArrray = new string[1] { "0" };
        //the last action perform by the user
        private CalculatorUtilities.LastAction LastAction;
        // the current function by the calculator
        private CalculatorUtilities.CalculatorFunction CurrentFunction;
        // the current calculator state
        private CalculatorUtilities.CalculatorState CalculatorState;
        //holds if the use can clear the main input
        private bool CanClear { get; set; }
        //the memory item list that holds the items from the file
        private List<double> memoryItem = new List<double>();
        //the path to the memory items file
        private string memoryFilePath;
        #endregion

        #region Static Fields
        public static CalculatorStructure Instance = new CalculatorStructure();
        #endregion

        #region Constructor
        /// <summary>
        /// default calculator constructor, this is a method where the default values of the calculator is set.
        /// </summary>
        public CalculatorStructure()
        {
            //main block starts with initial of zero;
            this.mainBlock = 0;
            //set the memory file Path
            memoryFilePath = Environment.CurrentDirectory + "\\MemoryFile.txt";
            //Load the memory item from a file
            LoadMemoryItemFromFile();
            //checks for memory item
            CheckMemoryItems();
            //set the current calculator state
            CalculatorState = CalculatorUtilities.CalculatorState.Default; 
            //Adds a default value to the digit list
            digitList.Add("0");
            //enable accepting inputs
            AcceptsInput = true;
            
        }
        #endregion

        #region Memory 
        /// <summary>
        /// Load Memory item from file  
        /// </summary>
        private void LoadMemoryItemFromFile()
        {
            //checks if the memory file exists if not create a new file
            if (File.Exists(memoryFilePath))
            {
                //if the file exists get the current memomry item and add it to the list
                string[] memoryLines = File.ReadLines(memoryFilePath).ToArray();
                if (memoryLines.Length > 0)
                {
                    foreach (string lines in memoryLines)
                    {
                        try
                        {
                            memoryItem.Add(Convert.ToDouble(lines));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            else
            {
                //if the file doesn't exist create a new empty file
                try
                {
                    using (FileStream fs = File.Create(memoryFilePath))
                    {

                    }

                }
                catch (Exception ex)
                {

                }

            }
        }
        /// <summary>
        /// Checks if there's a memory item availble
        /// </summary>
        /// <returns></returns>
        private void CheckMemoryItems()
        {
            //sets if a memory items exists
            if (memoryItem.Count > 0)
                MemoryExist = true;
            else
                MemoryExist = false;
        }
        /// <summary>
        /// Adds Item to the memory item list and save it to a file for recall
        /// </summary>
        /// <param name="theItem">the item</param>
        private void SaveItemToMemory(double theItem)
        {
            //adds the item to the list
            memoryItem.Add(theItem);
            //write to file
            string[] createText = { theItem.ToString() };
            File.WriteAllLines(memoryFilePath, createText, Encoding.UTF8);
            //update the memory items status
            CheckMemoryItems();
        }
        /// <summary>
        /// Updates the memory item on the list and the file
        /// </summary>
        /// <param name="theItem">the item</param>
        private void UpdateMemoryItem(double theItem)
        {
            //Updates the current item inside the memoryitem lists and the file
            memoryItem[0] = theItem;
            string[] line = File.ReadAllLines(memoryFilePath);
            line[0] = theItem.ToString();
            File.WriteAllLines(memoryFilePath, line, Encoding.UTF8);
            CheckMemoryItems();
        }
        /// <summary>
        /// Add the current input to the current memory item
        /// </summary>
        public void MemoryAdd()
        {
            //check if there's a memory item already
            if(memoryItem.Count > 0)
            {
                double result = memoryItem[0] + double.Parse(mainBlock.ToString());
                UpdateMemoryItem(result);
            }
            else
            {
                double result = 0 + double.Parse(mainBlock.ToString());
                SaveItemToMemory(result);
            }
        }
        /// <summary>
        /// Substract the current input from the current memory item 
        /// </summary>
        public void MemorySubstract()
        {
            //check if there's a memory item already
            if (memoryItem.Count > 0)
            {
                double result = memoryItem[0] - double.Parse(mainBlock.ToString());
                UpdateMemoryItem(result);
            }
            else
            {
                double result = 0 - double.Parse(mainBlock.ToString());
                SaveItemToMemory(result);
            }
        }
        /// <summary>
        /// Stores the current input as a memory item
        /// </summary>
        public void MemoryStore()
        {
            double result = double.Parse(mainBlock.ToString());
            if (memoryItem.Count > 0)
            {
                UpdateMemoryItem(result);
            }
            else
            {   
                SaveItemToMemory(result);
            }
        }
        /// <summary>
        /// Restores the current memory item as the current input
        /// </summary>
        public void MemoryRestore()
        {
            mainBlock = decimal.Parse(memoryItem[0].ToString());
        }
        /// <summary>
        /// Clears the memory items from the list and the file
        /// </summary>
        public void MemoryClear()
        {
            memoryItem.Clear();
            //clear the file by overwriting the file
            try
            {
                using (FileStream fs = File.Create(memoryFilePath))
                {

                }

            }
            catch (Exception ex)
            {
                
            }
            CheckMemoryItems();
        }

        #endregion


        #region Input Setters
        /// <summary>
        /// returns and the sets the values of the output model class
        /// </summary>
        /// <returns></returns>
        public OutputModel GetOutput()
        {
            return new OutputModel(topBlock, mainBlock);
        }

        /// <summary>
        /// sets Main Output block inputs
        /// </summary>
        /// <param name="thedig">input string</param>
        public void DigitSetter(string thedig)
        {

            
            //check for the input lenght and if the calculator is in default state
            if (CalculatorState == CalculatorUtilities.CalculatorState.Default)
            {
                //create a list of the current input
                //and check if the digitList length is less than 17
                if (digitList.Count <= 15)
                    digitList.Add(thedig);
                else
                    AcceptsInput = false;
                //join and parse the input to a single item container in this case an array
                digArrray[0] = String.Join("", digitList.ToArray());
                mainBlockInput = digArrray[0];

                LastAction = CalculatorUtilities.LastAction.NumberBtn;
            }
            //update the main block
            mainBlock = decimal.Parse(mainBlockInput);
            CanClear = true;
            //attempt to solve the current problem

        }
        /// <summary>
        /// Add decimal to the current input
        /// </summary>
        /// <param name="theDecimal"></param>
        public void DecimalSetter(string theDecimal)
        {
            //check for the input lenght and if the calculator is in default state
            if (CalculatorState == CalculatorUtilities.CalculatorState.Default)
            {
                // check if the current inputs has a decimal already
                if (!DecimalChecker())
                    DigitSetter(theDecimal);
            }
            
        }
        /// <summary>
        /// checks if the current inputs has a decimal
        /// </summary>
        /// <returns>True or False</returns>
        public bool DecimalChecker()
        {
            //check if digit list contains  a decimal
            return digitList.Contains(".");     
        }

        /// <summary>
        /// Sets the plus or minus to the mainBlock
        /// </summary>
        public void SetPlusOrMinus()
        {
            mainBlock = -(mainBlock);
            IsNegative = IsNegativeChecker();
            if (IsNegative)
                digitList.Insert(0, "-");
            else
                digitList.RemoveAt(0);

        }
        
        /// <summary>
        /// checks if the mainBlock is negative or not 
        /// </summary>
        /// <returns></returns>
        public bool IsNegativeChecker()
        {
            return mainBlock < 0;
        }
        #endregion

        #region Clear and Reset
        /// <summary>
        /// Resets the valur of the mainBlock to zero
        /// </summary>
        public void MainTextBlockReset(string theDisplay)
        {
            mainBlock = decimal.Parse(theDisplay);
            digitList.Clear();
            digitList.Add("0");
            AcceptsInput = true;
        }
        /// <summary>
        /// Clears the user input
        /// </summary>
        public void ClearInput()
        {
            if (CanClear)
            {
                if (digitList.Count > 1)
                {
                    digitList.RemoveAt(digitList.Count - 1);
                    digArrray[0] = String.Join("", digitList.ToArray());
                    mainBlockInput = digArrray[0];
                    AcceptsInput = true;
                }
                else
                {
                    digitList.Clear();
                    digitList.Add("0");
                    mainBlockInput = "0";
                }
                mainBlock = decimal.Parse(mainBlockInput);
            }
            

        }
        #endregion

        #region Arithmetic Function
        /// <summary>
        /// The main function to process the aritmetic operations on the calculator
        /// </summary>
        /// <param name="theFunc">the currennt operands</param>
        /// <param name="theOperator">the current operator</param>
        public void MainFunctionOp(string theFunc, CalculatorUtilities.CalculatorFunction theOperator)
        {
            string theInput = null;

            if (LastAction == CalculatorUtilities.LastAction.ArithmeticBtn)
                displayList[displayList.Count - 1] = theFunc;
            else
            {
                if (LastAction == CalculatorUtilities.LastAction.FunctionBtn) {
                    displayList.Add(theFunc);
                }
                else
                {
                    if (IsNegativeChecker())
                        theInput = "(" + mainBlock.ToString() + ")";
                    else
                        theInput = mainBlock.ToString();

                    displayList.Add(theInput);
                    displayList.Add(theFunc);
                }
                
                if (String.IsNullOrEmpty(theSolution))
                {
                    theSolution = mainBlock.ToString();
                    CanClear = false;
                }
                else
                {
                    theSolution = Solve(theSolution, mainBlock.ToString(), CurrentFunction);
                    CanClear = false;
                }
            }
            topBlock = String.Join("", displayList.ToArray());
            
            MainTextBlockReset(theSolution != null ? theSolution : "0");
            CurrentFunction = theOperator;
            LastAction = CalculatorUtilities.LastAction.ArithmeticBtn;
        }
        /// <summary>
        /// this is a method for solving the problem
        /// </summary>
        /// <param name="valueA">the first operand</param>
        /// <param name="valueB">the second operand</param>
        /// <param name="calculatorFunction">the enum class holding the functions</param>
        /// <returns>a string as a solution</returns>
        public string Solve(string valueA, string valueB, CalculatorUtilities.CalculatorFunction calculatorFunction)
        {
            string theVal = null;
            switch (calculatorFunction)
            {
                case CalculatorUtilities.CalculatorFunction.Plus:
                    theVal = (double.Parse(valueA) + double.Parse(valueB)).ToString();
                    break;
                case CalculatorUtilities.CalculatorFunction.Minus:
                    theVal= (double.Parse(valueA) - double.Parse(valueB)).ToString();
                    break;
                case CalculatorUtilities.CalculatorFunction.Multiply:
                    theVal = (double.Parse(valueA) * double.Parse(valueB)).ToString();
                    break;
                case CalculatorUtilities.CalculatorFunction.Divide:
                    theVal = (double.Parse(valueA) / double.Parse(valueB)).ToString();
                    break;
            }
            return theVal;
        }
        /// <summary>
        /// The Add operations 
        /// </summary>
        public void AddFunction()
        {

            MainFunctionOp(" + ", CalculatorUtilities.CalculatorFunction.Plus);
        }
        /// <summary>
        /// the Substract operation
        /// </summary>
        public void MinusFunction()
        {
            MainFunctionOp(" - ", CalculatorUtilities.CalculatorFunction.Minus);
        }
        /// <summary>
        /// the multiplication operation
        /// </summary>
        public void MultiplyFunction()
        {
            MainFunctionOp(" × ", CalculatorUtilities.CalculatorFunction.Multiply);
        }
        /// <summary>
        /// The divide operation
        /// </summary>
        public void DivideFunction()
        {
            MainFunctionOp(" ÷ ", CalculatorUtilities.CalculatorFunction.Divide);
        }
        /// <summary>
        /// The equal operation
        /// </summary>
        public void EqualFunction()
        {
            string theInput = null;
            if (IsNegativeChecker())
                theInput = "(" + mainBlock.ToString() + ")";
            else
                theInput = mainBlock.ToString();

            displayList.Clear();
            
            if (String.IsNullOrEmpty(theSolution))
            {
                theSolution = mainBlock.ToString();
                CanClear = false;
            }
            else
            {
                theSolution = Solve(theSolution, mainBlock.ToString(), CurrentFunction);
                CanClear = false;
            }
            //set the top block to the combination of the display Array
            topBlock = String.Join("", displayList.ToArray());
            MainTextBlockReset(theSolution != null ? theSolution : "0");
            theSolution = null;
            LastAction = CalculatorUtilities.LastAction.EqualsBtn;
        }
        #endregion

        #region Top Function
        /// <summary>
        /// the square root function
        /// </summary>
        public void SqrRt()
        {
            string theInput = "√(" + mainBlock.ToString() + ")";
            displayList.Add(theInput);
            //set the top block to the combination of the display Array
            topBlock = String.Join("", displayList.ToArray());
            mainBlock = (decimal)(Math.Sqrt(double.Parse(mainBlock.ToString())));
            LastAction = CalculatorUtilities.LastAction.FunctionBtn;
        }
        /// <summary>
        /// The square function
        /// </summary>
        public void Sqr()
        {
            string theInput = "sqr(" + mainBlock.ToString() + ")";
            displayList.Add(theInput);
            //set the top block to the combination of the display Array
            topBlock = String.Join("", displayList.ToArray());
            mainBlock = (decimal)(Math.Pow(double.Parse(mainBlock.ToString()), 2));
            LastAction = CalculatorUtilities.LastAction.FunctionBtn;
        }
        /// <summary>
        /// the inverse function
        /// </summary>
        public void Inverse()
        {
            string theInput = "1/(" + mainBlock.ToString() + ")";
            displayList.Add(theInput);
            //set the top block to the combination of the display Array
            topBlock = String.Join("", displayList.ToArray());
            mainBlock = (decimal)(1/(double.Parse(mainBlock.ToString())));
            LastAction = CalculatorUtilities.LastAction.FunctionBtn;
        }
        #endregion
    }
}
