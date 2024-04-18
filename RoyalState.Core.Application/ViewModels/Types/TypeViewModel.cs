﻿using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Core.Application.ViewModels.Types
{
    public class TypeViewModel
    {
        public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int PropertiesQuantity { get; set; }
        public List<PropertyViewModel>? Properties { get; set; }
    }
}
