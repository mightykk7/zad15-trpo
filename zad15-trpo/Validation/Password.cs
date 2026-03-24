using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace zad15_trpo.Validation
{
    public class Password : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            var input = value.ToString();

            if (value == null || string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Значение не может быть пустым");
            }

            if (!long.TryParse(input, out long longValue))
            {
                return new ValidationResult(false, "Пинкод состоит только из цифр");
            }

            if (input.Length != 4)
            {
                return new ValidationResult(false, "Пинкод состоит из 4 символов");
            }

            return ValidationResult.ValidResult;
        }
    }
}
