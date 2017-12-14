using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Library
{
    class CheckSearchBox : ValidationRule 
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || !(value is string)) return new ValidationResult(false, "Wrong input"); ;
            string input = value as string;
            if (input.Length > 50)
                return new ValidationResult(false, "Input is too long");
            if (input.Contains("\'"))
                return new ValidationResult(false, "Input contains invalid symbols");
            return new ValidationResult(true, null);
        }
    }
}
