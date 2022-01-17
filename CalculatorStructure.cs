using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Calculator_Extended
{
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
        #endregion

        #region Private Fields
        private string mainBlockInput { get; set; }
        private string theSolution { get; set; }

        private List<string> digitList = new List<string>();
        //strore the list of entries and the the 
        private List<string> displayList = new List<string>();
        private string[] digArrray = new string[1] { "0" };
        private CalculatorUtilities.LastAction LastAction;
        private CalculatorUtilities.CalculatorFunction CurrentFunction;
        private CalculatorUtilities.CalculatorState CalculatorState;
        private bool CanClear { get; set; }

        private List<double> memoryItem = new List<double>();
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

            CalculatorState = CalculatorUtilities.CalculatorState.Default; 
            
            digitList.Add("0");
            AcceptsInput = true;
            
        }
        #endregion

        #region Memory 
        /// <summary>
        /// Load Memory item from file  
        /// </summary>
        
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
        public void AddFunction()
        {

            MainFunctionOp(" + ", CalculatorUtilities.CalculatorFunction.Plus);
        }

        public void MinusFunction()
        {
            MainFunctionOp(" - ", CalculatorUtilities.CalculatorFunction.Minus);
        }

        public void MultiplyFunction()
        {
            MainFunctionOp(" × ", CalculatorUtilities.CalculatorFunction.Multiply);
        }

        public void DivideFunction()
        {
            MainFunctionOp(" ÷ ", CalculatorUtilities.CalculatorFunction.Divide);
        }

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
        public void SqrRt()
        {
            string theInput = "√(" + mainBlock.ToString() + ")";
            displayList.Add(theInput);
            //set the top block to the combination of the display Array
            topBlock = String.Join("", displayList.ToArray());
            mainBlock = (decimal)(Math.Sqrt(double.Parse(mainBlock.ToString())));
            LastAction = CalculatorUtilities.LastAction.FunctionBtn;
        }

        public void Sqr()
        {
            string theInput = "sqr(" + mainBlock.ToString() + ")";
            displayList.Add(theInput);
            //set the top block to the combination of the display Array
            topBlock = String.Join("", displayList.ToArray());
            mainBlock = (decimal)(Math.Pow(double.Parse(mainBlock.ToString()), 2));
            LastAction = CalculatorUtilities.LastAction.FunctionBtn;
        }
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
