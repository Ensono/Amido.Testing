using System.ComponentModel;
using Amido.Testing.Dbc;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace Amido.Testing.WebApi.ValidationRules
{
    /// <summary>
    /// Debug Text.
    /// This is not really a validation rule. It is merely a way of putting text into the output details.
    /// </summary>
    [DisplayName("Debug Text")]
    public class DebugTextValidationRule : ValidationRule
    {
        private readonly string debugText;

        /// <summary>
        /// Constructs the <see cref="ValidationRule"/>.
        /// </summary>
        /// <param name="debugText">The text you wish to be displayed.</param>
        public DebugTextValidationRule(string debugText)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(debugText), "The debug text cannot be null or empty.");

            this.debugText = debugText;
        }

        /// <summary>
        /// The validate method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ValidationEventArgs"/></param>
        public override void Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = true;
            e.Message = debugText;
        }
    }
}
