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

namespace Calculator_Extended
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        public CalculatorStructure calculator;
        //gets the current size of the mainTextBlock
        private double currentFontSize { get; set; }
        #endregion

        #region Initializer
        public MainWindow()
        {
            InitializeComponent();

            //create a new calculator instance
            calculator = new CalculatorStructure();

            //set out blocks
            OutputSetter();


            //Set the current mainBlock text Size
            currentFontSize = mainOutBlock.FontSize;
            //this.DataContext = new OutputViewModel().Data;
        }
        #endregion
        #region Functions Buttons Click
        //Modulus Function Btn

        private void ModulusBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        //Square Root Function Btn
        private void SqrRtBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.SqrRt();
            OutputSetter();
        }
        //Square Function Btn 
        private void SqrBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.Sqr();
            OutputSetter();
        }
        //Inverse Function Btn
        private void InverseBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.Inverse();
            OutputSetter();
        }

        //Plus Minus Function Btn
        private void PlusMinusBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.SetPlusOrMinus();
            OutputSetter();
        }

        #endregion

        #region Clear And Reset
        /// <summary>
        /// Resets the MainOutput Block
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CEBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.MainTextBlockReset("0");
            OutputSetter();
        }

        /// <summary>
        /// Resets the entire Calculator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator = new CalculatorStructure();
            OutputSetter();
        }
        /// <summary>
        /// Clears the current input 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.ClearInput();
            OutputSetter();
        }
        #endregion



        #region Digit Input Buttons
        /// <summary>
        /// detects if a number is pressed
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">empty</param>
        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            //Get The button Tag 
            string theBtnTag = ((Button)sender).Tag.ToString();
            if (calculator.AcceptsInput)
            {
                calculator.DigitSetter(theBtnTag);
                OutputSetter();
            }
            
            
        }
        /// <summary>
        /// Adds a decimal to the current input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PeriodBtn_Click(object sender, RoutedEventArgs e)
        {
            string theBtnTag = ((Button)sender).Tag.ToString();
            if (calculator.AcceptsInput) {
                calculator.DecimalSetter(theBtnTag);
                OutputSetter();
            }

        }

        #endregion


        #region Arithmetic Btns
        //Equals Button 
        private void EqualBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.EqualFunction();
            OutputSetter();
        }
        //Addition Button
        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.AddFunction();
            OutputSetter();
        }
        //Substract Button
        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.MinusFunction();
            OutputSetter();
        }
        //Multiply Button
        private void MultiplyBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.MultiplyFunction();
            OutputSetter();
        }
        //Divide Button
        private void DivideBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.DivideFunction();
            OutputSetter();
        }

        #endregion

        #region Memory Buttons
        /// <summary>
        /// Activates the disabled memory btns
        /// </summary>
        private void ActivateMemoryBtn()
        {
            MemoryClearBtn.IsEnabled = calculator.MemoryExist;
            MemoryRestoreBtn.IsEnabled = calculator.MemoryExist;
            MemoryDisplayBtn.IsEnabled = calculator.MemoryExist;
            
        }

        /// <summary>
        /// Memory Button Related Functions Buttons
        /// </summary>
        /// <param name="sender">Memory Buttons</param>
        /// <param name="e"></param>
        private void MemoryBtn_Click(object sender, RoutedEventArgs e)
        {
            string theTag = ((Button)sender).Tag.ToString();

            switch (theTag)
            {
                case "M+":
                    //check for existing memory item
                    calculator.MemoryAdd();
                    break;
                case "M-":
                    //check for existing memory item
                    calculator.MemorySubstract();
                    break;
                case "MS":
                    //Store into Memory
                    calculator.MemoryStore();
                    break;
                case "MR":
                    //Recall Memory
                    calculator.MemoryRestore();
                    break;
                case "MC":
                    calculator.MemoryClear();
                    break;
            }
            OutputSetter();
        }
        #endregion



        #region Helper Function
        /// <summary>
        /// this method sets the Output model and updates the UI
        /// </summary>
        public void OutputSetter()
        {
            OutputModel model = calculator.GetOutput();
            topOutBlock.Text = model.topOutputBlock;
            mainOutBlock.Text = model.MainOutputBlock;
            //call the update ui method
            UpdateTextBlocksUI();
            //check the memory active
            ActivateMemoryBtn();
        }
        /// <summary>
        /// A Method that uodates the fontsize of the mainOutput block
        /// </summary>
        private void UpdateTextBlocksUI()
        {
            //get the current text block content
            string currentText = mainOutBlock.Text;
            //check if the current text is less than 16
            if(currentText.Length <= 15)
            {
                //keep the font size the same
                mainOutBlock.FontSize = 43;
            }
            else
            {
                //reduce the font size
                //substract 16 from the current size to get the proper size to remove and mulitpy the result by to 2 to move it by 2 steps
                int sizeTo = (currentText.Length - 15) * 2;
                //get the new size from the current fontsize
                double newCurrentFontSize = currentFontSize - sizeTo;
                //set the mainBlock font size with the new size
                mainOutBlock.FontSize = newCurrentFontSize;

            }

        }
        #endregion
    }
}
