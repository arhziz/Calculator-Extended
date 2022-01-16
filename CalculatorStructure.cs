using System;
using System.Collections.Generic;

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
        private List<string> digitList = new List<string>();
        private string[] digArrray = new string[1] { "0" };
        private CalculatorUtilities.CalculatorState CalculatorState;
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
            CalculatorState = CalculatorUtilities.CalculatorState.Default; 
            this.mainBlock = 0;
            digitList.Add("0");
            AcceptsInput = true;
            
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
            }
            //update the main block
            mainBlock = decimal.Parse(mainBlockInput);

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
        public void MainTextBlockReset()
        {
            mainBlock = 0;
            digitList.Clear();
            AcceptsInput = true;
        }
        /// <summary>
        /// Clears the user input
        /// </summary>
        public void ClearInput()
        {
            if(digitList.Count > 1)
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
        #endregion

        #region Arithmetic Function
            
        public void AddFunction()
        {
            topBlock = mainBlock.ToString() + " + ";
        }

        #endregion
    }
}
