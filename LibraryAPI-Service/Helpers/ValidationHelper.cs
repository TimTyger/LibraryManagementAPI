using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI_Service.Helpers
{
    internal class ValidationHelper
    {

        public class GreaterThanZeroAttribute : ValidationAttribute
        {
            public GreaterThanZeroAttribute()
            {
                ErrorMessage = "The value must be greater than 0.";
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is int intValue && intValue > 0)
                {
                    return ValidationResult.Success!;
                }

                return new ValidationResult(ErrorMessage);
            }
        }

    }


}
