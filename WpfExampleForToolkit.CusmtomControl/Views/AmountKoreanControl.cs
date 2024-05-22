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

namespace WpfExampleForToolkit.CustomControl.Views
{
    /// <summary>
    /// XAML 파일에서 이 사용자 지정 컨트롤을 사용하려면 1a 또는 1b단계를 수행한 다음 2단계를 수행하십시오.
    ///
    /// 1a단계) 현재 프로젝트에 있는 XAML 파일에서 이 사용자 지정 컨트롤 사용.
    /// 이 XmlNamespace 특성을 사용할 마크업 파일의 루트 요소에 이 특성을 
    /// 추가합니다.
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfExampleForToolkit.CustomControl.Views"
    ///
    ///
    /// 1b단계) 다른 프로젝트에 있는 XAML 파일에서 이 사용자 지정 컨트롤 사용.
    /// 이 XmlNamespace 특성을 사용할 마크업 파일의 루트 요소에 이 특성을 
    /// 추가합니다.
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfExampleForToolkit.CustomControl.Views;assembly=WpfExampleForToolkit.CustomControl.Views"
    ///
    /// 또한 XAML 파일이 있는 프로젝트의 프로젝트 참조를 이 프로젝트에 추가하고
    /// 다시 빌드하여 컴파일 오류를 방지해야 합니다.
    ///
    ///     솔루션 탐색기에서 대상 프로젝트를 마우스 오른쪽 단추로 클릭하고
    ///     [참조 추가]->[프로젝트]를 차례로 클릭한 다음 이 프로젝트를 찾아서 선택합니다.
    ///
    ///
    /// 2단계)
    /// 계속 진행하여 XAML 파일에서 컨트롤을 사용합니다.
    ///
    ///     <MyNamespace:AmountKoreanControl/>
    ///
    /// </summary>
    [TemplatePart(Name = _textBlockName, Type = typeof(TextBlock))]
    [TemplatePart(Name = _textBoxName, Type = typeof(TextBox))]
    public class AmountKoreanControl : Control
    {
        /// <summary>
        /// AmountKoreanControl 컨트롤에서 제어할 컨트롤들의 이름
        /// </summary>
        private const string _textBlockName = "PART_KoreanDisplay";
        private const string _textBoxName = "PART_Amount";

        /// <summary>
        /// AmountKoreanControl 컨트롤에서 제어할 컨트롤들
        /// </summary>
        private TextBlock? _koreanDisplayTextBlock;
        private TextBox? _amountTextBox;
        private bool _isWork;

        public decimal Amount
        {
            get { return (decimal)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        /// <summary>
        /// 컨트롤 종료자
        /// </summary>
        ~AmountKoreanControl()
        {
            _amountTextBox.TextChanged -= AmountTextBox_TextChanged;
            _amountTextBox.PreviewKeyDown -= AmountTextBox_PreviewKeyDown;
        }

        /// <summary>
        /// Amount DP
        /// </summary>
        public static readonly DependencyProperty AmountProperty =DependencyProperty.Register(nameof(Amount), typeof(decimal), typeof(AmountKoreanControl), new PropertyMetadata(decimal.Zero, AmountChanged));

        static AmountKoreanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AmountKoreanControl), new FrameworkPropertyMetadata(typeof(AmountKoreanControl)));
        }


        public override void OnApplyTemplate()
        {
            //커스텀 컨트롤 각 PART를 내부에서 사용할 수 있도록 가져옴
            _koreanDisplayTextBlock = GetTemplateChild(_textBlockName) as TextBlock;
            _amountTextBox = GetTemplateChild(_textBoxName) as TextBox;

            if (_amountTextBox == null || _koreanDisplayTextBlock == null)
            {
                throw new NullReferenceException("컨트롤의 PART를 찾을 수 없습니다.");
            }

            //Amount 초기값을 설정
            DecimalToFormatString(Amount);

            _amountTextBox.TextChanged += AmountTextBox_TextChanged;
            _amountTextBox.PreviewKeyDown += AmountTextBox_PreviewKeyDown;
        }
        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //중복실행방지
            if (_isWork)
            {
                return;
            }
            _isWork = true;

            //전처리
            var numberTextOnly = _amountTextBox?.Text.Trim().Replace(",", "");
            if (decimal.TryParse(numberTextOnly, out decimal decimalValue))
            {
                DecimalToFormatString(decimalValue);
            }
            else
            {
                Amount = 0;
                _koreanDisplayTextBlock.Text = "";
            }
            _isWork = false;
        }

        private void AmountTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isDigit = false;
            switch ((int)e.Key)
            {
                //D0~D9 NumPad0 ~ 9 74-83
                case int n when ((34 <= n && 43 >= n) || (74 <= n && 83 >= n)):
                    isDigit = true;
                    break;
            }
            if (!(isDigit || e.Key == Key.Back
                || e.Key == Key.Left || e.Key == Key.Right))
            {
                e.Handled = true;
            }
        }

        private static void AmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AmountKoreanControl)d;
            control.SetAmount();
        }

        private void SetAmount()
        {
            if (_isWork)
            {
                return;
            }
            _isWork = true;
            DecimalToFormatString(Amount);
            _isWork = false;
        }

        /// <summary>
        /// 데시멀을 포멧스트링으로 입력과 한글 금액 출력
        /// </summary>
        /// <param name="decimalValue"></param>
        private void DecimalToFormatString(decimal decimalValue)
        {
            //DP 변경 이벤트로 호출되는 경우 초기화 전에 들어오는 것 방지
            if (_amountTextBox == null)
            {
                return;
            }
            //StringFormat 출력
            _amountTextBox.Text = string.Format("{0:#,##0}", decimalValue);
            //캐럿을 맨뒤로
            _amountTextBox.SelectionStart = _amountTextBox.Text.Length;

            //DP에 값 입력
            Amount = decimalValue;
            var korean = Number2Hangle(Amount);
            _koreanDisplayTextBlock.Text = korean;
        }

        /// <summary>
        /// 숫자를 한글로 - 이 부분은 따로 Utility로 빼서 사용하는 것이 더 좋을듯
        /// </summary>
        /// <param name="lngNumber"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://redqueen-textcube.blogspot.com/2009/12/c-%EC%88%AB%EC%9E%90%EA%B8%88%EC%95%A1-%EB%B3%80%ED%99%98.html
        /// </remarks>
        private string Number2Hangle(decimal lngNumber)
        {
            string sign = "";
            string[] numberChar = new string[] { "", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
            string[] levelChar = new string[] { "", "십", "백", "천" };
            string[] decimalChar = new string[] { "", "만", "억", "조", "경" };

            string strValue = string.Format("{0}", lngNumber);
            string numToKorea = sign;
            bool useDecimal = false;

            int i;
            for (i = 0; i < strValue.Length; i++)
            {
                int Level = strValue.Length - i;
                if (strValue.Substring(i, 1) != "0")
                {
                    useDecimal = true;
                    if (((Level - 1) % 4) == 0)
                    {
                        numToKorea = numToKorea + numberChar[int.Parse(strValue.Substring(i, 1))] + decimalChar[(Level - 1) / 4];
                        useDecimal = false;
                    }
                    else
                    {
                        if (strValue.Substring(i, 1) == "1")
                        {
                            numToKorea = numToKorea + levelChar[(Level - 1) % 4];
                        }
                        else
                        {
                            numToKorea = numToKorea + numberChar[int.Parse(strValue.Substring(i, 1))] + levelChar[(Level - 1) % 4];
                        }
                    }
                }
                else
                {
                    if ((Level % 4 == 0) && useDecimal)
                    {
                        numToKorea = numToKorea + decimalChar[Level / 4];
                        useDecimal = false;
                    }
                }
            }
            return numToKorea;
        }
    }
}
