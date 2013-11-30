using System;
using System.Text;
using TurtleVM;
using visual_turtle.Common;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace visual_turtle
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            StringBuilder b = new StringBuilder();
            b.AppendLine("CONST");
            b.AppendLine("  m := 1,");
            b.AppendLine("  n := 2;");
            b.AppendLine("");
            b.AppendLine("VAR");
            b.AppendLine("  x, y, z;");
            b.AppendLine("");
            b.AppendLine("PROCEDURE foo;");
            b.AppendLine("BEGIN");
            b.AppendLine("END;");
            b.AppendLine("");
            b.AppendLine("BEGIN");
            b.AppendLine("END.");
            tbSourceCode.Text = b.ToString();
            CompileAndRun();
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void tbSourceCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //CompileAndRun();
        }

        private void CompileAndRun()
        {
            Tokenizer tokenizer = new Tokenizer(TokenTypes.ALL_TYPES, tbSourceCode.Text);
            Parser parser = new Parser(tokenizer);

            StringBuilder sbTokens = new StringBuilder();
            tokenizer.onMatched += (token) =>
            {
                sbTokens.AppendLine(token.ToString());
            };

            AST.Node program = null;
            StringBuilder sbNodes = new StringBuilder();
            try
            {
                program = parser.Parse();
                ASTNodeToString(sbNodes, program, "");
            }
            catch (Exception ex)
            {
                sbNodes.AppendLine(ex.ToString());
            }

            tbTokenStream.Text = sbTokens.ToString();
            tbSyntaxTree.Text = sbNodes.ToString();

            cvsTurtleActions.Children.Clear();
            Turtle turtle = new Turtle(cvsTurtleActions);
            if (program != null)
            {
                Interpreter interpreter = new Interpreter();
                interpreter.Symbols.Add("MoveForward", new SymbolValue(() => { turtle.MoveForward(); }));
                interpreter.Symbols.Add("TurnLeft", new SymbolValue(() => { turtle.TurnLeft(); }));
                interpreter.Symbols.Add("TurnRight", new SymbolValue(() => { turtle.TurnRight(); }));
                interpreter.Symbols.Add("PenDown", new SymbolValue(() => { turtle.PenDown(); }));
                interpreter.Symbols.Add("PenUp", new SymbolValue(() => { turtle.PenUp(); }));
                interpreter.Evaluate(program);
            }
            turtle.DrawTurtle();
        }

        private void ASTNodeToString(StringBuilder sb, AST.Node node, string depth)
        {
            if(node != null)
            {
                sb.AppendLine(depth + "[Type: " + node.type + " (Text: " + node.text + " Number: " + node.number +")]");
                if(node.children != null)
                    foreach (AST.Node child in node.children)
                        ASTNodeToString(sb, child, depth + "-");
            }
        }

        private void btnRun_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CompileAndRun();

        }
    }
}
