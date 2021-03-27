﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedModels
{
    public class ShelveItem: IValidatableObject, IClientEntity
    {
        public string PartitionKey { get; init; }
        public string RowKey { get; init; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime? ExpirationTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //    Validator.TryValidateValue(Name, validationContext, )
            return null;
        }
    }
}
