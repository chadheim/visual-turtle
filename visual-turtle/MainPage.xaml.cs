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
            b.AppendLine("  m :=  7,");
            b.AppendLine("  n := 85;");
            b.AppendLine("VAR");
            b.AppendLine("  x, y, z, q, r;");
            b.AppendLine("PROCEDURE multiply;");
            b.AppendLine("VAR a, b;");
            b.AppendLine("BEGIN");
            b.AppendLine("  a := x;");
            b.AppendLine("  b := y;");
            b.AppendLine("  z := 0;");
            b.AppendLine("  WHILE b > 0 DO BEGIN");
            b.AppendLine("    IF ODD b THEN z := z + a;");
            b.AppendLine("    a := 2 * a;");
            b.AppendLine("    b := b / 2");
            b.AppendLine("  END");
            b.AppendLine("END;");
            b.AppendLine("PROCEDURE divide;");
            b.AppendLine("VAR w;");
            b.AppendLine("BEGIN");
            b.AppendLine("  r := x;");
            b.AppendLine("  q := 0;");
            b.AppendLine("  w := y;");
            b.AppendLine("  WHILE w <= r DO w := 2 * w;");
            b.AppendLine("  WHILE w > y DO BEGIN");
            b.AppendLine("    q := 2 * q;");
            b.AppendLine("    w := w / 2;");
            b.AppendLine("    IF w <= r THEN BEGIN");
            b.AppendLine("      r := r - w;");
            b.AppendLine("      q := q + 1");
            b.AppendLine("    END");
            b.AppendLine("  END");
            b.AppendLine("END;");
            b.AppendLine("PROCEDURE gcd;");
            b.AppendLine("VAR f, g;");
            b.AppendLine("BEGIN");
            b.AppendLine("  f := x;");
            b.AppendLine("  g := y;");
            b.AppendLine("  WHILE f # g DO BEGIN");
            b.AppendLine("    IF f < g THEN g := g - f;");
            b.AppendLine("    IF g < f THEN f := f - g");
            b.AppendLine("  END;");
            b.AppendLine("  z := f");
            b.AppendLine("END;");
            b.AppendLine("BEGIN");
            b.AppendLine("  x := m;");
            b.AppendLine("  y := n;");
            b.AppendLine("  CALL multiply;");
            b.AppendLine("  x := 25;");
            b.AppendLine("  y :=  3;");
            b.AppendLine("  CALL divide;");
            b.AppendLine("  x := 84;");
            b.AppendLine("  y := 36;");
            b.AppendLine("  CALL gcd");
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
            CompileAndRun();
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
            tbParseTree.Text = sbNodes.ToString();

            cvsTurtleActions.Children.Clear();
            Turtle turtle = new Turtle(cvsTurtleActions);
            if (program != null)
            {
                Interpreter interpreter = new Interpreter();
                interpreter.Symbols.Add("MoveForward", new SymbolValue(() => { turtle.MoveForward(); }));
                interpreter.Symbols.Add("TurnLeft", new SymbolValue(() => { turtle.TurnLeft(); }));
                interpreter.Symbols.Add("TurnRight", new SymbolValue(() => { turtle.TurnRight(); }));
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
    }
}
