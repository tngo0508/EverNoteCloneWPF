using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace EverNoteCloneWPF.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public NotesWindow()
        {
            InitializeComponent();

            var fontFamilites = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontFamilyComboBox.ItemsSource = fontFamilites;

            List<double> fontSizes = new List<double>() {8, 9, 10, 11, 12, 14, 16, 28, 30};
            FontSizeComboBox.ItemsSource = fontSizes;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Go to https://portal.azure.com/ and create the speech resource to get region and key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SpeechButton_OnClick(object sender, RoutedEventArgs e)
        {
            string region = "westus";
            string key = "49556d62f56c4c038fc49e6871b31e67";

            var speechConfig = SpeechConfig.FromSubscription(key, region);
            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new SpeechRecognizer(speechConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    ContentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            }
        }

        private void ContentRichTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int amountOfCharacters = (new TextRange(ContentRichTextBox.Document.ContentStart,
                ContentRichTextBox.Document.ContentEnd)).Text.Length;
            StatusTextBlock.Text = $"Document length: {amountOfCharacters} characters";
        }

        private void BoldBtn_OnClick(object sender, RoutedEventArgs e)
        {
            bool isBtnChecked = ((ToggleButton) sender).IsChecked ?? false;
            ContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty,
                isBtnChecked ? FontWeights.Bold : FontWeights.Normal);
        }

        private void ContentRichTextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = ContentRichTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            BoldBtn.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && selectedWeight.Equals(FontWeights.Bold);

            var selectedStyle = ContentRichTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            ItalicBtn.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && selectedStyle.Equals(FontStyles.Italic);

            var selectedDecoration = ContentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderLineBtn.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) && selectedDecoration.Equals(TextDecorations.Underline);

            FontFamilyComboBox.SelectedItem = ContentRichTextBox.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            FontSizeComboBox.Text = ContentRichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty).ToString();

        }

        private void ItalicBtn_OnClick(object sender, RoutedEventArgs e)
        {
            bool isBtnChecked = ((ToggleButton) sender).IsChecked ?? false;
            ContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty,
                isBtnChecked ? FontStyles.Italic : FontStyles.Normal);
        }

        private void UnderLineBtn_OnClick(object sender, RoutedEventArgs e)
        {
            bool isBtnChecked = ((ToggleButton) sender).IsChecked ?? false;
            if (isBtnChecked)
            {
                ContentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                ((TextDecorationCollection) ContentRichTextBox.Selection.GetPropertyValue(
                        Inline.TextDecorationsProperty))
                    .TryRemove(TextDecorations.Underline, out var textDecorations);
                ContentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void FontFamilyComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectionBoxItem != null)
            {
                ContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, FontFamilyComboBox.SelectedItem);
            }
        }

        private void FontSizeComboBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ContentRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, FontSizeComboBox.Text);
        }
    }
}
